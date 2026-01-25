using System.Collections.Generic;
using System.Linq;
using NvimClient.APIGenerator.Properties.Models;
using System;
using NvimClient.Models.Nvim;
using System.Text;
using System.Globalization;
using System.IO;
using NvimClient.Models.MsgPack;

namespace NvimClient.APIGenerator;

/// <summary>
/// A class that generates C# api source code from nvim source code
/// </summary>
public sealed class NvimAPIGenerator {
    private readonly Dictionary<string, CSDocumentation> _functionDocs;
    private readonly NvimAPIMetadata apiMetadata;
    private readonly string outputDir;

    public NvimAPIGenerator(string outputDir, NvimAPIMetadata mdata, Dictionary<string, CSDocumentation> funcDocs) {
        apiMetadata = mdata;
        _functionDocs = funcDocs;
        this.outputDir = outputDir;
    }

    public void GenerateCSharpFile() {

        // Filter out functions only callable from Lua.
        apiMetadata.Functions = apiMetadata.Functions.Where(static f => !f.Parameters.Any(static p => p.ArgumentType == "LuaRef")).ToArray();

        GenerateCSharpClasses(apiMetadata, _functionDocs);
    }

    public static void PrintFileLog(string filename, bool withLF) {
        Console.Write("Generating File: ");
        ConsoleColor currentcolor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Magenta;
        if (withLF) {
            Console.WriteLine("{0}", filename);
        } else {
            Console.Write("{0}", filename);
        }
        Console.ForegroundColor = currentcolor;
    }


    ///<summary>
    ///    Generates one NvimAPI class as well as nvim type classes
    ///</summary>
    private void GenerateCSharpClasses(NvimAPIMetadata apiMetadata, Dictionary<string, CSDocumentation> docs) {
        ConsoleUtils.BlueWriteLine("=========== Starting Code Generation ===========");
        string api_generated = Path.Combine(outputDir, "NvimAPI.Generated.cs");
        PrintFileLog(api_generated, withLF: true);
        GenerateNvimAPIClass(api_generated, apiMetadata, docs);
        Console.Write("\n\n");

        ConsoleUtils.BlueWriteLine("=========== Generating Nvim Types ===========");
        foreach (KeyValuePair<string, NvimType> kvp in apiMetadata.Types) {
            string filename = Path.Combine(outputDir, $"Nvim{kvp.Key}.cs");
            //The line will be change from GenerateNvimTypeClass
            PrintFileLog(filename, withLF: false);
            GenerateNvimTypeClass(filename, $"Nvim{kvp.Key}", kvp.Value, apiMetadata);
        }
        Console.Write("\n\n");

        ConsoleUtils.BlueWriteLine("=========== Generating Nvim Events ===========");
        foreach (NvimUIEvent ev in apiMetadata.SupportedUIEvents()) {
            string filename = $"{StringUtil.ConvertToCamelCase(ev.Name, true)}EventArgs.cs";
            string full_filename = Path.Combine(outputDir, filename);
            PrintFileLog(full_filename, withLF: true);
            GenerateNvimEventArgs(full_filename, ev);
        }
    }

    public static void GenerateNvimAPIClass(string outputPath, NvimAPIMetadata apiMetadata, Dictionary<string, CSDocumentation> docs) {
        ClassWriter cw = new(outputPath) {
            IsSealedClass = true,
            IsPartialClass = true,
            ClassName = "NvimAPI",
            Namespace = "NvimClient.API",
            Usings = [
                "System",
                "System.IO",
                "System.Linq",
                "System.Security",
                "System.Collections",
                "System.Collections.Generic",
                "System.Threading.Tasks",
                "System.Runtime.Serialization",
                "MsgPack",
                "NvimClient.NvimMsgpack.Models"
            ],
            EventDeclarations = [],
            FunctionDeclarations = []
        };

        IEnumerable<NvimUIEvent> events = apiMetadata.SupportedUIEvents();
        foreach (NvimUIEvent e in events) {
            CSEventDeclaration ev = CSEventDeclaration.FromNvimEvent(e);
            cw.EventDeclarations.Add(ev);
        }

        IEnumerable<NvimFunction> funcs = apiMetadata.AvailableFunctions();
        foreach (NvimFunction f in funcs) {
            CSFunction fn = CSFunction.FromNvimFunction(f, "nvim_", isVirtualMethod: false);
            if (f.DeprecatedSince is not null) {
                CSFunctionAttributeDescription attr = new() {
                    AttributeName = "Obsolete"
                };
                fn.Attribute = attr;
            }

            if (docs.TryGetValue(f.Name, out CSDocumentation? value)) {
                fn.Documentation = value;
            }

            cw.FunctionDeclarations.Add(fn);
        }

        //    private void CallUIEventHandler(string eventName, object[] args)
        CSFunction args = new() {
            Specifiers = ["private"],
            ReturnType = "void",
            Name = "CallUIEventHandler",
            Arguments = [
                new CSArgument() {
                    ArgumentType = "string",
                    ArgumentName = "eventName"
                },
                new CSArgument() {
                    ArgumentType = "object[]",
                    ArgumentName = "args"
                }
            ],
            Code = string.Empty
        };

        StringBuilder sb = new();
        _ = sb.Append("switch (eventName) {\n");

        foreach (NvimUIEvent ev in events) {
            string name = StringUtil.ConvertToCamelCase(ev.Name, capitalizeFirstChar: true);
            string nameEvent = $"{name}Event";
            string nameArgs = $"{name}EventArgs";
            _ = sb.Append(CultureInfo.InvariantCulture, $"    case \"{ev.Name}\": {{\n");

            if (ev.Parameters.Length > 0) {

                _ = sb.Append("        ").Append(nameArgs).Append(" specificArgs").Append(" = new() {\n");
                int index = 0;
                foreach (NvimParameter aa in ev.Parameters) {
                    string evType = NvimTypesMap.GetCSharpType(aa.ArgumentType);
                    string evName = StringUtil.ConvertToCamelCase(aa.ArgumentName, capitalizeFirstChar: true);
                    _ = sb.Append(CultureInfo.InvariantCulture, $"            {evName} = ({evType})args[{index++}]");
                    if (index != ev.Parameters.Length) {
                        _ = sb.Append(",\n");
                    } else {
                        _ = sb.Append('\n');
                    }
                }
                _ = sb.Append("        ").Append("};\n");
                _ = sb.Append("        ").Append(nameEvent).Append('?').Append(".Invoke(this, specificArgs);\n");
            } else {
                _ = sb.Append("        ").Append(nameEvent).Append('?').Append(".Invoke(this, EventArgs.Empty);\n");
            }
            _ = sb.Append("        ").Append("break;\n");
            _ = sb.Append("    ").Append("}\n");
        }
        _ = sb.Append("    ").Append("default:\n");
        _ = sb.Append("        ").Append("break;\n");
        _ = sb.Append("}\n");

        args.Code = sb.ToString();
        cw.FunctionDeclarations.Add(args);
        cw.WriteClassFile();
    }


