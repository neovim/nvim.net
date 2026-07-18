using System;
using NvimClient.API;

namespace NvimClient.Test.Module
{
  public static class Program
  {
    public static void Main()
    {
      var nvim = NvimAPI.CreateFromStandardIO();
      nvim.RegisterHandler(
        "example.add",
        args => (long)args[0] + (long)args[1]
      );
      Console.Error.WriteLine("ready");
      nvim.WaitForDisconnect();
    }
  }
}
