namespace NvimClient.NvimMsgpack.Models
{
  public abstract class NvimFunctionEventBase
  {
    public string Name { get; set; }
    public NvimParameter[] Parameters { get; set; }
    public int Since { get; set; }
    public int? DeprecatedSince { get; set; }
  }
}
