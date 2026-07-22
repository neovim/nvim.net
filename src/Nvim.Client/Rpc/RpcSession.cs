using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using MessagePack;

namespace Nvim.Client.Rpc;

/// <summary>
///
/// </summary>
public abstract class RpcSession
{
  private enum MessageKind
  {
    Request = 0,
    Response = 1,
    Notification = 2,
  }

  private readonly RpcTransport _transport;
  private readonly SemaphoreSlim _writeGate = new(1, 1);
  private readonly CancellationTokenSource _shutdown = new();
  private readonly ConcurrentDictionary<
    long,
    TaskCompletionSource<NvimValue>
  > _pendingRequests = new();
  private readonly HandlerRegistry _handlers = new();
  private readonly TaskCompletionSource _completion = new(
    TaskCreationOptions.RunContinuationsAsynchronously
  );
  private long _nextRequestId;
  private Exception? _terminationException;
  private int _terminated;

  internal RpcSession(RpcTransport transport)
  {
    _transport =
      transport ?? throw new ArgumentNullException(nameof(transport));
    _ = ReceiveLoopAsync();
  }

  /// <inheritdoc/>
  public Task Completion => _completion.Task;

  internal int PendingCount => _pendingRequests.Count;

  internal Exception? TerminalException => _terminationException;

  /// <inheritdoc/>
  public async Task<NvimValue> RequestAsync(
    string method,
    IReadOnlyList<NvimValue> arguments,
    CancellationToken cancellationToken = default
  )
  {
    ValidateInvocation(method, arguments);
    cancellationToken.ThrowIfCancellationRequested();
    ThrowIfTerminated();

    var id = Interlocked.Increment(ref _nextRequestId);
    var pending = new TaskCompletionSource<NvimValue>(
      TaskCreationOptions.RunContinuationsAsynchronously
    );

    if (!_pendingRequests.TryAdd(id, pending))
      throw new NvimConnectionException("Unable to allocate RPC request.");

    if (Volatile.Read(ref _terminated) != 0)
    {
      _pendingRequests.TryRemove(id, out _);
      throw new NvimConnectionException("The Neovim client is stopped.");
    }

    var writeStarted = 0;
    using var cancellation = cancellationToken.Register(() =>
    {
      if (Volatile.Read(ref writeStarted) != 0)
        pending.TrySetCanceled(cancellationToken);
    });

    try
    {
      await WriteMessageAsync(
          new NvimArray([
            new NvimInteger((int)MessageKind.Request),
            new NvimInteger(id),
            new NvimString(method),
            new NvimArray(arguments),
          ]),
          cancellationToken,
          () => Interlocked.Exchange(ref writeStarted, 1)
        )
        .ConfigureAwait(false);

      return await pending.Task.ConfigureAwait(false);
    }
    catch
    {
      if (Volatile.Read(ref writeStarted) == 0)
        _pendingRequests.TryRemove(id, out _);

      throw;
    }
  }

  /// <inheritdoc/>
  public Task NotifyAsync(
    string method,
    IReadOnlyList<NvimValue> arguments,
    CancellationToken cancellationToken = default
  )
  {
    ValidateInvocation(method, arguments);

    return WriteMessageAsync(
      new NvimArray([
        new NvimInteger((int)MessageKind.Notification),
        new NvimString(method),
        new NvimArray(arguments),
      ]),
      cancellationToken
    );
  }

  /// <inheritdoc/>
  public NvimHandlerRegistration RegisterRequestHandler(
    string method,
    Func<IReadOnlyList<NvimValue>, CancellationToken, Task<NvimValue>> handler
  ) => _handlers.RegisterRequest(method, handler);

  /// <inheritdoc/>
  public NvimHandlerRegistration RegisterNotificationHandler(
    string method,
    Func<IReadOnlyList<NvimValue>, CancellationToken, Task> handler
  ) => _handlers.RegisterNotification(method, handler);

  /// <inheritdoc/>
  public NvimHandlerRegistration RegisterUiEventHandler(
    Func<IReadOnlyList<NvimUiEvent>, CancellationToken, Task> handler
  ) => _handlers.RegisterUiEvents(handler);

