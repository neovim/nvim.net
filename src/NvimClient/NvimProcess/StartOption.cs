using System;

namespace NvimClient.NvimProcess
{
  [Flags]
  public enum StartOption
  {
    None = 0,

    Embed = 1,

    Headless = 2,

    ApiInfo = 4,
  }
}
