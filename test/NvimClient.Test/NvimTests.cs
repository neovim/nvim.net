using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsgPack.Serialization;
using NvimClient.NvimMsgpack.Models;
using NvimClient.NvimProcess;

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

    [DataTestMethod]
    [DataRow("aaaa", "aaaa")]
    [DataRow("AAAA", "aaaa")]
    [DataRow("aaAa", "aa_aa")]
    [DataRow("aaAA", "aa_aa")]
    [DataRow("AAaa", "a_aaa")]
    public void TestConvertToSnakeCase(string input, string expected)
    {
      Assert.AreEqual(expected, StringUtil.ConvertToSnakeCase(input));
    }

    [TestMethod]
    public void TestApiMetadataDeserialization()
    {
      var process = Process.Start(
        new NvimProcessStartInfo(StartOption.ApiInfo | StartOption.Headless));

      var context = new SerializationContext();
      context.DictionarySerlaizationOptions.KeyTransformer =
        StringUtil.ConvertToSnakeCase;
      var serializer  = context.GetSerializer<NvimAPIMetadata>();
      var apiMetadata = serializer.Unpack(process.StandardOutput.BaseStream);

      Assert.IsNotNull(apiMetadata.Version);
      Assert.IsTrue(apiMetadata.Functions.Any()
                    && apiMetadata.UIEvents.Any()
                    && apiMetadata.Types.Any()
                    && apiMetadata.ErrorTypes.Any());
    }
  }
}
