using System;

namespace NvimClient.NvimMsgpack
{
  internal class NvimMessageTypeAttribute : Attribute
  {
    public NvimMessageTypeAttribute(long id) => Id = id;
    public long Id { get; }
  }
}
