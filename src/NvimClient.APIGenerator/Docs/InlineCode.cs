namespace NvimClient.APIGenerator.Docs
{
  internal class InlineCode : IDocElement
  {
    private readonly string _code;

    public InlineCode(string code) => _code = code;

    public override string ToString() => _code;
  }
}
