using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsgPack;
using MsgPack.Serialization;
using NvimClient.API;
using NvimClient.NvimMsgpack;
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

    [TestMethod]
    public void TestMessageDeserialization()
    {
      var process = Process.Start(
        new NvimProcessStartInfo(StartOption.Embed | StartOption.Headless));

      var context = new SerializationContext();
      context.Serializers.Register(new NvimMessageSerializer(context));
      var serializer = MessagePackSerializer.Get<NvimMessage>(context);

      const string testString = "hello world";
      var request = new NvimRequest
      {
        MessageId = 42,
        Method    = "nvim_eval",
        Arguments = new MessagePackObject(new MessagePackObject[] {$"'{testString}'"})
      };
      serializer.Pack(process.StandardInput.BaseStream, request);

      var response = (NvimResponse) serializer.Unpack(process.StandardOutput.BaseStream);

      Assert.IsTrue(response.MessageId == request.MessageId
                    && response.Error == MessagePackObject.Nil
                    && response.Result == testString);
    }

    [DataTestMethod]
    [DataRow("aaaa", "Aaaa", true)]
    [DataRow("aaaa", "aaaa", false)]
    [DataRow("_aaaa_", "Aaaa", true)]
    [DataRow("_aaaa_", "aaaa", false)]
    [DataRow("aa_aa", "AaAa", true)]
    [DataRow("aa_aa", "aaAa", false)]
    [DataRow("aaa_a", "AaaA", true)]
    [DataRow("aaa_a", "aaaA", false)]
    public void TestConvertToCamelCase(string input, string expected,
      bool capitalizeFirstChar)
    {
      Assert.AreEqual(expected,
        StringUtil.ConvertToCamelCase(input, capitalizeFirstChar));
    }

    [TestMethod]
    public async Task TestAsyncAPICall()
    {
      var api = new NvimAPI();
      var result = await api.NvimEval("2 + 2");
      Assert.AreEqual(4, result);
    }
  }
}
