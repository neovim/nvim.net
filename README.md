# neovim-dotnet-client
[![AppVeyor Build Status](https://ci.appveyor.com/api/projects/status/github/b-r-o-c-k/neovim-dotnet-client?branch=master&svg=true)](https://ci.appveyor.com/project/b-r-o-c-k/neovim-dotnet-client/branch/master)
[![Travis Build Status](https://travis-ci.org/b-r-o-c-k/neovim-dotnet-client.svg?branch=master)](https://travis-ci.org/b-r-o-c-k/neovim-dotnet-client)

.NET client for [Neovim](https://github.com/neovim/neovim)

## Plugin Host Installation
Using [vim-plug](https://github.com/junegunn/vim-plug):
```vim
Plug 'b-r-o-c-k/neovim-dotnet-client'
```

## Plugin Development with C#
1. Create a new solution and class library project.  
	```powershell
	mkdir my-plugin
	dotnet new sln
	dotnet new classlib --output my-plugin
	dotnet sln add my-plugin/my-plugin.csproj
	```  
1. Install the `NvimClient.API` NuGet package.  
	```powershell
	dotnet add my-plugin/my-plugin.csproj package NvimClient.API
	```  
1. Create a class similar to the following:  
	```csharp
	// Make sure the class is public and has the NvimPlugin attribute.
	[NvimPlugin]
	public class MyPlugin
	{
	  private readonly NvimAPI _nvim;
	
	  // The constructor should only have one
	  // parameter, which is the NvimAPI instance.
	  public MyPlugin(NvimAPI nvim)
	  {
	    _nvim = nvim;
	  }
	
	  // Use attributes to expose functions, commands, and autocommands.
	  // Valid parameter types and return types are:
	  //   string, bool, long, double, T[], and Dictionary<T, T>
	  [NvimFunction]
	  public long MyFunction(long num1, long num2)
	  {
	    return num1 + num2;
	  }
	
	  [NvimCommand(Range = ".", NArgs = "*")]
	  public void MyCommand(long[] range, params object[] args)
	  {
	    _nvim.SetCurrentLine(
	      $"Command with args: {args}, range: {range[0]}-{range[1]}");
	  }
	
	  [NvimAutocmd("BufEnter", Pattern = "*.cs", Eval = "expand('<afile>')")]
	  public void OnBufEnter(string filename)
	  {
	    _nvim.OutWrite($"my-plugin is in '{filename}'\n");
	  }
	}
	```  
1. Make the directory `rplugin/dotnet` in the same directory as the solution
   file.  
	```powershell
	mkdir rplugin/dotnet
	```

## Build
```
dotnet build
```

## Test
Run all tests (`nvim` must be in the `PATH`):
```
dotnet test test/NvimClient.Test/NvimClient.Test.csproj
```

Run only the `TestMessageDeserialization` test.
```
dotnet test --filter TestMessageDeserialization test/NvimClient.Test/NvimClient.Test.csproj
```
