using System;
using System.Threading;
using System.Threading.Tasks;

namespace Nvim.Client;

/// <summary>
/// Owns the lifetime of one registered RPC or UI handler.
/// </summary>
public sealed class NvimHandlerRegistration : IDisposable
{
  private readonly Action<NvimHandlerRegistration> _unregister;
  private readonly CancellationTokenSource _lifetime = new();
  private readonly TaskCompletionSource _completion = new(
    TaskCreationOptions.RunContinuationsAsynchronously
  );
  private int _completed;

  internal NvimHandlerRegistration(Action<NvimHandlerRegistration> unregister)
  {
    _unregister = unregister;
  }

  internal CancellationToken Token => _lifetime.Token;
  internal bool IsCompleted => Volatile.Read(ref _completed) != 0;

  /// <summary>
  /// Completes when disposal, client termination, or handler failure ends the registration.
  /// </summary>
  public Task Completion => _completion.Task;

  /// <summary>
  /// Removes the handler, cancels its token, and completes its registration.
  /// </summary>
  public void Dispose()
  {
    if (!TryEnd())
      return;

    _unregister(this);
    _completion.TrySetResult();
  }

  internal void Complete()
  {
    TryEnd();
    _completion.TrySetResult();
  }

  internal void Fail(Exception exception)
  {
    TryEnd();
    _completion.TrySetException(exception);
  }

  private bool TryEnd()
  {
    if (Interlocked.Exchange(ref _completed, 1) != 0)
      return false;

    _lifetime.Cancel();
    return true;
  }
}
