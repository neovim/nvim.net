using NvimClient.API;
using NvimClient.NvimPlugin;
using NvimClient.NvimPlugin.Attributes;

namespace NvimClient.Test
{
  [NvimPlugin(Version = "0.0.1")]
  internal class TestPlugin
  {
    private readonly NvimAPI _nvim;
    public static bool AutocmdCalled;
    public static bool CommandCalled;

    public TestPlugin(NvimAPI nvim) => _nvim = nvim;

    [NvimFunction]
    public long AddNumbers(long num1, long num2) => num1 + num2;

    [NvimCommand(Range = ".", NArgs = "*")]
    public void TestCommand(long[] range, params object[] args)
    {
      _nvim.SetCurrentLine(
        $"Command with args: {args}, range: {range[0]}-{range[1]}");
      CommandCalled = true;
    }

    [NvimAutocmd("BufEnter", Pattern = "*.cs", Eval = "expand('<afile>')")]
    public void OnBufEnter(string filename)
    {
      _nvim.OutWrite($"testplugin is in '{filename}'\n");
      AutocmdCalled = true;
    }
  }
}
