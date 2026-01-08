using System.Collections.Generic;
using System.Linq;
using NvimClient.APIGenerator.Docs;
using NvimClient.APIGenerator.Models;
using NvimClient.NvimMsgpack.Models;
using NvimClient.APIGenerator.Properties.Models;
using System;
using NvimClient.NvimMsgpack;
using System.Text;
using System.Globalization;

namespace NvimClient.APIGenerator;

/// <summary>
/// A class that generates C# api source code from nvim source code
/// </summary>
public sealed class NvimAPIGenerator {
    private readonly Dictionary<string, FunctionDoc> _functionDocs;
    private readonly NvimAPIMetadata apiMetadata;

    public NvimAPIGenerator(NvimAPIMetadata mdata, Dictionary<string, FunctionDoc> funcDocs) {
        apiMetadata = mdata;
        _functionDocs = funcDocs;
    }

    public void GenerateCSharpFile() {
        //_functionDocs = functionDocs?.ToDictionary(static functionDoc => functionDoc.Function, static funcDoc => funcDoc);
        //

        // Filter out functions only callable from Lua.
        apiMetadata.Functions = apiMetadata.Functions.Where(static f => !f.Parameters.Any(static p => p.ArgumentType == "LuaRef")).ToArray();

        GenerateNvimAPIClass("hola_test.cs", apiMetadata);
        GenerateCSharpClasses(apiMetadata);
    }


    ///<summary>
    ///    Generates one NvimAPI class as well as nvim type classes
    ///</summary>
    private static void GenerateCSharpClasses(NvimAPIMetadata apiMetadata) {
        GenerateNvimAPIClass("NvimAPI.Generated.cs", apiMetadata);
        foreach (KeyValuePair<string, NvimType> kvp in apiMetadata.Types) {
            GenerateNvimTypeClass($"Nvim{kvp.Key}.cs", $"Nvim{kvp.Key}", kvp.Value, apiMetadata);
        }

        foreach (NvimUIEvent ev in apiMetadata.SupportedUIEvents()) {
            GenerateNvimEventArgs($"{StringUtil.ConvertToCamelCase(ev.Name, true)}EventArgs.cs", ev, apiMetadata);
        }
    }

    public static void GenerateNvimAPIClass(string outputPath, NvimAPIMetadata apiMetadata) {
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

        IEnumerable<NvimFunction> funcs = apiMetadata.SupportedFunctions();
        foreach (NvimFunction f in funcs) {
            CSFunction fn = CSFunction.FromNvimFunction(f, "nvim_", isVirtualMethod: false);
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
    public static void GenerateNvimEventArgs(string outputPath, NvimUIEvent theEvent, NvimAPIMetadata apiMetadata) {
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

        Console.WriteLine("{0} Functions selected for {1}", cw.FunctionDeclarations.Count, classname);


        cw.WriteClassFile();
    }

}