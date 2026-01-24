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
    public required string Namespace { get; set; }
    public required string ClassName { get; set; }
    public List<string>? BaseClasses { get; set; }
    public List<CSField>? Fields { get; set; }
    public List<CSProperty>? Properties { get; set; }

    public List<CSEventDeclaration>? EventDeclarations { get; set; }

    public List<CSFunction>? FunctionDeclarations { get; set; }

    public required bool IsPartialClass { get; set; }
    public required bool IsSealedClass { get; set; }

    private int identationLevel;
    private const int identationWidth = 4;

    private readonly StreamWriter streamWriter;

    public ClassWriter(string filename) {
        streamWriter = new StreamWriter(filename);
        streamWriter.NewLine = "\n";
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
            streamWriter.WriteLine(s);
        }

        streamWriter.WriteLine();
        streamWriter.WriteLine();

        streamWriter.WriteLine($"namespace {Namespace};");

        streamWriter.WriteLine();
        streamWriter.WriteLine();
    }

    public void WriteIdentation() {
        for (int i = 0; i < identationLevel; i++) {
            for (int j = 0; j < identationWidth; j++) {
                streamWriter.Write(' ');
            }
        }
    }

    /// <summary>
    /// Writes the opening line for the class
    /// </summary>
    public void ClassStart() {
        streamWriter.Write("public ");
        if (IsPartialClass) {
            streamWriter.Write("partial ");
        }

        if (IsSealedClass) {
            streamWriter.Write("sealed ");
        }

        streamWriter.Write($"class {ClassName} ");

        if (BaseClasses is not null && BaseClasses.Count > 0) {
            streamWriter.Write(": ");
            for (int i = 0; i < BaseClasses.Count; i++) {
                streamWriter.Write(BaseClasses[i]);
                if (i != BaseClasses.Count - 1) {
                    streamWriter.Write(", ");
                }
            }
        }

        streamWriter.Write(" {\n");

        identationLevel++;
    }


    public void ClassEnd() {
        streamWriter.WriteLine("}");

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
                streamWriter.WriteLine(f.ToCode());
            }
        }

        if (Properties is not null) {
            foreach (CSProperty p in Properties) {
                WriteIdentation();
                streamWriter.WriteLine(p.ToCode());
            }
        }

        if (EventDeclarations is not null) {
            streamWriter.WriteLine();
            streamWriter.WriteLine();
            foreach (CSEventDeclaration e in EventDeclarations) {
                WriteIdentation();
                streamWriter.WriteLine(e.ToCode());
            }
        }


        if (FunctionDeclarations is not null) {
            streamWriter.WriteLine();
            streamWriter.WriteLine();
            foreach (CSFunction f in FunctionDeclarations) {
                streamWriter.WriteLine(f.ToCode(identationLevel));
                streamWriter.WriteLine();
                streamWriter.WriteLine();
            }
        }

        ClassEnd();
        streamWriter.Flush();
        streamWriter.Close();
    }

    public void Dispose() {
        streamWriter.Dispose();
    }
}