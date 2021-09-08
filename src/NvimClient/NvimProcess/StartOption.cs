using System;

namespace NvimClient.NvimProcess
{
  [Flags]
  public enum StartOption
  {
    None = 0,
    [Argument("--embed")] Embed = 1,
    [Argument("--headless")] Headless = 2,
    [Argument("--api-info")] ApiInfo = 4
  }
}
