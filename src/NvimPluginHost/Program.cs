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

namespace NvimPluginHost
{
  internal static class Program
  {
    private static void Main()
    {
      Log.WriteLine("Plugin host started");

      var nvim = new NvimAPI(Console.OpenStandardOutput(),
        Console.OpenStandardInput());
      nvim.OnUnhandledRequest += (sender, request) =>
      {
        // Load the plugin and get the handler asynchronously
        Task.Run(() =>
        {
          var handler = GetPluginHandler(nvim, request.MethodName);
          if (handler == null)
          {
            var error =
              $"Could not find request handler for {request.MethodName}";
            request.SendResponse(null, error);
            Log.WriteLine(error);
            return;
          }

          Log.WriteLine($"Loaded handler for \"{request.MethodName}\"");
          try
          {
            var result = handler(request.Arguments);
            request.SendResponse(result);
          }
          catch (Exception exception)
          {
            request.SendResponse(null, exception);
          }
        });
      };

      nvim.OnUnhandledNotification += (sender, notification) =>
      {
        // Load the plugin and get the handler asynchronously
        Task.Run(() =>
        {
          GetPluginHandler(nvim, notification.MethodName)
            ?.Invoke(notification.Arguments);
        });
      };

      nvim.RegisterHandler("poll", args => "ok");
      nvim.RegisterHandler("specs", args =>
      {
        var slnFilePath = (string)args.First();
        var pluginType = GetPluginFromSolutionPath(slnFilePath);
        return pluginType == null
          ? null
          : PluginHost.GetPluginSpecs(pluginType);
      });

      nvim.WaitForDisconnect();

      Log.WriteLine("Plugin host stopping");
    }

    private static Func<object[], object> GetPluginHandler(NvimAPI nvim,
      string methodName)
    {
      var methodNameSplit = methodName.Split(':');
      // On Windows absolute paths contain a colon after
      // the drive letter, so the first two elements must
      // be joined together to obtain the file path.
      var slnFilePath =
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
          ? $"{methodNameSplit[0]}:{methodNameSplit[1]}"
          : methodNameSplit[0];
      var pluginType = GetPluginFromSolutionPath(slnFilePath);
      var exports =
        PluginHost.RegisterPluginExports(nvim, slnFilePath, pluginType);
      return exports.FirstOrDefault(export =>
        export.HandlerName == methodName)?.Handler;
    }

    private static Type GetPluginFromSolutionPath(string slnFilePath)
    {
      var slnFileInfo = new FileInfo(slnFilePath);
      // Run the dotnet build command, in case the plugin hasn't
      // been built yet, or it needs to be rebuilt.
      var buildProcess = Process.Start(
        new ProcessStartInfo
        {
          FileName = "dotnet",
          Arguments = "build " + slnFileInfo.FullName,
          CreateNoWindow = true
        });
      buildProcess?.WaitForExit();

      var plugin = slnFileInfo.Directory.EnumerateFiles(
          "*.dll", SearchOption.AllDirectories)
        .SelectMany(dll =>
        {
          try
          {
            return new AssemblyResolver(dll.FullName).Assembly.ExportedTypes;
          }
          catch
          {
            // Ignore assembly loading failures
            return Enumerable.Empty<Type>();
          }
        })
        .FirstOrDefault(type =>
        {
          try
          {
            return type.GetCustomAttribute<NvimPluginAttribute>() != null;
          }
          catch
          {
            // Ignore type resolution failures
            return false;
          }
        });
      return plugin;
    }
  }
}
