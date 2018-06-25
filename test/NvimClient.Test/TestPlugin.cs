using NvimClient.API;
using NvimClient.NvimPlugin;
using NvimClient.NvimPlugin.Attributes;

namespace NvimClient.Test
{
  [NvimPlugin(Version = "0.0.1")]
  internal class TestPlugin
  {
    private readonly NvimAPI _nvim;

    public TestPlugin(NvimAPI nvim) => _nvim = nvim;

    [NvimFunction(Sync = true)]
    public long AddNumbers(long num1, long num2) => num1 + num2;

    [NvimCommand]
    public void TestCommand(string range, params object[] args)
    {
      _nvim.SetCurrentLine($"Command with args: {args}, range: {range}");
    }

    [NvimAutocmd("BufEnter", Pattern = "*.cs", Eval = "expand('<afile>')")]
    public void OnBufEnter(string filename)
    {
      _nvim.OutWrite($"testplugin is in '{filename}'\n");
    }
  }
}
