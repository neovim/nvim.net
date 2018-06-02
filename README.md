# neovim-dotnet-client
.NET client for [Neovim](https://github.com/neovim/neovim)

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
