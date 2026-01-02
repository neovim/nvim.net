using NvimClient.NvimMsgpack.Models;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace NvimClient.API;

internal sealed class PendingRequest : IDisposable {
    private readonly TimeSpan _responseTimeout = TimeSpan.FromSeconds(10);
    private readonly ManualResetEvent _receivedResponseEvent;
    private NvimResponse? _response;
    private readonly TaskCompletionSource<NvimResponse> taskCompletionSource;

    internal PendingRequest(NvimRequest request) {
        _receivedResponseEvent = new ManualResetEvent(false);
        taskCompletionSource = new();
    }


    internal Task<NvimResponse> GetResponse() {
        RegisterResponseEvent(_responseTimeout);
        return taskCompletionSource.Task;
    }

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



    internal void Complete(NvimResponse response) {
        _response = response;
        _ = _receivedResponseEvent.Set();
    }

    public void Dispose() {
        _receivedResponseEvent.Dispose();
    }
}