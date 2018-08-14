using NvimClient.API;
using NvimClient.API.NvimPlugin.Attributes;
using NvimClient.API.NvimPlugin.Parameters;

namespace NvimClient.Test
{
  [NvimPlugin(Version = "0.0.1")]
  internal class TestPlugin
  {
    private readonly NvimAPI _nvim;
    public static bool AutocmdCalled;
    public static string[] Command1Args;
    public static string Command2Args;
    public static long CountLinesReturn;

    public TestPlugin(NvimAPI nvim) => _nvim = nvim;

    [NvimFunction]
    public long AddNumbers(long num1, long num2) => num1 + num2;

    [NvimFunction]
    public long CountLines(NvimRange range)
    {
      var lineCount = range.LastLine - range.FirstLine + 1;
      _nvim.OutWrite(
        $"Function {nameof(CountLines)} called with {lineCount} lines in range");
      CountLinesReturn = lineCount;
      return lineCount;
    }

    [NvimCommand(NArgs = "*")]
    public void TestCommand1(string[] args)
    {
      Command1Args = args;
    }

    [NvimCommand(NArgs = "?")]
    public void TestCommand2(string optionalArg)
    {
      Command2Args = optionalArg;
    }

    [NvimAutocmd("BufEnter", Pattern = "*.cs")]
    public void OnBufEnter([NvimEval("expand('<afile>')")] string filename,
      [NvimEval("&shiftwidth")] long shiftWidth)
    {
      var indent = new string(' ', (int) shiftWidth);
      _nvim.SetCurrentLine(
        indent + $"{nameof(OnBufEnter)} called with {filename}");
      AutocmdCalled = true;
    }
  }
}
