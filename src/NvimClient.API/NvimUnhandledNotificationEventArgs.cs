using System;

namespace NvimClient.API
{
  public class NvimUnhandledNotificationEventArgs : EventArgs
  {
    internal NvimUnhandledNotificationEventArgs(string methodName,
      object[] arguments)
    {
      MethodName = methodName;
      Arguments = arguments;
    }

    public string MethodName { get; }
    public object[] Arguments { get; }
  }
}
