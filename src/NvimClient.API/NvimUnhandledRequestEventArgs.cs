using System;

namespace NvimClient.API;

/// <summary>
/// Event arguments for the nvim UnhandledRequest event
/// </summary>
public class NvimUnhandledRequestEventArgs : EventArgs {
    private readonly NvimAPI _nvim;

    /// <summary>
    /// The method name
    /// </summary>
    public string MethodName { get; }

    /// <summary>
    /// The method arguments
    /// </summary>
    public object[] Arguments { get; }

    /// <summary>
    /// The request id
    /// </summary>
    public uint RequestId { get; }

    internal NvimUnhandledRequestEventArgs(NvimAPI nvim, uint requestId, string methodName, object[] arguments) {
        _nvim = nvim;
        RequestId = requestId;
        MethodName = methodName;
        Arguments = arguments;
    }


    /// <summary>
    /// Sends a response
    /// </summary>
    public void SendResponse(object result, object? error = null) {
        _nvim.SendResponse(this, result, error);
    }
}