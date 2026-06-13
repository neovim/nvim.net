using System.Collections.Generic;
using System.Linq;

namespace NvimClient.NvimProcess
{
  internal readonly record struct StartOptionFlag(
    StartOption Option,
    string Flag
  );

  internal static class NvimStartOptions
  {
    private static readonly StartOptionFlag[] Flags =
    [
      new(StartOption.Embed, "--embed"),
      new(StartOption.Headless, "--headless"),
      new(StartOption.ApiInfo, "--api-info"),
    ];

    public static IEnumerable<string> ToFlags(StartOption startOptions) =>
      Flags
        .Where(optionFlag => startOptions.HasFlag(optionFlag.Option))
        .Select(optionFlag => optionFlag.Flag);
  }
}
