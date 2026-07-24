using System;
using System.Collections.Generic;
using System.Numerics;

namespace Nvim.Client;

/// <summary>
/// Represents one MessagePack value exchanged with Neovim.
/// </summary>
public abstract record NvimValue
{
  private protected NvimValue() { }
}

/// <summary>
/// Represents the MessagePack nil value.
/// </summary>
public sealed record NvimNil : NvimValue;

/// <summary>
/// Represents a MessagePack Boolean value.
/// </summary>
public sealed record NvimBoolean(bool Value) : NvimValue;

/// <summary>
/// Represents a MessagePack integer in the protocol range.
/// </summary>
public sealed record NvimInteger : NvimValue
{
  /// <summary>
  /// Creates an integer value in the supported protocol range.
  /// </summary>
  public NvimInteger(BigInteger value)
  {
    if (value < long.MinValue || value > ulong.MaxValue)
      throw new ArgumentOutOfRangeException(nameof(value));

    Value = value;
  }

  /// <summary>
  /// Gets the integer payload.
  /// </summary>
  public BigInteger Value { get; }
}

/// <summary>
/// Represents a MessagePack floating-point value.
/// </summary>
public sealed record NvimFloat(double Value) : NvimValue;

/// <summary>
/// Represents a MessagePack string value.
/// </summary>
public sealed record NvimString : NvimValue
{
  /// <summary>
  /// Creates a string value.
  /// </summary>
  public NvimString(string value)
  {
    ArgumentNullException.ThrowIfNull(value);
    Value = value;
  }

  /// <summary>
  /// Gets the string payload.
  /// </summary>
  public string Value { get; }
}

/// <summary>
/// Represents an immutable MessagePack binary value.
/// </summary>
public sealed record NvimBinary : NvimValue
{
  private readonly byte[] _buffer;

  /// <summary>
  /// Creates a binary value from a snapshot of the supplied bytes.
  /// </summary>
  public NvimBinary(IEnumerable<byte> bytes)
  {
    ArgumentNullException.ThrowIfNull(bytes);

    _buffer = [.. bytes];
    Bytes = Array.AsReadOnly(_buffer);
  }

  /// <summary>
  /// Gets the binary payload.
  /// </summary>
  public IReadOnlyList<byte> Bytes { get; }

  internal byte[] Buffer => _buffer;
}

/// <summary>
/// Represents an immutable MessagePack array value.
/// </summary>
public sealed record NvimArray : NvimValue
{
  /// <summary>
  /// Creates an array value from a snapshot of the supplied items.
  /// </summary>
  public NvimArray(IEnumerable<NvimValue> items)
  {
    ArgumentNullException.ThrowIfNull(items);
    Items = Array.AsReadOnly<NvimValue>([.. items]);
  }

  /// <summary>
  /// Gets the array items.
  /// </summary>
  public IReadOnlyList<NvimValue> Items { get; }
}

/// <summary>
/// Represents one key-value pair in a MessagePack map.
/// </summary>
public sealed record NvimMapEntry(NvimValue Key, NvimValue Value);

/// <summary>
/// Represents an immutable MessagePack map value.
/// </summary>
public sealed record NvimMap : NvimValue
{
  /// <summary>
  /// Creates a map value from a snapshot of the supplied entries.
  /// </summary>
  public NvimMap(IEnumerable<NvimMapEntry> entries)
  {
    ArgumentNullException.ThrowIfNull(entries);
    Entries = Array.AsReadOnly<NvimMapEntry>([.. entries]);
  }

  /// <summary>
  /// Gets the map entries.
  /// </summary>
  public IReadOnlyList<NvimMapEntry> Entries { get; }
}

/// <summary>
/// Represents an immutable MessagePack extension value.
/// </summary>
public sealed record NvimExtension : NvimValue
{
  private readonly byte[] _buffer;

  /// <summary>
  /// Creates an extension value from a tag and byte snapshot.
  /// </summary>
  public NvimExtension(sbyte tag, IEnumerable<byte> data)
  {
    ArgumentNullException.ThrowIfNull(data);

    Tag = tag;
    _buffer = [.. data];
    Data = Array.AsReadOnly(_buffer);
  }

  /// <summary>
  /// Gets the MessagePack extension tag.
  /// </summary>
  public sbyte Tag { get; }

  /// <summary>
  /// Gets the extension payload.
  /// </summary>
  public IReadOnlyList<byte> Data { get; }

  internal byte[] Buffer => _buffer;
}
