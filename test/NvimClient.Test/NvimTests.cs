using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MsgPack;
using MsgPack.Serialization;
using NvimClient.API;
using NvimClient.NvimMsgpack;
using NvimClient.NvimMsgpack.Models;
using NvimClient.NvimProcess;
using Xunit;
using TestModule = NvimClient.Test.Module.Program;

namespace NvimClient.Test
{
  public class NvimTests
  {
    [Fact]
    public void VersionArgumentReportsNvimVersion()
    {
      var process = NvimProcess.NvimProcess.Start(
        new ProcessStartInfo
        {
          Arguments = "--version",
          RedirectStandardOutput = true,
        }
      );
      process.WaitForExit();

      Assert.Contains("NVIM", process.StandardOutput.ReadToEnd());
      Assert.Equal(0, process.ExitCode);
    }

    [Fact]
    public void ApiMetadataDeserializes()
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

      Assert.NotNull(apiMetadata.Version);
      Assert.NotEmpty(apiMetadata.Functions);
      Assert.NotEmpty(apiMetadata.UIEvents);
      Assert.NotEmpty(apiMetadata.Types);
      Assert.NotEmpty(apiMetadata.ErrorTypes);
    }

    [Fact]
    public void EmbeddedNvimRequestReturnsExpectedResponse()
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

      Assert.Equal(request.MessageId, response.MessageId);
      Assert.Equal(MessagePackObject.Nil, response.Error);
      Assert.Equal(testString, response.Result);
    }

    [Fact]
    public async Task AsyncApiCallReturnsResult()
    {
      var api = new NvimAPI();

      var result = await api.Eval("2 + 2");

      Assert.Equal(4L, result);
    }

    [Fact]
    public async Task RegisteredHandlerRepliesToNvimRequest()
    {
      var api = new NvimAPI();
      api.RegisterHandler(
        "client-call",
        args =>
        {
          Assert.Equal(new object[] { 1L, 2L, 3L }, args);
          return new[] { 4, 5, 6 };
        }
      );
      var objects = await api.GetApiInfo();
      var channelID = (long)objects.First();

      await api.Command(
        $"let g:result = rpcrequest({channelID}, 'client-call', 1, 2, 3)"
      );
      var result = (object[])await api.GetVar("result");

      Assert.Equal(new object[] { 4L, 5L, 6L }, result);
    }

    [Fact(Timeout = 15000)]
    public async Task RedrawNotificationRaisesTitleEvent()
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

      Assert.Same(
        titleSetTask.Task,
        await Task.WhenAny(
          titleSetTask.Task,
          Task.Delay(
            TimeSpan.FromSeconds(5),
            TestContext.Current.CancellationToken
          )
        )
      );
      Assert.True(await titleSetTask.Task);
    }

    [Fact(Timeout = 15000)]
    public void StandardIoModuleRepliesToRequest()
    {
      using var process = Process.Start(
        new ProcessStartInfo
        {
          FileName = "dotnet",
          Arguments = $"\"{typeof(TestModule).Assembly.Location}\"",
          RedirectStandardInput = true,
          RedirectStandardOutput = true,
          RedirectStandardError = true,
          UseShellExecute = false,
        }
      );

      try
      {
        Assert.Equal("ready", process.StandardError.ReadLine());

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

        Assert.Equal(request.MessageId, response.MessageId);
        Assert.Equal(MessagePackObject.Nil, response.Error);
        Assert.Equal(3L, response.Result);
      }
      finally
      {
        if (!process.HasExited)
        {
          process.Kill(true);
        }
      }
    }

    [Fact(Timeout = 15000)]
    public async Task NvimRpcJobCallsStandardIoModule()
    {
      var scriptPath = Path.GetTempFileName();
      await File.WriteAllTextAsync(
        scriptPath,
        """
        local module_path = assert(arg[1], "missing .NET module path")
        local channel
        local ready = false

        local ok, error_message = xpcall(function()
          channel = vim.fn.jobstart(
            { "dotnet", module_path },
            {
              rpc = true,
              on_stderr = function(_, data)
                if vim.tbl_contains(data, "ready") then
                  ready = true
                end
              end,
            }
          )
          assert(channel > 0, "failed to start the .NET module")
          assert(
            vim.wait(5000, function()
              return ready
            end),
            "timed out waiting for the .NET module"
          )
          assert(
            vim.fn.rpcrequest(channel, "example.add", 1, 2) == 3,
            "example.add returned an unexpected result"
          )
        end, debug.traceback)

        if channel and channel > 0 then
          vim.fn.jobstop(channel)
        end

        if not ok then
          vim.api.nvim_err_writeln(error_message)
          vim.cmd("cquit")
        end

        vim.cmd("quitall")
        """,
        TestContext.Current.CancellationToken
      );

      var startInfo = new ProcessStartInfo
      {
        FileName = "nvim",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
      };
      startInfo.ArgumentList.Add("--clean");
      startInfo.ArgumentList.Add("--headless");
      startInfo.ArgumentList.Add("-l");
      startInfo.ArgumentList.Add(scriptPath);
      startInfo.ArgumentList.Add(typeof(TestModule).Assembly.Location);

      using var process = Process.Start(startInfo);

      try
      {
        var standardOutputTask = process.StandardOutput.ReadToEndAsync(
          TestContext.Current.CancellationToken
        );
        var standardErrorTask = process.StandardError.ReadToEndAsync(
          TestContext.Current.CancellationToken
        );

        await process.WaitForExitAsync(TestContext.Current.CancellationToken);

        var standardOutput = await standardOutputTask;
        var standardError = await standardErrorTask;
        Assert.True(
          process.ExitCode == 0,
          $"Neovim failed.\nstdout:\n{standardOutput}\nstderr:\n{standardError}"
        );
      }
      finally
      {
        if (!process.HasExited)
        {
          process.Kill(true);
        }

        File.Delete(scriptPath);
      }
    }

    [Fact]
    public async Task TcpClientCallsNvim()
    {
      var nvimStdio = new NvimAPI();
      var serverAddress = (string)
        await nvimStdio.CallFunction(
          "serverstart",
          new object[] { System.Net.IPAddress.Loopback + ":" }
        );
      var nvimTCPSocket = new NvimAPI(serverAddress);

      var output = await nvimTCPSocket.CommandOutput("version");

      Assert.NotNull(output);
    }

    [Fact]
    public async Task LocalSocketClientCallsNvim()
    {
      var nvimStdio = new NvimAPI();
      var serverAddress = (string)
        await nvimStdio.CallFunction("serverstart", new object[0]);
      var nvimLocalSocket = new NvimAPI(serverAddress);

      var output = await nvimLocalSocket.CommandOutput("version");

      Assert.NotNull(output);
    }
  }
}
