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
   dotnet test --solution NvimClient.slnx
   ```

Plugin Example
--------------

The following is a complete, working RPC module. It adds an `:ExampleAdd`
command that prints `3`.

1. From the repository root, create a console project and reference
   `NvimClient.API`:
   ```sh
   dotnet new console --output NvimNetExample
   dotnet add NvimNetExample/NvimNetExample.csproj reference src/NvimClient.API/NvimClient.API.csproj
   cd NvimNetExample
   ```
2. Replace `Program.cs` with:
   ```csharp
   using System;
   using NvimClient.API;

   var nvim = NvimAPI.CreateFromStandardIO();
   nvim.RegisterHandler(
     "example.add",
     args => (long)args[0] + (long)args[1]
   );
   Console.Error.WriteLine("ready");
   nvim.WaitForDisconnect();
   ```
   Standard output is the MessagePack RPC stream. Write diagnostics only to
   standard error.
3. Publish the module:
   ```sh
   dotnet publish --output publish
   ```
4. Create `example.lua` in the project directory:
   ```lua
   local module_path = vim.fn.getcwd() .. "/publish/NvimNetExample.dll"
   local ready = false
   local channel = vim.fn.jobstart(
     { "dotnet", module_path },
     {
       rpc = true,
       on_stderr = function(_, data)
         if vim.tbl_contains(data, "ready") then
           ready = true
         end
       end,
     }
   )
   assert(channel > 0, "failed to start the .NET module")
   assert(vim.wait(5000, function()
     return ready
   end), "timed out waiting for the .NET module")

   vim.api.nvim_create_user_command("ExampleAdd", function()
     print(vim.fn.rpcrequest(channel, "example.add", 1, 2))
   end, {})
   ```
5. Start Neovim from the project directory and run `:ExampleAdd`:
   ```sh
   nvim --clean -u example.lua
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

    dotnet test --solution NvimClient.slnx

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
