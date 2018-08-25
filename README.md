Nvim.NET
========

[![AppVeyor Build status](https://ci.appveyor.com/api/projects/status/8c2dw5gc2knhvqjc/branch/master?svg=true)](https://ci.appveyor.com/project/neovim/nvim-net/branch/master)
[![Travis Build Status](https://travis-ci.org/neovim/nvim.net.svg?branch=master)](https://travis-ci.org/neovim/nvim.net)

.NET client for [Neovim](https://github.com/neovim/neovim)

Plugin Host Install
------------------------

Using [vim-plug](https://github.com/junegunn/vim-plug):

    Plug 'neovim/nvim.net'

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

Plugin Development with C#
--------------------------

1. Create a new solution and class library project.
   ```
   mkdir my-plugin
   dotnet new sln
   dotnet new classlib --output my-plugin
   dotnet sln add my-plugin/my-plugin.csproj
   ```
2. Install the `NvimClient.API` NuGet package
   ```
   dotnet add my-plugin/my-plugin.csproj package NvimClient.API
   ```
3. Create a class like this
   ```csharp
   using NvimClient.API;
   using NvimClient.API.NvimPlugin.Attributes;
   using NvimClient.API.NvimPlugin.Parameters;

   namespace MyPlugin {
     // Make sure the class is public and has the NvimPlugin attribute.
     [NvimPlugin]
     public class MyPlugin {
       private readonly NvimAPI _nvim;
       // Constructor with exactly one `NvimAPI` parameter.
       public MyPlugin(NvimAPI nvim) {
         _nvim = nvim;
       }
       // Use attributes to expose functions, commands, and autocommands.
       // Valid parameter types and return types are:
       //   string, bool, long, double, T[], and IDictionary<T, T>
       [NvimFunction]
       public long MyFunction(long num1, long num2) {
         return num1 + num2;
       }
       [NvimCommand(Range = ".", NArgs = "*")]
       public void MyCommand(long[] range, params object[] args) {
         _nvim.SetCurrentLine(
           $"Command with args: {args}, range: {range[0]}-{range[1]}");
       }
       [NvimAutocmd("BufEnter", Pattern = "*.cs")]
       public void OnBufEnter(
           [NvimEval("expand('<afile>')")] string filename) {
         _nvim.OutWrite($"my-plugin is in '{filename}'\n");
       }
     }
   }
   ```
4. Make the directory `rplugin/dotnet` in the same directory as the solution file.
   If you are using git, you will need to create a file inside the directory
   so it can be tracked.
   ```
   mkdir rplugin/dotnet
   echo '' > rplugin/dotnet/.keep
   git add rplugin/dotnet
   ```
5. Start `nvim` and run `:UpdateRemotePlugins`.

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
