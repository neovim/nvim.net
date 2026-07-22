using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Nvim.Client.Generator.Api;
using Nvim.Client.Generator.CSharp;

if (args is not [var source])
{
  Console.Error.WriteLine(
    "Usage: Nvim.Client.Generator.Cli <neovim-source-directory>"
  );
  return 2;
}

var apiDirectory = Path.Combine(Path.GetFullPath(source), "src", "nvim", "api");

if (!Directory.Exists(apiDirectory))
  throw new ArgumentException(
    "Expected a Neovim source tree containing src/nvim/api.",
    nameof(source)
  );

// Doxygen requires a configuration file and writes several intermediate files,
// so isolate all in a temp dir.
var temporaryDirectory = Directory.CreateTempSubdirectory(
  "nvim-client-doxygen-"
);

try
{
  var config = Path.Combine(temporaryDirectory.FullName, "Doxyfile");
  var documentation = Path.Combine(
    temporaryDirectory.FullName,
    "xml",
    "index.xml"
  );

  await File.WriteAllTextAsync(
    config,
    $"""
    PROJECT_NAME = Neovim
    OUTPUT_DIRECTORY = {Quote(temporaryDirectory.FullName)}
    INPUT = {Quote(apiDirectory)}
    INPUT_ENCODING = UTF-8
    RECURSIVE = YES
    FILE_PATTERNS = *.c *.h
    EXTENSION_MAPPING = c=C
    EXTRACT_ALL = YES
    GENERATE_HTML = NO
    GENERATE_LATEX = NO
    GENERATE_XML = YES
    XML_OUTPUT = xml
    XML_PROGRAMLISTING = NO
    QUIET = YES
    JAVADOC_AUTOBRIEF = YES
    ENABLE_PREPROCESSING = YES
    MACRO_EXPANSION = YES
    EXPAND_ONLY_PREDEF = YES
    PREDEFINED = "ArrayOf(x)=Array" "DictionaryOf(x)=Dictionary" "Dict(x)=Dictionary" "DictOf(x)=Dictionary" "DictAs(x)=Dictionary" "Tuple(...)=Array" "Union(...)=Object"
    """
  );

  var doxygen = await RunAsync("doxygen", config);

  if (doxygen.ExitCode != 0)
    throw new InvalidOperationException(
      $"doxygen failed with exit code {doxygen.ExitCode}: {doxygen.Error}"
    );

  if (!File.Exists(documentation))
    throw new InvalidOperationException("doxygen did not produce XML output.");

  // Neovim emits MessagePack bytes rather than text, so stdout must be
  // captured directly from BaseStream without character decoding.
  var nvim = await RunAsync("nvim", "--api-info", "--headless");

  if (nvim.ExitCode != 0)
  {
    Console.Error.WriteLine(
      $"nvim --api-info failed with exit code {nvim.ExitCode}: {nvim.Error}"
    );
    return nvim.ExitCode;
  }

  var definition = DoxygenDocumentation.Apply(
    MetadataReader.Read(nvim.Output),
    documentation
  );
  var files = new SourceEmitter().Emit(definition);

  foreach (var diagnostic in files.Diagnostics)
    Console.Error.WriteLine($"{diagnostic.Code}: {diagnostic.Message}");

  var output = Path.GetFullPath(Path.Combine("src", "Nvim.Client"));

  foreach (var file in files.Files)
  {
    var path = Path.Combine(output, file.RelativePath);
    var directory = Path.GetDirectoryName(path);

    if (!string.IsNullOrEmpty(directory))
      Directory.CreateDirectory(directory);

    await File.WriteAllTextAsync(path, file.Content);
  }

  return 0;
}
finally
{
  temporaryDirectory.Delete(recursive: true);
}

static async Task<(int ExitCode, byte[] Output, string Error)> RunAsync(
  string command,
  params string[] arguments
)
{
  var startInfo = new ProcessStartInfo(command)
  {
    RedirectStandardOutput = true,
    RedirectStandardError = true,
    UseShellExecute = false,
  };

  // ArgumentList avoids shell parsing and handles spaces in paths correctly.
  foreach (var argument in arguments)
    startInfo.ArgumentList.Add(argument);

  using var process =
    Process.Start(startInfo)
    ?? throw new InvalidOperationException(
      $"Unable to start {command} from PATH."
    );
  using var output = new MemoryStream();

  // Drain stdout and stderr concurrently. Waiting for the process before
  // reading both streams can deadlock if either redirected pipe fills.
  var copyOutput = process.StandardOutput.BaseStream.CopyToAsync(output);
  var readError = process.StandardError.ReadToEndAsync();
  var waitForExit = process.WaitForExitAsync();

  await Task.WhenAll(copyOutput, readError, waitForExit);

  return (process.ExitCode, output.ToArray(), await readError);
}

// These values are embedded in the Doxygen config file, not passed through the process shell,
// so use Doxygen-compatible quoting.
static string Quote(string value) => $"\"{value.Replace("\"", "\\\"")}\"";
