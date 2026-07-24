using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Nvim.Client.Rpc;

namespace Nvim.Client;

/// <summary>
/// Provides a lifecycle-safe MessagePack-RPC client for Neovim.
/// </summary>
public sealed partial class NvimClient : RpcSession, INvimClient
{
  private NvimClient(RpcTransport transport)
    : base(transport) { }

  /// <summary>
  /// Attaches to one borrowed duplex RPC stream.
  /// </summary>
  /// <param name="stream">The stream used for RPC reads and writes.</param>
  /// <returns>A client that does not own <paramref name="stream" />.</returns>
  public static NvimClient Attach(Stream stream) =>
    new(RpcTransport.Borrow(stream));

  /// <summary>
  /// Attaches to borrowed RPC read and write streams.
  /// </summary>
  /// <param name="readStream">The stream used for inbound RPC frames.</param>
  /// <param name="writeStream">The stream used for outbound RPC frames.</param>
  /// <returns>A client that does not own either supplied stream.</returns>
  public static NvimClient Attach(Stream readStream, Stream writeStream) =>
    new(RpcTransport.Borrow(readStream, writeStream));

  /// <summary>
  /// Attaches to the borrowed standard streams of a process.
  /// </summary>
  /// <param name="process">The process whose redirected streams carry RPC.</param>
  /// <returns>A client that does not own or terminate <paramref name="process" />.</returns>
  public static NvimClient Attach(Process process) =>
    new(RpcTransport.Borrow(process));

  /// <summary>
  /// Attaches to borrowed standard input and output.
  /// </summary>
  /// <returns>A client that does not close the process standard streams.</returns>
  public static NvimClient AttachToStandardIO() =>
    new(RpcTransport.BorrowStandardIO());

  /// <summary>
  /// Starts an owned embedded headless Neovim process.
  /// </summary>
  /// <param name="executable">The Neovim executable to launch.</param>
  /// <returns>A client that owns the process and its redirected streams.</returns>
  public static NvimClient Start(string executable = "nvim") =>
    new(RpcTransport.StartProcess(executable));

  /// <summary>
  /// Connects to an owned TCP, Unix-socket, or named-pipe endpoint.
  /// </summary>
  /// <param name="address">A selected Neovim endpoint address.</param>
  /// <param name="cancellationToken">Cancels connection establishment.</param>
  /// <returns>The connected client.</returns>
  public static async Task<NvimClient> ConnectAsync(
    string address,
    CancellationToken cancellationToken = default
  ) =>
    new(
      await RpcTransport
        .ConnectAsync(address, cancellationToken)
        .ConfigureAwait(false)
    );
}
