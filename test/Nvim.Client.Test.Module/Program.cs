using System;

namespace Nvim.Client.Test.Module;

public static class Program
{
  public static void Main()
  {
    using var nvim = NvimClient.AttachToStandardIO();
    using var registration = nvim.RegisterRequestHandler(
      "example.add",
      (args, _) =>
        System.Threading.Tasks.Task.FromResult<NvimValue>(
          new NvimInteger(
            ((NvimInteger)args[0]).Value + ((NvimInteger)args[1]).Value
          )
        )
    );
    Console.Error.WriteLine("ready");
    nvim.Completion.GetAwaiter().GetResult();
  }
}
