namespace NvimClient.APIGenerator.Docs
{
  internal class Text : IDocElement
  {
    private readonly string _text;

    public Text(string text) => _text = text;

    public override string ToString() => _text;
  }
}
