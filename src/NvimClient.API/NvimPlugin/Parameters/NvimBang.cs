namespace NvimClient.API.NvimPlugin.Parameters
{
  public class NvimBang
  {
    private readonly bool _value;
    internal NvimBang(bool value) => _value = value;

    public static implicit operator bool(NvimBang bang) => bang._value;
  }
}
