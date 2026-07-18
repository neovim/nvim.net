using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
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
      var process = NvimProcess.NvimProcess.Start(
        new ProcessStartInfo
        {
          Arguments = "--version",
          RedirectStandardOutput = true,
        }
      );
      process.WaitForExit();
      Assert.IsTrue(
        process.StandardOutput.ReadToEnd().Contains("NVIM")
          && process.ExitCode == 0
      );
    }

    [TestMethod]
    public void TestApiMetadataDeserialization()
    {
      var process = Process.Start(
        new NvimProcessStartInfo(StartOption.ApiInfo | StartOption.Headless)
      );

      var context = new SerializationContext();
      context.DictionarySerlaizationOptions.KeyTransformer = JsonNamingPolicy
        .SnakeCaseLower
        .ConvertName;
      var serializer = context.GetSerializer<NvimAPIMetadata>();
      var apiMetadata = serializer.Unpack(process.StandardOutput.BaseStream);

      Assert.IsNotNull(apiMetadata.Version);
      Assert.IsTrue(
        apiMetadata.Functions.Any()
          && apiMetadata.UIEvents.Any()
          && apiMetadata.Types.Any()
          && apiMetadata.ErrorTypes.Any()
      );
    }

    [TestMethod]
    public void TestMessageDeserialization()
    {
      var process = Process.Start(
        new NvimProcessStartInfo(StartOption.Embed | StartOption.Headless)
      );

      var context = new SerializationContext();
      context.Serializers.Register(new NvimMessageSerializer(context));
      var serializer = MessagePackSerializer.Get<NvimMessage>(context);

      const string testString = "hello world";
      var request = new NvimRequest
      {
        MessageId = 42,
        Method = "nvim_eval",
        Arguments = new MessagePackObject(
          new MessagePackObject[] { $"'{testString}'" }
        ),
      };
      serializer.Pack(process.StandardInput.BaseStream, request);

      var response = (NvimResponse)
        serializer.Unpack(process.StandardOutput.BaseStream);

      Assert.IsTrue(
        response.MessageId == request.MessageId
          && response.Error == MessagePackObject.Nil
          && response.Result == testString
      );
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
      api.RegisterHandler(
        "client-call",
        args =>
        {
          CollectionAssert.AreEqual(new[] { 1L, 2L, 3L }, args);
          return new[] { 4, 5, 6 };
        }
      );
      var objects = await api.GetApiInfo();
      var channelID = (long)objects.First();
      await api.Command(
        $"let g:result = rpcrequest({channelID}, 'client-call', 1, 2, 3)"
      );
      var result = (object[])await api.GetVar("result");
      CollectionAssert.AreEqual(new[] { 4L, 5L, 6L }, result);
    }

    [TestMethod]
    [Timeout(15000)]
    public async Task TestNvimUIEvent()
    {
      const string testString = "hello_world";
      var titleSetTask = new TaskCompletionSource<bool>(
        TaskCreationOptions.RunContinuationsAsynchronously
      );
      var api = new NvimAPI();
      api.SetTitleEvent += (sender, args) =>
      {
        if (args.Title == testString)
        {
          titleSetTask.TrySetResult(true);
        }
      };
      var objects = await api.GetApiInfo();
      var channelID = (long)objects.First();
      var serverAddress = (string)
        await api.CallFunction(
          "serverstart",
          new object[] { System.Net.IPAddress.Loopback + ":" }
        );
      var sender = new NvimAPI(serverAddress);
      await sender.Command(
        $"call rpcnotify({channelID}, 'redraw', ['set_title', ['{testString}']])"
      );
      Assert.AreEqual(
        titleSetTask.Task,
        await Task.WhenAny(
          titleSetTask.Task,
          Task.Delay(TimeSpan.FromSeconds(5))
        )
      );
      Assert.IsTrue(await titleSetTask.Task);
    }

    [TestMethod]
    [Timeout(15000)]
    public void TestCreateFromStandardIO()
    {
      using var process = Process.Start(
        new ProcessStartInfo
        {
          FileName = "dotnet",
          Arguments = $"\"{typeof(Module.Program).Assembly.Location}\"",
          RedirectStandardInput = true,
          RedirectStandardOutput = true,
          UseShellExecute = false,
        }
      );

      try
      {
        var context = new SerializationContext();
        context.Serializers.Register(new NvimMessageSerializer(context));
        var serializer = MessagePackSerializer.Get<NvimMessage>(context);
        var request = new NvimRequest
        {
          MessageId = 42,
          Method = "example.add",
          Arguments = new MessagePackObject(new MessagePackObject[] { 1L, 2L }),
        };

        serializer.Pack(process.StandardInput.BaseStream, request);
        var response = (NvimResponse)
          serializer.Unpack(process.StandardOutput.BaseStream);

        Assert.AreEqual(request.MessageId, response.MessageId);
        Assert.AreEqual(MessagePackObject.Nil, response.Error);
        Assert.AreEqual(3L, response.Result);
      }
      finally
      {
        if (!process.HasExited)
        {
          process.Kill(true);
        }
      }
    }

    [TestMethod]
    public async Task TestTCPSocket()
    {
      var nvimStdio = new NvimAPI();
      var serverAddress = (string)
        await nvimStdio.CallFunction(
          "serverstart",
          new object[] { System.Net.IPAddress.Loopback + ":" }
        );

      var nvimTCPSocket = new NvimAPI(serverAddress);
      Assert.IsNotNull(await nvimTCPSocket.CommandOutput("version"));
    }

    [TestMethod]
    public async Task TestLocalSocket()
    {
      var nvimStdio = new NvimAPI();
      var serverAddress = (string)
        await nvimStdio.CallFunction("serverstart", new object[0]);

      var nvimLocalSocket = new NvimAPI(serverAddress);
      Assert.IsNotNull(await nvimLocalSocket.CommandOutput("version"));
    }

    [DataTestMethod]
    [DataRow(typeof(bool), true)]
    [DataRow(typeof(Boolean), true)]
    [DataRow(typeof(int), false)]
    [DataRow(typeof(Int32), false)]
    [DataRow(typeof(long), true)]
    [DataRow(typeof(Int64), true)]
    [DataRow(typeof(object[]), true)]
    [DataRow(typeof(long[]), true)]
    [DataRow(typeof(int[]), false)]
    [DataRow(typeof(IDictionary), true)]
    [DataRow(typeof(IDictionary<object, object>), true)]
    [DataRow(typeof(IDictionary<long, string>), true)]
    [DataRow(typeof(IDictionary<string, DateTime>), false)]
    [DataRow(typeof(IDictionary<Random, long>), false)]
    public void TestNvimTypeValidation(Type type, bool shouldBeValid)
    {
      Assert.AreEqual(shouldBeValid, NvimTypesMap.IsValidType(type));
    }

    [DataTestMethod]
    [DataRow("Boolean", "bool")]
    [DataRow("Array", "object[]")]
    [DataRow("Dict", "IDictionary")]
    [DataRow("DictAs(get_mode)", "IDictionary")]
    [DataRow("DictOf(Integer)", "IDictionary")]
    [DataRow("Dict(win_config)", "IDictionary")]
    [DataRow("Dictionary", "IDictionary")]
    [DataRow("ArrayOf(Float)", "double[]")]
    [DataRow("ArrayOf(Integer, 2)", "long[]")]
    [DataRow("ArrayOf(Buffer)", "NvimBuffer[]")]
    [DataRow(
      "ArrayOf(DictionaryOf(String, String))",
      "IDictionary<string, string>[]"
    )]
    [DataRow(
      "DictionaryOf(Integer, ArrayOf(String))",
      "IDictionary<long, string[]>"
    )]
    public void TestCSharpTypeConversion(string nvimType, string csharpType)
    {
      Assert.AreEqual(csharpType, NvimTypesMap.GetCSharpType(nvimType));
    }
  }
}
