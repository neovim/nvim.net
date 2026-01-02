using NvimClient.NvimMsgpack.Models;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace NvimClient.API;

/// <summary>
/// Represents a request that is not yet responded from an nvim instance. This
/// class implement a blocking/awaitable mechanism for callers to receive their
/// requests. It essentially maps a pending <see cref="NvimRequest"/> to it's
/// caller.
/// </summary>
internal sealed class PendingRequest : IDisposable {

    /// <summary>
    /// The Nvim that has created the <see cref="PendingRequest"/>
    /// </summary>
    public NvimRequest Request { get; set; }

    /// <summary>
    /// The default timeout of the request
    /// </summary>
    private readonly TimeSpan _responseTimeout = TimeSpan.FromSeconds(10);

    /// <summary>
    /// Represents the producer side of a Task unbound to a delegate, providing
    /// access to the consumer side through the Task property.
    /// </summary>
    private readonly TaskCompletionSource<NvimResponse?> taskCompletionSource;

    /// <summary>
    /// The reset event that is used for the signaling
    /// </summary>
    private readonly ManualResetEvent _receivedResponseEvent;

    /// <summary>
    /// The eventual <see cref="NvimResponse"/> that will be received
    /// </summary>
    private NvimResponse? _response;


    /// <summary>
    /// Creates a Pending request
    /// </summary>
    internal PendingRequest(NvimRequest request) {
        Request = request;
        _receivedResponseEvent = new ManualResetEvent(false);
        taskCompletionSource = new();
    }


    /// <summary>
    /// Returns a Task that will be completed later by another thread
    /// It does not:
    ///     perform asynchronous I/O
    ///     suspend execution
    ///     need to wait for anything itself
    ///     The waiting happens outside this method, when the caller awaits the
    ///     returned task.
    /// </summary>
    internal Task<NvimResponse?> GetResponseReceptionTask() {
        RegisterResponseEvent(_responseTimeout);
        return taskCompletionSource.Task;
    }

    /// <summary>
    /// This method Waits up to the given timeout. If no response arrives: Logs a warning
    /// Re-registers the wait without a timeout. The task is never faulted or canceled
    /// </summary>
    private void RegisterResponseEvent(TimeSpan timeout) {
        _ = ThreadPool.RegisterWaitForSingleObject(_receivedResponseEvent, new WaitOrTimerCallback(OnWaitOrTimer), this, timeout, true);
    }


    private void OnWaitOrTimer(object? state, bool timedOut) {
        if (timedOut) {
            Debug.WriteLine($"Warning: response was not received within {_responseTimeout.TotalSeconds} seconds");
            // Continue waiting without a timeout
            RegisterResponseEvent(Timeout.InfiniteTimeSpan);
        } else {
            if (state is null) {
                return;
            }
            PendingRequest the_state = (PendingRequest)state;
            taskCompletionSource.SetResult(the_state._response);
        }
    }


    /// <summary>
    /// Completes the pending request by
    /// </summary>
    internal void Complete(NvimResponse response) {
        _response = response;
        _ = _receivedResponseEvent.Set();
    }

    public void Dispose() {
        _receivedResponseEvent.Dispose();
    }
}