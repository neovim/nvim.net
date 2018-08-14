
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MsgPack;
using NvimClient.NvimMsgpack.Models;

namespace NvimClient.API
{
  public partial class NvimAPI
  {
    public event EventHandler<ResizeEventArgs> Resize;
    public event EventHandler Clear;
    public event EventHandler EolClear;
    public event EventHandler<CursorGotoEventArgs> CursorGoto;
    public event EventHandler<ModeInfoSetEventArgs> ModeInfoSet;
    public event EventHandler UpdateMenu;
    public event EventHandler BusyStart;
    public event EventHandler BusyStop;
    public event EventHandler MouseOn;
    public event EventHandler MouseOff;
    public event EventHandler<ModeChangeEventArgs> ModeChange;
    public event EventHandler<SetScrollRegionEventArgs> SetScrollRegion;
    public event EventHandler<ScrollEventArgs> Scroll;
    public event EventHandler<HighlightSetEventArgs> HighlightSet;
    public event EventHandler<PutEventArgs> Put;
    public event EventHandler Bell;
    public event EventHandler VisualBell;
    public event EventHandler Flush;
    public event EventHandler<UpdateFgEventArgs> UpdateFg;
    public event EventHandler<UpdateBgEventArgs> UpdateBg;
    public event EventHandler<UpdateSpEventArgs> UpdateSp;
    public event EventHandler<DefaultColorsSetEventArgs> DefaultColorsSet;
    public event EventHandler Suspend;
    public event EventHandler<SetTitleEventArgs> SetTitle;
    public event EventHandler<SetIconEventArgs> SetIcon;
    public event EventHandler<OptionSetEventArgs> OptionSet;
    public event EventHandler<PopupmenuShowEventArgs> PopupmenuShow;
    public event EventHandler PopupmenuHide;
    public event EventHandler<PopupmenuSelectEventArgs> PopupmenuSelect;
    public event EventHandler<TablineUpdateEventArgs> TablineUpdate;
    public event EventHandler<CmdlineShowEventArgs> CmdlineShow;
    public event EventHandler<CmdlinePosEventArgs> CmdlinePos;
    public event EventHandler<CmdlineSpecialCharEventArgs> CmdlineSpecialChar;
    public event EventHandler<CmdlineHideEventArgs> CmdlineHide;
    public event EventHandler<CmdlineBlockShowEventArgs> CmdlineBlockShow;
    public event EventHandler<CmdlineBlockAppendEventArgs> CmdlineBlockAppend;
    public event EventHandler CmdlineBlockHide;
    public event EventHandler<WildmenuShowEventArgs> WildmenuShow;
    public event EventHandler<WildmenuSelectEventArgs> WildmenuSelect;
    public event EventHandler WildmenuHide;

