Nvim.NET
========

[![Test Workflow Status](https://github.com/neovim/nvim.net/actions/workflows/test.yml/badge.svg)](https://github.com/neovim/nvim.net/actions/workflows/test.yml)

.NET client for [Neovim](https://github.com/neovim/neovim)

Quickstart for Linux
--------------------

1. [Install dotnet](https://www.microsoft.com/net/download/linux-package-manager/ubuntu16-04/sdk-current)
2. Clone the Nvim .NET client (this repo):
   ```
   git clone https://github.com/neovim/nvim.net
   ```
3. Run the tests, to check that everything is working:
   ```
   dotnet test test/NvimClient.Test/NvimClient.Test.csproj
   ```

Neovim RPC module
-----------------

Install the `NvimClient.API` package in an executable project. Its standard
output is the MessagePack RPC stream: write diagnostics only to standard error.

```csharp
using System;
using NvimClient.API;

internal static class Program
{
  private static void Main()
  {
    var nvim = NvimAPI.CreateFromStandardIO();
    nvim.RegisterHandler(
      "example.add",
      args => (long)args[0] + (long)args[1]
    );
    nvim.RegisterHandler(
      "example.buf-enter",
      args => Console.Error.WriteLine($"Opened {args[0]}")
    );
    nvim.WaitForDisconnect();
  }
}
```

Start the module from Lua and own the Neovim bindings there:

```lua
local channel = vim.fn.jobstart(
  { "dotnet", "run", "--project", "/path/to/MyModule.csproj" },
  { rpc = true }
)

vim.api.nvim_create_user_command("ExampleAdd", function()
  print(vim.fn.rpcrequest(channel, "example.add", 1, 2))
end, {})

vim.api.nvim_create_autocmd("BufEnter", {
  pattern = "*.cs",
  callback = function(args)
    vim.fn.rpcnotify(channel, "example.buf-enter", args.file)
  end,
})
```

Build
-----

    dotnet build

Generate API
-----

The API generator takes two arguments: the output path of the generated C# class
file and the path to the Neovim source directory. `nvim` and `doxygen` must be
in the `PATH`.

     dotnet run --project src/NvimClient.APIGenerator/NvimClient.APIGenerator.csproj
       src/NvimClient.API/NvimAPI.generated.cs
       /usr/local/src/neovim/

Test
----

Run all tests (`nvim` must be in the `PATH`):

    dotnet test test/NvimClient.Test/NvimClient.Test.csproj

Run only the `TestMessageDeserialization` test:

    dotnet test --filter TestMessageDeserialization test/NvimClient.Test/NvimClient.Test.csproj

Release
-----

1. Update version as necessary in `src/NvimClient.API/NvimClient.API.csproj`.
2. Ensure the repository secret `NUGET_API_KEY` is set to a nuget.org API key
   with package push permission. No extra GitHub Packages secret is required;
   the workflow uses the built-in `GITHUB_TOKEN`.
3. Run the [publish workflow](https://github.com/neovim/nvim.net/actions/workflows/publish.yml)
   from the branch or tag to release. Leave `package-version` empty to use the
   project version, or set it to override `PackageVersion` for that run.
4. The workflow builds, packs, uploads the `.nupkg` and `.snupkg` artifacts,
   then pushes the `.nupkg` packages to GitHub Packages and nuget.org.
