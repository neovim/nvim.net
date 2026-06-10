# Contributing

## Formatting

Format C# files, XML project/configuration files, and `.slnx` files with
CSharpier before opening a pull request.

Install CSharpier as a local .NET tool:

```sh
dotnet tool install --global CSharpier
```

Format the checked files locally:

```sh
dotnet csharpier format $(git ls-files '*.cs' '*.csx' '*.csproj' '*.props' '*.targets' '*.xml' '*.slnx')
```

Check formatting without writing changes:

```sh
dotnet csharpier check $(git ls-files '*.cs' '*.csx' '*.csproj' '*.props' '*.targets' '*.xml' '*.slnx')
```

See the official CSharpier docs for
[installation](https://csharpier.com/docs/About) and
[editor integration](https://csharpier.com/docs/Editors), including format on
save.