    public Task UiAttach(long @width, long @height, IDictionary @options) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_ui_attach",
        Arguments = GetRequestArguments(
          @width, @height, @options)
      });

    public Task UiDetach() =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_ui_detach",
        Arguments = GetRequestArguments(
          )
      });

    public Task UiTryResize(long @width, long @height) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_ui_try_resize",
        Arguments = GetRequestArguments(
          @width, @height)
      });

    public Task UiSetOption(string @name, object @value) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_ui_set_option",
        Arguments = GetRequestArguments(
          @name, @value)
      });

    /// <summary>
    /// <para>
    /// Executes an ex-command.
    /// </para>
    /// </summary>
    /// <param name="command">
    /// <para>
    /// Ex-command string 
    /// </para>
    /// </param>
    public Task Command(string @command) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_command",
        Arguments = GetRequestArguments(
          @command)
      });

    /// <summary>
    /// <para>
    /// Gets a highlight definition by name.
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Highlight group name 
    /// </para>
    /// </param>
    /// <param name="rgb">
    /// <para>
    /// Export RGB colors 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Highlight definition map 
    /// </para>
    /// </returns>
    public Task<IDictionary> GetHlByName(string @name, bool @rgb) =>
      SendAndReceive<IDictionary>(new NvimRequest
      {
        Method = "nvim_get_hl_by_name",
        Arguments = GetRequestArguments(
          @name, @rgb)
      });

    /// <summary>
    /// <para>
    /// Gets a highlight definition by id. |hlID()|
    /// </para>
    /// </summary>
    /// <param name="hlId">
    /// <para>
    /// Highlight id as returned by |hlID()| 
    /// </para>
    /// </param>
    /// <param name="rgb">
    /// <para>
    /// Export RGB colors 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Highlight definition map 
    /// </para>
    /// </returns>
    public Task<IDictionary> GetHlById(long @hlId, bool @rgb) =>
      SendAndReceive<IDictionary>(new NvimRequest
      {
        Method = "nvim_get_hl_by_id",
        Arguments = GetRequestArguments(
          @hlId, @rgb)
      });

    /// <summary>
    /// <para>
    /// Sends input-keys to Nvim, subject to various quirks controlled by 
    /// </para>
    /// </summary>
    /// <param name="keys">
    /// <para>
    /// to be typed 
    /// </para>
    /// </param>
    /// <param name="mode">
    /// <para>
    /// behavior flags, see |feedkeys()| 
    /// </para>
    /// </param>
    /// <param name="escapeCsi">
    /// <para>
    /// If true, escape K_SPECIAL/CSI bytes in 
    /// </para>
    /// </param>
    public Task Feedkeys(string @keys, string @mode, bool @escapeCsi) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_feedkeys",
        Arguments = GetRequestArguments(
          @keys, @mode, @escapeCsi)
      });

    /// <summary>
    /// <para>
    /// Queues raw user-input. Unlike |nvim_feedkeys()|, this uses a low-level input buffer and the call is non-blocking (input is processed asynchronously by the eventloop).
    /// </para>
    /// </summary>
    /// <param name="keys">
    /// <para>
    /// to be typed 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Number of bytes actually written (can be fewer than requested if the buffer becomes full). 
    /// </para>
    /// </returns>
    /// <remarks>
    /// |keycodes| like &lt;CR&gt; are translated, so &quot;&lt;&quot; is special. To input a literal &quot;&lt;&quot;, send &lt;LT&gt;.
    /// </remarks>
    public Task<long> Input(string @keys) =>
      SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_input",
        Arguments = GetRequestArguments(
          @keys)
      });

    /// <summary>
    /// <para>
    /// Replaces terminal codes and |keycodes| (&lt;CR&gt;, &lt;Esc&gt;, ...) in a string with the internal representation.
    /// </para>
    /// </summary>
    /// <param name="str">
    /// <para>
    /// String to be converted. 
    /// </para>
    /// </param>
    /// <param name="fromPart">
    /// <para>
    /// Legacy Vim parameter. Usually true. 
    /// </para>
    /// </param>
    /// <param name="doLt">
    /// <para>
    /// Also translate &lt;lt&gt;. Ignored if 
    /// </para>
    /// </param>
    /// <param name="special">
    /// <para>
    /// Replace |keycodes|, e.g. &lt;CR&gt; becomes a &quot;\n&quot; char. 
    /// </para>
    /// </param>
    public Task<string> ReplaceTermcodes(string @str, bool @fromPart, bool @doLt, bool @special) =>
      SendAndReceive<string>(new NvimRequest
      {
        Method = "nvim_replace_termcodes",
        Arguments = GetRequestArguments(
          @str, @fromPart, @doLt, @special)
      });

    /// <summary>
    /// <para>
    /// Executes an ex-command and returns its (non-error) output. Shell |:!| output is not captured.
    /// </para>
    /// </summary>
    /// <param name="command">
    /// <para>
    /// Ex-command string 
    /// </para>
    /// </param>
    public Task<string> CommandOutput(string @command) =>
      SendAndReceive<string>(new NvimRequest
      {
        Method = "nvim_command_output",
        Arguments = GetRequestArguments(
          @command)
      });

    /// <summary>
    /// <para>
    /// Evaluates a VimL expression (:help expression). Dictionaries and Lists are recursively expanded.
    /// </para>
    /// </summary>
    /// <param name="expr">
    /// <para>
    /// VimL expression string 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Evaluation result or expanded object 
    /// </para>
    /// </returns>
    public Task<object> Eval(string @expr) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_eval",
        Arguments = GetRequestArguments(
          @expr)
      });

    /// <summary>
    /// <para>
    /// Execute lua code. Parameters (if any) are available as 
    /// </para>
    /// </summary>
    /// <param name="code">
    /// <para>
    /// lua code to execute 
    /// </para>
    /// </param>
    /// <param name="args">
    /// <para>
    /// Arguments to the code 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Return value of lua code if present or NIL. 
    /// </para>
    /// </returns>
    public Task<object> ExecuteLua(string @code, object[] @args) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_execute_lua",
        Arguments = GetRequestArguments(
          @code, @args)
      });

    /// <summary>
    /// <para>
    /// Calls a VimL function with the given arguments.
    /// </para>
    /// </summary>
    /// <param name="fn">
    /// <para>
    /// Function to call 
    /// </para>
    /// </param>
    /// <param name="args">
    /// <para>
    /// Function arguments packed in an Array 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Result of the function call 
    /// </para>
    /// </returns>
    public Task<object> CallFunction(string @fn, object[] @args) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_call_function",
        Arguments = GetRequestArguments(
          @fn, @args)
      });

    /// <summary>
    /// <para>
    /// Calls a VimL |Dictionary-function| with the given arguments.
    /// </para>
    /// </summary>
    /// <param name="dict">
    /// <para>
    /// Dictionary, or String evaluating to a VimL |self| dict 
    /// </para>
    /// </param>
    /// <param name="fn">
    /// <para>
    /// Name of the function defined on the VimL dict 
    /// </para>
    /// </param>
    /// <param name="args">
    /// <para>
    /// Function arguments packed in an Array 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Result of the function call 
    /// </para>
    /// </returns>
    public Task<object> CallDictFunction(object @dict, string @fn, object[] @args) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_call_dict_function",
        Arguments = GetRequestArguments(
          @dict, @fn, @args)
      });

    /// <summary>
    /// <para>
    /// Calculates the number of display cells occupied by 
    /// </para>
    /// </summary>
    /// <param name="text">
    /// <para>
    /// Some text 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Number of cells 
    /// </para>
    /// </returns>
    public Task<long> Strwidth(string @text) =>
      SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_strwidth",
        Arguments = GetRequestArguments(
          @text)
      });

    /// <summary>
    /// <para>
    /// Gets the paths contained in &apos;runtimepath&apos;.
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// List of paths 
    /// </para>
    /// </returns>
    public Task<string[]> ListRuntimePaths() =>
      SendAndReceive<string[]>(new NvimRequest
      {
        Method = "nvim_list_runtime_paths",
        Arguments = GetRequestArguments(
          )
      });

    /// <summary>
    /// <para>
    /// Changes the global working directory.
    /// </para>
    /// </summary>
    /// <param name="dir">
    /// <para>
    /// Directory path 
    /// </para>
    /// </param>
    public Task SetCurrentDir(string @dir) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_current_dir",
        Arguments = GetRequestArguments(
          @dir)
      });

    /// <summary>
    /// <para>
    /// Gets the current line
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// Current line string 
    /// </para>
    /// </returns>
    public Task<string> GetCurrentLine() =>
      SendAndReceive<string>(new NvimRequest
      {
        Method = "nvim_get_current_line",
        Arguments = GetRequestArguments(
          )
      });

    /// <summary>
    /// <para>
    /// Sets the current line
    /// </para>
    /// </summary>
    /// <param name="line">
    /// <para>
    /// Line contents 
    /// </para>
    /// </param>
    public Task SetCurrentLine(string @line) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_current_line",
        Arguments = GetRequestArguments(
          @line)
      });

    /// <summary>
    /// <para>
    /// Deletes the current line
    /// </para>
    /// </summary>
    public Task DelCurrentLine() =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_del_current_line",
        Arguments = GetRequestArguments(
          )
      });

    /// <summary>
    /// <para>
    /// Gets a global (g:) variable
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Variable name 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Variable value 
    /// </para>
    /// </returns>
    public Task<object> GetVar(string @name) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_get_var",
        Arguments = GetRequestArguments(
          @name)
      });

    /// <summary>
    /// <para>
    /// Sets a global (g:) variable
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Variable name 
    /// </para>
    /// </param>
    /// <param name="value">
    /// <para>
    /// Variable value 
    /// </para>
    /// </param>
    public Task SetVar(string @name, object @value) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_var",
        Arguments = GetRequestArguments(
          @name, @value)
      });

    /// <summary>
    /// <para>
    /// Removes a global (g:) variable
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Variable name 
    /// </para>
    /// </param>
    public Task DelVar(string @name) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_del_var",
        Arguments = GetRequestArguments(
          @name)
      });

    /// <summary>
    /// <para>
    /// Gets a v: variable
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Variable name 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Variable value 
    /// </para>
    /// </returns>
    public Task<object> GetVvar(string @name) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_get_vvar",
        Arguments = GetRequestArguments(
          @name)
      });

    /// <summary>
    /// <para>
    /// Gets an option value string
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Option name 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Option value (global) 
    /// </para>
    /// </returns>
    public Task<object> GetOption(string @name) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_get_option",
        Arguments = GetRequestArguments(
          @name)
      });

    /// <summary>
    /// <para>
    /// Sets an option value
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Option name 
    /// </para>
    /// </param>
    /// <param name="value">
    /// <para>
    /// New option value 
    /// </para>
    /// </param>
    public Task SetOption(string @name, object @value) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_option",
        Arguments = GetRequestArguments(
          @name, @value)
      });

    /// <summary>
    /// <para>
    /// Writes a message to the Vim output buffer. Does not append &quot;\n&quot;, the message is buffered (won&apos;t display) until a linefeed is written.
    /// </para>
    /// </summary>
    /// <param name="str">
    /// <para>
    /// Message 
    /// </para>
    /// </param>
    public Task OutWrite(string @str) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_out_write",
        Arguments = GetRequestArguments(
          @str)
      });

    /// <summary>
    /// <para>
    /// Writes a message to the Vim error buffer. Does not append &quot;\n&quot;, the message is buffered (won&apos;t display) until a linefeed is written.
    /// </para>
    /// </summary>
    /// <param name="str">
    /// <para>
    /// Message 
    /// </para>
    /// </param>
    public Task ErrWrite(string @str) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_err_write",
        Arguments = GetRequestArguments(
          @str)
      });

    /// <summary>
    /// <para>
    /// Writes a message to the Vim error buffer. Appends &quot;\n&quot;, so the buffer is flushed (and displayed).
    /// </para>
    /// </summary>
    /// <param name="str">
    /// <para>
    /// Message 
    /// </para>
    /// </param>
    public Task ErrWriteln(string @str) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_err_writeln",
        Arguments = GetRequestArguments(
          @str)
      });

    /// <summary>
    /// <para>
    /// Gets the current list of buffer handles
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// List of buffer handles 
    /// </para>
    /// </returns>
    public Task<NvimBuffer[]> ListBufs() =>
      SendAndReceive<NvimBuffer[]>(new NvimRequest
      {
        Method = "nvim_list_bufs",
        Arguments = GetRequestArguments(
          )
      });

    /// <summary>
    /// <para>
    /// Gets the current buffer
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// Buffer handle 
    /// </para>
    /// </returns>
    public Task<NvimBuffer> GetCurrentBuf() =>
      SendAndReceive<NvimBuffer>(new NvimRequest
      {
        Method = "nvim_get_current_buf",
        Arguments = GetRequestArguments(
          )
      });

    /// <summary>
    /// <para>
    /// Sets the current buffer
    /// </para>
    /// </summary>
    /// <param name="buffer">
    /// <para>
    /// Buffer handle 
    /// </para>
    /// </param>
    public Task SetCurrentBuf(NvimBuffer @buffer) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_current_buf",
        Arguments = GetRequestArguments(
          @buffer)
      });

    /// <summary>
    /// <para>
    /// Gets the current list of window handles
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// List of window handles 
    /// </para>
    /// </returns>
    public Task<NvimWindow[]> ListWins() =>
      SendAndReceive<NvimWindow[]>(new NvimRequest
      {
        Method = "nvim_list_wins",
        Arguments = GetRequestArguments(
          )
      });

    /// <summary>
    /// <para>
    /// Gets the current window
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// Window handle 
    /// </para>
    /// </returns>
    public Task<NvimWindow> GetCurrentWin() =>
      SendAndReceive<NvimWindow>(new NvimRequest
      {
        Method = "nvim_get_current_win",
        Arguments = GetRequestArguments(
          )
      });

    /// <summary>
    /// <para>
    /// Sets the current window
    /// </para>
    /// </summary>
    /// <param name="window">
    /// <para>
    /// Window handle 
    /// </para>
    /// </param>
    public Task SetCurrentWin(NvimWindow @window) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_current_win",
        Arguments = GetRequestArguments(
          @window)
      });

    /// <summary>
    /// <para>
    /// Gets the current list of tabpage handles
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// List of tabpage handles 
    /// </para>
    /// </returns>
    public Task<NvimTabpage[]> ListTabpages() =>
      SendAndReceive<NvimTabpage[]>(new NvimRequest
      {
        Method = "nvim_list_tabpages",
        Arguments = GetRequestArguments(
          )
      });

    /// <summary>
    /// <para>
    /// Gets the current tabpage
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// Tabpage handle 
    /// </para>
    /// </returns>
    public Task<NvimTabpage> GetCurrentTabpage() =>
      SendAndReceive<NvimTabpage>(new NvimRequest
      {
        Method = "nvim_get_current_tabpage",
        Arguments = GetRequestArguments(
          )
      });

    /// <summary>
    /// <para>
    /// Sets the current tabpage
    /// </para>
    /// </summary>
    /// <param name="tabpage">
    /// <para>
    /// Tabpage handle 
    /// </para>
    /// </param>
    public Task SetCurrentTabpage(NvimTabpage @tabpage) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_current_tabpage",
        Arguments = GetRequestArguments(
          @tabpage)
      });

    /// <summary>
    /// <para>
    /// Subscribes to event broadcasts
    /// </para>
    /// </summary>
    /// <param name="event">
    /// <para>
    /// Event type string 
    /// </para>
    /// </param>
    public Task Subscribe(string @event) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_subscribe",
        Arguments = GetRequestArguments(
          @event)
      });

    /// <summary>
    /// <para>
    /// Unsubscribes to event broadcasts
    /// </para>
    /// </summary>
    /// <param name="event">
    /// <para>
    /// Event type string 
    /// </para>
    /// </param>
    public Task Unsubscribe(string @event) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_unsubscribe",
        Arguments = GetRequestArguments(
          @event)
      });

    public Task<long> GetColorByName(string @name) =>
      SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_get_color_by_name",
        Arguments = GetRequestArguments(
          @name)
      });

    public Task<IDictionary> GetColorMap() =>
      SendAndReceive<IDictionary>(new NvimRequest
      {
        Method = "nvim_get_color_map",
        Arguments = GetRequestArguments(
          )
      });

    /// <summary>
    /// <para>
    /// Gets the current mode. |mode()| &quot;blocking&quot; is true if Nvim is waiting for input.
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// Dictionary { &quot;mode&quot;: String, &quot;blocking&quot;: Boolean } 
    /// </para>
    /// </returns>
    public Task<IDictionary> GetMode() =>
      SendAndReceive<IDictionary>(new NvimRequest
      {
        Method = "nvim_get_mode",
        Arguments = GetRequestArguments(
          )
      });

    /// <summary>
    /// <para>
    /// Gets a list of global (non-buffer-local) |mapping| definitions.
    /// </para>
    /// </summary>
    /// <param name="mode">
    /// <para>
    /// Mode short-name (&quot;n&quot;, &quot;i&quot;, &quot;v&quot;, ...) 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Array of maparg()-like dictionaries describing mappings. The &quot;buffer&quot; key is always zero. 
    /// </para>
    /// </returns>
    public Task<IDictionary[]> GetKeymap(string @mode) =>
      SendAndReceive<IDictionary[]>(new NvimRequest
      {
        Method = "nvim_get_keymap",
        Arguments = GetRequestArguments(
          @mode)
      });

    /// <summary>
    /// <para>
    /// Gets a map of global (non-buffer-local) Ex commands.
    /// </para>
    /// </summary>
    /// <param name="opts">
    /// <para>
    /// Optional parameters. Currently only supports {&quot;builtin&quot;:false} 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Map of maps describing commands. 
    /// </para>
    /// </returns>
    public Task<IDictionary> GetCommands(IDictionary @opts) =>
      SendAndReceive<IDictionary>(new NvimRequest
      {
        Method = "nvim_get_commands",
        Arguments = GetRequestArguments(
          @opts)
      });

    /// <summary>
    /// <para>
    /// Returns a 2-tuple (Array), where item 0 is the current channel id and item 1 is the |api-metadata| map (Dictionary).
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// 2-tuple [{channel-id}, {api-metadata}] 
    /// </para>
    /// </returns>
    public Task<object[]> GetApiInfo() =>
      SendAndReceive<object[]>(new NvimRequest
      {
        Method = "nvim_get_api_info",
        Arguments = GetRequestArguments(
          )
      });

    /// <summary>
    /// <para>
    /// Identify the client for nvim. Can be called more than once, but subsequent calls will remove earlier info, which should be resent if it is still valid. (This could happen if a library first identifies the channel, and a plugin using that library later overrides that info)
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// short name for the connected client 
    /// </para>
    /// </param>
    /// <param name="version">
    /// <para>
    /// Dictionary describing the version, with the following possible keys (all optional)
    /// </para>
    /// </param>
    /// <param name="type">
    /// <para>
    /// Must be one of the following values. A client library should use &quot;remote&quot; if the library user hasn&apos;t specified other value.
    /// </para>
    /// </param>
    /// <param name="methods">
    /// <para>
    /// Builtin methods in the client. For a host, this does not include plugin methods which will be discovered later. The key should be the method name, the values are dicts with the following (optional) keys:
    /// </para>
    /// </param>
    /// <param name="attributes">
    /// <para>
    /// Informal attributes describing the client. Clients might define their own keys, but the following are suggested:
    /// </para>
    /// </param>
    public Task SetClientInfo(string @name, IDictionary @version, string @type, IDictionary @methods, IDictionary @attributes) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_client_info",
        Arguments = GetRequestArguments(
          @name, @version, @type, @methods, @attributes)
      });

    /// <summary>
    /// <para>
    /// Get information about a channel.
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// a Dictionary, describing a channel with the following keys:
    /// </para>
    /// </returns>
    public Task<IDictionary> GetChanInfo(long @chan) =>
      SendAndReceive<IDictionary>(new NvimRequest
      {
        Method = "nvim_get_chan_info",
        Arguments = GetRequestArguments(
          @chan)
      });

    /// <summary>
    /// <para>
    /// Get information about all open channels.
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// Array of Dictionaries, each describing a channel with the format specified at |nvim_get_chan_info|. 
    /// </para>
    /// </returns>
    public Task<object[]> ListChans() =>
      SendAndReceive<object[]>(new NvimRequest
      {
        Method = "nvim_list_chans",
        Arguments = GetRequestArguments(
          )
      });

    /// <summary>
    /// <para>
    /// Calls many API methods atomically.
    /// </para>
    /// </summary>
    /// <param name="calls">
    /// <para>
    /// an array of calls, where each call is described by an array with two elements: the request name, and an array of arguments. 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// an array with two elements. The first is an array of return values. The second is NIL if all calls succeeded. If a call resulted in an error, it is a three-element array with the zero-based index of the call which resulted in an error, the error type and the error message. If an error occurred, the values from all preceding calls will still be returned. 
    /// </para>
    /// </returns>
    public Task<object[]> CallAtomic(object[] @calls) =>
      SendAndReceive<object[]>(new NvimRequest
      {
        Method = "nvim_call_atomic",
        Arguments = GetRequestArguments(
          @calls)
      });

    /// <summary>
    /// <para>
    /// Parse a VimL expression
    /// </para>
    /// </summary>
    /// <param name="expr">
    /// <para>
    /// Expression to parse. Is always treated as a single line. 
    /// </para>
    /// </param>
    /// <param name="flags">
    /// <para>
    /// Flags: 
    /// </para>
    /// </param>
    /// <param name="highlight">
    /// <para>
    /// If true, return value will also include &quot;highlight&quot; key containing array of 4-tuples (arrays) (Integer, Integer, Integer, String), where first three numbers define the highlighted region and represent line, starting column and ending column (latter exclusive: one should highlight region [start_col, end_col)).
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// AST: top-level dictionary with these keys: 
    /// </para>
    /// </returns>
    public Task<IDictionary> ParseExpression(string @expr, string @flags, bool @highlight) =>
      SendAndReceive<IDictionary>(new NvimRequest
      {
        Method = "nvim_parse_expression",
        Arguments = GetRequestArguments(
          @expr, @flags, @highlight)
      });

    /// <summary>
    /// <para>
    /// Gets a list of dictionaries representing attached UIs.
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// Array of UI dictionaries
    /// </para>
    /// </returns>
    public Task<object[]> ListUis() =>
      SendAndReceive<object[]>(new NvimRequest
      {
        Method = "nvim_list_uis",
        Arguments = GetRequestArguments(
          )
      });

    /// <summary>
    /// <para>
    /// Gets the immediate children of process 
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// Array of child process ids, empty if process not found. 
    /// </para>
    /// </returns>
    public Task<object[]> GetProcChildren(long @pid) =>
      SendAndReceive<object[]>(new NvimRequest
      {
        Method = "nvim_get_proc_children",
        Arguments = GetRequestArguments(
          @pid)
      });

    /// <summary>
    /// <para>
    /// Gets info describing process 
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// Map of process properties, or NIL if process not found. 
    /// </para>
    /// </returns>
    public Task<object> GetProc(long @pid) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_get_proc",
        Arguments = GetRequestArguments(
          @pid)
      });


  public class NvimBuffer
  {
    private readonly NvimAPI _api;
    private readonly MessagePackExtendedTypeObject _msgPackExtObj;
    internal NvimBuffer(NvimAPI api, MessagePackExtendedTypeObject msgPackExtObj)
    {
      _api = api;
      _msgPackExtObj = msgPackExtObj;
    }
    
    /// <summary>
    /// <para>
    /// Gets the buffer line count
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// Line count 
    /// </para>
    /// </returns>
    public Task<long> LineCount() =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_buf_line_count",
        Arguments = GetRequestArguments(
          _msgPackExtObj)
      });

    /// <summary>
    /// <para>
    /// Activate updates from this buffer to the current channel.
    /// </para>
    /// </summary>
    /// <param name="sendBuffer">
    /// <para>
    /// Set to true if the initial notification should contain the whole buffer. If so, the first notification will be a 
    /// </para>
    /// </param>
    /// <param name="opts">
    /// <para>
    /// Optional parameters. Currently not used. 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// False when updates couldn&apos;t be enabled because the buffer isn&apos;t loaded or 
    /// </para>
    /// </returns>
    public Task<bool> Attach(bool @sendBuffer, IDictionary @opts) =>
      _api.SendAndReceive<bool>(new NvimRequest
      {
        Method = "nvim_buf_attach",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @sendBuffer, @opts)
      });

    /// <summary>
    /// <para>
    /// Deactivate updates from this buffer to the current channel.
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// False when updates couldn&apos;t be disabled because the buffer isn&apos;t loaded; otherwise True. 
    /// </para>
    /// </returns>
    public Task<bool> Detach() =>
      _api.SendAndReceive<bool>(new NvimRequest
      {
        Method = "nvim_buf_detach",
        Arguments = GetRequestArguments(
          _msgPackExtObj)
      });

    /// <summary>
    /// <para>
    /// Gets a line-range from the buffer.
    /// </para>
    /// </summary>
    /// <param name="start">
    /// <para>
    /// First line index 
    /// </para>
    /// </param>
    /// <param name="end">
    /// <para>
    /// Last line index (exclusive) 
    /// </para>
    /// </param>
    /// <param name="strictIndexing">
    /// <para>
    /// Whether out-of-bounds should be an error. 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Array of lines 
    /// </para>
    /// </returns>
    public Task<string[]> GetLines(long @start, long @end, bool @strictIndexing) =>
      _api.SendAndReceive<string[]>(new NvimRequest
      {
        Method = "nvim_buf_get_lines",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @start, @end, @strictIndexing)
      });

    /// <summary>
    /// <para>
    /// Sets (replaces) a line-range in the buffer.
    /// </para>
    /// </summary>
    /// <param name="start">
    /// <para>
    /// First line index 
    /// </para>
    /// </param>
    /// <param name="end">
    /// <para>
    /// Last line index (exclusive) 
    /// </para>
    /// </param>
    /// <param name="strictIndexing">
    /// <para>
    /// Whether out-of-bounds should be an error. 
    /// </para>
    /// </param>
    /// <param name="replacement">
    /// <para>
    /// Array of lines to use as replacement 
    /// </para>
    /// </param>
    public Task SetLines(long @start, long @end, bool @strictIndexing, string[] @replacement) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_set_lines",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @start, @end, @strictIndexing, @replacement)
      });

    /// <summary>
    /// <para>
    /// Gets a buffer-scoped (b:) variable.
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Variable name 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Variable value 
    /// </para>
    /// </returns>
    public Task<object> GetVar(string @name) =>
      _api.SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_buf_get_var",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @name)
      });

    /// <summary>
    /// <para>
    /// Gets a changed tick of a buffer
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// <c>$b:changedtick</c>
    /// </para>
    /// </returns>
    public Task<long> GetChangedtick() =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_buf_get_changedtick",
        Arguments = GetRequestArguments(
          _msgPackExtObj)
      });

    /// <summary>
    /// <para>
    /// Gets a list of buffer-local |mapping| definitions.
    /// </para>
    /// </summary>
    /// <param name="mode">
    /// <para>
    /// Mode short-name (&quot;n&quot;, &quot;i&quot;, &quot;v&quot;, ...) 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Array of maparg()-like dictionaries describing mappings. The &quot;buffer&quot; key holds the associated buffer handle. 
    /// </para>
    /// </returns>
    public Task<IDictionary[]> GetKeymap(string @mode) =>
      _api.SendAndReceive<IDictionary[]>(new NvimRequest
      {
        Method = "nvim_buf_get_keymap",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @mode)
      });

    /// <summary>
    /// <para>
    /// Gets a map of buffer-local |user-commands|.
    /// </para>
    /// </summary>
    /// <param name="opts">
    /// <para>
    /// Optional parameters. Currently not used. 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Map of maps describing commands. 
    /// </para>
    /// </returns>
    public Task<IDictionary> GetCommands(IDictionary @opts) =>
      _api.SendAndReceive<IDictionary>(new NvimRequest
      {
        Method = "nvim_buf_get_commands",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @opts)
      });

    /// <summary>
    /// <para>
    /// Sets a buffer-scoped (b:) variable
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Variable name 
    /// </para>
    /// </param>
    /// <param name="value">
    /// <para>
    /// Variable value 
    /// </para>
    /// </param>
    public Task SetVar(string @name, object @value) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_set_var",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @name, @value)
      });

    /// <summary>
    /// <para>
    /// Removes a buffer-scoped (b:) variable
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Variable name 
    /// </para>
    /// </param>
    public Task DelVar(string @name) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_del_var",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @name)
      });

    /// <summary>
    /// <para>
    /// Gets a buffer option value
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Option name 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Option value 
    /// </para>
    /// </returns>
    public Task<object> GetOption(string @name) =>
      _api.SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_buf_get_option",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @name)
      });

    /// <summary>
    /// <para>
    /// Sets a buffer option value. Passing &apos;nil&apos; as value deletes the option (only works if there&apos;s a global fallback)
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Option name 
    /// </para>
    /// </param>
    /// <param name="value">
    /// <para>
    /// Option value 
    /// </para>
    /// </param>
    public Task SetOption(string @name, object @value) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_set_option",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @name, @value)
      });

    /// <summary>
    /// <para>
    /// Gets the full file name for the buffer
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// Buffer name 
    /// </para>
    /// </returns>
    public Task<string> GetName() =>
      _api.SendAndReceive<string>(new NvimRequest
      {
        Method = "nvim_buf_get_name",
        Arguments = GetRequestArguments(
          _msgPackExtObj)
      });

    /// <summary>
    /// <para>
    /// Sets the full file name for a buffer
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Buffer name 
    /// </para>
    /// </param>
    public Task SetName(string @name) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_set_name",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @name)
      });

    /// <summary>
    /// <para>
    /// Checks if a buffer is valid
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// true if the buffer is valid, false otherwise 
    /// </para>
    /// </returns>
    public Task<bool> IsValid() =>
      _api.SendAndReceive<bool>(new NvimRequest
      {
        Method = "nvim_buf_is_valid",
        Arguments = GetRequestArguments(
          _msgPackExtObj)
      });

    /// <summary>
    /// <para>
    /// Return a tuple (row,col) representing the position of the named mark
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Mark name 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// (row, col) tuple 
    /// </para>
    /// </returns>
    public Task<long[]> GetMark(string @name) =>
      _api.SendAndReceive<long[]>(new NvimRequest
      {
        Method = "nvim_buf_get_mark",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @name)
      });

    /// <summary>
    /// <para>
    /// Adds a highlight to buffer.
    /// </para>
    /// </summary>
    /// <param name="srcId">
    /// <para>
    /// Source group to use or 0 to use a new group, or -1 for ungrouped highlight 
    /// </para>
    /// </param>
    /// <param name="hlGroup">
    /// <para>
    /// Name of the highlight group to use 
    /// </para>
    /// </param>
    /// <param name="line">
    /// <para>
    /// Line to highlight (zero-indexed) 
    /// </para>
    /// </param>
    /// <param name="colStart">
    /// <para>
    /// Start of (byte-indexed) column range to highlight 
    /// </para>
    /// </param>
    /// <param name="colEnd">
    /// <para>
    /// End of (byte-indexed) column range to highlight, or -1 to highlight to end of line 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// The src_id that was used 
    /// </para>
    /// </returns>
    public Task<long> AddHighlight(long @srcId, string @hlGroup, long @line, long @colStart, long @colEnd) =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_buf_add_highlight",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @srcId, @hlGroup, @line, @colStart, @colEnd)
      });

    /// <summary>
    /// <para>
    /// Clears highlights from a given source group and a range of lines
    /// </para>
    /// </summary>
    /// <param name="srcId">
    /// <para>
    /// Highlight source group to clear, or -1 to clear all. 
    /// </para>
    /// </param>
    /// <param name="lineStart">
    /// <para>
    /// Start of range of lines to clear 
    /// </para>
    /// </param>
    /// <param name="lineEnd">
    /// <para>
    /// End of range of lines to clear (exclusive) or -1 to clear to end of file. 
    /// </para>
    /// </param>
    public Task ClearHighlight(long @srcId, long @lineStart, long @lineEnd) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_clear_highlight",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @srcId, @lineStart, @lineEnd)
      });

  }
  public class NvimWindow
  {
    private readonly NvimAPI _api;
    private readonly MessagePackExtendedTypeObject _msgPackExtObj;
    internal NvimWindow(NvimAPI api, MessagePackExtendedTypeObject msgPackExtObj)
    {
      _api = api;
      _msgPackExtObj = msgPackExtObj;
    }
    
    /// <summary>
    /// <para>
    /// Gets the current buffer in a window
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// Buffer handle 
    /// </para>
    /// </returns>
    public Task<NvimBuffer> GetBuf() =>
      _api.SendAndReceive<NvimBuffer>(new NvimRequest
      {
        Method = "nvim_win_get_buf",
        Arguments = GetRequestArguments(
          _msgPackExtObj)
      });

    /// <summary>
    /// <para>
    /// Gets the cursor position in the window
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// (row, col) tuple 
    /// </para>
    /// </returns>
    public Task<long[]> GetCursor() =>
      _api.SendAndReceive<long[]>(new NvimRequest
      {
        Method = "nvim_win_get_cursor",
        Arguments = GetRequestArguments(
          _msgPackExtObj)
      });

    /// <summary>
    /// <para>
    /// Sets the cursor position in the window
    /// </para>
    /// </summary>
    /// <param name="pos">
    /// <para>
    /// (row, col) tuple representing the new position 
    /// </para>
    /// </param>
    public Task SetCursor(long[] @pos) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_set_cursor",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @pos)
      });

    /// <summary>
    /// <para>
    /// Gets the window height
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// Height as a count of rows 
    /// </para>
    /// </returns>
    public Task<long> GetHeight() =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_win_get_height",
        Arguments = GetRequestArguments(
          _msgPackExtObj)
      });

    /// <summary>
    /// <para>
    /// Sets the window height. This will only succeed if the screen is split horizontally.
    /// </para>
    /// </summary>
    /// <param name="height">
    /// <para>
    /// Height as a count of rows 
    /// </para>
    /// </param>
    public Task SetHeight(long @height) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_set_height",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @height)
      });

    /// <summary>
    /// <para>
    /// Gets the window width
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// Width as a count of columns 
    /// </para>
    /// </returns>
    public Task<long> GetWidth() =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_win_get_width",
        Arguments = GetRequestArguments(
          _msgPackExtObj)
      });

    /// <summary>
    /// <para>
    /// Sets the window width. This will only succeed if the screen is split vertically.
    /// </para>
    /// </summary>
    /// <param name="width">
    /// <para>
    /// Width as a count of columns 
    /// </para>
    /// </param>
    public Task SetWidth(long @width) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_set_width",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @width)
      });

    /// <summary>
    /// <para>
    /// Gets a window-scoped (w:) variable
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Variable name 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Variable value 
    /// </para>
    /// </returns>
    public Task<object> GetVar(string @name) =>
      _api.SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_win_get_var",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @name)
      });

    /// <summary>
    /// <para>
    /// Sets a window-scoped (w:) variable
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Variable name 
    /// </para>
    /// </param>
    /// <param name="value">
    /// <para>
    /// Variable value 
    /// </para>
    /// </param>
    public Task SetVar(string @name, object @value) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_set_var",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @name, @value)
      });

    /// <summary>
    /// <para>
    /// Removes a window-scoped (w:) variable
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Variable name 
    /// </para>
    /// </param>
    public Task DelVar(string @name) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_del_var",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @name)
      });

    /// <summary>
    /// <para>
    /// Gets a window option value
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Option name 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Option value 
    /// </para>
    /// </returns>
    public Task<object> GetOption(string @name) =>
      _api.SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_win_get_option",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @name)
      });

    /// <summary>
    /// <para>
    /// Sets a window option value. Passing &apos;nil&apos; as value deletes the option(only works if there&apos;s a global fallback)
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Option name 
    /// </para>
    /// </param>
    /// <param name="value">
    /// <para>
    /// Option value 
    /// </para>
    /// </param>
    public Task SetOption(string @name, object @value) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_set_option",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @name, @value)
      });

    /// <summary>
    /// <para>
    /// Gets the window position in display cells. First position is zero.
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// (row, col) tuple with the window position 
    /// </para>
    /// </returns>
    public Task<long[]> GetPosition() =>
      _api.SendAndReceive<long[]>(new NvimRequest
      {
        Method = "nvim_win_get_position",
        Arguments = GetRequestArguments(
          _msgPackExtObj)
      });

    /// <summary>
    /// <para>
    /// Gets the window tabpage
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// Tabpage that contains the window 
    /// </para>
    /// </returns>
    public Task<NvimTabpage> GetTabpage() =>
      _api.SendAndReceive<NvimTabpage>(new NvimRequest
      {
        Method = "nvim_win_get_tabpage",
        Arguments = GetRequestArguments(
          _msgPackExtObj)
      });

    /// <summary>
    /// <para>
    /// Gets the window number
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// Window number 
    /// </para>
    /// </returns>
    public Task<long> GetNumber() =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_win_get_number",
        Arguments = GetRequestArguments(
          _msgPackExtObj)
      });

    /// <summary>
    /// <para>
    /// Checks if a window is valid
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// true if the window is valid, false otherwise 
    /// </para>
    /// </returns>
    public Task<bool> IsValid() =>
      _api.SendAndReceive<bool>(new NvimRequest
      {
        Method = "nvim_win_is_valid",
        Arguments = GetRequestArguments(
          _msgPackExtObj)
      });

  }
  public class NvimTabpage
  {
    private readonly NvimAPI _api;
    private readonly MessagePackExtendedTypeObject _msgPackExtObj;
    internal NvimTabpage(NvimAPI api, MessagePackExtendedTypeObject msgPackExtObj)
    {
      _api = api;
      _msgPackExtObj = msgPackExtObj;
    }
    
    /// <summary>
    /// <para>
    /// Gets the windows in a tabpage
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// List of windows in 
    /// </para>
    /// </returns>
    public Task<NvimWindow[]> ListWins() =>
      _api.SendAndReceive<NvimWindow[]>(new NvimRequest
      {
        Method = "nvim_tabpage_list_wins",
        Arguments = GetRequestArguments(
          _msgPackExtObj)
      });

    /// <summary>
    /// <para>
    /// Gets a tab-scoped (t:) variable
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Variable name 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Variable value 
    /// </para>
    /// </returns>
    public Task<object> GetVar(string @name) =>
      _api.SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_tabpage_get_var",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @name)
      });

    /// <summary>
    /// <para>
    /// Sets a tab-scoped (t:) variable
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Variable name 
    /// </para>
    /// </param>
    /// <param name="value">
    /// <para>
    /// Variable value 
    /// </para>
    /// </param>
    public Task SetVar(string @name, object @value) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_tabpage_set_var",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @name, @value)
      });

    /// <summary>
    /// <para>
    /// Removes a tab-scoped (t:) variable
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Variable name 
    /// </para>
    /// </param>
    public Task DelVar(string @name) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_tabpage_del_var",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @name)
      });

    /// <summary>
    /// <para>
    /// Gets the current window in a tabpage
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// Window handle 
    /// </para>
    /// </returns>
    public Task<NvimWindow> GetWin() =>
      _api.SendAndReceive<NvimWindow>(new NvimRequest
      {
        Method = "nvim_tabpage_get_win",
        Arguments = GetRequestArguments(
          _msgPackExtObj)
      });

    /// <summary>
    /// <para>
    /// Gets the tabpage number
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// Tabpage number 
    /// </para>
    /// </returns>
    public Task<long> GetNumber() =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_tabpage_get_number",
        Arguments = GetRequestArguments(
          _msgPackExtObj)
      });

    /// <summary>
    /// <para>
    /// Checks if a tabpage is valid
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// true if the tabpage is valid, false otherwise 
    /// </para>
    /// </returns>
    public Task<bool> IsValid() =>
      _api.SendAndReceive<bool>(new NvimRequest
      {
        Method = "nvim_tabpage_is_valid",
        Arguments = GetRequestArguments(
          _msgPackExtObj)
      });

  }

  public class ResizeEventArgs : EventArgs
  {
    public long Width { get; set; }
    public long Height { get; set; }

  }
  public class CursorGotoEventArgs : EventArgs
  {
    public long Row { get; set; }
    public long Col { get; set; }

  }
  public class ModeInfoSetEventArgs : EventArgs
  {
    public bool Enabled { get; set; }
    public object[] CursorStyles { get; set; }

  }
  public class ModeChangeEventArgs : EventArgs
  {
    public string Mode { get; set; }
    public long ModeIdx { get; set; }

  }
  public class SetScrollRegionEventArgs : EventArgs
  {
    public long Top { get; set; }
    public long Bot { get; set; }
    public long Left { get; set; }
    public long Right { get; set; }

  }
  public class ScrollEventArgs : EventArgs
  {
    public long Count { get; set; }

  }
  public class HighlightSetEventArgs : EventArgs
  {
    public IDictionary Attrs { get; set; }

  }
  public class PutEventArgs : EventArgs
  {
    public string Str { get; set; }

  }
  public class UpdateFgEventArgs : EventArgs
  {
    public long Fg { get; set; }

  }
  public class UpdateBgEventArgs : EventArgs
  {
    public long Bg { get; set; }

  }
  public class UpdateSpEventArgs : EventArgs
  {
    public long Sp { get; set; }

  }
  public class DefaultColorsSetEventArgs : EventArgs
  {
    public long RgbFg { get; set; }
    public long RgbBg { get; set; }
    public long RgbSp { get; set; }
    public long CtermFg { get; set; }
    public long CtermBg { get; set; }

  }
  public class SetTitleEventArgs : EventArgs
  {
    public string Title { get; set; }

  }
  public class SetIconEventArgs : EventArgs
  {
    public string Icon { get; set; }

  }
  public class OptionSetEventArgs : EventArgs
  {
    public string Name { get; set; }
    public object Value { get; set; }

  }
  public class PopupmenuShowEventArgs : EventArgs
  {
    public object[] Items { get; set; }
    public long Selected { get; set; }
    public long Row { get; set; }
    public long Col { get; set; }

  }
  public class PopupmenuSelectEventArgs : EventArgs
  {
    public long Selected { get; set; }

  }
  public class TablineUpdateEventArgs : EventArgs
  {
    public NvimTabpage Current { get; set; }
    public object[] Tabs { get; set; }

  }
  public class CmdlineShowEventArgs : EventArgs
  {
    public object[] Content { get; set; }
    public long Pos { get; set; }
    public string Firstc { get; set; }
    public string Prompt { get; set; }
    public long Indent { get; set; }
    public long Level { get; set; }

  }
  public class CmdlinePosEventArgs : EventArgs
  {
    public long Pos { get; set; }
    public long Level { get; set; }

  }
  public class CmdlineSpecialCharEventArgs : EventArgs
  {
    public string C { get; set; }
    public bool Shift { get; set; }
    public long Level { get; set; }

  }
  public class CmdlineHideEventArgs : EventArgs
  {
    public long Level { get; set; }

  }
  public class CmdlineBlockShowEventArgs : EventArgs
  {
    public object[] Lines { get; set; }

  }
  public class CmdlineBlockAppendEventArgs : EventArgs
  {
    public object[] Lines { get; set; }

  }
  public class WildmenuShowEventArgs : EventArgs
  {
    public object[] Items { get; set; }

  }
  public class WildmenuSelectEventArgs : EventArgs
  {
    public long Selected { get; set; }

  }
    private void CallUIEventHandler(string eventName, object[] args)
    {
      switch (eventName)
      {
  
      case "resize":
          Resize?.Invoke(this, new ResizeEventArgs
          {
            Width = (long) args[0],
            Height = (long) args[1]
          });
          break;

      case "clear":
          Clear?.Invoke(this, EventArgs.Empty);
          break;

      case "eol_clear":
          EolClear?.Invoke(this, EventArgs.Empty);
          break;

      case "cursor_goto":
          CursorGoto?.Invoke(this, new CursorGotoEventArgs
          {
            Row = (long) args[0],
            Col = (long) args[1]
          });
          break;

      case "mode_info_set":
          ModeInfoSet?.Invoke(this, new ModeInfoSetEventArgs
          {
            Enabled = (bool) args[0],
            CursorStyles = (object[]) args[1]
          });
          break;

      case "update_menu":
          UpdateMenu?.Invoke(this, EventArgs.Empty);
          break;

      case "busy_start":
          BusyStart?.Invoke(this, EventArgs.Empty);
          break;

      case "busy_stop":
          BusyStop?.Invoke(this, EventArgs.Empty);
          break;

      case "mouse_on":
          MouseOn?.Invoke(this, EventArgs.Empty);
          break;

      case "mouse_off":
          MouseOff?.Invoke(this, EventArgs.Empty);
          break;

      case "mode_change":
          ModeChange?.Invoke(this, new ModeChangeEventArgs
          {
            Mode = (string) args[0],
            ModeIdx = (long) args[1]
          });
          break;

      case "set_scroll_region":
          SetScrollRegion?.Invoke(this, new SetScrollRegionEventArgs
          {
            Top = (long) args[0],
            Bot = (long) args[1],
            Left = (long) args[2],
            Right = (long) args[3]
          });
          break;

      case "scroll":
          Scroll?.Invoke(this, new ScrollEventArgs
          {
            Count = (long) args[0]
          });
          break;

      case "highlight_set":
          HighlightSet?.Invoke(this, new HighlightSetEventArgs
          {
            Attrs = (IDictionary) args[0]
          });
          break;

      case "put":
          Put?.Invoke(this, new PutEventArgs
          {
            Str = (string) args[0]
          });
          break;

      case "bell":
          Bell?.Invoke(this, EventArgs.Empty);
          break;

      case "visual_bell":
          VisualBell?.Invoke(this, EventArgs.Empty);
          break;

      case "flush":
          Flush?.Invoke(this, EventArgs.Empty);
          break;

      case "update_fg":
          UpdateFg?.Invoke(this, new UpdateFgEventArgs
          {
            Fg = (long) args[0]
          });
          break;

      case "update_bg":
          UpdateBg?.Invoke(this, new UpdateBgEventArgs
          {
            Bg = (long) args[0]
          });
          break;

      case "update_sp":
          UpdateSp?.Invoke(this, new UpdateSpEventArgs
          {
            Sp = (long) args[0]
          });
          break;

      case "default_colors_set":
          DefaultColorsSet?.Invoke(this, new DefaultColorsSetEventArgs
          {
            RgbFg = (long) args[0],
            RgbBg = (long) args[1],
            RgbSp = (long) args[2],
            CtermFg = (long) args[3],
            CtermBg = (long) args[4]
          });
          break;

      case "suspend":
          Suspend?.Invoke(this, EventArgs.Empty);
          break;

      case "set_title":
          SetTitle?.Invoke(this, new SetTitleEventArgs
          {
            Title = (string) args[0]
          });
          break;

      case "set_icon":
          SetIcon?.Invoke(this, new SetIconEventArgs
          {
            Icon = (string) args[0]
          });
          break;

      case "option_set":
          OptionSet?.Invoke(this, new OptionSetEventArgs
          {
            Name = (string) args[0],
            Value = (object) args[1]
          });
          break;

      case "popupmenu_show":
          PopupmenuShow?.Invoke(this, new PopupmenuShowEventArgs
          {
            Items = (object[]) args[0],
            Selected = (long) args[1],
            Row = (long) args[2],
            Col = (long) args[3]
          });
          break;

      case "popupmenu_hide":
          PopupmenuHide?.Invoke(this, EventArgs.Empty);
          break;

      case "popupmenu_select":
          PopupmenuSelect?.Invoke(this, new PopupmenuSelectEventArgs
          {
            Selected = (long) args[0]
          });
          break;

      case "tabline_update":
          TablineUpdate?.Invoke(this, new TablineUpdateEventArgs
          {
            Current = (NvimTabpage) args[0],
            Tabs = (object[]) args[1]
          });
          break;

      case "cmdline_show":
          CmdlineShow?.Invoke(this, new CmdlineShowEventArgs
          {
            Content = (object[]) args[0],
            Pos = (long) args[1],
            Firstc = (string) args[2],
            Prompt = (string) args[3],
            Indent = (long) args[4],
            Level = (long) args[5]
          });
          break;

      case "cmdline_pos":
          CmdlinePos?.Invoke(this, new CmdlinePosEventArgs
          {
            Pos = (long) args[0],
            Level = (long) args[1]
          });
          break;

      case "cmdline_special_char":
          CmdlineSpecialChar?.Invoke(this, new CmdlineSpecialCharEventArgs
          {
            C = (string) args[0],
            Shift = (bool) args[1],
            Level = (long) args[2]
          });
          break;

      case "cmdline_hide":
          CmdlineHide?.Invoke(this, new CmdlineHideEventArgs
          {
            Level = (long) args[0]
          });
          break;

      case "cmdline_block_show":
          CmdlineBlockShow?.Invoke(this, new CmdlineBlockShowEventArgs
          {
            Lines = (object[]) args[0]
          });
          break;

      case "cmdline_block_append":
          CmdlineBlockAppend?.Invoke(this, new CmdlineBlockAppendEventArgs
          {
            Lines = (object[]) args[0]
          });
          break;

      case "cmdline_block_hide":
          CmdlineBlockHide?.Invoke(this, EventArgs.Empty);
          break;

      case "wildmenu_show":
          WildmenuShow?.Invoke(this, new WildmenuShowEventArgs
          {
            Items = (object[]) args[0]
          });
          break;

      case "wildmenu_select":
          WildmenuSelect?.Invoke(this, new WildmenuSelectEventArgs
          {
            Selected = (long) args[0]
          });
          break;

      case "wildmenu_hide":
          WildmenuHide?.Invoke(this, EventArgs.Empty);
          break;

      }
    }

    private object GetExtensionType(MessagePackExtendedTypeObject msgPackExtObj)
    {
      switch (msgPackExtObj.TypeCode)
      {

        case 0:
          return new NvimBuffer(this, msgPackExtObj);
        case 1:
          return new NvimWindow(this, msgPackExtObj);
        case 2:
          return new NvimTabpage(this, msgPackExtObj);
        default:
          throw new SerializationException(
            $"Unknown extension type id {msgPackExtObj.TypeCode}");
      }
    }
  }
}