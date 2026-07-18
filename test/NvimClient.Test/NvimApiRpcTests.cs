using System;
using System.Linq;
using System.Threading.Tasks;
using NvimClient.API;
using Xunit;

namespace NvimClient.Test
{
  [Collection("Nvim")]
  public class NvimApiRpcTests
  {
    [Fact]
    public async Task UnhandledRequestEventRespondsToNvim()
    {
      NvimUnhandledRequestEventArgs receivedRequest = null;
      var api = new NvimAPI();
      api.OnUnhandledRequest += (_, request) =>
      {
        receivedRequest = request;
        request.SendResponse("handled");
      };
      var channelId = await GetChannelId(api);

      await api.Command(
        $"let g:unhandled_result = rpcrequest({channelId}, 'client-unhandled', 1, 'two')"
      );
      var result = await api.GetVar("unhandled_result");

      Assert.Equal("handled", result);
      Assert.NotNull(receivedRequest);
      Assert.Equal("client-unhandled", receivedRequest.MethodName);
      Assert.Equal(new object[] { 1L, "two" }, receivedRequest.Arguments);
    }

    [Fact(Timeout = 15000)]
    public async Task UnhandledNotificationRaisesEvent()
    {
      var receivedNotification =
        new TaskCompletionSource<NvimUnhandledNotificationEventArgs>(
          TaskCreationOptions.RunContinuationsAsynchronously
        );
      var api = new NvimAPI();
      api.OnUnhandledNotification += (_, notification) =>
      {
        if (notification.MethodName == "client-unhandled")
        {
          receivedNotification.TrySetResult(notification);
        }
      };
      var channelId = await GetChannelId(api);
      var sender = await CreateSender(api);

      await sender.Command(
        $"call rpcnotify({channelId}, 'client-unhandled', 1, 'two')"
      );
      var completedTask = await Task.WhenAny(
        receivedNotification.Task,
        Task.Delay(
          TimeSpan.FromSeconds(5),
          TestContext.Current.CancellationToken
        )
      );

      Assert.Same(receivedNotification.Task, completedTask);
      var notification = await receivedNotification.Task;
      Assert.Equal("client-unhandled", notification.MethodName);
      Assert.Equal(new object[] { 1L, "two" }, notification.Arguments);
    }

    [Fact]
    public async Task HandlerExceptionReturnsRpcError()
    {
      const string errorMessage = "expected handler failure";
      var api = new NvimAPI();

      object ThrowExpectedError(object[] _) =>
        throw new InvalidOperationException(errorMessage);

      api.RegisterHandler("client-error", ThrowExpectedError);
      var channelId = await GetChannelId(api);

      var result = (object[])
        await api.ExecLua(
          "return { pcall(vim.rpcrequest, ..., 'client-error') }",
          new object[] { channelId }
        );

      Assert.False((bool)result[0]);
      Assert.Contains(errorMessage, (string)result[1]);
    }

    private static async Task<NvimAPI> CreateSender(NvimAPI receiver)
    {
      var serverAddress = (string)
        await receiver.CallFunction(
          "serverstart",
          new object[] { System.Net.IPAddress.Loopback + ":" }
        );
      return new NvimAPI(serverAddress);
    }

    private static async Task<long> GetChannelId(NvimAPI api) =>
      (long)(await api.GetApiInfo()).First();
  }
}
