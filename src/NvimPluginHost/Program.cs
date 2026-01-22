using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using NvimClient.API;
using NvimClient.API.NvimPlugin;
using NvimClient.API.NvimPlugin.Attributes;

namespace NvimPluginHost;

internal static class Program {
    private static void Main() {
        Log.WriteLine("Plugin host started");

        NvimAPI nvim = new(Console.OpenStandardOutput(), Console.OpenStandardInput());
        nvim.OnUnhandledRequest += (sender, request) => {
            // Load the plugin and get the handler asynchronously
            _ = Task.Run(() => {
                Func<object[], object?>? handler = GetPluginHandler(nvim, request.MethodName);
                if (handler is null) {
                    string error = $"Could not find request handler for {request.MethodName}";
                    request.SendResponse(null, error);
                    Log.WriteLine(error);
                    return;
                }

                Log.WriteLine($"Loaded handler for \"{request.MethodName}\"");
                try {
                    object? result = handler(request.Arguments);
                    if (result is null) {
                        InvalidOperationException ex = new($"Handler for {request.MethodName} produced null");
                        request.SendResponse(null, ex);
                    } else {
                        request.SendResponse(result);
                    }
                } catch (Exception exception) {
                    request.SendResponse(null, exception);
                }
            });
        };

        nvim.OnUnhandledNotification += (sender, notification) => {
            // Load the plugin and get the handler asynchronously
            _ = Task.Run(() => {
                _ = GetPluginHandler(nvim, notification.MethodName)?.Invoke(notification.Arguments);
            });
        };

        nvim.RegisterHandler("poll", args => "ok");

        nvim.RegisterHandler("specs", args => {
            string slnFilePath = (string)args.First();
            Type? pluginType = GetPluginFromSolutionPath(slnFilePath);
            return pluginType is null ? null : PluginHost.GetPluginSpecs(pluginType);
        });

        nvim.WaitForDisconnect();

        Log.WriteLine("Plugin host stopping");
    }

    private static Func<object[], object?>? GetPluginHandler(NvimAPI nvim, string methodName) {
        string[] methodNameSplit = methodName.Split(':');
        // On Windows absolute paths contain a colon after the drive letter, so the first two elements must
        // be joined together to obtain the file path.
        string slnFilePath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? $"{methodNameSplit[0]}:{methodNameSplit[1]}" : methodNameSplit[0];
        Type? pluginType = GetPluginFromSolutionPath(slnFilePath);
        if (pluginType is null) {
            return null;
        }
        NvimPluginExport[] exports = PluginHost.RegisterPluginExports(nvim, slnFilePath, pluginType);
        return exports.FirstOrDefault(export => export.HandlerName == methodName)?.Handler;
    }

    /// <summary>
    /// Gets a plugin from the directory containing a solution
    /// </summary>
    private static Type? GetPluginFromSolutionPath(string slnFilePath) {
        FileInfo slnFileInfo = new(slnFilePath);
        // Run the dotnet build command, in case the plugin hasn't been built yet, or it needs to be rebuilt.
        ProcessStartInfo dotnet_process = new() {
            FileName = "dotnet",
            Arguments = "build " + slnFileInfo.FullName,
            CreateNoWindow = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
        };
        Process? buildProcess = Process.Start(dotnet_process);
        buildProcess?.WaitForExit();

        Type? plugin = slnFileInfo.Directory?.EnumerateFiles("*.dll", SearchOption.AllDirectories).SelectMany(static dll => {
            try {
                return new AssemblyResolver(dll.FullName).Assembly.ExportedTypes;
            } catch {
                // Ignore assembly loading failures
                return [];
            }
        }).FirstOrDefault(static type => {
            try {
                return type.GetCustomAttribute<NvimPluginAttribute>() != null;
            } catch {
                // Ignore type resolution failures
                return false;
            }
        });
        return plugin;
    }
}