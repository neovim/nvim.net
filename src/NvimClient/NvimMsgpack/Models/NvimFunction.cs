namespace NvimClient.NvimMsgpack.Models
{
  public class NvimFunction : NvimFunctionEventBase
  {
    public bool Method { get; set; }
    public string ReturnType { get; set; }
  }
}
