namespace NvimClient.API.NvimPlugin.Parameters
{
  public class NvimCount
  {
    private readonly long _value;
    internal NvimCount(long value) => _value = value;

    public static implicit operator long(NvimCount count) => count._value;
  }
}
