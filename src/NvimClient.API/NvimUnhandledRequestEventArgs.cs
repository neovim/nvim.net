using System;

namespace NvimClient.API
{
  public class NvimUnhandledRequestEventArgs : EventArgs
  {
    private readonly NvimAPI _nvim;

    internal NvimUnhandledRequestEventArgs(NvimAPI nvim, long requestId,
      string methodName, object[] arguments)
    {
      _nvim      = nvim;
      RequestId  = requestId;
      MethodName = methodName;
      Arguments  = arguments;
    }

    public string   MethodName { get; }
    public object[] Arguments  { get; }
    public long     RequestId  { get; }

    public void SendResponse(object result, object error = null) =>
      _nvim.SendResponse(this, result, error);
  }
}
