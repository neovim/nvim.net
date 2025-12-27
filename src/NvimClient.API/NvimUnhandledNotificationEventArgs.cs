using System;

namespace NvimClient.API;

/// <summary>
/// Event arguments for an NVIM unhandled notification
/// </summary>
public class NvimUnhandledNotificationEventArgs : EventArgs {
    public string MethodName { get; }
    public object[] Arguments { get; }

    internal NvimUnhandledNotificationEventArgs(string methodName, object[] arguments) {
        MethodName = methodName;
        Arguments = arguments;
    }

}