  /// <inheritdoc/>
  public async Task StopAsync(CancellationToken cancellationToken = default)
  {
    Terminate(new NvimConnectionException("The Neovim client stopped."));
    await _transport.StopAsync(cancellationToken).ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public void Dispose()
  {
    Terminate(new NvimConnectionException("The Neovim client was disposed."));
    _transport.Dispose();
  }

  private async Task WriteMessageAsync(
    NvimValue value,
    CancellationToken cancellationToken,
    Action? writeStarted = null
  )
  {
    cancellationToken.ThrowIfCancellationRequested();
    await _writeGate.WaitAsync(cancellationToken).ConfigureAwait(false);

    try
    {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfTerminated();

      var frame = NvimValueCodec.Encode(value);
      writeStarted?.Invoke();

      await _transport
        .Output.WriteAsync(frame, CancellationToken.None)
        .ConfigureAwait(false);
      await _transport
        .Output.FlushAsync(CancellationToken.None)
        .ConfigureAwait(false);
    }
    catch (Exception exception)
      when (exception is not NvimConnectionException
        && exception is not OperationCanceledException
      )
    {
      var connection = new NvimConnectionException(
        "Neovim RPC write failed.",
        exception
      );
      Terminate(connection);
      throw connection;
    }
    finally
    {
      _writeGate.Release();
    }
  }

  private async Task ReceiveLoopAsync()
  {
    try
    {
      using var reader = new MessagePackStreamReader(_transport.Input, true);

      while (Volatile.Read(ref _terminated) == 0)
      {
        var message = await reader
          .ReadAsync(_shutdown.Token)
          .ConfigureAwait(false);

        if (!message.HasValue)
        {
          if (!reader.RemainingBytes.IsEmpty)
            throw new NvimProtocolException("Incomplete MessagePack value.");

          break;
        }

        if (Volatile.Read(ref _terminated) != 0)
          break;

        DispatchMessage(NvimValueCodec.Decode(message.Value));
      }

      Terminate(
        new NvimConnectionException("Neovim closed the RPC connection.")
      );
    }
    catch (OperationCanceledException)
      when (Volatile.Read(ref _terminated) != 0) { }
    catch (NvimProtocolException exception)
    {
      Terminate(exception);
    }
    catch (EndOfStreamException exception)
    {
      Terminate(
        new NvimProtocolException("Incomplete MessagePack value.", exception)
      );
    }
    catch (Exception exception)
    {
      Terminate(
        exception as NvimConnectionException
          ?? new NvimConnectionException(
            "Neovim RPC receive failed.",
            exception
          )
      );
    }
  }

  private void DispatchMessage(NvimValue message)
  {
    if (
      Volatile.Read(ref _terminated) != 0
      || message is not NvimArray envelope
      || envelope.Items.Count < 3
      || envelope.Items[0] is not NvimInteger kind
      || kind.Value < 0
      || kind.Value > 2
    )
      throw new NvimProtocolException("Invalid RPC envelope.");

    switch ((MessageKind)(int)kind.Value)
    {
      case MessageKind.Response:
        HandleResponse(envelope);
        return;
      case MessageKind.Request:
        HandleRequest(envelope);
        return;
      case MessageKind.Notification:
        HandleNotification(envelope);
        return;
      default:
        throw new NvimProtocolException("Unknown RPC envelope.");
    }
  }

  private void HandleResponse(NvimArray envelope)
  {
    if (
      envelope.Items.Count != 4
      || !TryGetRequestId(envelope.Items[1], out var id)
    )
      throw new NvimProtocolException("Invalid RPC response.");

    if (!_pendingRequests.TryRemove(id, out var pending))
      return;

    if (envelope.Items[2] is NvimNil)
      pending.TrySetResult(envelope.Items[3]);
    else
      pending.TrySetException(
        new NvimRpcException(envelope.Items[2].ToString())
      );
  }

  private void HandleRequest(NvimArray envelope)
  {
    if (
      envelope.Items.Count != 4
      || !TryGetRequestId(envelope.Items[1], out var id)
      || envelope.Items[2] is not NvimString method
      || envelope.Items[3] is not NvimArray arguments
    )
      throw new NvimProtocolException("Invalid RPC request.");

    _handlers.Observe(ProcessRequestAsync(id, method.Value, arguments.Items));
  }

  private void HandleNotification(NvimArray envelope)
  {
    if (
      envelope.Items.Count != 3
      || envelope.Items[1] is not NvimString method
      || envelope.Items[2] is not NvimArray arguments
    )
      throw new NvimProtocolException("Invalid RPC notification.");

    if (method.Value == "redraw")
      _handlers.DispatchUiEvents(arguments.Items);
    else
      _handlers.DispatchNotification(method.Value, arguments.Items);
  }

  private async Task ProcessRequestAsync(
    long id,
    string method,
    IReadOnlyList<NvimValue> arguments
  )
  {
    try
    {
      var result = await _handlers
        .DispatchRequestAsync(method, arguments)
        .ConfigureAwait(false);

      await WriteMessageAsync(
          new NvimArray([
            new NvimInteger((int)MessageKind.Response),
            new NvimInteger(id),
            new NvimNil(),
            result,
          ]),
          CancellationToken.None
        )
        .ConfigureAwait(false);
    }
    catch (Exception exception)
    {
      if (Volatile.Read(ref _terminated) != 0)
        return;

      try
      {
        await WriteMessageAsync(
            new NvimArray([
              new NvimInteger((int)MessageKind.Response),
              new NvimInteger(id),
              new NvimString(exception.Message),
              new NvimNil(),
            ]),
            CancellationToken.None
          )
          .ConfigureAwait(false);
      }
      catch (NvimConnectionException) when (Volatile.Read(ref _terminated) != 0)
      { }
    }
  }

  private void Terminate(Exception exception)
  {
    if (Interlocked.Exchange(ref _terminated, 1) != 0)
      return;

    _terminationException = exception;
    _shutdown.Cancel();
    _handlers.Terminate();

    foreach (var pending in _pendingRequests.Values)
      pending.TrySetException(exception);

    _pendingRequests.Clear();
    _completion.TrySetResult();
  }

  private void ThrowIfTerminated()
  {
    if (Volatile.Read(ref _terminated) != 0)
      throw new NvimConnectionException("The Neovim client is stopped.");
  }

  private static bool TryGetRequestId(NvimValue value, out long id)
  {
    if (
      value is not NvimInteger integer
      || integer.Value < BigInteger.Zero
      || integer.Value > long.MaxValue
    )
    {
      id = default;
      return false;
    }

    id = (long)integer.Value;
    return true;
  }

  private static void ValidateInvocation(
    string method,
    IReadOnlyList<NvimValue> arguments
  )
  {
    ArgumentException.ThrowIfNullOrWhiteSpace(method);
    ArgumentNullException.ThrowIfNull(arguments);
  }
}
