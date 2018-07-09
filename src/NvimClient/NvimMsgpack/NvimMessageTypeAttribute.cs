using System;

namespace NvimClient.NvimMsgpack
{
  [AttributeUsage(AttributeTargets.Class)]
  internal class NvimMessageTypeAttribute : Attribute
  {
    public NvimMessageTypeAttribute(byte id) => Id = id;
    public byte Id { get; }
  }
}
