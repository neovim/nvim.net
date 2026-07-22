using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nvim.Client.Rpc;

internal sealed class HandlerRegistry
{
  private readonly object _gate = new();
  private readonly Dictionary<string, RequestHandler> _requestHandlers = new(
    StringComparer.Ordinal
  );
  private readonly Dictionary<
    string,
    List<Handler<NvimValue>>
  > _notificationHandlers = new(StringComparer.Ordinal);
  private readonly List<Handler<NvimUiEvent>> _uiEventHandlers = [];
  private bool _terminated;

  internal NvimHandlerRegistration RegisterRequest(
    string method,
    Func<IReadOnlyList<NvimValue>, CancellationToken, Task<NvimValue>> handler
  )
  {
    ValidateHandler(method, handler);

    lock (_gate)
    {
      ThrowIfTerminated();

      if (_requestHandlers.ContainsKey(method))
        throw new InvalidOperationException(
          $"A handler is already registered for {method}."
        );

      var registration = new NvimHandlerRegistration(Unregister);
      _requestHandlers.Add(method, new RequestHandler(handler, registration));
      return registration;
    }
  }

  internal NvimHandlerRegistration RegisterNotification(
    string method,
    Func<IReadOnlyList<NvimValue>, CancellationToken, Task> handler
  )
  {
    ValidateHandler(method, handler);

    lock (_gate)
    {
      ThrowIfTerminated();

      var registration = new NvimHandlerRegistration(Unregister);

      if (!_notificationHandlers.TryGetValue(method, out var handlers))
        _notificationHandlers.Add(method, handlers = []);

      handlers.Add(new Handler<NvimValue>(handler, registration));
      return registration;
    }
  }

  internal NvimHandlerRegistration RegisterUiEvents(
    Func<IReadOnlyList<NvimUiEvent>, CancellationToken, Task> handler
  )
  {
    ArgumentNullException.ThrowIfNull(handler);

    lock (_gate)
    {
      ThrowIfTerminated();

      var registration = new NvimHandlerRegistration(Unregister);
      _uiEventHandlers.Add(new Handler<NvimUiEvent>(handler, registration));
      return registration;
    }
  }

  internal Task<NvimValue> DispatchRequestAsync(
    string method,
    IReadOnlyList<NvimValue> arguments
  )
  {
    RequestHandler? handler;

    lock (_gate)
    {
      if (_terminated || !_requestHandlers.TryGetValue(method, out handler))
        return Task.FromException<NvimValue>(
          new NvimRpcException($"Unhandled request: {method}.")
        );
    }

    var invocation = handler.InvokeAsync(arguments);
    Observe(invocation);
    return invocation;
  }

  internal void DispatchNotification(
    string method,
    IReadOnlyList<NvimValue> arguments
  )
  {
    Handler<NvimValue>[] handlers;

    lock (_gate)
    {
      if (
        _terminated
        || !_notificationHandlers.TryGetValue(method, out var registered)
      )
        return;

      handlers = registered.ToArray();
    }

    foreach (var handler in handlers)
      Observe(InvokeHandlerAsync(handler, arguments));
  }

  internal void DispatchUiEvents(IReadOnlyList<NvimValue> batch)
  {
    Handler<NvimUiEvent>[] handlers;

    lock (_gate)
    {
      if (_terminated)
        return;

      handlers = _uiEventHandlers.ToArray();
    }

    if (handlers.Length == 0)
      return;

    var events = NvimUiEventFactory.Decode(batch);

    foreach (var handler in handlers)
      Observe(InvokeHandlerAsync(handler, events));
  }

  internal void Observe(Task task) =>
    _ = task.ContinueWith(
      static completed =>
      {
        _ = completed.Exception;
      },
      CancellationToken.None,
      TaskContinuationOptions.OnlyOnFaulted
        | TaskContinuationOptions.ExecuteSynchronously,
      TaskScheduler.Default
    );

  internal void Terminate()
  {
    NvimHandlerRegistration[] registrations;

    lock (_gate)
    {
      if (_terminated)
        return;

      _terminated = true;
      registrations = _requestHandlers
        .Values.Select(handler => handler.Registration)
        .Concat(
          _notificationHandlers.Values.SelectMany(handlers =>
            handlers.Select(handler => handler.Registration)
          )
        )
        .Concat(_uiEventHandlers.Select(handler => handler.Registration))
        .ToArray();

      _requestHandlers.Clear();
      _notificationHandlers.Clear();
      _uiEventHandlers.Clear();
    }

    foreach (var registration in registrations)
      registration.Complete();
  }

  private async Task InvokeHandlerAsync<T>(
    Handler<T> handler,
    IReadOnlyList<T> values
  )
  {
    try
    {
      await handler.InvokeAsync(values).ConfigureAwait(false);
    }
    catch (OperationCanceledException) when (handler.Registration.IsCompleted)
    { }
    catch (Exception exception)
    {
      Fail(handler.Registration, exception);
    }
  }

  private void Fail(NvimHandlerRegistration registration, Exception exception)
  {
    Unregister(registration);
    registration.Fail(exception);
  }

  private void Unregister(NvimHandlerRegistration registration)
  {
    lock (_gate)
    {
      foreach (
        var pair in _requestHandlers
          .Where(pair => ReferenceEquals(pair.Value.Registration, registration))
          .ToArray()
      )
        _requestHandlers.Remove(pair.Key);

      foreach (var pair in _notificationHandlers.ToArray())
      {
        pair.Value.RemoveAll(handler =>
          ReferenceEquals(handler.Registration, registration)
        );

        if (pair.Value.Count == 0)
          _notificationHandlers.Remove(pair.Key);
      }

      _uiEventHandlers.RemoveAll(handler =>
        ReferenceEquals(handler.Registration, registration)
      );
    }
  }

  private void ThrowIfTerminated()
  {
    if (_terminated)
      throw new NvimConnectionException("The Neovim client is stopped.");
  }

  private static void ValidateHandler<T>(string method, T handler)
    where T : class
  {
    ArgumentException.ThrowIfNullOrWhiteSpace(method);
    ArgumentNullException.ThrowIfNull(handler);
  }

  private sealed class RequestHandler(
    Func<IReadOnlyList<NvimValue>, CancellationToken, Task<NvimValue>> callback,
    NvimHandlerRegistration registration
  )
  {
    internal NvimHandlerRegistration Registration { get; } = registration;

    internal async Task<NvimValue> InvokeAsync(
      IReadOnlyList<NvimValue> arguments
    ) => await callback(arguments, Registration.Token).ConfigureAwait(false);
  }

  private sealed class Handler<T>(
    Func<IReadOnlyList<T>, CancellationToken, Task> callback,
    NvimHandlerRegistration registration
  )
  {
    internal NvimHandlerRegistration Registration { get; } = registration;

    internal Task InvokeAsync(IReadOnlyList<T> values) =>
      callback(values, Registration.Token);
  }
}
