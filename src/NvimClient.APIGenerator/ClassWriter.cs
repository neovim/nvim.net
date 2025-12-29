using NvimClient.APIGenerator.Models;
using NvimClient.APIGenerator.Properties.Models;
using System;
using System.Collections.Generic;
using System.IO;


namespace NvimClient.APIGenerator;
/// <summary>
/// A class that writes code
/// </summary>
public sealed class ClassWriter : IDisposable {
    public List<string>? Usings { get; set; }
    public List<CSField>? Fields { get; set; }

    public List<CSEventDeclaration>? EventDeclarations { get; set; }

    public List<CSFunction>? FunctionDeclarations { get; set; }

    public required string Namespace { get; set; }
    public required string ClassName { get; set; }
    public required bool IsPartialClass { get; set; }
    public required bool IsSealedClass { get; set; }

    private int identationLevel;
    private const int identationWidth = 4;

    private readonly StreamWriter sw;

    public ClassWriter(string filename) {
        sw = new StreamWriter(filename);
    }

    /// <summary>
    /// Writes the usings of the file as well as it's namespace
    /// </summary>
    public void WriteHeader() {
        if (Usings is null) {
            return;
        }

        foreach (string u in Usings) {
            string s = $"using {u};";
            sw.WriteLine(s);
        }

        sw.WriteLine();
        sw.WriteLine();

        sw.WriteLine($"namespace {Namespace};");

        sw.WriteLine();
        sw.WriteLine();
    }

    public void WriteIdentation() {
        for (int i = 0; i < identationLevel; i++) {
            for (int j = 0; j < identationWidth; j++) {
                sw.Write(' ');
            }
        }
    }

    /// <summary>
    /// Writes the opening line for the class
    /// </summary>
    public void ClassStart() {
        if (IsPartialClass) {
            sw.WriteLine($"public partial sealed class {ClassName} {{");
        } else {
            sw.WriteLine($"public sealed class {ClassName} {{");
        }
        identationLevel++;
    }


    public void ClassEnd() {
        sw.WriteLine("}");

        if (identationLevel > 0) {
            identationLevel--;
        }
    }

    /// <summary>
    /// Writes the complete class file with the data that it requires
    /// </summary>
    public void WriteClassFile() {
        WriteHeader();
        ClassStart();

        if (Fields is not null) {
            foreach (CSField f in Fields) {
                WriteIdentation();
                sw.WriteLine(f.ToCode());
            }
        }

        if (EventDeclarations is not null) {
            foreach (CSEventDeclaration e in EventDeclarations) {
                WriteIdentation();
                sw.WriteLine(e.ToCode());
            }
        }

        if (FunctionDeclarations is not null) {
            foreach (CSFunction f in FunctionDeclarations) {
                //WriteIdentation();
                sw.WriteLine(f.ToCode(identationLevel));
            }
        }

        ClassEnd();
        sw.Flush();
        sw.Close();
    }

    public void Dispose() {
        sw.Dispose();
    }
}