using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NvimClient.Test
{
  [TestClass]
  public class NvimTests
  {
    [TestMethod]
    public void TestProcessStarts()
    {
      var process = NvimProcess.NvimProcess.Start(new ProcessStartInfo
      {
        Arguments = "--version",
        RedirectStandardOutput = true
      });
      process.WaitForExit();
      Assert.IsTrue(
        process.StandardOutput.ReadToEnd().Contains("NVIM") &&
        process.ExitCode == 0);
    }
  }
}
