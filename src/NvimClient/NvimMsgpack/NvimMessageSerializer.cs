using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using MsgPack;
using MsgPack.Serialization;
using NvimClient.NvimMsgpack.Models;

namespace NvimClient.NvimMsgpack
{
  public class NvimMessageSerializer : MessagePackSerializer<NvimMessage>
  {
    private static readonly Dictionary<long, Type> NvimMessageTypes;

    static NvimMessageSerializer()
    {
      long GetTypeId(Type type)
      {
        var attribute = type.GetCustomAttribute<NvimMessageTypeAttribute>();
        return attribute?.Id ??
               throw new TypeLoadException(
                 $"{type} does not have {nameof(NvimMessageTypeAttribute)}");
      }

      var baseType = typeof(NvimMessage);
      NvimMessageTypes = baseType.Assembly.GetTypes()
                                 .Where(type => type.IsSubclassOf(baseType))
                                 .ToDictionary(GetTypeId);
    }

    public NvimMessageSerializer(SerializationContext ctx) : base(ctx)
    {
    }

    protected override void PackToCore(Packer packer, NvimMessage message)
    {
      message.TypeId = message.GetType()
        .GetCustomAttribute<NvimMessageTypeAttribute>().Id;

      packer.PackObject(message);
      packer.Flush();
    }

    protected override NvimMessage UnpackFromCore(Unpacker unpacker)
    {
      var messagePackObject = unpacker.UnpackSubtreeData();
      if (!messagePackObject.IsArray)
      {
        throw new SerializationException("Expected array");
      }

      var messageTypeId = messagePackObject.AsEnumerable().First().AsInt64();
      if (!NvimMessageTypes.TryGetValue(messageTypeId, out var messageType))
      {
        throw new SerializationException($"Unknown message type (id {messageTypeId})");
      }

      return (NvimMessage) OwnerContext.GetSerializer(messageType)
        .FromMessagePackObject(messagePackObject);
    }
  }
}
