using System;
using System.Buffers;
using System.IO;
using System.Numerics;
using MessagePack;

namespace Nvim.Client.Rpc;

internal static class NvimValueCodec
{
  private static readonly MessagePackSecurity Security =
    MessagePackSecurity.UntrustedData;

  internal static byte[] Encode(NvimValue value)
  {
    var buffer = new ArrayBufferWriter<byte>();
    var writer = new MessagePackWriter(buffer);
    Write(ref writer, value);
    writer.Flush();
    return buffer.WrittenSpan.ToArray();
  }

  internal static NvimValue Decode(ReadOnlySequence<byte> bytes)
  {
    try
    {
      var reader = new MessagePackReader(bytes);
      var value = Read(ref reader, 0);

      if (!reader.End)
        throw new NvimProtocolException("Trailing MessagePack data.");

      return value;
    }
    catch (NvimProtocolException)
    {
      throw;
    }
    catch (Exception exception)
      when (exception
          is MessagePackSerializationException
            or EndOfStreamException
            or InvalidOperationException
            or OverflowException
            or ArgumentException
      )
    {
      throw new NvimProtocolException("Invalid MessagePack value.", exception);
    }
  }

  internal static void Write(ref MessagePackWriter writer, NvimValue value)
  {
    switch (value)
    {
      case NvimNil:
        writer.WriteNil();
        break;
      case NvimBoolean boolean:
        writer.Write(boolean.Value);
        break;
      case NvimInteger integer:
        WriteInteger(ref writer, integer.Value);
        break;
      case NvimFloat floating:
        writer.Write(floating.Value);
        break;
      case NvimString text:
        writer.Write(text.Value);
        break;
      case NvimBinary binary:
        writer.Write(binary.Buffer);
        break;
      case NvimArray array:
        writer.WriteArrayHeader(array.Items.Count);
        foreach (var item in array.Items)
          Write(ref writer, item);
        break;
      case NvimMap map:
        writer.WriteMapHeader(map.Entries.Count);
        foreach (var entry in map.Entries)
        {
          Write(ref writer, entry.Key);
          Write(ref writer, entry.Value);
        }
        break;
      case NvimExtension extension:
        writer.WriteExtensionFormat(
          new ExtensionResult(extension.Tag, extension.Buffer)
        );
        break;
      default:
        throw new NvimProtocolException("Unsupported MessagePack value.");
    }
  }

  private static void WriteInteger(
    ref MessagePackWriter writer,
    BigInteger value
  )
  {
    if (value >= long.MinValue && value <= long.MaxValue)
      writer.Write((long)value);
    else
      writer.Write((ulong)value);
  }

  private static NvimValue Read(ref MessagePackReader reader, int depth)
  {
    if (depth >= Security.MaximumObjectGraphDepth)
      throw new NvimProtocolException(
        "MessagePack value exceeds the allowed depth."
      );

    switch (reader.NextMessagePackType)
    {
      case MessagePackType.Nil:
        reader.ReadNil();
        return new NvimNil();
      case MessagePackType.Boolean:
        return new NvimBoolean(reader.ReadBoolean());
      case MessagePackType.Integer:
        return IsSignedInteger(reader.NextCode)
          ? new NvimInteger(reader.ReadInt64())
          : new NvimInteger(reader.ReadUInt64());
      case MessagePackType.Float:
        return new NvimFloat(reader.ReadDouble());
      case MessagePackType.String:
        return new NvimString(
          reader.ReadString() ?? throw new NvimProtocolException("Null string.")
        );
      case MessagePackType.Binary:
        return new NvimBinary(
          reader.ReadBytes()?.ToArray()
            ?? throw new NvimProtocolException("Null binary.")
        );
      case MessagePackType.Array:
        var items = new NvimValue[reader.ReadArrayHeader()];
        for (var index = 0; index < items.Length; index++)
          items[index] = Read(ref reader, depth + 1);
        return new NvimArray(items);
      case MessagePackType.Map:
        var entries = new NvimMapEntry[reader.ReadMapHeader()];
        for (var index = 0; index < entries.Length; index++)
          entries[index] = new(
            Read(ref reader, depth + 1),
            Read(ref reader, depth + 1)
          );
        return new NvimMap(entries);
      case MessagePackType.Extension:
        var extension = reader.ReadExtensionFormat();
        return new NvimExtension(extension.TypeCode, extension.Data.ToArray());
      default:
        throw new NvimProtocolException("Unsupported MessagePack value.");
    }
  }

  private static bool IsSignedInteger(byte code) =>
    code is 0xd0 or 0xd1 or 0xd2 or 0xd3 || code >= 0xe0;
}
