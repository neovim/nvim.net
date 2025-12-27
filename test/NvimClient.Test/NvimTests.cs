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

namespace NvimClient.Test;

[TestClass]
public class NvimTests {
    [TestMethod]
    public void TestProcessStarts() {
        Process? process = NvimProcess.NvimProcess.Start(new ProcessStartInfo {
            Arguments = "--version",
            RedirectStandardOutput = true
        });

        Assert.IsNotNull(process);

        process.WaitForExit();
        Assert.IsTrue(process.StandardOutput.ReadToEnd().Contains("NVIM") && process.ExitCode == 0);
    }

    [TestMethod]
    [DataRow("aaaa", "aaaa")]
    [DataRow("AAAA", "aaaa")]
    [DataRow("aaAa", "aa_aa")]
    [DataRow("aaAA", "aa_aa")]
    [DataRow("AAaa", "a_aaa")]
    public void TestConvertToSnakeCase(string input, string expected) {
        Assert.AreEqual(expected, StringUtil.ConvertToSnakeCase(input));
    }

    [TestMethod]
    public void TestApiMetadataDeserialization() {
        Process? process = Process.Start(new NvimProcessStartInfo(StartOption.ApiInfo | StartOption.Headless));

        Assert.IsNotNull(process);

        SerializationContext context = new();
        context.DictionarySerlaizationOptions.KeyTransformer = StringUtil.ConvertToSnakeCase;
        MessagePackSerializer<NvimAPIMetadata> serializer = context.GetSerializer<NvimAPIMetadata>();
        NvimAPIMetadata apiMetadata = serializer.Unpack(process.StandardOutput.BaseStream);

        Assert.IsNotNull(apiMetadata.Version);
        Assert.IsTrue(apiMetadata.Functions.Length != 0
                      && apiMetadata.UIEvents.Length != 0
                      && apiMetadata.Types.Count != 0
                      && apiMetadata.ErrorTypes.Count != 0);
    }

    [TestMethod]
    public void TestMessageDeserialization() {
        Process? process = Process.Start(new NvimProcessStartInfo(StartOption.Embed | StartOption.Headless));

        Assert.IsNotNull(process);

        SerializationContext context = new();
        _ = context.Serializers.Register(new NvimMessageSerializer(context));
        MessagePackSerializer<NvimMessage> serializer = MessagePackSerializer.Get<NvimMessage>(context);

        const string testString = "hello world";
        NvimRequest request = new() {
            MessageId = 42,
            Method = "nvim_eval",
            Arguments = new MessagePackObject([$"'{testString}'"])
        };
        serializer.Pack(process.StandardInput.BaseStream, request);

        NvimResponse response = (NvimResponse)serializer.Unpack(process.StandardOutput.BaseStream);

        Assert.IsTrue(response.MessageId == request.MessageId
                      && response.Error == MessagePackObject.Nil
                      && response.Result == testString);
    }

    [TestMethod]
    [DataRow("aaaa", "Aaaa", true)]
    [DataRow("aaaa", "aaaa", false)]
    [DataRow("_aaaa_", "Aaaa", true)]
    [DataRow("_aaaa_", "aaaa", false)]
    [DataRow("aa_aa", "AaAa", true)]
    [DataRow("aa_aa", "aaAa", false)]
    [DataRow("aaa_a", "AaaA", true)]
    [DataRow("aaa_a", "aaaA", false)]
    public void TestConvertToCamelCase(string input, string expected,
      bool capitalizeFirstChar) {
        Assert.AreEqual(expected,
          StringUtil.ConvertToCamelCase(input, capitalizeFirstChar));
    }

    [TestMethod]
    public async Task TestAsyncAPICall() {
        NvimAPI api = new();
        object result = await api.Eval("2 + 2");
        Assert.AreEqual(4L, result);
    }

    [TestMethod]
    public async Task TestCallAndReply() {
        NvimAPI api = new();
        api.RegisterHandler("client-call", static args => {
            CollectionAssert.AreEqual(new[] { 1L, 2L, 3L }, args);
            return new[] { 4, 5, 6 };
        });
        object[] objects = await api.GetApiInfo();
        long channelID = (long)objects.First();
        await api.Command(
          $"let g:result = rpcrequest({channelID}, 'client-call', 1, 2, 3)");
        object[] result = (object[])await api.GetVar("result");
        CollectionAssert.AreEqual(new[] { 4L, 5L, 6L }, result);
    }

