using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsgPack;
using MsgPack.Serialization;
using NvimClient.API;
using NvimClient.API.NvimPlugin;
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
      var result = await api.Eval("2 + 2");
      Assert.AreEqual(4L, result);
    }

    [TestMethod]
    public async Task TestCallAndReply()
    {
      var api = new NvimAPI();
      api.RegisterHandler("client-call", args =>
      {
        CollectionAssert.AreEqual(new[] {1L, 2L, 3L}, args);
        return new[]{4, 5, 6};
      });
      var objects = await api.GetApiInfo();
      var channelID = (long) objects.First();
      await api.Command(
        $"let g:result = rpcrequest({channelID}, 'client-call', 1, 2, 3)");
      var result = (object[]) await api.GetVar("result");
      CollectionAssert.AreEqual(new[]{4L, 5L, 6L}, result);
    }

    [TestMethod]
    public async Task TestNvimUIEvent()
    {
      const string testString = "hello_world";
      var titleSetEvent = new ManualResetEvent(false);
      var api = new NvimAPI();
      await api.UiAttach(100, 200, new Dictionary<string, string>());
      api.SetTitle += (sender, args) =>
      {
        if (args.Title == testString)
        {
          titleSetEvent.Set();
        }
      };
      await api.Command($"set titlestring={testString} | set title");
      Assert.IsTrue(titleSetEvent.WaitOne(TimeSpan.FromSeconds(5)));
    }

    [TestMethod]
    public async Task TestPluginExports()
    {
      const string pluginPath = "/path/to/plugin.sln";
      var api = new NvimAPI();
      await PluginHost.RegisterPlugin<TestPlugin>(api, pluginPath);

      await api.Command(
        $"let g:result = {nameof(TestPlugin.AddNumbers)}(1, 2)");
      var result = await api.GetVar("result");
      Assert.AreEqual(3L, result);

      await api.Command($"{nameof(TestPlugin.TestCommand1)} a b c");
      CollectionAssert.AreEqual(new[] {"a", "b", "c"}, TestPlugin.Command1Args);

      await api.Command($"{nameof(TestPlugin.TestCommand2)} 1 2 3");
      Assert.AreEqual("1 2 3", TestPlugin.Command2Args);

      await api.Command("edit test.cs");
      Assert.IsTrue(TestPlugin.AutocmdCalled);

      await api.Command($"call {nameof(TestPlugin.CountLines)}()");
      Assert.IsTrue(TestPlugin.CountLinesReturn == 1);
    }
  }
}
