using System;
using System.Xml.Linq;

namespace NvimClient.APIGenerator.Properties.Models;

public record CSDocumentation {
    public string? Summary { get; set; }
    public string[]? ParameterReferences { get; set; }


    /// <summary>
    /// Converts the Doxygen XML documentation to Csharp doxygen documentation
    /// </summary>
    ///
    /// <param name="memberdef">
    ///     The XML elemenet that is to be conveted this is a memberdef item
    /// </param>
    public static CSDocumentation FromXElement(XElement memberdef) {
        //Console.WriteLine(element.Value);

        Console.WriteLine("Element {0}", memberdef.Name);
        foreach (XElement e in memberdef.Elements()) {
            Console.WriteLine("Contains Element Name {0}", e.Name);
        }

        XElement? desc = memberdef.Element("detaileddescription");
        string? summary = null;
        if (desc is not null) {
            summary = desc.Value;
        }


        return new CSDocumentation() {
            Summary = summary,
            ParameterReferences = null
        };
    }

    /// <summary>
    /// Parses a detailed description doxygen ekement into a C Sharp documentation
    /// item summary.
    /// </summary>
    private static string ParseDetailedDescription(XElement detailed) {
        return "";
    }
}



///<memberdef kind="function" id="autocmd_8c_1ae2f19bc604ddaf1c87234b8457d7af9f" prot="public" static="no" const="no" explicit="no" inline="no" virt="non-virtual">
///  <type>Array</type>
///  <definition>Array nvim_get_autocmds</definition>
///  <argsstring>(Dict(get_autocmds) *opts, Arena *arena, Error *err) FUNC_API_SINCE(9)</argsstring>
///  <name>nvim_get_autocmds</name>
///  <param>
///    <type><ref refid="struct_dict" kindref="compound">Dict</ref>(get_autocmds) *</type>
///    <declname>opts</declname>
///  </param>
///  <param>
///    <type>Arena *</type>
///    <declname>arena</declname>
///  </param>
///  <param>
///    <type>Error *</type>
///    <declname>err</declname>
///  </param>
///  <briefdescription>
///  </briefdescription>
///  <detaileddescription>
///    <para>
///      Get all autocommands that match the corresponding {opts}.
///    </para>
///    <para>
///      These examples will get autocommands matching ALL the given criteria:
///    </para>
///    <para>
///      <programlisting filename=".lua">
///        <codeline>
///          <highlight class="normal">--<sp/>Matches<sp/>all<sp/>criteria</highlight>
///        </codeline>
///        <codeline>
///          <highlight class="normal">autocommands<sp/>=<sp/>vim.api.nvim_get_autocmds({</highlight>
///        </codeline>
///        <codeline>
///          <highlight class="normal"><sp/><sp/>group<sp/>=<sp/>&apos;MyGroup&apos;,</highlight>
///        </codeline>
///        <codeline>
///          <highlight class="normal"><sp/><sp/>event<sp/>=<sp/>{&apos;BufEnter&apos;,<sp/>&apos;BufWinEnter&apos;},</highlight>
///        </codeline>
///        <codeline>
///          <highlight class="normal"><sp/><sp/>pattern<sp/>=<sp/>{&apos;*.c&apos;,<sp/>&apos;*.h&apos;}</highlight>
///        </codeline>
///        <codeline>
///          <highlight class="normal">})</highlight>
///        </codeline>
///        <codeline></codeline>
///        <codeline>
///          <highlight class="normal">--<sp/>All<sp/>commands<sp/>from<sp/>one<sp/>group</highlight>
///        </codeline>
///        <codeline>
///          <highlight class="normal">autocommands<sp/>=<sp/>vim.api.nvim_get_autocmds({</highlight>
///        </codeline>
///        <codeline>
///          <highlight class="normal"><sp/><sp/>group<sp/>=<sp/>&apos;MyGroup&apos;,</highlight>
///        </codeline>
///        <codeline>
///          <highlight class="normal">})</highlight>
///        </codeline>
///      </programlisting>
///    </para>
///    <para>
///      NOTE: When multiple patterns or events are provided, it will find all the autocommands that match any combination of them.
///    </para>
///    <para>
///      <parameterlist kind="param"><parameteritem>
///          <parameternamelist>
///            <parametername>opts</parametername>
///          </parameternamelist>
///          <parameterdescription>
///            <para><ref refid="struct_dict" kindref="compound">Dict</ref> with at least one of the following:<itemizedlist>
///                <listitem><para>buffer: (integer) Buffer number or list of buffer numbers for buffer local autocommands |autocmd-buflocal|. Cannot be used with {pattern}</para>
///                  </listitem><listitem><para>event: (string|table) event or events to match against |autocmd-events|.</para>
///                  </listitem><listitem><para>id: (integer) Autocommand ID to match.</para>
///                  </listitem><listitem><para>group: (string|table) the autocommand group name or id to match against.</para>
///                  </listitem><listitem><para>pattern: (string|table) pattern or patterns to match against |autocmd-pattern|. Cannot be used with {buffer} </para>
///              </listitem></itemizedlist>
///            </para>
///          </parameterdescription>
///        </parameteritem>
///      </parameterlist>
///      <simplesect kind="return"><para>Array of autocommands matching the criteria, with each item containing the following fields:<itemizedlist>
///            <listitem><para>buffer: (integer) the buffer number.</para>
///              </listitem><listitem><para>buflocal: (boolean) true if the autocommand is buffer local.</para>
///              </listitem><listitem><para>command: (string) the autocommand command. Note: this will be empty if a callback is set.</para>
///              </listitem><listitem><para>callback: (function|string|nil): Lua function or name of a Vim script function which is executed when this autocommand is triggered.</para>
///              </listitem><listitem><para>desc: (string) the autocommand description.</para>
///              </listitem><listitem><para>event: (string) the autocommand event.</para>
///              </listitem><listitem><para>id: (integer) the autocommand id (only when defined with the API).</para>
///              </listitem><listitem><para>group: (integer) the autocommand group id.</para>
///              </listitem><listitem><para>group_name: (string) the autocommand group name.</para>
///              </listitem><listitem><para>once: (boolean) whether the autocommand is only run once.</para>
///              </listitem><listitem><para>pattern: (string) the autocommand pattern. If the autocommand is buffer local |autocmd-buffer-local|: </para>
///          </listitem></itemizedlist>
///        </para>
///      </simplesect>
///    </para>
///  </detaileddescription>
///  <inbodydescription>
///  </inbodydescription>
///  <location file="D:/source/repos/neovim/src/nvim/api/autocmd.c" line="93" column="7" bodyfile="D:/source/repos/neovim/src/nvim/api/autocmd.c" bodystart="93" bodyend="331"/>
///</memberdef>