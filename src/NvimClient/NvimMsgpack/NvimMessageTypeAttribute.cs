using System;

namespace NvimClient.NvimMsgpack
{
  [AttributeUsage(AttributeTargets.Class)]
  internal class NvimMessageTypeAttribute : Attribute
  {
    public NvimMessageTypeAttribute(long id) => Id = id;
    public long Id { get; }
  }
}
