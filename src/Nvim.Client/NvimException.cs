using System;

namespace Nvim.Client;

/// <summary>
/// Represents an error reported by the Neovim client.
/// </summary>
public abstract class NvimException(
  string message,
  Exception? innerException = null
) : Exception(message, innerException);

/// <summary>
/// Represents an error returned by a remote Neovim RPC response.
/// </summary>
/// <param name="message">The remote error message.</param>
public sealed class NvimRpcException(string message) : NvimException(message);

/// <summary>
/// Represents terminal transport or owned-process failure.
/// </summary>
/// <param name="message">The connection failure message.</param>
/// <param name="innerException">The underlying transport failure.</param>
public sealed class NvimConnectionException(
  string message,
  Exception? innerException = null
) : NvimException(message, innerException);

/// <summary>
/// Represents malformed MessagePack or RPC protocol data.
/// </summary>
/// <param name="message">The protocol failure message.</param>
/// <param name="innerException">The underlying decoding failure.</param>
public sealed class NvimProtocolException(
  string message,
  Exception? innerException = null
) : NvimException(message, innerException);
