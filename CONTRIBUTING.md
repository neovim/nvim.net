# Contributing

## Build and test

```sh
dotnet restore Nvim.Client.slnx
dotnet build Nvim.Client.slnx --no-restore
dotnet test Nvim.Client.slnx --no-restore
```

## Updating the generated API

The tracked generated API is built normally; generation is only needed when
updating that output. Use the Neovim `v0.12.4` source at commit
`68ea43cd0c28af25cd47731308c94fedfcfd1b0b`:

```sh
git -C /path/to/neovim checkout v0.12.4
git -C /path/to/neovim rev-parse HEAD
dotnet run --project src/Nvim.Client.Generator.Cli -- \
  /path/to/neovim
```

## Formatting

Format C# files, XML project/configuration files, and `.slnx` files with
CSharpier before opening a pull request.

Install CSharpier as a local .NET tool:

```sh
dotnet tool install --global CSharpier --version 1.3.0
```

Format the checked files locally:

```sh
csharpier format $(git ls-files '*.cs' '*.csx' '*.csproj' '*.props' '*.targets' '*.xml' '*.slnx')
```

Check formatting without writing changes:

```sh
csharpier check $(git ls-files '*.cs' '*.csx' '*.csproj' '*.props' '*.targets' '*.xml' '*.slnx')
```

See the official CSharpier docs for
[installation](https://csharpier.com/docs/About) and
[editor integration](https://csharpier.com/docs/Editors), including format on
save.
