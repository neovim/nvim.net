using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSharpier.Core;
using CSharpier.Core.CSharp;
using Nvim.Client.Generator.Api;
using Nvim.Client.Generator.CSharp;
using Xunit;

namespace Nvim.Client.Test;

public sealed class CSharpEmitterTests
{
  [Fact]
  public async Task CurrentNeovimMetadataEmitsFormattedSurface()
  {
    using var process = Process.Start(
      new ProcessStartInfo("nvim", "--api-info --headless")
      {
        RedirectStandardOutput = true,
        UseShellExecute = false,
      }
    )!;
    using var metadata = new MemoryStream();

    await process.StandardOutput.BaseStream.CopyToAsync(
      metadata,
      TestContext.Current.CancellationToken
    );
    await process.WaitForExitAsync(TestContext.Current.CancellationToken);

    Assert.Equal(0, process.ExitCode);
    var generated = new SourceEmitter().Emit(
      MetadataReader.Read(metadata.ToArray())
    );

    Assert.Equal(5, generated.Files.Length);
    Assert.NotEmpty(generated.Diagnostics);
    Assert.All(
      generated.Diagnostics,
      diagnostic => Assert.Equal("NVIMGEN001", diagnostic.Code)
    );
    Assert.Contains(
      "public partial interface INvimClient",
      generated
        .Files.Single(file => file.RelativePath == "Generated/INvimClient.g.cs")
        .Content
    );
    Assert.Contains(
      "NvimUiEventFactory",
      generated
        .Files.Single(file =>
          file.RelativePath.EndsWith("NvimUiEventFactory.g.cs")
        )
        .Content
    );

    foreach (var file in generated.Files)
    {
      var formatted = (
        await CSharpFormatter.FormatAsync(
          file.Content,
          new CodeFormatterOptions
          {
            EndOfLine = EndOfLine.LF,
            IndentSize = 2,
            Width = 80,
          },
          TestContext.Current.CancellationToken
        )
      ).Code;
      Assert.Equal(file.Content, formatted);
    }
  }
}
