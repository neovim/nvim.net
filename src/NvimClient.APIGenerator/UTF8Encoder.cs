using System.IO;
using System.Text;

namespace NvimClient.APIGenerator;

/// <summary>
/// Sometimes some XML files wrongly claim UTF8 while they are in ISO-8859-1
/// We will make a conversion here. This is a naive implementation as it loads
/// the complete file into memory. But it will do for now.
/// </summary>
public sealed class UTF8Encoder {


    /// <summary>
    /// Converts a text file from one ISO-8859-1 code page to utf8
    /// </summary>
    public static void ConvertIso88591FileToUtf8(string inputPath, string outputPath) {
        Encoding sourceEncoding = Encoding.GetEncoding("ISO-8859-1");

        //Do not emmit a byte order mark
        Encoding targetEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

        string text = File.ReadAllText(inputPath, sourceEncoding);
        File.WriteAllText(outputPath, text, targetEncoding);
    }

}