using System;

namespace NvimClient.API
{
  public class NvimUnhandledRequestEventArgs : EventArgs
  {
    private readonly NvimAPI _nvim;

    internal NvimUnhandledRequestEventArgs(NvimAPI nvim, uint requestId,
      string methodName, object[] arguments)
    {
      _nvim      = nvim;
      RequestId  = requestId;
      MethodName = methodName;
      Arguments  = arguments;
    }

    public string   MethodName { get; }
    public object[] Arguments  { get; }
    public uint     RequestId  { get; }

    public void SendResponse(object result, object error = null) =>
      _nvim.SendResponse(this, result, error);
  }
}
