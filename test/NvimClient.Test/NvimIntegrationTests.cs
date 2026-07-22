using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Nvim.Client;
using Xunit;

namespace Nvim.Client.Test;

public sealed class NvimIntegrationTests
{
  [Fact]
  public async Task EmbeddedClientRoundTripsRequestsAndHandlers()
  {
    using var client = Nvim.Client.NvimClient.Start();
    var cancellationToken = TestContext.Current.CancellationToken;

    var evaluated = await client.EvalAsync(
      new NvimString("2 + 2"),
      cancellationToken
    );
    Assert.Equal(new NvimInteger(4), evaluated);

    using var registration = client.RegisterRequestHandler(
      "example.add",
      (arguments, _) =>
        Task.FromResult<NvimValue>(
          new NvimInteger(
            ((NvimInteger)arguments[0]).Value
              + ((NvimInteger)arguments[1]).Value
          )
        )
    );
    var apiInfo = await client.GetApiInfoAsync(cancellationToken);
    var channel = Assert.IsType<NvimInteger>(apiInfo[0]);
    var handled = await client.ExecLuaAsync(
      new NvimString("return vim.fn.rpcrequest(..., 'example.add', 1, 2)"),
      [channel],
      cancellationToken
    );

    Assert.Equal(new NvimInteger(3), handled);

    var notification = new TaskCompletionSource<IReadOnlyList<NvimValue>>(
      TaskCreationOptions.RunContinuationsAsynchronously
    );
    using var notificationRegistration = client.RegisterNotificationHandler(
      "example.notify",
      (arguments, _) =>
      {
        notification.TrySetResult(arguments);
        return Task.CompletedTask;
      }
    );

    await client.ExecLuaAsync(
      new NvimString("vim.rpcnotify(..., 'example.notify', 4, 5)"),
      [channel],
      cancellationToken
    );
    var notificationArguments = await notification.Task.WaitAsync(
      cancellationToken
    );

    Assert.Collection(
      notificationArguments,
      value => Assert.Equal(new NvimInteger(4), value),
      value => Assert.Equal(new NvimInteger(5), value)
    );

    const string title = "integration-title";
    var redraw = new TaskCompletionSource<IReadOnlyList<NvimUiEvent>>(
      TaskCreationOptions.RunContinuationsAsynchronously
    );
    using var uiRegistration = client.RegisterUiEventHandler(
      (events, _) =>
      {
        redraw.TrySetResult(events);
        return Task.CompletedTask;
      }
    );

    await client.ExecLuaAsync(
      new NvimString(
        $"vim.rpcnotify(..., 'redraw', {{'set_title', {{'{title}'}}}})"
      ),
      [channel],
      cancellationToken
    );
    var uiEvent = Assert.Single(await redraw.Task.WaitAsync(cancellationToken));

    Assert.Equal(new SetTitle(new NvimString(title)), uiEvent);

    const string errorMessage = "expected handler failure";
    using var failureRegistration = client.RegisterRequestHandler(
      "example.fail",
      (_, _) => throw new InvalidOperationException(errorMessage)
    );
    var failure = Assert.IsType<NvimArray>(
      await client.ExecLuaAsync(
        new NvimString("return { pcall(vim.rpcrequest, ..., 'example.fail') }"),
        [channel],
        cancellationToken
      )
    );

    Assert.Collection(
      failure.Value,
      value => Assert.False(Assert.IsType<NvimBoolean>(value).Value),
      value =>
        Assert.Contains(errorMessage, Assert.IsType<NvimString>(value).Value)
    );

    await client.StopAsync(cancellationToken);
  }

  [Fact]
  public async Task TcpAddressConnects()
  {
    using var listener = new TcpListener(IPAddress.Loopback, 0);
    listener.Start();
    var endpoint = (IPEndPoint)listener.LocalEndpoint;
    var accepted = listener.AcceptTcpClientAsync(
      TestContext.Current.CancellationToken
    );
    using var client = await Nvim.Client.NvimClient.ConnectAsync(
      $"127.0.0.1:{endpoint.Port}",
      TestContext.Current.CancellationToken
    );
    using var server = await accepted;

    await client.StopAsync(TestContext.Current.CancellationToken);
    Assert.True(client.Completion.IsCompletedSuccessfully);
  }

  [Fact]
  public async Task UnixAddressConnectsOnUnix()
  {
    if (OperatingSystem.IsWindows())
      return;

    var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".sock");
    using var listener = new Socket(
      AddressFamily.Unix,
      SocketType.Stream,
      ProtocolType.Unspecified
    );

    try
    {
      listener.Bind(new UnixDomainSocketEndPoint(path));
      listener.Listen(1);
      var accepted = listener.AcceptAsync(
        TestContext.Current.CancellationToken
      );
      using var client = await Nvim.Client.NvimClient.ConnectAsync(
        path,
        TestContext.Current.CancellationToken
      );
      using var server = await accepted;

      await client.StopAsync(TestContext.Current.CancellationToken);
      Assert.True(client.Completion.IsCompletedSuccessfully);
    }
    finally
    {
      if (File.Exists(path))
        File.Delete(path);
    }
  }

  [Fact]
  public async Task StandardIoModuleHandlesInboundRequest()
  {
    var module = typeof(global::NvimClient.Test.Module.Program)
      .Assembly
      .Location;
    using var process = Process.Start(
      new ProcessStartInfo("dotnet", $"\"{module}\"")
      {
        RedirectStandardInput = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
      }
    )!;
    using var client = Nvim.Client.NvimClient.Attach(process);

    var value = await client.RequestAsync(
      "example.add",
      [new NvimInteger(1), new NvimInteger(2)],
      TestContext.Current.CancellationToken
    );

    Assert.Equal(new NvimInteger(3), value);
    process.StandardInput.Close();
    await process.WaitForExitAsync(TestContext.Current.CancellationToken);
  }
}
