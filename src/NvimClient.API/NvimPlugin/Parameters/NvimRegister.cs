namespace NvimClient.API.NvimPlugin.Parameters
{
  public class NvimRegister
  {
    private readonly string _value;
    internal NvimRegister(string value) => _value = value;

    public static implicit operator string(NvimRegister register) =>
      register._value;

    public override string ToString() => _value;
  }
}