    ///<summary>
    ///     Generates a C# class for a specific <see cref=NvimUIEvent/>
    ///</summary>
    public static void GenerateNvimEventArgs(string outputPath, NvimUIEvent theEvent) {
        ClassWriter cw = new(outputPath) {
            IsSealedClass = false,
            IsPartialClass = false,
            ClassName = $"{StringUtil.ConvertToCamelCase(theEvent.Name, capitalizeFirstChar: true)}EventArgs",
            BaseClasses = ["EventArgs"],
            Namespace = "NvimClient.API",
            Usings = [],
            EventDeclarations = [],
            FunctionDeclarations = [],
            Properties = []
        };

        foreach (NvimParameter p in theEvent.Parameters) {
            CSProperty prop = new() {
                Specifiers = ["public"],
                Type = NvimTypesMap.GetCSharpType(p.ArgumentType),
                Name = StringUtil.ConvertToCamelCase(p.ArgumentName, capitalizeFirstChar: true)
            };
            cw.Properties.Add(prop);
        }
        cw.WriteClassFile();
    }

    ///<summary>
    ///     Generates a C# class for a specific <see cref="NvimType"/>
    ///</summary>
    public static void GenerateNvimTypeClass(string outputPath, string classname, NvimType theType, NvimAPIMetadata apiMetadata) {
        ClassWriter cw = new(outputPath) {
            IsSealedClass = true,
            IsPartialClass = false,
            ClassName = classname,
            Namespace = "NvimClient.API",
            Usings = [],
            EventDeclarations = [],
            FunctionDeclarations = [],
            Fields = [
                new CSField() {
                    Specifiers = ["private","readonly"],
                    Type = "NvimAPI",
                    Name = "_api",
                },
                new CSField() {
                    Specifiers = ["private","readonly"],
                    Type = "MessagePackExtendedTypeObject",
                    Name = "_msgPackExtObj",
                }
            ]
        };

        //Add a constructor
        CSFunction ctor = new() {
            Specifiers = ["public"],
            ReturnType = string.Empty,
            Name = classname,
            Arguments = [
                new CSArgument() {
                    ArgumentType = "NvimAPI",
                    ArgumentName = "api"
                },
                new CSArgument() {
                    ArgumentType = "MessagePackExtendedTypeObject",
                    ArgumentName = "msgPackExtObj"
                },
            ],
            Code = """
                    _api = api;
                    _msgPackExtObj = msgPackExtObj;
                """,
        };
        cw.FunctionDeclarations.Add(ctor);

        //List<NvimFunction> funcs = [.. apiMetadata.SupportedFunctions().Where(fn => fn.Name.StartsWith(theType.Prefix, StringComparison.Ordinal)).Select(fn => fn)];
        IEnumerable<NvimFunction> funcs = apiMetadata.SupportedMethods(theType);

        foreach (NvimFunction f in funcs) {
            CSFunction fn = CSFunction.FromNvimTypeMethod(theType, f, isVirtualMethod: false);
            cw.FunctionDeclarations.Add(fn);
        }

        //One function is the constructor
        if (cw.FunctionDeclarations.Count is 1) {
            Console.WriteLine(" Containing no Functions for {0}", classname);
        } else {
            Console.WriteLine(" Containing {0} Functions selected for {1}", cw.FunctionDeclarations.Count - 1, classname);
        }

        foreach (NvimFunction f in funcs) {
            Console.Write("Member {0,30}", f.Name);
            if (f.DeprecatedSince is null) {
                ConsoleUtils.ColorWriteLine(ConsoleColor.DarkGreen, " Active Sinde Api Level {0,5}", f.Since);
            } else {
                ConsoleUtils.ColorWrite(ConsoleColor.Red, "{0,15}", " Deprecated ");
                Console.WriteLine("Since Api Level {0}", f.DeprecatedSince.Value);
            }
        }


        cw.WriteClassFile();
    }

}