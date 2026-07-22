<div align="center">

# Nvim.Client

**A lifecycle-safe .NET client for embedding, controlling, and extending
[Neovim](https://neovim.io/) over MessagePack-RPC.**

  
[![Test Workflow Status](https://github.com/neovim/nvim.net/actions/workflows/test.yml/badge.svg)](https://github.com/neovim/nvim.net/actions/workflows/test.yml)
[![NuGet](https://img.shields.io/nuget/v/Nvim.Client.svg?logo=nuget)](https://www.nuget.org/packages/Nvim.Client)
[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/github/license/neovim/nvim.net)](LICENSE)
  
</div>

## Install

```sh
dotnet add package Nvim.Client
```

Start an owned embedded Neovim instance, or attach to streams supplied by your
host:

```csharp
using Nvim.Client;

using var nvim = NvimClient.Start();
var result = await nvim.RequestAsync("nvim_eval", [new NvimString("2 + 2")]);
```

For a stdio RPC module, use `NvimClient.BorrowStandardIO()`, register
asynchronous handlers, and write diagnostics only to standard error. Attached
streams and processes are borrowed; stopping the client never closes or kills
them. `Start` and `ConnectAsync` own their transport resources.

```csharp
using var nvim = NvimClient.AttachToStandardIO();
using var registration = nvim.RegisterRequestHandler(
  "example.add",
  (arguments, cancellationToken) => Task.FromResult<NvimValue>(
    new NvimInteger(((NvimInteger)arguments[0]).Value + ((NvimInteger)arguments[1]).Value)
  )
);
await nvim.Completion;
```

See [CONTRIBUTING.md](CONTRIBUTING.md) for development instructions.
