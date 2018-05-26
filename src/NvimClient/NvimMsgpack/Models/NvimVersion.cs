namespace NvimClient.NvimMsgpack.Models
{
  public class NvimVersion
  {
    public int Major { get; set; }
    public int Minor { get; set; }
    public int Patch { get; set; }
    public int ApiLevel { get; set; }
    public int ApiCompatible { get; set; }
    public bool ApiPrerelease { get; set; }
  }
}
