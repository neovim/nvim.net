using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Nvim.Client;

/// <summary>
/// The Neovim RPC client.
/// </summary>
public partial interface INvimClient : IDisposable
{
  /// <summary>
  /// Gets a task that completes when the client reaches its terminal state.
  /// </summary>
  Task Completion { get; }

  /// <summary>
  /// Sends an RPC request and awaits its result.
  /// </summary>
  /// <param name="method">The Neovim RPC method.</param>
  /// <param name="arguments">The ordered RPC arguments.</param>
  /// <param name="cancellationToken">Cancels only the caller after a write begins.</param>
  /// <returns>The RPC response value.</returns>
  Task<NvimValue> RequestAsync(
    string method,
    IReadOnlyList<NvimValue> arguments,
    CancellationToken cancellationToken = default
  );

  /// <summary>
  /// Sends an RPC notification.
  /// </summary>
  /// <param name="method">The Neovim RPC method.</param>
  /// <param name="arguments">The ordered RPC arguments.</param>
  /// <param name="cancellationToken">Cancels before a frame write starts.</param>
  /// <returns>A task that completes after the frame has been written.</returns>
  Task NotifyAsync(
    string method,
    IReadOnlyList<NvimValue> arguments,
    CancellationToken cancellationToken = default
  );

  /// <summary>
  /// Registers the sole request handler for a method.
  /// </summary>
  /// <param name="method">The method owned by the handler.</param>
  /// <param name="handler">The asynchronous request handler.</param>
  /// <returns>A registration that removes and cancels the handler when disposed.</returns>
  NvimHandlerRegistration RegisterRequestHandler(
    string method,
    Func<IReadOnlyList<NvimValue>, CancellationToken, Task<NvimValue>> handler
  );

  /// <summary>
  /// Registers an additional notification handler for a method.
  /// </summary>
  /// <param name="method">The notification method to observe.</param>
  /// <param name="handler">The asynchronous notification handler.</param>
  /// <returns>A registration that removes and cancels the handler when disposed.</returns>
  NvimHandlerRegistration RegisterNotificationHandler(
    string method,
    Func<IReadOnlyList<NvimValue>, CancellationToken, Task> handler
  );

  /// <summary>
  /// Registers an additional handler for ordered UI redraw batches.
  /// </summary>
  /// <param name="handler">The asynchronous UI-event batch handler.</param>
  /// <returns>A registration that removes and cancels the handler when disposed.</returns>
  NvimHandlerRegistration RegisterUiEventHandler(
    Func<IReadOnlyList<NvimUiEvent>, CancellationToken, Task> handler
  );

  /// <summary>
  /// Logically stops the client and gracefully stops owned resources.
  /// </summary>
  /// <param name="cancellationToken">Cancels graceful owned-process shutdown.</param>
  /// <returns>A task that completes after owned shutdown work finishes.</returns>
  Task StopAsync(CancellationToken cancellationToken = default);

  /// <summary>
  /// Immediately stops owned resources and logically terminates the client.
  /// </summary>
  new void Dispose();
}
