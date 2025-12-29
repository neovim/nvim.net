using MsgPack.Serialization;
using NvimClient.NvimMsgpack.Models;
using NvimClient.NvimProcess;
using System.Diagnostics;

namespace NvimClient.APIGenerator;

/// <summary>
/// A class that provides Nvim API metadata
/// </summary>
public sealed class MetaDataProvider {

    /// <summary>
    /// Launches an nvim process wit the appropriate arguments and retreive
    /// the required api metatdata.
    /// </summary>
    public static NvimAPIMetadata? GetAPIMetadata() {
        //TODO: Check nvim version before the metadata
        NvimProcessStartInfo nvim_start = new(StartOption.ApiInfo | StartOption.Headless);

        //Use implicit conversion here
        Process? process = Process.Start(nvim_start);

        if (process is null) {
            return null;
        }

        SerializationContext context = new();
        context.DictionarySerlaizationOptions.KeyTransformer = StringUtil.ConvertToSnakeCase;
        MessagePackSerializer<NvimAPIMetadata> serializer = context.GetSerializer<NvimAPIMetadata>();
        NvimAPIMetadata apiMetadata = serializer.Unpack(process.StandardOutput.BaseStream);
        return apiMetadata;
    }

}