    [TestMethod]
    public async Task TestNvimUIEvent() {
        const string testString = "hello_world";
        ManualResetEvent titleSetEvent = new(false);
        NvimAPI api = new();
        await api.UiAttach(100, 200, new Dictionary<string, string>());
        api.SetTitleEvent += (sender, args) => {
            if (args.Title == testString) {
                bool ok = titleSetEvent.Set();
                Assert.IsTrue(ok);
            }
        };
        await api.Command($"set titlestring={testString} | set title");
        Assert.IsTrue(titleSetEvent.WaitOne(TimeSpan.FromSeconds(5)));
    }

    [TestMethod]
    public async Task TestPluginExports() {
        const string pluginPath = "/path/to/plugin.sln";
        NvimAPI api = new();
        await PluginHost.RegisterPlugin<TestPlugin>(api, pluginPath);

        await api.Command(
          $"let g:result = {nameof(TestPlugin.AddNumbers)}(1, 2)");
        object result = await api.GetVar("result");
        Assert.AreEqual(3L, result);

        await api.Command($"{nameof(TestPlugin.TestCommand1)} a b c");
        CollectionAssert.AreEqual(new[] { "a", "b", "c" }, TestPlugin.Command1Args);

        await api.Command($"{nameof(TestPlugin.TestCommand2)} 1 2 3");
        Assert.AreEqual("1 2 3", TestPlugin.Command2Args);

        await api.Command("edit test.cs");
        Assert.IsTrue(TestPlugin.AutocmdCalled);

        await api.Command($"call {nameof(TestPlugin.CountLines)}()");
        Assert.AreEqual(1, TestPlugin.CountLinesReturn);
    }

    [TestMethod]
    public async Task TestTCPSocket() {
        NvimAPI nvimStdio = new();
        string serverAddress = (string)await nvimStdio.CallFunction("serverstart", [System.Net.IPAddress.Loopback + ":"]);

        NvimAPI nvimTCPSocket = new(serverAddress);
        Assert.IsNotNull(await nvimTCPSocket.CommandOutput("version"));
    }

    [TestMethod]
    public async Task TestLocalSocket() {
        NvimAPI nvimStdio = new();
        string serverAddress =
          (string)await nvimStdio.CallFunction("serverstart", []);

        NvimAPI nvimLocalSocket = new(serverAddress);
        Assert.IsNotNull(await nvimLocalSocket.CommandOutput("version"));
    }

    [TestMethod]
    [DataRow(typeof(bool), true)]
    [DataRow(typeof(int), false)]
    [DataRow(typeof(long), true)]
    [DataRow(typeof(object[]), true)]
    [DataRow(typeof(long[]), true)]
    [DataRow(typeof(int[]), false)]
    [DataRow(typeof(IDictionary<object, object>), true)]
    [DataRow(typeof(IDictionary<long, string>), true)]
    [DataRow(typeof(IDictionary<string, DateTime>), false)]
    [DataRow(typeof(IDictionary<Random, long>), false)]
    public void TestNvimTypeValidation(Type type, bool shouldBeValid) {
        Assert.AreEqual(shouldBeValid, NvimTypesMap.IsValidType(type));
    }

    [TestMethod]
    [DataRow("Boolean", "bool")]
    [DataRow("Array", "object[]")]
    [DataRow("Dictionary", "IDictionary")]
    [DataRow("ArrayOf(Float)", "double[]")]
    [DataRow("ArrayOf(Integer, 2)", "long[]")]
    [DataRow("ArrayOf(Buffer)", "NvimBuffer[]")]
    [DataRow("ArrayOf(DictionaryOf(String, String))", "IDictionary<string, string>[]")]
    [DataRow("DictionaryOf(Integer, ArrayOf(String))", "IDictionary<long, string[]>")]
    public void TestCSharpTypeConversion(string nvimType, string csharpType) {
        Assert.AreEqual(csharpType, NvimTypesMap.GetCSharpType(nvimType));
    }
}