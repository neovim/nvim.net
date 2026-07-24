using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Nvim.Client.Rpc;

internal sealed class RpcTransport : IDisposable
{
  private readonly bool _ownsResources;
  private readonly Process? _ownedProcess;

  private RpcTransport(
    Stream input,
    Stream output,
    bool ownsResources,
    Process? ownedProcess = null
  )
  {
    ArgumentNullException.ThrowIfNull(input);
    ArgumentNullException.ThrowIfNull(output);

    Input = input;
    Output = output;
    _ownsResources = ownsResources;
    _ownedProcess = ownedProcess;
  }

  internal Stream Input { get; }
  internal Stream Output { get; }

  internal static RpcTransport Borrow(Stream stream) => Borrow(stream, stream);

  internal static RpcTransport Borrow(Stream input, Stream output) =>
    new(input, output, ownsResources: false);

  internal static RpcTransport Borrow(Process process)
  {
    ArgumentNullException.ThrowIfNull(process);

    return Borrow(
      process.StandardOutput.BaseStream,
      process.StandardInput.BaseStream
    );
  }

  internal static RpcTransport BorrowStandardIO() =>
    Borrow(Console.OpenStandardInput(), Console.OpenStandardOutput());

  internal static RpcTransport StartProcess(string executable)
  {
    ArgumentException.ThrowIfNullOrWhiteSpace(executable);

    try
    {
      var startInfo = new ProcessStartInfo(executable)
      {
        RedirectStandardInput = true,
        RedirectStandardOutput = true,
        UseShellExecute = false,
      };
      startInfo.ArgumentList.Add("--embed");
      startInfo.ArgumentList.Add("--headless");

      var process =
        Process.Start(startInfo)
        ?? throw new NvimConnectionException($"Unable to start {executable}.");

      return new(
        process.StandardOutput.BaseStream,
        process.StandardInput.BaseStream,
        ownsResources: true,
        ownedProcess: process
      );
    }
    catch (Exception exception) when (exception is not NvimConnectionException)
    {
      throw new NvimConnectionException(
        $"Unable to start {executable}.",
        exception
      );
    }
  }

  internal static Task<RpcTransport> ConnectAsync(
    string address,
    CancellationToken cancellationToken = default
  )
  {
    ArgumentException.ThrowIfNullOrWhiteSpace(address);

    if (address.StartsWith(@"\\", StringComparison.Ordinal))
      return ConnectNamedPipeAsync(address, cancellationToken);

    return Path.IsPathRooted(address)
      ? ConnectUnixAsync(address, cancellationToken)
      : ConnectTcpAsync(address, cancellationToken);
  }

  internal async Task StopAsync(CancellationToken cancellationToken = default)
  {
    if (!_ownsResources)
      return;

    Output.Dispose();

    var process = _ownedProcess;
    if (process is null)
      return;

    try
    {
      await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
    }
    catch (OperationCanceledException)
    {
      TryKill(process);
      throw;
    }
  }

  public void Dispose()
  {
    if (!_ownsResources)
      return;

    Input.Dispose();

    if (!ReferenceEquals(Input, Output))
      Output.Dispose();

    var process = _ownedProcess;
    if (process is null)
      return;

    TryKill(process);
    process.Dispose();
  }

  private static async Task<RpcTransport> ConnectTcpAsync(
    string address,
    CancellationToken cancellationToken
  )
  {
    var (host, port) = ParseTcpAddress(address);
    var client = new TcpClient();

    try
    {
      await client
        .ConnectAsync(host, port, cancellationToken)
        .ConfigureAwait(false);

      return Own(client.GetStream());
    }
    catch (OperationCanceledException)
    {
      client.Dispose();
      throw;
    }
    catch (Exception exception)
    {
      client.Dispose();
      throw ConnectFailure(address, exception);
    }
  }

  private static async Task<RpcTransport> ConnectUnixAsync(
    string path,
    CancellationToken cancellationToken
  )
  {
    if (OperatingSystem.IsWindows())
      throw new PlatformNotSupportedException(
        "Unix sockets are not supported on Windows."
      );

    var socket = new Socket(
      AddressFamily.Unix,
      SocketType.Stream,
      ProtocolType.Unspecified
    );

    try
    {
      await socket
        .ConnectAsync(new UnixDomainSocketEndPoint(path), cancellationToken)
        .ConfigureAwait(false);

      return Own(new NetworkStream(socket, ownsSocket: true));
    }
    catch (OperationCanceledException)
    {
      socket.Dispose();
      throw;
    }
    catch (Exception exception)
    {
      socket.Dispose();
      throw ConnectFailure(path, exception);
    }
  }

  private static async Task<RpcTransport> ConnectNamedPipeAsync(
    string address,
    CancellationToken cancellationToken
  )
  {
    var (server, pipeName) = ParseNamedPipeAddress(address);

    if (!OperatingSystem.IsWindows())
      throw new PlatformNotSupportedException(
        "Named pipes are only supported on Windows."
      );

    var pipe = new NamedPipeClientStream(
      server,
      pipeName,
      PipeDirection.InOut,
      PipeOptions.Asynchronous
    );

    try
    {
      await pipe.ConnectAsync(cancellationToken).ConfigureAwait(false);
      return Own(pipe);
    }
    catch (OperationCanceledException)
    {
      pipe.Dispose();
      throw;
    }
    catch (Exception exception)
    {
      pipe.Dispose();
      throw ConnectFailure(address, exception);
    }
  }

  private static (string Host, int Port) ParseTcpAddress(string address)
  {
    if (
      !Uri.TryCreate($"tcp://{address}", UriKind.Absolute, out var endpoint)
      || string.IsNullOrWhiteSpace(endpoint.DnsSafeHost)
      || endpoint.Port is < 1 or > 65535
      || endpoint.UserInfo.Length != 0
      || endpoint.AbsolutePath is not ("" or "/")
      || endpoint.Query.Length != 0
      || endpoint.Fragment.Length != 0
    )
      throw new ArgumentException(
        "Expected host:port or [IPv6]:port.",
        nameof(address)
      );

    return (endpoint.DnsSafeHost, endpoint.Port);
  }

  private static (string Server, string PipeName) ParseNamedPipeAddress(
    string address
  )
  {
    var parts = address[2..].Split('\\');

    if (
      parts is not [var server, var marker, var pipeName]
      || !marker.Equals("pipe", StringComparison.OrdinalIgnoreCase)
      || string.IsNullOrWhiteSpace(server)
      || string.IsNullOrWhiteSpace(pipeName)
    )
      throw new ArgumentException(
        "Expected canonical named-pipe address.",
        nameof(address)
      );

    return (server, pipeName);
  }

  private static RpcTransport Own(Stream stream) =>
    new(stream, stream, ownsResources: true);

  private static void TryKill(Process process)
  {
    try
    {
      if (!process.HasExited)
        process.Kill();
    }
    catch (InvalidOperationException) { }
  }

  private static NvimConnectionException ConnectFailure(
    string address,
    Exception exception
  ) => new($"Unable to connect to {address}.", exception);
}
