using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using MsgPack;
using NvimClient.NvimMsgpack.Models;

namespace NvimClient.API
{
    public partial class NvimAPI
    {
        public event EventHandler<ModeInfoSetEventArgs> ModeInfoSetEvent;
        public event EventHandler UpdateMenuEvent;
        public event EventHandler BusyStartEvent;
        public event EventHandler BusyStopEvent;
        public event EventHandler MouseOnEvent;
        public event EventHandler MouseOffEvent;
        public event EventHandler<ModeChangeEventArgs> ModeChangeEvent;
        public event EventHandler BellEvent;
        public event EventHandler VisualBellEvent;
        public event EventHandler FlushEvent;
        public event EventHandler<ConnectEventArgs> ConnectEvent;
        public event EventHandler<RestartEventArgs> RestartEvent;
        public event EventHandler SuspendEvent;
        public event EventHandler<SetTitleEventArgs> SetTitleEvent;
        public event EventHandler<SetIconEventArgs> SetIconEvent;
        public event EventHandler<ScreenshotEventArgs> ScreenshotEvent;
        public event EventHandler<OptionSetEventArgs> OptionSetEvent;
        public event EventHandler<ChdirEventArgs> ChdirEvent;
        public event EventHandler<UiSendEventArgs> UiSendEvent;
        public event EventHandler<UpdateFgEventArgs> UpdateFgEvent;
        public event EventHandler<UpdateBgEventArgs> UpdateBgEvent;
        public event EventHandler<UpdateSpEventArgs> UpdateSpEvent;
        public event EventHandler<ResizeEventArgs> ResizeEvent;
        public event EventHandler ClearEvent;
        public event EventHandler EolClearEvent;
        public event EventHandler<CursorGotoEventArgs> CursorGotoEvent;
        public event EventHandler<HighlightSetEventArgs> HighlightSetEvent;
        public event EventHandler<PutEventArgs> PutEvent;
        public event EventHandler<SetScrollRegionEventArgs> SetScrollRegionEvent;
        public event EventHandler<ScrollEventArgs> ScrollEvent;
        public event EventHandler<DefaultColorsSetEventArgs> DefaultColorsSetEvent;
        public event EventHandler<HlAttrDefineEventArgs> HlAttrDefineEvent;
        public event EventHandler<HlGroupSetEventArgs> HlGroupSetEvent;
        public event EventHandler<GridResizeEventArgs> GridResizeEvent;
        public event EventHandler<GridClearEventArgs> GridClearEvent;
        public event EventHandler<GridCursorGotoEventArgs> GridCursorGotoEvent;
        public event EventHandler<GridLineEventArgs> GridLineEvent;
        public event EventHandler<GridScrollEventArgs> GridScrollEvent;
        public event EventHandler<GridDestroyEventArgs> GridDestroyEvent;
        public event EventHandler<WinPosEventArgs> WinPosEvent;
        public event EventHandler<WinFloatPosEventArgs> WinFloatPosEvent;
        public event EventHandler<WinExternalPosEventArgs> WinExternalPosEvent;
        public event EventHandler<WinHideEventArgs> WinHideEvent;
        public event EventHandler<WinCloseEventArgs> WinCloseEvent;
        public event EventHandler<MsgSetPosEventArgs> MsgSetPosEvent;
        public event EventHandler<WinViewportEventArgs> WinViewportEvent;
        public event EventHandler<WinViewportMarginsEventArgs> WinViewportMarginsEvent;
        public event EventHandler<WinExtmarkEventArgs> WinExtmarkEvent;
        public event EventHandler<PopupmenuShowEventArgs> PopupmenuShowEvent;
        public event EventHandler PopupmenuHideEvent;
        public event EventHandler<PopupmenuSelectEventArgs> PopupmenuSelectEvent;
        public event EventHandler<TablineUpdateEventArgs> TablineUpdateEvent;
        public event EventHandler<CmdlineShowEventArgs> CmdlineShowEvent;
        public event EventHandler<CmdlinePosEventArgs> CmdlinePosEvent;
        public event EventHandler<CmdlineSpecialCharEventArgs> CmdlineSpecialCharEvent;
        public event EventHandler<CmdlineHideEventArgs> CmdlineHideEvent;
        public event EventHandler<CmdlineBlockShowEventArgs> CmdlineBlockShowEvent;
        public event EventHandler<CmdlineBlockAppendEventArgs> CmdlineBlockAppendEvent;
        public event EventHandler CmdlineBlockHideEvent;
        public event EventHandler<WildmenuShowEventArgs> WildmenuShowEvent;
        public event EventHandler<WildmenuSelectEventArgs> WildmenuSelectEvent;
        public event EventHandler WildmenuHideEvent;
        public event EventHandler<MsgShowEventArgs> MsgShowEvent;
        public event EventHandler MsgClearEvent;
        public event EventHandler<MsgShowcmdEventArgs> MsgShowcmdEvent;
        public event EventHandler<MsgShowmodeEventArgs> MsgShowmodeEvent;
        public event EventHandler<MsgRulerEventArgs> MsgRulerEvent;
        public event EventHandler<MsgHistoryShowEventArgs> MsgHistoryShowEvent;
        public event EventHandler<ErrorExitEventArgs> ErrorExitEvent;

        /// <summary>
        /// <para>
        /// Gets all autocommands matching ALL criteria in the {opts} query.
        /// </para>
        /// </summary>
        /// <param name="opts">
        /// <para>
        /// Dict
        /// </para>
        /// </param>
        /// <returns>
        /// <para>
        /// Array of matching autocommands, where each item has:
        /// </para>
        /// </returns>
        public Task<object[]> GetAutocmds(IDictionary @opts) =>
            SendAndReceive<object[]>(
                new NvimRequest
                {
                    Method = "nvim_get_autocmds",
                    Arguments = GetRequestArguments(@opts),
                }
            );

        /// <summary>
        /// <para>
        /// Creates an |autocommand| event handler, defined by
        /// </para>
        /// </summary>
        /// <param name="event">
        /// <para>
        /// Event(s) that will trigger the handler (
        /// </para>
        /// </param>
        /// <param name="opts">
        /// <para>
        /// Options dict:
        /// </para>
        /// </param>
        /// <returns>
        /// <para>
        /// Autocommand id (number)
        /// </para>
        /// </returns>
        public Task<long> CreateAutocmd(object @event, IDictionary @opts) =>
            SendAndReceive<long>(
                new NvimRequest
                {
                    Method = "nvim_create_autocmd",
                    Arguments = GetRequestArguments(@event, @opts),
                }
            );

        /// <summary>
        /// <para>
        /// Deletes an autocommand by id.
        /// </para>
        /// </summary>
        /// <param name="id">
        /// <para>
        /// Autocommand id returned by |nvim_create_autocmd()|
        /// </para>
        /// </param>
        public Task DelAutocmd(long @id) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_del_autocmd",
                    Arguments = GetRequestArguments(@id),
                }
            );

        /// <summary>
        /// <para>
        /// Clears all autocommands matching the {opts} query. To delete autocmds see |nvim_del_autocmd()|.
        /// </para>
        /// </summary>
        /// <param name="opts">
        /// <para>
        /// Optional parameters:
        /// </para>
        /// </param>
        public Task ClearAutocmds(IDictionary @opts) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_clear_autocmds",
                    Arguments = GetRequestArguments(@opts),
                }
            );

        /// <summary>
        /// <para>
        /// Create or get an autocommand group |autocmd-groups|.
        /// </para>
        /// </summary>
        /// <param name="name">
        /// <para>
        /// Group name
        /// </para>
        /// </param>
        /// <param name="opts">
        /// <para>
        /// Optional parameters:
        /// </para>
        /// </param>
        /// <returns>
        /// <para>
        /// Group id.
        /// </para>
        /// </returns>
        public Task<long> CreateAugroup(string @name, IDictionary @opts) =>
            SendAndReceive<long>(
                new NvimRequest
                {
                    Method = "nvim_create_augroup",
                    Arguments = GetRequestArguments(@name, @opts),
                }
            );

        /// <summary>
        /// <para>
        /// Delete an autocommand group by id.
        /// </para>
        /// </summary>
        /// <param name="id">
        /// <para>
        /// Group id.
        /// </para>
        /// </param>
        public Task DelAugroupById(long @id) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_del_augroup_by_id",
                    Arguments = GetRequestArguments(@id),
                }
            );

        /// <summary>
        /// <para>
        /// Delete an autocommand group by name.
        /// </para>
        /// </summary>
        /// <param name="name">
        /// <para>
        /// Group name.
        /// </para>
        /// </param>
        public Task DelAugroupByName(string @name) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_del_augroup_by_name",
                    Arguments = GetRequestArguments(@name),
                }
            );

        /// <param name="event">
        /// <para>
        /// Event(s) to execute.
        /// </para>
        /// </param>
        /// <param name="opts">
        /// <para>
        /// Optional filters:
        /// </para>
        /// </param>
        public Task ExecAutocmds(object @event, IDictionary @opts) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_exec_autocmds",
                    Arguments = GetRequestArguments(@event, @opts),
                }
            );

        public Task<IDictionary> ParseCmd(string @str, IDictionary @opts) =>
            SendAndReceive<IDictionary>(
                new NvimRequest
                {
                    Method = "nvim_parse_cmd",
                    Arguments = GetRequestArguments(@str, @opts),
                }
            );

        /// <summary>
        /// <para>
        /// Executes an Ex command
        /// </para>
        /// </summary>
        /// <param name="cmd">
        /// <para>
        /// Command to execute, a
        /// </para>
        /// </param>
        /// <param name="opts">
        /// <para>
        /// Optional parameters.
        /// </para>
        /// </param>
        /// <returns>
        /// <para>
        /// Command output (non-error, non-shell |:!|) if
        /// </para>
        /// </returns>
        public Task<string> Cmd(IDictionary @cmd, IDictionary @opts) =>
            SendAndReceive<string>(
                new NvimRequest
                {
                    Method = "nvim_cmd",
                    Arguments = GetRequestArguments(@cmd, @opts),
                }
            );

        /// <summary>
        /// <para>
        /// Creates a global |user-commands| command.
        /// </para>
        /// </summary>
        /// <param name="name">
        /// <para>
        /// Name of the new user command. Must begin with an uppercase letter.
        /// </para>
        /// </param>
        /// <param name="cmd">
        /// <para>
        /// Replacement command to execute when this user command is executed. When called from Lua, the command can also be a Lua function. The function is called with a single table argument that contains the following keys:
        /// </para>
        /// </param>
        /// <param name="opts">
        /// <para>
        /// Optional flags
        /// </para>
        /// </param>
        public Task CreateUserCommand(string @name, object @cmd, IDictionary @opts) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_create_user_command",
                    Arguments = GetRequestArguments(@name, @cmd, @opts),
                }
            );

        /// <summary>
        /// <para>
        /// Delete a user-defined command.
        /// </para>
        /// </summary>
        /// <param name="name">
        /// <para>
        /// Name of the command to delete.
        /// </para>
        /// </param>
        public Task DelUserCommand(string @name) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_del_user_command",
                    Arguments = GetRequestArguments(@name),
                }
            );

        public Task<IDictionary> GetCommands(IDictionary @opts) =>
            SendAndReceive<IDictionary>(
                new NvimRequest
                {
                    Method = "nvim_get_commands",
                    Arguments = GetRequestArguments(@opts),
                }
            );

        /// <summary>
        /// <para>
        /// DeprecatedUse nvim_exec2() instead.
        /// </para>
        /// </summary>
        public Task<string> Exec(string @src, bool @output) =>
            SendAndReceive<string>(
                new NvimRequest
                {
                    Method = "nvim_exec",
                    Arguments = GetRequestArguments(@src, @output),
                }
            );

        /// <summary>
        /// <para>
        /// Deprecated
        /// </para>
        /// </summary>
        public Task<string> CommandOutput(string @command) =>
            SendAndReceive<string>(
                new NvimRequest
                {
                    Method = "nvim_command_output",
                    Arguments = GetRequestArguments(@command),
                }
            );

        /// <summary>
        /// <para>
        /// DeprecatedUse nvim_exec_lua() instead.
        /// </para>
        /// </summary>
        public Task<object> ExecuteLua(string @code, object[] @args) =>
            SendAndReceive<object>(
                new NvimRequest
                {
                    Method = "nvim_execute_lua",
                    Arguments = GetRequestArguments(@code, @args),
                }
            );

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
            SendAndReceive<IDictionary>(
                new NvimRequest
                {
                    Method = "nvim_get_hl_by_id",
                    Arguments = GetRequestArguments(@hlId, @rgb),
                }
            );

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
            SendAndReceive<IDictionary>(
                new NvimRequest
                {
                    Method = "nvim_get_hl_by_name",
                    Arguments = GetRequestArguments(@name, @rgb),
                }
            );

        public Task<IDictionary> GetOptionInfo(string @name) =>
            SendAndReceive<IDictionary>(
                new NvimRequest
                {
                    Method = "nvim_get_option_info",
                    Arguments = GetRequestArguments(@name),
                }
            );

        /// <summary>
        /// <para>
        /// Sets the global value of an option.
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
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_set_option",
                    Arguments = GetRequestArguments(@name, @value),
                }
            );

        /// <summary>
        /// <para>
        /// Gets the global value of an option.
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
            SendAndReceive<object>(
                new NvimRequest
                {
                    Method = "nvim_get_option",
                    Arguments = GetRequestArguments(@name),
                }
            );

        /// <summary>
        /// <para>
        /// DeprecatedUse nvim_exec_lua() instead.
        /// </para>
        /// </summary>
        /// <param name="calls">
        /// <para>
        /// an array of calls, where each call is described by an array with two elements: the request name, and an array of arguments.
        /// </para>
        /// </param>
        /// <returns>
        /// <para>
        /// Array of two elements. The first is an array of return values. The second is NIL if all calls succeeded. If a call resulted in an error, it is a three-element array with the zero-based index of the call which resulted in an error, the error type and the error message. If an error occurred, the values from all preceding calls will still be returned.
        /// </para>
        /// </returns>
        public Task<object[]> CallAtomic(object[] @calls) =>
            SendAndReceive<object[]>(
                new NvimRequest
                {
                    Method = "nvim_call_atomic",
                    Arguments = GetRequestArguments(@calls),
                }
            );

        /// <summary>
        /// <para>
        /// Deprecated
        /// </para>
        /// </summary>
        /// <param name="event">
        /// <para>
        /// Event type string
        /// </para>
        /// </param>
        public Task Subscribe(string @event) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_subscribe",
                    Arguments = GetRequestArguments(@event),
                }
            );

        /// <summary>
        /// <para>
        /// Deprecated
        /// </para>
        /// </summary>
        /// <param name="event">
        /// <para>
        /// Event type string
        /// </para>
        /// </param>
        public Task Unsubscribe(string @event) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_unsubscribe",
                    Arguments = GetRequestArguments(@event),
                }
            );

        /// <summary>
        /// <para>
        /// Deprecated
        /// </para>
        /// </summary>
        /// <param name="str">
        /// <para>
        /// Message
        /// </para>
        /// </param>
        public Task OutWrite(string @str) =>
            SendAndReceive(
                new NvimRequest { Method = "nvim_out_write", Arguments = GetRequestArguments(@str) }
            );

        /// <summary>
        /// <para>
        /// Deprecated
        /// </para>
        /// </summary>
        /// <param name="str">
        /// <para>
        /// Message
        /// </para>
        /// </param>
        public Task ErrWrite(string @str) =>
            SendAndReceive(
                new NvimRequest { Method = "nvim_err_write", Arguments = GetRequestArguments(@str) }
            );

        /// <summary>
        /// <para>
        /// Deprecated
        /// </para>
        /// </summary>
        /// <param name="str">
        /// <para>
        /// Message
        /// </para>
        /// </param>
        public Task ErrWriteln(string @str) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_err_writeln",
                    Arguments = GetRequestArguments(@str),
                }
            );

        /// <summary>
        /// <para>
        /// Deprecated
        /// </para>
        /// </summary>
        /// <param name="msg">
        /// <para>
        /// Message to display to the user
        /// </para>
        /// </param>
        /// <param name="logLevel">
        /// <para>
        /// The log level
        /// </para>
        /// </param>
        /// <param name="opts">
        /// <para>
        /// Reserved for future use.
        /// </para>
        /// </param>
        public Task<object> Notify(string @msg, long @logLevel, IDictionary @opts) =>
            SendAndReceive<object>(
                new NvimRequest
                {
                    Method = "nvim_notify",
                    Arguments = GetRequestArguments(@msg, @logLevel, @opts),
                }
            );

        /// <summary>
        /// <para>
        /// Emitted by the TUI client to signal when a host-terminal event occurred.
        /// </para>
        /// </summary>
        /// <param name="event">
        /// <para>
        /// Event name
        /// </para>
        /// </param>
        /// <param name="value">
        /// <para>
        /// Event payload
        /// </para>
        /// </param>
        public Task UiTermEvent(string @event, object @value) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_ui_term_event",
                    Arguments = GetRequestArguments(@event, @value),
                }
            );

        /// <summary>
        /// <para>
        /// Creates a new namespace or gets an existing one. [namespace]()
        /// </para>
        /// </summary>
        /// <param name="name">
        /// <para>
        /// Namespace name or empty string
        /// </para>
        /// </param>
        /// <returns>
        /// <para>
        /// Namespace id
        /// </para>
        /// </returns>
        public Task<long> CreateNamespace(string @name) =>
            SendAndReceive<long>(
                new NvimRequest
                {
                    Method = "nvim_create_namespace",
                    Arguments = GetRequestArguments(@name),
                }
            );

        public Task<IDictionary> GetNamespaces() =>
            SendAndReceive<IDictionary>(
                new NvimRequest
                {
                    Method = "nvim_get_namespaces",
                    Arguments = GetRequestArguments(),
                }
            );

        /// <summary>
        /// <para>
        /// Set or change decoration provider for a |namespace|
        /// </para>
        /// </summary>
        /// <param name="nsId">
        /// <para>
        /// Namespace id from |nvim_create_namespace()|
        /// </para>
        /// </param>
        /// <param name="opts">
        /// <para>
        /// Table of callbacks:
        /// </para>
        /// </param>
        public Task SetDecorationProvider(long @nsId, IDictionary @opts) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_set_decoration_provider",
                    Arguments = GetRequestArguments(@nsId, @opts),
                }
            );

        /// <summary>
        /// <para>
        /// Gets the value of an option. The behavior of this function matches that of |:set|: the local value of an option is returned if it exists; otherwise, the global value is returned. Local values always correspond to the current buffer or window, unless &quot;buf&quot; or &quot;win&quot; is set in {opts}.
        /// </para>
        /// </summary>
        /// <param name="name">
        /// <para>
        /// Option name
        /// </para>
        /// </param>
        /// <param name="opts">
        /// <para>
        /// Optional parameters
        /// </para>
        /// </param>
        /// <returns>
        /// <para>
        /// Option value
        /// </para>
        /// </returns>
        public Task<object> GetOptionValue(string @name, IDictionary @opts) =>
            SendAndReceive<object>(
                new NvimRequest
                {
                    Method = "nvim_get_option_value",
                    Arguments = GetRequestArguments(@name, @opts),
                }
            );

        /// <summary>
        /// <para>
        /// Sets the value of an option. The behavior of this function matches that of |:set|: for global-local options, both the global and local value are set unless otherwise specified with {scope}.
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
        /// <param name="opts">
        /// <para>
        /// Optional parameters
        /// </para>
        /// </param>
        public Task SetOptionValue(string @name, object @value, IDictionary @opts) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_set_option_value",
                    Arguments = GetRequestArguments(@name, @value, @opts),
                }
            );

        /// <summary>
        /// <para>
        /// Gets the option information for all options.
        /// </para>
        /// </summary>
        /// <returns>
        /// <para>
        /// dict of all options
        /// </para>
        /// </returns>
        public Task<IDictionary> GetAllOptionsInfo() =>
            SendAndReceive<IDictionary>(
                new NvimRequest
                {
                    Method = "nvim_get_all_options_info",
                    Arguments = GetRequestArguments(),
                }
            );

        public Task<IDictionary> GetOptionInfo2(string @name, IDictionary @opts) =>
            SendAndReceive<IDictionary>(
                new NvimRequest
                {
                    Method = "nvim_get_option_info2",
                    Arguments = GetRequestArguments(@name, @opts),
                }
            );

        /// <summary>
        /// <para>
        /// Opens a new tabpage.
        /// </para>
        /// </summary>
        /// <param name="buf">
        /// <para>
        /// Buffer to open in the first window of the new tabpage. Use 0 for current buffer.
        /// </para>
        /// </param>
        /// <param name="enter">
        /// <para>
        /// Enter the tabpage (make it the current tabpage).
        /// </para>
        /// </param>
        /// <param name="config">
        /// <para>
        /// Configuration for the new tabpage. Keys:
        /// </para>
        /// </param>
        /// <returns>
        /// <para>
        /// |tab-ID| of the new tabpage
        /// </para>
        /// </returns>
        public Task<NvimTabpage> OpenTabpage(NvimBuffer @buf, bool @enter, IDictionary @config) =>
            SendAndReceive<NvimTabpage>(
                new NvimRequest
                {
                    Method = "nvim_open_tabpage",
                    Arguments = GetRequestArguments(@buf, @enter, @config),
                }
            );

        /// <summary>
        /// <para>
        /// Activates UI events on the channel.
        /// </para>
        /// </summary>
        /// <param name="width">
        /// <para>
        /// Requested screen columns
        /// </para>
        /// </param>
        /// <param name="height">
        /// <para>
        /// Requested screen rows
        /// </para>
        /// </param>
        /// <param name="options">
        /// <para>
        /// |ui-option| map
        /// </para>
        /// </param>
        /// <remarks>
        /// If multiple UI clients are attached, the global screen dimensions degrade to the smallest client. E.g. if client A requests 80x40 but client B requests 200x100, the global screen has size 80x40.
        /// </remarks>
        public Task UiAttach(long @width, long @height, IDictionary @options) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_ui_attach",
                    Arguments = GetRequestArguments(@width, @height, @options),
                }
            );

        public Task UiSetFocus(bool @gained) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_ui_set_focus",
                    Arguments = GetRequestArguments(@gained),
                }
            );

        /// <summary>
        /// <para>
        /// Deactivates UI events on the channel.
        /// </para>
        /// </summary>
        public Task UiDetach() =>
            SendAndReceive(
                new NvimRequest { Method = "nvim_ui_detach", Arguments = GetRequestArguments() }
            );

        public Task UiTryResize(long @width, long @height) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_ui_try_resize",
                    Arguments = GetRequestArguments(@width, @height),
                }
            );

        public Task UiSetOption(string @name, object @value) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_ui_set_option",
                    Arguments = GetRequestArguments(@name, @value),
                }
            );

        /// <summary>
        /// <para>
        /// Tell Nvim to resize a grid. Triggers a grid_resize event with the requested grid size or the maximum size if it exceeds size limits.
        /// </para>
        /// </summary>
        /// <param name="grid">
        /// <para>
        /// The handle of the grid to be changed.
        /// </para>
        /// </param>
        /// <param name="width">
        /// <para>
        /// The new requested width.
        /// </para>
        /// </param>
        /// <param name="height">
        /// <para>
        /// The new requested height.
        /// </para>
        /// </param>
        public Task UiTryResizeGrid(long @grid, long @width, long @height) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_ui_try_resize_grid",
                    Arguments = GetRequestArguments(@grid, @width, @height),
                }
            );

        /// <summary>
        /// <para>
        /// Tells Nvim the number of elements displaying in the popupmenu, to decide [&lt;PageUp&gt;] and [&lt;PageDown&gt;] movement.
        /// </para>
        /// </summary>
        /// <param name="height">
        /// <para>
        /// Popupmenu height, must be greater than zero.
        /// </para>
        /// </param>
        public Task UiPumSetHeight(long @height) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_ui_pum_set_height",
                    Arguments = GetRequestArguments(@height),
                }
            );

        /// <summary>
        /// <para>
        /// Tells Nvim the geometry of the popupmenu, to align floating windows with an external popup menu.
        /// </para>
        /// </summary>
        /// <param name="width">
        /// <para>
        /// Popupmenu width.
        /// </para>
        /// </param>
        /// <param name="height">
        /// <para>
        /// Popupmenu height.
        /// </para>
        /// </param>
        /// <param name="row">
        /// <para>
        /// Popupmenu row.
        /// </para>
        /// </param>
        /// <param name="col">
        /// <para>
        /// Popupmenu height.
        /// </para>
        /// </param>
        public Task UiPumSetBounds(double @width, double @height, double @row, double @col) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_ui_pum_set_bounds",
                    Arguments = GetRequestArguments(@width, @height, @row, @col),
                }
            );

        /// <summary>
        /// <para>
        /// Sends arbitrary data to a UI. Use this instead of |nvim_chan_send()| or
        /// </para>
        /// </summary>
        /// <param name="content">
        /// <para>
        /// Content to write to the TTY
        /// </para>
        /// </param>
        public Task UiSend(string @content) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_ui_send",
                    Arguments = GetRequestArguments(@content),
                }
            );

        /// <summary>
        /// <para>
        /// Gets a highlight group by name
        /// </para>
        /// </summary>
        public Task<long> GetHlIdByName(string @name) =>
            SendAndReceive<long>(
                new NvimRequest
                {
                    Method = "nvim_get_hl_id_by_name",
                    Arguments = GetRequestArguments(@name),
                }
            );

        public Task<IDictionary> GetHl(long @nsId, IDictionary @opts) =>
            SendAndReceive<IDictionary>(
                new NvimRequest
                {
                    Method = "nvim_get_hl",
                    Arguments = GetRequestArguments(@nsId, @opts),
                }
            );

        /// <summary>
        /// <para>
        /// Sets a highlight group. By default, replaces the entire definition (e.g.
        /// </para>
        /// </summary>
        /// <param name="nsId">
        /// <para>
        /// Namespace id for this highlight |nvim_create_namespace()|. Use 0 to set a highlight group globally |:highlight|. Highlights from non-global namespaces are not active by default, use |nvim_set_hl_ns()| or |nvim_win_set_hl_ns()| to activate them.
        /// </para>
        /// </param>
        /// <param name="name">
        /// <para>
        /// Highlight group name, e.g. &quot;ErrorMsg&quot;
        /// </para>
        /// </param>
        /// <param name="val">
        /// <para>
        /// Highlight definition map, accepts the following keys:
        /// </para>
        /// </param>
        /// <remarks>
        /// The fg and bg keys also accept the string values &quot;fg&quot; or &quot;bg&quot; which act as aliases to the corresponding foreground and background values of the Normal group. If the Normal group has not been defined, using these values results in an error.
        /// </remarks>
        public Task SetHl(long @nsId, string @name, IDictionary @val) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_set_hl",
                    Arguments = GetRequestArguments(@nsId, @name, @val),
                }
            );

        /// <summary>
        /// <para>
        /// Gets the active highlight namespace.
        /// </para>
        /// </summary>
        /// <param name="opts">
        /// <para>
        /// Optional parameters
        /// </para>
        /// </param>
        /// <returns>
        /// <para>
        /// Namespace id, or -1
        /// </para>
        /// </returns>
        public Task<long> GetHlNs(IDictionary @opts) =>
            SendAndReceive<long>(
                new NvimRequest
                {
                    Method = "nvim_get_hl_ns",
                    Arguments = GetRequestArguments(@opts),
                }
            );

        /// <summary>
        /// <para>
        /// Set active namespace for highlights defined with |nvim_set_hl()|. This can be set for a single window, see |nvim_win_set_hl_ns()|.
        /// </para>
        /// </summary>
        /// <param name="nsId">
        /// <para>
        /// the namespace to use
        /// </para>
        /// </param>
        public Task SetHlNs(long @nsId) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_set_hl_ns",
                    Arguments = GetRequestArguments(@nsId),
                }
            );

        /// <summary>
        /// <para>
        /// Set active namespace for highlights defined with |nvim_set_hl()| while redrawing.
        /// </para>
        /// </summary>
        /// <param name="nsId">
        /// <para>
        /// the namespace to activate
        /// </para>
        /// </param>
        public Task SetHlNsFast(long @nsId) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_set_hl_ns_fast",
                    Arguments = GetRequestArguments(@nsId),
                }
            );

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
        /// <param name="escapeKs">
        /// <para>
        /// If true, escape K_SPECIAL bytes in
        /// </para>
        /// </param>
        public Task Feedkeys(string @keys, string @mode, bool @escapeKs) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_feedkeys",
                    Arguments = GetRequestArguments(@keys, @mode, @escapeKs),
                }
            );

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
        /// |keycodes| like [&lt;CR&gt;] are translated, so &quot;&lt;&quot; is special. To input a literal &quot;&lt;&quot;, send [&lt;LT&gt;].
        /// </remarks>
        public Task<long> Input(string @keys) =>
            SendAndReceive<long>(
                new NvimRequest { Method = "nvim_input", Arguments = GetRequestArguments(@keys) }
            );

        /// <summary>
        /// <para>
        /// Send mouse event from GUI.
        /// </para>
        /// </summary>
        /// <param name="button">
        /// <para>
        /// Mouse button: one of &quot;left&quot;, &quot;right&quot;, &quot;middle&quot;, &quot;wheel&quot;, &quot;move&quot;, &quot;x1&quot;, &quot;x2&quot;.
        /// </para>
        /// </param>
        /// <param name="action">
        /// <para>
        /// For ordinary buttons, one of &quot;press&quot;, &quot;drag&quot;, &quot;release&quot;. For the wheel, one of &quot;up&quot;, &quot;down&quot;, &quot;left&quot;, &quot;right&quot;. Ignored for &quot;move&quot;.
        /// </para>
        /// </param>
        /// <param name="modifier">
        /// <para>
        /// String of modifiers each represented by a single char. The same specifiers are used as for a key press, except that the &quot;-&quot; separator is optional, so &quot;C-A-&quot;, &quot;c-a&quot; and &quot;CA&quot; can all be used to specify Ctrl+Alt+click.
        /// </para>
        /// </param>
        /// <param name="grid">
        /// <para>
        /// Grid number (used by |ui-multigrid| client), or 0 to let Nvim decide positioning of windows. For more information, see [dev-ui-multigrid]
        /// </para>
        /// </param>
        /// <param name="row">
        /// <para>
        /// Mouse row-position (zero-based, like redraw events)
        /// </para>
        /// </param>
        /// <param name="col">
        /// <para>
        /// Mouse column-position (zero-based, like redraw events)
        /// </para>
        /// </param>
        /// <remarks>
        /// Currently this doesn&apos;t support &quot;scripting&quot; multiple mouse events by calling it multiple times in a loop: the intermediate mouse positions will be ignored. It should be used to implement real-time mouse input in a GUI. The deprecated pseudokey form (&lt;LeftMouse&gt;&lt;col,row&gt;) of |nvim_input()| has the same limitation.
        /// </remarks>
        public Task InputMouse(
            string @button,
            string @action,
            string @modifier,
            long @grid,
            long @row,
            long @col
        ) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_input_mouse",
                    Arguments = GetRequestArguments(@button, @action, @modifier, @grid, @row, @col),
                }
            );

        /// <summary>
        /// <para>
        /// Replaces terminal codes and |keycodes| ([&lt;CR&gt;], [&lt;Esc&gt;], ...) in a string with the internal representation.
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
        /// Also translate [&lt;lt&gt;]. Ignored if
        /// </para>
        /// </param>
        /// <param name="special">
        /// <para>
        /// Replace |keycodes|, e.g. [&lt;CR&gt;] becomes a &quot;\r&quot; char.
        /// </para>
        /// </param>
        /// <remarks>
        /// Lua can use |vim.keycode()| instead.
        /// </remarks>
        public Task<string> ReplaceTermcodes(
            string @str,
            bool @fromPart,
            bool @doLt,
            bool @special
        ) =>
            SendAndReceive<string>(
                new NvimRequest
                {
                    Method = "nvim_replace_termcodes",
                    Arguments = GetRequestArguments(@str, @fromPart, @doLt, @special),
                }
            );

        /// <summary>
        /// <para>
        /// Executes Lua code. Arguments are available as
        /// </para>
        /// </summary>
        /// <param name="code">
        /// <para>
        /// Lua code to execute.
        /// </para>
        /// </param>
        /// <param name="args">
        /// <para>
        /// Arguments to the Lua code.
        /// </para>
        /// </param>
        /// <returns>
        /// <para>
        /// Value returned by the Lua code (if any), or NIL.
        /// </para>
        /// </returns>
        public Task<object> ExecLua(string @code, object[] @args) =>
            SendAndReceive<object>(
                new NvimRequest
                {
                    Method = "nvim_exec_lua",
                    Arguments = GetRequestArguments(@code, @args),
                }
            );

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
            SendAndReceive<long>(
                new NvimRequest { Method = "nvim_strwidth", Arguments = GetRequestArguments(@text) }
            );

        /// <summary>
        /// <para>
        /// Gets the paths contained in |runtime-search-path|.
        /// </para>
        /// </summary>
        /// <returns>
        /// <para>
        /// List of paths
        /// </para>
        /// </returns>
        public Task<object[]> ListRuntimePaths() =>
            SendAndReceive<object[]>(
                new NvimRequest
                {
                    Method = "nvim_list_runtime_paths",
                    Arguments = GetRequestArguments(),
                }
            );

        /// <summary>
        /// <para>
        /// Finds files in runtime directories, in &apos;runtimepath&apos; order.
        /// </para>
        /// </summary>
        /// <param name="name">
        /// <para>
        /// pattern of files to search for
        /// </para>
        /// </param>
        /// <param name="all">
        /// <para>
        /// whether to return all matches or only the first
        /// </para>
        /// </param>
        /// <returns>
        /// <para>
        /// list of absolute paths to the found files
        /// </para>
        /// </returns>
        public Task<object[]> GetRuntimeFile(string @name, bool @all) =>
            SendAndReceive<object[]>(
                new NvimRequest
                {
                    Method = "nvim_get_runtime_file",
                    Arguments = GetRequestArguments(@name, @all),
                }
            );

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
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_set_current_dir",
                    Arguments = GetRequestArguments(@dir),
                }
            );

        /// <summary>
        /// <para>
        /// Gets the current line.
        /// </para>
        /// </summary>
        /// <returns>
        /// <para>
        /// Current line string
        /// </para>
        /// </returns>
        public Task<string> GetCurrentLine() =>
            SendAndReceive<string>(
                new NvimRequest
                {
                    Method = "nvim_get_current_line",
                    Arguments = GetRequestArguments(),
                }
            );

        /// <summary>
        /// <para>
        /// Sets the text on the current line.
        /// </para>
        /// </summary>
        /// <param name="line">
        /// <para>
        /// Line contents
        /// </para>
        /// </param>
        public Task SetCurrentLine(string @line) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_set_current_line",
                    Arguments = GetRequestArguments(@line),
                }
            );

        /// <summary>
        /// <para>
        /// Deletes the current line.
        /// </para>
        /// </summary>
        public Task DelCurrentLine() =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_del_current_line",
                    Arguments = GetRequestArguments(),
                }
            );

        /// <summary>
        /// <para>
        /// Gets a global (g:) variable.
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
            SendAndReceive<object>(
                new NvimRequest { Method = "nvim_get_var", Arguments = GetRequestArguments(@name) }
            );

        /// <summary>
        /// <para>
        /// Sets a global (g:) variable.
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
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_set_var",
                    Arguments = GetRequestArguments(@name, @value),
                }
            );

        /// <summary>
        /// <para>
        /// Removes a global (g:) variable.
        /// </para>
        /// </summary>
        /// <param name="name">
        /// <para>
        /// Variable name
        /// </para>
        /// </param>
        public Task DelVar(string @name) =>
            SendAndReceive(
                new NvimRequest { Method = "nvim_del_var", Arguments = GetRequestArguments(@name) }
            );

        /// <summary>
        /// <para>
        /// Gets a v: variable.
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
            SendAndReceive<object>(
                new NvimRequest { Method = "nvim_get_vvar", Arguments = GetRequestArguments(@name) }
            );

        /// <summary>
        /// <para>
        /// Sets a v: variable, if it is not readonly.
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
        public Task SetVvar(string @name, object @value) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_set_vvar",
                    Arguments = GetRequestArguments(@name, @value),
                }
            );

        public Task<object> Echo(object[] @chunks, bool @history, IDictionary @opts) =>
            SendAndReceive<object>(
                new NvimRequest
                {
                    Method = "nvim_echo",
                    Arguments = GetRequestArguments(@chunks, @history, @opts),
                }
            );

        /// <summary>
        /// <para>
        /// Gets the current list of buffers.
        /// </para>
        /// </summary>
        /// <returns>
        /// <para>
        /// List of buffer ids
        /// </para>
        /// </returns>
        public Task<object[]> ListBufs() =>
            SendAndReceive<object[]>(
                new NvimRequest { Method = "nvim_list_bufs", Arguments = GetRequestArguments() }
            );

        /// <summary>
        /// <para>
        /// Gets the current buffer.
        /// </para>
        /// </summary>
        /// <returns>
        /// <para>
        /// Buffer id
        /// </para>
        /// </returns>
        public Task<NvimBuffer> GetCurrentBuf() =>
            SendAndReceive<NvimBuffer>(
                new NvimRequest
                {
                    Method = "nvim_get_current_buf",
                    Arguments = GetRequestArguments(),
                }
            );

        /// <summary>
        /// <para>
        /// Sets the current window&apos;s buffer to
        /// </para>
        /// </summary>
        /// <param name="buf">
        /// <para>
        /// Buffer id
        /// </para>
        /// </param>
        public Task SetCurrentBuf(NvimBuffer @buf) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_set_current_buf",
                    Arguments = GetRequestArguments(@buf),
                }
            );

        /// <summary>
        /// <para>
        /// Gets the current list of all |window-ID|s in all tabpages.
        /// </para>
        /// </summary>
        /// <returns>
        /// <para>
        /// List of |window-ID|s
        /// </para>
        /// </returns>
        public Task<object[]> ListWins() =>
            SendAndReceive<object[]>(
                new NvimRequest { Method = "nvim_list_wins", Arguments = GetRequestArguments() }
            );

        /// <summary>
        /// <para>
        /// Gets the current window.
        /// </para>
        /// </summary>
        /// <returns>
        /// <para>
        /// |window-ID|
        /// </para>
        /// </returns>
        public Task<NvimWindow> GetCurrentWin() =>
            SendAndReceive<NvimWindow>(
                new NvimRequest
                {
                    Method = "nvim_get_current_win",
                    Arguments = GetRequestArguments(),
                }
            );

        /// <summary>
        /// <para>
        /// Navigates to the given window (and tabpage, implicitly).
        /// </para>
        /// </summary>
        /// <param name="win">
        /// <para>
        /// |window-ID| to focus
        /// </para>
        /// </param>
        public Task SetCurrentWin(NvimWindow @win) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_set_current_win",
                    Arguments = GetRequestArguments(@win),
                }
            );

        /// <summary>
        /// <para>
        /// Creates a new, empty, unnamed buffer.
        /// </para>
        /// </summary>
        /// <param name="listed">
        /// <para>
        /// Sets &apos;buflisted&apos;
        /// </para>
        /// </param>
        /// <param name="scratch">
        /// <para>
        /// Creates a &quot;throwaway&quot; |scratch-buffer| for temporary work (always &apos;nomodified&apos;). Also sets &apos;nomodeline&apos; on the buffer.
        /// </para>
        /// </param>
        /// <returns>
        /// <para>
        /// Buffer id, or 0 on error
        /// </para>
        /// </returns>
        public Task<NvimBuffer> CreateBuf(bool @listed, bool @scratch) =>
            SendAndReceive<NvimBuffer>(
                new NvimRequest
                {
                    Method = "nvim_create_buf",
                    Arguments = GetRequestArguments(@listed, @scratch),
                }
            );

        /// <summary>
        /// <para>
        /// Open a terminal instance in a buffer
        /// </para>
        /// </summary>
        /// <param name="buf">
        /// <para>
        /// Buffer to use. Buffer contents (if any) will be written to the PTY.
        /// </para>
        /// </param>
        /// <param name="opts">
        /// <para>
        /// Optional parameters.
        /// </para>
        /// </param>
        /// <returns>
        /// <para>
        /// Channel id, or 0 on error
        /// </para>
        /// </returns>
        public Task<long> OpenTerm(NvimBuffer @buf, IDictionary @opts) =>
            SendAndReceive<long>(
                new NvimRequest
                {
                    Method = "nvim_open_term",
                    Arguments = GetRequestArguments(@buf, @opts),
                }
            );

        /// <summary>
        /// <para>
        /// Sends raw data to channel
        /// </para>
        /// </summary>
        /// <param name="chan">
        /// <para>
        /// Channel id
        /// </para>
        /// </param>
        /// <param name="data">
        /// <para>
        /// Data to write. 8-bit clean: may contain NUL bytes.
        /// </para>
        /// </param>
        public Task ChanSend(long @chan, string @data) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_chan_send",
                    Arguments = GetRequestArguments(@chan, @data),
                }
            );

        /// <summary>
        /// <para>
        /// Gets the current list of |tab-ID|s.
        /// </para>
        /// </summary>
        /// <returns>
        /// <para>
        /// List of |tab-ID|s
        /// </para>
        /// </returns>
        public Task<object[]> ListTabpages() =>
            SendAndReceive<object[]>(
                new NvimRequest { Method = "nvim_list_tabpages", Arguments = GetRequestArguments() }
            );

        /// <summary>
        /// <para>
        /// Gets the current tabpage.
        /// </para>
        /// </summary>
        /// <returns>
        /// <para>
        /// |tab-ID|
        /// </para>
        /// </returns>
        public Task<NvimTabpage> GetCurrentTabpage() =>
            SendAndReceive<NvimTabpage>(
                new NvimRequest
                {
                    Method = "nvim_get_current_tabpage",
                    Arguments = GetRequestArguments(),
                }
            );

        /// <summary>
        /// <para>
        /// Sets the current tabpage.
        /// </para>
        /// </summary>
        /// <param name="tabpage">
        /// <para>
        /// |tab-ID| to focus
        /// </para>
        /// </param>
        public Task SetCurrentTabpage(NvimTabpage @tabpage) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_set_current_tabpage",
                    Arguments = GetRequestArguments(@tabpage),
                }
            );

        /// <summary>
        /// <para>
        /// Pastes at cursor (in any mode), and sets &quot;redo&quot; so dot (|.|) will repeat the input. UIs call this to implement &quot;paste&quot;, but it&apos;s also intended for use by scripts to input large, dot-repeatable blocks of text (as opposed to |nvim_input()| which is subject to mappings/events and is thus much slower).
        /// </para>
        /// </summary>
        /// <param name="data">
        /// <para>
        /// Multiline input. Lines break at LF (&quot;\n&quot;). May be binary (containing NUL bytes).
        /// </para>
        /// </param>
        /// <param name="crlf">
        /// <para>
        /// Also break lines at CR and CRLF.
        /// </para>
        /// </param>
        /// <param name="phase">
        /// <para>
        /// -1: paste in a single call (i.e. without streaming). To &quot;stream&quot; a paste, call
        /// </para>
        /// </param>
        /// <returns>
        /// <para>
        /// <list type="bullet">
        /// <item><description>
        /// true: Client may continue pasting.
        /// </description></item>
        /// </list>
        /// </para>
        /// </returns>
        public Task<bool> Paste(string @data, bool @crlf, long @phase) =>
            SendAndReceive<bool>(
                new NvimRequest
                {
                    Method = "nvim_paste",
                    Arguments = GetRequestArguments(@data, @crlf, @phase),
                }
            );

        /// <summary>
        /// <para>
        /// Puts text at cursor, in any mode. For dot-repeatable input, use |nvim_paste()|.
        /// </para>
        /// </summary>
        /// <param name="lines">
        /// <para>
        /// |readfile()|-style list of lines. |channel-lines|
        /// </para>
        /// </param>
        /// <param name="type">
        /// <para>
        /// Edit behavior: any |getregtype()| result, or:
        /// </para>
        /// </param>
        /// <param name="after">
        /// <para>
        /// If true insert after cursor (like |p|), or before (like |P|).
        /// </para>
        /// </param>
        /// <param name="follow">
        /// <para>
        /// If true place cursor at end of inserted text.
        /// </para>
        /// </param>
        public Task Put(object[] @lines, string @type, bool @after, bool @follow) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_put",
                    Arguments = GetRequestArguments(@lines, @type, @after, @follow),
                }
            );

        /// <summary>
        /// <para>
        /// Returns the 24-bit RGB value of a |nvim_get_color_map()| color name or &quot;#rrggbb&quot; hexadecimal string.
        /// </para>
        /// </summary>
        /// <param name="name">
        /// <para>
        /// Color name or &quot;#rrggbb&quot; string
        /// </para>
        /// </param>
        /// <returns>
        /// <para>
        /// 24-bit RGB value, or -1 for invalid argument.
        /// </para>
        /// </returns>
        public Task<long> GetColorByName(string @name) =>
            SendAndReceive<long>(
                new NvimRequest
                {
                    Method = "nvim_get_color_by_name",
                    Arguments = GetRequestArguments(@name),
                }
            );

        public Task<IDictionary> GetColorMap() =>
            SendAndReceive<IDictionary>(
                new NvimRequest { Method = "nvim_get_color_map", Arguments = GetRequestArguments() }
            );

        /// <summary>
        /// <para>
        /// Gets a map of the current editor state.
        /// </para>
        /// </summary>
        /// <param name="opts">
        /// <para>
        /// Optional parameters.
        /// </para>
        /// </param>
        /// <returns>
        /// <para>
        /// map of global |context|.
        /// </para>
        /// </returns>
        public Task<IDictionary> GetContext(IDictionary @opts) =>
            SendAndReceive<IDictionary>(
                new NvimRequest
                {
                    Method = "nvim_get_context",
                    Arguments = GetRequestArguments(@opts),
                }
            );

        /// <summary>
        /// <para>
        /// Sets the current editor state from the given |context| map.
        /// </para>
        /// </summary>
        /// <param name="dict">
        /// <para>
        /// |Context| map.
        /// </para>
        /// </param>
        public Task<object> LoadContext(IDictionary @dict) =>
            SendAndReceive<object>(
                new NvimRequest
                {
                    Method = "nvim_load_context",
                    Arguments = GetRequestArguments(@dict),
                }
            );

        public Task<IDictionary> GetMode() =>
            SendAndReceive<IDictionary>(
                new NvimRequest { Method = "nvim_get_mode", Arguments = GetRequestArguments() }
            );

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
        /// Array of |maparg()|-like dictionaries describing mappings. The &quot;buf&quot; key is always zero.
        /// </para>
        /// </returns>
        public Task<object[]> GetKeymap(string @mode) =>
            SendAndReceive<object[]>(
                new NvimRequest
                {
                    Method = "nvim_get_keymap",
                    Arguments = GetRequestArguments(@mode),
                }
            );

        /// <summary>
        /// <para>
        /// Sets a global |mapping| for the given mode.
        /// </para>
        /// </summary>
        /// <param name="mode">
        /// <para>
        /// Mode short-name (map command prefix: &quot;n&quot;, &quot;i&quot;, &quot;v&quot;, &quot;x&quot;, …) or &quot;!&quot; for |:map!|, or empty string for |:map|. &quot;ia&quot;, &quot;ca&quot; or &quot;!a&quot; for abbreviation in Insert mode, Cmdline mode, or both, respectively
        /// </para>
        /// </param>
        /// <param name="lhs">
        /// <para>
        /// Left-hand-side |{lhs}| of the mapping.
        /// </para>
        /// </param>
        /// <param name="rhs">
        /// <para>
        /// Right-hand-side |{rhs}| of the mapping.
        /// </para>
        /// </param>
        /// <param name="opts">
        /// <para>
        /// Optional parameters map: Accepts all |:map-arguments| as keys except [&lt;buffer&gt;], values are booleans (default false). Also:
        /// </para>
        /// </param>
        public Task SetKeymap(string @mode, string @lhs, string @rhs, IDictionary @opts) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_set_keymap",
                    Arguments = GetRequestArguments(@mode, @lhs, @rhs, @opts),
                }
            );

        /// <summary>
        /// <para>
        /// Unmaps a global |mapping| for the given mode.
        /// </para>
        /// </summary>
        public Task DelKeymap(string @mode, string @lhs) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_del_keymap",
                    Arguments = GetRequestArguments(@mode, @lhs),
                }
            );

        /// <summary>
        /// <para>
        /// Returns a 2-tuple (Array), where item 0 is the current channel id and item 1 is the |api-metadata| map (
        /// </para>
        /// </summary>
        /// <returns>
        /// <para>
        /// 2-tuple
        /// </para>
        /// </returns>
        public Task<object[]> GetApiInfo() =>
            SendAndReceive<object[]>(
                new NvimRequest { Method = "nvim_get_api_info", Arguments = GetRequestArguments() }
            );

        /// <summary>
        /// <para>
        /// Self-identifies the client, and sets optional flags on the channel. Defines the
        /// </para>
        /// </summary>
        /// <param name="name">
        /// <para>
        /// Client short-name. Sets the
        /// </para>
        /// </param>
        /// <param name="version">
        /// <para>
        /// Dict
        /// </para>
        /// </param>
        /// <param name="type">
        /// <para>
        /// Must be one of the following values. Client libraries should default to &quot;remote&quot; unless overridden by the user.
        /// </para>
        /// </param>
        /// <param name="methods">
        /// <para>
        /// Builtin methods in the client. For a host, this does not include plugin methods which will be discovered later. The key should be the method name, the values are dicts with these (optional) keys (more keys may be added in future versions of Nvim, thus unknown keys are ignored. Clients must only use keys defined in this or later versions of Nvim):
        /// </para>
        /// </param>
        /// <param name="attributes">
        /// <para>
        /// Arbitrary string:string map of informal client properties. Suggested keys:
        /// </para>
        /// </param>
        public Task SetClientInfo(
            string @name,
            IDictionary @version,
            string @type,
            IDictionary @methods,
            IDictionary @attributes
        ) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_set_client_info",
                    Arguments = GetRequestArguments(@name, @version, @type, @methods, @attributes),
                }
            );

        /// <summary>
        /// <para>
        /// Gets information about a channel.
        /// </para>
        /// </summary>
        /// <param name="chan">
        /// <para>
        /// channel_id, or 0 for current channel
        /// </para>
        /// </param>
        /// <returns>
        /// <para>
        /// Channel info dict with these keys:
        /// </para>
        /// </returns>
        public Task<IDictionary> GetChanInfo(long @chan) =>
            SendAndReceive<IDictionary>(
                new NvimRequest
                {
                    Method = "nvim_get_chan_info",
                    Arguments = GetRequestArguments(@chan),
                }
            );

        /// <summary>
        /// <para>
        /// Get information about all open channels.
        /// </para>
        /// </summary>
        /// <returns>
        /// <para>
        /// Array of Dictionaries, each describing a channel with the format specified at |nvim_get_chan_info()|.
        /// </para>
        /// </returns>
        public Task<object[]> ListChans() =>
            SendAndReceive<object[]>(
                new NvimRequest { Method = "nvim_list_chans", Arguments = GetRequestArguments() }
            );

        /// <summary>
        /// <para>
        /// Gets a list of dictionaries representing attached UIs.
        /// </para>
        /// </summary>
        /// <returns>
        /// <para>
        /// Array of UI dictionaries, each with these keys:
        /// </para>
        /// </returns>
        public Task<object[]> ListUis() =>
            SendAndReceive<object[]>(
                new NvimRequest { Method = "nvim_list_uis", Arguments = GetRequestArguments() }
            );

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
            SendAndReceive<object[]>(
                new NvimRequest
                {
                    Method = "nvim_get_proc_children",
                    Arguments = GetRequestArguments(@pid),
                }
            );

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
            SendAndReceive<object>(
                new NvimRequest { Method = "nvim_get_proc", Arguments = GetRequestArguments(@pid) }
            );

        /// <summary>
        /// <para>
        /// Selects an item in the completion popup menu.
        /// </para>
        /// </summary>
        /// <param name="item">
        /// <para>
        /// Index (zero-based) of the item to select. Value of -1 selects nothing and restores the original text.
        /// </para>
        /// </param>
        /// <param name="insert">
        /// <para>
        /// For |ins-completion|, whether the selection should be inserted in the buffer. Ignored for |cmdline-completion|.
        /// </para>
        /// </param>
        /// <param name="finish">
        /// <para>
        /// Finish the completion and dismiss the popup menu. Implies {insert}.
        /// </para>
        /// </param>
        /// <param name="opts">
        /// <para>
        /// Optional parameters. Reserved for future use.
        /// </para>
        /// </param>
        public Task SelectPopupmenuItem(
            long @item,
            bool @insert,
            bool @finish,
            IDictionary @opts
        ) =>
            SendAndReceive(
                new NvimRequest
                {
                    Method = "nvim_select_popupmenu_item",
                    Arguments = GetRequestArguments(@item, @insert, @finish, @opts),
                }
            );

        /// <summary>
        /// <para>
        /// Deletes an uppercase/file named mark. See |mark-motions|.
        /// </para>
        /// </summary>
        /// <param name="name">
        /// <para>
        /// Mark name
        /// </para>
        /// </param>
        /// <returns>
        /// <para>
        /// true if the mark was deleted, else false.
        /// </para>
        /// </returns>
        /// <remarks>
        /// Lowercase name (or other buffer-local mark) is an error.
        /// </remarks>
        public Task<bool> DelMark(string @name) =>
            SendAndReceive<bool>(
                new NvimRequest { Method = "nvim_del_mark", Arguments = GetRequestArguments(@name) }
            );

        public Task<object[]> GetMark(string @name, IDictionary @opts) =>
            SendAndReceive<object[]>(
                new NvimRequest
                {
                    Method = "nvim_get_mark",
                    Arguments = GetRequestArguments(@name, @opts),
                }
            );

        public Task<IDictionary> EvalStatusline(string @str, IDictionary @opts) =>
            SendAndReceive<IDictionary>(
                new NvimRequest
                {
                    Method = "nvim_eval_statusline",
                    Arguments = GetRequestArguments(@str, @opts),
                }
            );

        /// <summary>
        /// <para>
        /// Executes Vimscript (multiline block of Ex commands), like anonymous |:source|.
        /// </para>
        /// </summary>
        /// <param name="src">
        /// <para>
        /// Vimscript code
        /// </para>
        /// </param>
        /// <param name="opts">
        /// <para>
        /// Optional parameters.
        /// </para>
        /// </param>
        /// <returns>
        /// <para>
        /// Dict
        /// </para>
        /// </returns>
        public Task<IDictionary> Exec2(string @src, IDictionary @opts) =>
            SendAndReceive<IDictionary>(
                new NvimRequest
                {
                    Method = "nvim_exec2",
                    Arguments = GetRequestArguments(@src, @opts),
                }
            );

        /// <summary>
        /// <para>
        /// Executes an Ex command.
        /// </para>
        /// </summary>
        /// <param name="cmd">
        /// <para>
        /// Ex command string
        /// </para>
        /// </param>
        public Task Command(string @cmd) =>
            SendAndReceive(
                new NvimRequest { Method = "nvim_command", Arguments = GetRequestArguments(@cmd) }
            );

        /// <summary>
        /// <para>
        /// Evaluates a Vimscript |expression|. Dicts and Lists are recursively expanded.
        /// </para>
        /// </summary>
        /// <param name="expr">
        /// <para>
        /// Vimscript expression string
        /// </para>
        /// </param>
        /// <returns>
        /// <para>
        /// Evaluation result or expanded object
        /// </para>
        /// </returns>
        public Task<object> Eval(string @expr) =>
            SendAndReceive<object>(
                new NvimRequest { Method = "nvim_eval", Arguments = GetRequestArguments(@expr) }
            );

        /// <summary>
        /// <para>
        /// Calls a Vimscript function with the given arguments.
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
            SendAndReceive<object>(
                new NvimRequest
                {
                    Method = "nvim_call_function",
                    Arguments = GetRequestArguments(@fn, @args),
                }
            );

        /// <summary>
        /// <para>
        /// Calls a Vimscript |Dictionary-function| with the given arguments.
        /// </para>
        /// </summary>
        /// <param name="dict">
        /// <para>
        /// Dict
        /// </para>
        /// </param>
        /// <param name="fn">
        /// <para>
        /// Name of the function defined on the Vimscript dict
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
            SendAndReceive<object>(
                new NvimRequest
                {
                    Method = "nvim_call_dict_function",
                    Arguments = GetRequestArguments(@dict, @fn, @args),
                }
            );

        public Task<IDictionary> ParseExpression(string @expr, string @flags, bool @hl) =>
            SendAndReceive<IDictionary>(
                new NvimRequest
                {
                    Method = "nvim_parse_expression",
                    Arguments = GetRequestArguments(@expr, @flags, @hl),
                }
            );

        /// <summary>
        /// <para>
        /// Opens a new split window, floating window, or external window.
        /// </para>
        /// </summary>
        /// <param name="buf">
        /// <para>
        /// Buffer to display, or 0 for current buffer
        /// </para>
        /// </param>
        /// <param name="enter">
        /// <para>
        /// Enter the window (make it the current window)
        /// </para>
        /// </param>
        /// <param name="config">
        /// <para>
        /// Map defining the window configuration. Keys:
        /// </para>
        /// </param>
        /// <returns>
        /// <para>
        /// |window-ID|, or 0 on error
        /// </para>
        /// </returns>
        public Task<NvimWindow> OpenWin(NvimBuffer @buf, bool @enter, IDictionary @config) =>
            SendAndReceive<NvimWindow>(
                new NvimRequest
                {
                    Method = "nvim_open_win",
                    Arguments = GetRequestArguments(@buf, @enter, @config),
                }
            );

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
            /// help
            /// For more information on buffers, see |buffers|.
            ///
            /// Unloaded Buffers: ~
            ///
            /// Buffers may be unloaded by the |:bunload| command or the buffer&apos;s
            /// &apos;bufhidden&apos; option. When a buffer is unloaded its file contents are freed
            /// from memory and vim cannot operate on the buffer lines until it is reloaded
            /// (usually by opening the buffer again in a new window). API methods such as
            /// |nvim_buf_get_lines()| and |nvim_buf_line_count()| will be affected.
            ///
            /// You can use |nvim_buf_is_loaded()| or |nvim_buf_line_count()| to check
            /// whether a buffer is loaded.
            ///
            /// </para>
            /// </summary>
            /// <returns>
            /// <para>
            /// Line count, or 0 for unloaded buffer. |api-buffer|
            /// </para>
            /// </returns>
            public Task<long> LineCount() =>
                _api.SendAndReceive<long>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_line_count",
                        Arguments = GetRequestArguments(_msgPackExtObj),
                    }
                );

            /// <summary>
            /// <para>
            /// Activates |api-buffer-updates| events on a channel, or as Lua callbacks.
            /// </para>
            /// </summary>
            /// <param name="sendBuffer">
            /// <para>
            /// True if the initial notification should contain the whole buffer: first notification will be
            /// </para>
            /// </param>
            /// <param name="opts">
            /// <para>
            /// Optional parameters.
            /// </para>
            /// </param>
            /// <returns>
            /// <para>
            /// False if attach failed (invalid parameter, or buffer isn&apos;t loaded); otherwise True.
            /// </para>
            /// </returns>
            public Task<bool> Attach(bool @sendBuffer, IDictionary @opts) =>
                _api.SendAndReceive<bool>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_attach",
                        Arguments = GetRequestArguments(_msgPackExtObj, @sendBuffer, @opts),
                    }
                );

            /// <summary>
            /// <para>
            /// Deactivates buffer-update events on the channel.
            /// </para>
            /// </summary>
            /// <returns>
            /// <para>
            /// False if detach failed (because the buffer isn&apos;t loaded); otherwise True.
            /// </para>
            /// </returns>
            public Task<bool> Detach() =>
                _api.SendAndReceive<bool>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_detach",
                        Arguments = GetRequestArguments(_msgPackExtObj),
                    }
                );

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
            /// Last line index, exclusive
            /// </para>
            /// </param>
            /// <param name="strictIndexing">
            /// <para>
            /// Whether out-of-bounds should be an error.
            /// </para>
            /// </param>
            /// <returns>
            /// <para>
            /// Array of lines, or empty array for unloaded buffer.
            /// </para>
            /// </returns>
            public Task<object[]> GetLines(long @start, long @end, bool @strictIndexing) =>
                _api.SendAndReceive<object[]>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_get_lines",
                        Arguments = GetRequestArguments(
                            _msgPackExtObj,
                            @start,
                            @end,
                            @strictIndexing
                        ),
                    }
                );

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
            /// Last line index, exclusive
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
            public Task SetLines(
                long @start,
                long @end,
                bool @strictIndexing,
                object[] @replacement
            ) =>
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_buf_set_lines",
                        Arguments = GetRequestArguments(
                            _msgPackExtObj,
                            @start,
                            @end,
                            @strictIndexing,
                            @replacement
                        ),
                    }
                );

            /// <summary>
            /// <para>
            /// Sets (replaces) a range in the buffer
            /// </para>
            /// </summary>
            /// <param name="startRow">
            /// <para>
            /// First line index
            /// </para>
            /// </param>
            /// <param name="startCol">
            /// <para>
            /// Starting column (byte offset) on first line
            /// </para>
            /// </param>
            /// <param name="endRow">
            /// <para>
            /// Last line index, inclusive
            /// </para>
            /// </param>
            /// <param name="endCol">
            /// <para>
            /// Ending column (byte offset) on last line, exclusive
            /// </para>
            /// </param>
            /// <param name="replacement">
            /// <para>
            /// Array of lines to use as replacement
            /// </para>
            /// </param>
            /// <remarks>
            /// Prefer |nvim_buf_set_lines()| (for performance) to add or delete entire lines.
            /// </remarks>
            public Task SetText(
                long @startRow,
                long @startCol,
                long @endRow,
                long @endCol,
                object[] @replacement
            ) =>
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_buf_set_text",
                        Arguments = GetRequestArguments(
                            _msgPackExtObj,
                            @startRow,
                            @startCol,
                            @endRow,
                            @endCol,
                            @replacement
                        ),
                    }
                );

            /// <summary>
            /// <para>
            /// Gets a range from the buffer (may be partial lines, unlike |nvim_buf_get_lines()|).
            /// </para>
            /// </summary>
            /// <param name="startRow">
            /// <para>
            /// First line index
            /// </para>
            /// </param>
            /// <param name="startCol">
            /// <para>
            /// Starting column (byte offset) on first line
            /// </para>
            /// </param>
            /// <param name="endRow">
            /// <para>
            /// Last line index, inclusive
            /// </para>
            /// </param>
            /// <param name="endCol">
            /// <para>
            /// Ending column (byte offset) on last line, exclusive
            /// </para>
            /// </param>
            /// <param name="opts">
            /// <para>
            /// Optional parameters. Currently unused.
            /// </para>
            /// </param>
            /// <returns>
            /// <para>
            /// Array of lines, or empty array for unloaded buffer.
            /// </para>
            /// </returns>
            public Task<object[]> GetText(
                long @startRow,
                long @startCol,
                long @endRow,
                long @endCol,
                IDictionary @opts
            ) =>
                _api.SendAndReceive<object[]>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_get_text",
                        Arguments = GetRequestArguments(
                            _msgPackExtObj,
                            @startRow,
                            @startCol,
                            @endRow,
                            @endCol,
                            @opts
                        ),
                    }
                );

            /// <summary>
            /// <para>
            /// Returns the byte offset of a line (0-indexed). |api-indexing|
            /// </para>
            /// </summary>
            /// <param name="index">
            /// <para>
            /// Line index
            /// </para>
            /// </param>
            /// <returns>
            /// <para>
            /// Integer byte offset, or -1 for unloaded buffer.
            /// </para>
            /// </returns>
            public Task<long> GetOffset(long @index) =>
                _api.SendAndReceive<long>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_get_offset",
                        Arguments = GetRequestArguments(_msgPackExtObj, @index),
                    }
                );

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
                _api.SendAndReceive<object>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_get_var",
                        Arguments = GetRequestArguments(_msgPackExtObj, @name),
                    }
                );

            /// <summary>
            /// <para>
            /// Gets a changed tick of a buffer
            /// </para>
            /// </summary>
            /// <returns>
            /// <para>
            /// <c>b:changedtick</c>
            /// </para>
            /// </returns>
            public Task<long> GetChangedtick() =>
                _api.SendAndReceive<long>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_get_changedtick",
                        Arguments = GetRequestArguments(_msgPackExtObj),
                    }
                );

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
            /// Array of |maparg()|-like dictionaries describing mappings. The &quot;buf&quot; key holds the associated buffer id.
            /// </para>
            /// </returns>
            public Task<object[]> GetKeymap(string @mode) =>
                _api.SendAndReceive<object[]>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_get_keymap",
                        Arguments = GetRequestArguments(_msgPackExtObj, @mode),
                    }
                );

            /// <summary>
            /// <para>
            /// Sets a buffer-local |mapping| for the given mode.
            /// </para>
            /// </summary>
            public Task SetKeymap(string @mode, string @lhs, string @rhs, IDictionary @opts) =>
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_buf_set_keymap",
                        Arguments = GetRequestArguments(_msgPackExtObj, @mode, @lhs, @rhs, @opts),
                    }
                );

            /// <summary>
            /// <para>
            /// Unmaps a buffer-local |mapping| for the given mode.
            /// </para>
            /// </summary>
            public Task DelKeymap(string @mode, string @lhs) =>
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_buf_del_keymap",
                        Arguments = GetRequestArguments(_msgPackExtObj, @mode, @lhs),
                    }
                );

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
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_buf_set_var",
                        Arguments = GetRequestArguments(_msgPackExtObj, @name, @value),
                    }
                );

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
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_buf_del_var",
                        Arguments = GetRequestArguments(_msgPackExtObj, @name),
                    }
                );

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
                _api.SendAndReceive<string>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_get_name",
                        Arguments = GetRequestArguments(_msgPackExtObj),
                    }
                );

            /// <summary>
            /// <para>
            /// Sets the full file name for a buffer, like |:file_f|
            /// </para>
            /// </summary>
            /// <param name="name">
            /// <para>
            /// Buffer name
            /// </para>
            /// </param>
            public Task SetName(string @name) =>
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_buf_set_name",
                        Arguments = GetRequestArguments(_msgPackExtObj, @name),
                    }
                );

            /// <summary>
            /// <para>
            /// Checks if a buffer is valid and loaded. See |api-buffer| for more info about unloaded buffers.
            /// </para>
            /// </summary>
            /// <returns>
            /// <para>
            /// true if the buffer is valid and loaded, false otherwise.
            /// </para>
            /// </returns>
            public Task<bool> IsLoaded() =>
                _api.SendAndReceive<bool>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_is_loaded",
                        Arguments = GetRequestArguments(_msgPackExtObj),
                    }
                );

            /// <summary>
            /// <para>
            /// Deletes a buffer and its metadata (like |:bwipeout|).
            /// </para>
            /// </summary>
            /// <param name="opts">
            /// <para>
            /// Optional parameters. Keys:
            /// </para>
            /// </param>
            public Task Delete(IDictionary @opts) =>
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_buf_delete",
                        Arguments = GetRequestArguments(_msgPackExtObj, @opts),
                    }
                );

            /// <summary>
            /// <para>
            /// Checks if a buffer is valid.
            /// </para>
            /// </summary>
            /// <returns>
            /// <para>
            /// true if the buffer is valid, false otherwise.
            /// </para>
            /// </returns>
            /// <remarks>
            /// Even if a buffer is valid it may have been unloaded. See |api-buffer| for more info about unloaded buffers.
            /// </remarks>
            public Task<bool> IsValid() =>
                _api.SendAndReceive<bool>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_is_valid",
                        Arguments = GetRequestArguments(_msgPackExtObj),
                    }
                );

            /// <summary>
            /// <para>
            /// Deletes a named mark in the buffer. See |mark-motions|.
            /// </para>
            /// </summary>
            /// <param name="name">
            /// <para>
            /// Mark name
            /// </para>
            /// </param>
            /// <returns>
            /// <para>
            /// true if the mark was deleted, else false.
            /// </para>
            /// </returns>
            /// <remarks>
            /// only deletes marks set in the buffer, if the mark is not set in the buffer it will return false.
            /// </remarks>
            public Task<bool> DelMark(string @name) =>
                _api.SendAndReceive<bool>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_del_mark",
                        Arguments = GetRequestArguments(_msgPackExtObj, @name),
                    }
                );

            /// <summary>
            /// <para>
            /// Sets a named mark in the given buffer, all marks are allowed file/uppercase, visual, last change, etc. See |mark-motions|.
            /// </para>
            /// </summary>
            /// <param name="name">
            /// <para>
            /// Mark name
            /// </para>
            /// </param>
            /// <param name="line">
            /// <para>
            /// Line number
            /// </para>
            /// </param>
            /// <param name="col">
            /// <para>
            /// Column/row number
            /// </para>
            /// </param>
            /// <param name="opts">
            /// <para>
            /// Optional parameters. Reserved for future use.
            /// </para>
            /// </param>
            /// <returns>
            /// <para>
            /// true if the mark was set, else false.
            /// </para>
            /// </returns>
            /// <remarks>
            /// Passing 0 as line deletes the mark
            /// </remarks>
            public Task<bool> SetMark(string @name, long @line, long @col, IDictionary @opts) =>
                _api.SendAndReceive<bool>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_set_mark",
                        Arguments = GetRequestArguments(_msgPackExtObj, @name, @line, @col, @opts),
                    }
                );

            /// <summary>
            /// <para>
            /// Returns a
            /// </para>
            /// </summary>
            /// <param name="name">
            /// <para>
            /// Mark name
            /// </para>
            /// </param>
            /// <returns>
            /// <para>
            /// (row, col) tuple, (0, 0) if the mark is not set, or is an uppercase/file mark set in another buffer.
            /// </para>
            /// </returns>
            public Task<object[]> GetMark(string @name) =>
                _api.SendAndReceive<object[]>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_get_mark",
                        Arguments = GetRequestArguments(_msgPackExtObj, @name),
                    }
                );

            /// <summary>
            /// <para>
            /// Creates a buffer-local command |user-commands|.
            /// </para>
            /// </summary>
            public Task CreateUserCommand(string @name, object @cmd, IDictionary @opts) =>
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_buf_create_user_command",
                        Arguments = GetRequestArguments(_msgPackExtObj, @name, @cmd, @opts),
                    }
                );

            /// <summary>
            /// <para>
            /// Delete a buffer-local user-defined command.
            /// </para>
            /// </summary>
            /// <param name="name">
            /// <para>
            /// Name of the command to delete.
            /// </para>
            /// </param>
            public Task DelUserCommand(string @name) =>
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_buf_del_user_command",
                        Arguments = GetRequestArguments(_msgPackExtObj, @name),
                    }
                );

            public Task<IDictionary> GetCommands(IDictionary @opts) =>
                _api.SendAndReceive<IDictionary>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_get_commands",
                        Arguments = GetRequestArguments(_msgPackExtObj, @opts),
                    }
                );

            /// <summary>
            /// <para>
            /// Clears highlights and virtual text from namespace and range of lines
            /// </para>
            /// </summary>
            /// <param name="nsId">
            /// <para>
            /// Namespace to clear, or -1 to clear all.
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
            public Task ClearHighlight(long @nsId, long @lineStart, long @lineEnd) =>
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_buf_clear_highlight",
                        Arguments = GetRequestArguments(
                            _msgPackExtObj,
                            @nsId,
                            @lineStart,
                            @lineEnd
                        ),
                    }
                );

            /// <summary>
            /// <para>
            /// Adds a highlight to buffer.
            /// </para>
            /// </summary>
            /// <param name="nsId">
            /// <para>
            /// namespace to use or -1 for ungrouped highlight
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
            /// The ns_id that was used
            /// </para>
            /// </returns>
            public Task<long> AddHighlight(
                long @nsId,
                string @hlGroup,
                long @line,
                long @colStart,
                long @colEnd
            ) =>
                _api.SendAndReceive<long>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_add_highlight",
                        Arguments = GetRequestArguments(
                            _msgPackExtObj,
                            @nsId,
                            @hlGroup,
                            @line,
                            @colStart,
                            @colEnd
                        ),
                    }
                );

            /// <summary>
            /// <para>
            /// Set the virtual text (annotation) for a buffer line.
            /// </para>
            /// </summary>
            /// <param name="srcId">
            /// <para>
            /// Namespace to use or 0 to create a namespace, or -1 for a ungrouped annotation
            /// </para>
            /// </param>
            /// <param name="line">
            /// <para>
            /// Line to annotate with virtual text (zero-indexed)
            /// </para>
            /// </param>
            /// <param name="chunks">
            /// <para>
            /// A list of [text, hl_group] arrays, each representing a text chunk with specified highlight.
            /// </para>
            /// </param>
            /// <param name="opts">
            /// <para>
            /// Optional parameters. Currently not used.
            /// </para>
            /// </param>
            /// <returns>
            /// <para>
            /// The ns_id that was used
            /// </para>
            /// </returns>
            public Task<long> SetVirtualText(
                long @srcId,
                long @line,
                object[] @chunks,
                IDictionary @opts
            ) =>
                _api.SendAndReceive<long>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_set_virtual_text",
                        Arguments = GetRequestArguments(
                            _msgPackExtObj,
                            @srcId,
                            @line,
                            @chunks,
                            @opts
                        ),
                    }
                );

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
                _api.SendAndReceive<object>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_get_option",
                        Arguments = GetRequestArguments(_msgPackExtObj, @name),
                    }
                );

            /// <summary>
            /// <para>
            /// Sets a buffer option value. Passing
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
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_buf_set_option",
                        Arguments = GetRequestArguments(_msgPackExtObj, @name, @value),
                    }
                );

            public Task<object[]> GetExtmarkById(long @nsId, long @id, IDictionary @opts) =>
                _api.SendAndReceive<object[]>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_get_extmark_by_id",
                        Arguments = GetRequestArguments(_msgPackExtObj, @nsId, @id, @opts),
                    }
                );

            /// <summary>
            /// <para>
            /// Gets |extmarks| in &quot;traversal order&quot; from a |charwise| region defined by buffer positions (inclusive, 0-indexed |api-indexing|).
            /// </para>
            /// </summary>
            /// <param name="nsId">
            /// <para>
            /// Namespace id from |nvim_create_namespace()| or -1 for all namespaces
            /// </para>
            /// </param>
            /// <param name="start">
            /// <para>
            /// Start of range: a 0-indexed (row, col) or valid extmark id (whose position defines the bound). |api-indexing|
            /// </para>
            /// </param>
            /// <param name="end">
            /// <para>
            /// End of range (inclusive): a 0-indexed (row, col) or valid extmark id (whose position defines the bound). |api-indexing|
            /// </para>
            /// </param>
            /// <param name="opts">
            /// <para>
            /// Optional parameters. Keys:
            /// </para>
            /// </param>
            /// <returns>
            /// <para>
            /// List of
            /// </para>
            /// </returns>
            public Task<object[]> GetExtmarks(
                long @nsId,
                object @start,
                object @end,
                IDictionary @opts
            ) =>
                _api.SendAndReceive<object[]>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_get_extmarks",
                        Arguments = GetRequestArguments(_msgPackExtObj, @nsId, @start, @end, @opts),
                    }
                );

            /// <summary>
            /// <para>
            /// Creates or updates an |extmark|.
            /// </para>
            /// </summary>
            /// <param name="nsId">
            /// <para>
            /// Namespace id from |nvim_create_namespace()|
            /// </para>
            /// </param>
            /// <param name="line">
            /// <para>
            /// Line where to place the mark, 0-based. |api-indexing|
            /// </para>
            /// </param>
            /// <param name="col">
            /// <para>
            /// Column where to place the mark, 0-based. |api-indexing|
            /// </para>
            /// </param>
            /// <param name="opts">
            /// <para>
            /// Optional parameters.
            /// </para>
            /// </param>
            /// <returns>
            /// <para>
            /// Id of the created/updated extmark
            /// </para>
            /// </returns>
            public Task<long> SetExtmark(long @nsId, long @line, long @col, IDictionary @opts) =>
                _api.SendAndReceive<long>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_set_extmark",
                        Arguments = GetRequestArguments(_msgPackExtObj, @nsId, @line, @col, @opts),
                    }
                );

            /// <summary>
            /// <para>
            /// Removes an |extmark|.
            /// </para>
            /// </summary>
            /// <param name="nsId">
            /// <para>
            /// Namespace id from |nvim_create_namespace()|
            /// </para>
            /// </param>
            /// <param name="id">
            /// <para>
            /// Extmark id
            /// </para>
            /// </param>
            /// <returns>
            /// <para>
            /// true if the extmark was found, else false
            /// </para>
            /// </returns>
            public Task<bool> DelExtmark(long @nsId, long @id) =>
                _api.SendAndReceive<bool>(
                    new NvimRequest
                    {
                        Method = "nvim_buf_del_extmark",
                        Arguments = GetRequestArguments(_msgPackExtObj, @nsId, @id),
                    }
                );

            /// <summary>
            /// <para>
            /// Clears |namespace|d objects (highlights, |extmarks|, virtual text) from a region.
            /// </para>
            /// </summary>
            /// <param name="nsId">
            /// <para>
            /// Namespace to clear, or -1 to clear all namespaces.
            /// </para>
            /// </param>
            /// <param name="lineStart">
            /// <para>
            /// Start of range of lines to clear
            /// </para>
            /// </param>
            /// <param name="lineEnd">
            /// <para>
            /// End of range of lines to clear (exclusive) or -1 to clear to end of buffer.
            /// </para>
            /// </param>
            public Task ClearNamespace(long @nsId, long @lineStart, long @lineEnd) =>
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_buf_clear_namespace",
                        Arguments = GetRequestArguments(
                            _msgPackExtObj,
                            @nsId,
                            @lineStart,
                            @lineEnd
                        ),
                    }
                );
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
                _api.SendAndReceive<object>(
                    new NvimRequest
                    {
                        Method = "nvim_win_get_option",
                        Arguments = GetRequestArguments(_msgPackExtObj, @name),
                    }
                );

            /// <summary>
            /// <para>
            /// Sets a window option value. Passing
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
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_win_set_option",
                        Arguments = GetRequestArguments(_msgPackExtObj, @name, @value),
                    }
                );

            /// <summary>
            /// <para>
            /// Reconfigures the layout and properties of a window.
            /// </para>
            /// </summary>
            /// <param name="config">
            /// <para>
            /// Map defining the window configuration, see [nvim_open_win()]
            /// </para>
            /// </param>
            public Task SetConfig(IDictionary @config) =>
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_win_set_config",
                        Arguments = GetRequestArguments(_msgPackExtObj, @config),
                    }
                );

            public Task<IDictionary> GetConfig() =>
                _api.SendAndReceive<IDictionary>(
                    new NvimRequest
                    {
                        Method = "nvim_win_get_config",
                        Arguments = GetRequestArguments(_msgPackExtObj),
                    }
                );

            /// <summary>
            /// <para>
            /// Gets the current buffer in a window
            /// </para>
            /// </summary>
            /// <returns>
            /// <para>
            /// Buffer id
            /// </para>
            /// </returns>
            public Task<NvimBuffer> GetBuf() =>
                _api.SendAndReceive<NvimBuffer>(
                    new NvimRequest
                    {
                        Method = "nvim_win_get_buf",
                        Arguments = GetRequestArguments(_msgPackExtObj),
                    }
                );

            /// <summary>
            /// <para>
            /// Sets the current buffer in a window.
            /// </para>
            /// </summary>
            /// <param name="buf">
            /// <para>
            /// Buffer id
            /// </para>
            /// </param>
            public Task SetBuf(NvimBuffer @buf) =>
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_win_set_buf",
                        Arguments = GetRequestArguments(_msgPackExtObj, @buf),
                    }
                );

            /// <summary>
            /// <para>
            /// Gets the (1,0)-indexed, buffer-relative cursor position for a given window (different windows showing the same buffer have independent cursor positions). |api-indexing|
            /// </para>
            /// </summary>
            /// <returns>
            /// <para>
            /// (row, col) tuple
            /// </para>
            /// </returns>
            public Task<object[]> GetCursor() =>
                _api.SendAndReceive<object[]>(
                    new NvimRequest
                    {
                        Method = "nvim_win_get_cursor",
                        Arguments = GetRequestArguments(_msgPackExtObj),
                    }
                );

            /// <summary>
            /// <para>
            /// Sets the (1,0)-indexed cursor position (byte offset) in the window. |api-indexing| This scrolls the window even if it is not the current one.
            /// </para>
            /// </summary>
            /// <param name="pos">
            /// <para>
            /// (row, col) tuple representing the new position
            /// </para>
            /// </param>
            public Task SetCursor(object[] @pos) =>
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_win_set_cursor",
                        Arguments = GetRequestArguments(_msgPackExtObj, @pos),
                    }
                );

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
                _api.SendAndReceive<long>(
                    new NvimRequest
                    {
                        Method = "nvim_win_get_height",
                        Arguments = GetRequestArguments(_msgPackExtObj),
                    }
                );

            /// <summary>
            /// <para>
            /// Sets the window height.
            /// </para>
            /// </summary>
            /// <param name="height">
            /// <para>
            /// Height as a count of rows
            /// </para>
            /// </param>
            public Task SetHeight(long @height) =>
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_win_set_height",
                        Arguments = GetRequestArguments(_msgPackExtObj, @height),
                    }
                );

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
                _api.SendAndReceive<long>(
                    new NvimRequest
                    {
                        Method = "nvim_win_get_width",
                        Arguments = GetRequestArguments(_msgPackExtObj),
                    }
                );

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
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_win_set_width",
                        Arguments = GetRequestArguments(_msgPackExtObj, @width),
                    }
                );

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
                _api.SendAndReceive<object>(
                    new NvimRequest
                    {
                        Method = "nvim_win_get_var",
                        Arguments = GetRequestArguments(_msgPackExtObj, @name),
                    }
                );

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
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_win_set_var",
                        Arguments = GetRequestArguments(_msgPackExtObj, @name, @value),
                    }
                );

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
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_win_del_var",
                        Arguments = GetRequestArguments(_msgPackExtObj, @name),
                    }
                );

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
            public Task<object[]> GetPosition() =>
                _api.SendAndReceive<object[]>(
                    new NvimRequest
                    {
                        Method = "nvim_win_get_position",
                        Arguments = GetRequestArguments(_msgPackExtObj),
                    }
                );

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
                _api.SendAndReceive<NvimTabpage>(
                    new NvimRequest
                    {
                        Method = "nvim_win_get_tabpage",
                        Arguments = GetRequestArguments(_msgPackExtObj),
                    }
                );

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
                _api.SendAndReceive<long>(
                    new NvimRequest
                    {
                        Method = "nvim_win_get_number",
                        Arguments = GetRequestArguments(_msgPackExtObj),
                    }
                );

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
                _api.SendAndReceive<bool>(
                    new NvimRequest
                    {
                        Method = "nvim_win_is_valid",
                        Arguments = GetRequestArguments(_msgPackExtObj),
                    }
                );

            /// <summary>
            /// <para>
            /// Closes the window and hide the buffer it contains (like |:hide| with a |window-ID|).
            /// </para>
            /// </summary>
            public Task Hide() =>
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_win_hide",
                        Arguments = GetRequestArguments(_msgPackExtObj),
                    }
                );

            /// <summary>
            /// <para>
            /// Closes the window (like |:close| with a |window-ID|).
            /// </para>
            /// </summary>
            /// <param name="force">
            /// <para>
            /// Behave like
            /// </para>
            /// </param>
            public Task Close(bool @force) =>
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_win_close",
                        Arguments = GetRequestArguments(_msgPackExtObj, @force),
                    }
                );

            /// <summary>
            /// <para>
            /// Set highlight namespace for a window. This will use highlights defined with |nvim_set_hl()| for this namespace, but fall back to global highlights (ns=0) when missing.
            /// </para>
            /// </summary>
            /// <param name="nsId">
            /// <para>
            /// the namespace to use
            /// </para>
            /// </param>
            public Task SetHlNs(long @nsId) =>
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_win_set_hl_ns",
                        Arguments = GetRequestArguments(_msgPackExtObj, @nsId),
                    }
                );

            public Task<IDictionary> TextHeight(IDictionary @opts) =>
                _api.SendAndReceive<IDictionary>(
                    new NvimRequest
                    {
                        Method = "nvim_win_text_height",
                        Arguments = GetRequestArguments(_msgPackExtObj, @opts),
                    }
                );
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
            public Task<object[]> ListWins() =>
                _api.SendAndReceive<object[]>(
                    new NvimRequest
                    {
                        Method = "nvim_tabpage_list_wins",
                        Arguments = GetRequestArguments(_msgPackExtObj),
                    }
                );

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
                _api.SendAndReceive<object>(
                    new NvimRequest
                    {
                        Method = "nvim_tabpage_get_var",
                        Arguments = GetRequestArguments(_msgPackExtObj, @name),
                    }
                );

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
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_tabpage_set_var",
                        Arguments = GetRequestArguments(_msgPackExtObj, @name, @value),
                    }
                );

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
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_tabpage_del_var",
                        Arguments = GetRequestArguments(_msgPackExtObj, @name),
                    }
                );

            /// <summary>
            /// <para>
            /// Gets the current window in a tabpage
            /// </para>
            /// </summary>
            /// <returns>
            /// <para>
            /// |window-ID|
            /// </para>
            /// </returns>
            public Task<NvimWindow> GetWin() =>
                _api.SendAndReceive<NvimWindow>(
                    new NvimRequest
                    {
                        Method = "nvim_tabpage_get_win",
                        Arguments = GetRequestArguments(_msgPackExtObj),
                    }
                );

            /// <summary>
            /// <para>
            /// Sets the current window in a tabpage
            /// </para>
            /// </summary>
            /// <param name="win">
            /// <para>
            /// |window-ID|, must already belong to {tabpage}
            /// </para>
            /// </param>
            public Task SetWin(NvimWindow @win) =>
                _api.SendAndReceive(
                    new NvimRequest
                    {
                        Method = "nvim_tabpage_set_win",
                        Arguments = GetRequestArguments(_msgPackExtObj, @win),
                    }
                );

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
                _api.SendAndReceive<long>(
                    new NvimRequest
                    {
                        Method = "nvim_tabpage_get_number",
                        Arguments = GetRequestArguments(_msgPackExtObj),
                    }
                );

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
                _api.SendAndReceive<bool>(
                    new NvimRequest
                    {
                        Method = "nvim_tabpage_is_valid",
                        Arguments = GetRequestArguments(_msgPackExtObj),
                    }
                );
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

        public class ConnectEventArgs : EventArgs
        {
            public string ServerAddr { get; set; }
        }

        public class RestartEventArgs : EventArgs
        {
            public string ListenAddr { get; set; }
        }

        public class SetTitleEventArgs : EventArgs
        {
            public string Title { get; set; }
        }

        public class SetIconEventArgs : EventArgs
        {
            public string Icon { get; set; }
        }

        public class ScreenshotEventArgs : EventArgs
        {
            public string Path { get; set; }
        }

        public class OptionSetEventArgs : EventArgs
        {
            public string Name { get; set; }
            public object Value { get; set; }
        }

        public class ChdirEventArgs : EventArgs
        {
            public string Path { get; set; }
        }

        public class UiSendEventArgs : EventArgs
        {
            public string Content { get; set; }
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

        public class HighlightSetEventArgs : EventArgs
        {
            public IDictionary Attrs { get; set; }
        }

        public class PutEventArgs : EventArgs
        {
            public string Str { get; set; }
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

        public class DefaultColorsSetEventArgs : EventArgs
        {
            public long RgbFg { get; set; }
            public long RgbBg { get; set; }
            public long RgbSp { get; set; }
            public long CtermFg { get; set; }
            public long CtermBg { get; set; }
        }

        public class HlAttrDefineEventArgs : EventArgs
        {
            public long Id { get; set; }
            public IDictionary RgbAttrs { get; set; }
            public IDictionary CtermAttrs { get; set; }
            public object[] Info { get; set; }
        }

        public class HlGroupSetEventArgs : EventArgs
        {
            public string Name { get; set; }
            public long Id { get; set; }
        }

        public class GridResizeEventArgs : EventArgs
        {
            public long Grid { get; set; }
            public long Width { get; set; }
            public long Height { get; set; }
        }

        public class GridClearEventArgs : EventArgs
        {
            public long Grid { get; set; }
        }

        public class GridCursorGotoEventArgs : EventArgs
        {
            public long Grid { get; set; }
            public long Row { get; set; }
            public long Col { get; set; }
        }

        public class GridLineEventArgs : EventArgs
        {
            public long Grid { get; set; }
            public long Row { get; set; }
            public long ColStart { get; set; }
            public object[] Data { get; set; }
            public bool Wrap { get; set; }
        }

        public class GridScrollEventArgs : EventArgs
        {
            public long Grid { get; set; }
            public long Top { get; set; }
            public long Bot { get; set; }
            public long Left { get; set; }
            public long Right { get; set; }
            public long Rows { get; set; }
            public long Cols { get; set; }
        }

        public class GridDestroyEventArgs : EventArgs
        {
            public long Grid { get; set; }
        }

        public class WinPosEventArgs : EventArgs
        {
            public long Grid { get; set; }
            public NvimWindow Win { get; set; }
            public long Startrow { get; set; }
            public long Startcol { get; set; }
            public long Width { get; set; }
            public long Height { get; set; }
        }

        public class WinFloatPosEventArgs : EventArgs
        {
            public long Grid { get; set; }
            public NvimWindow Win { get; set; }
            public string Anchor { get; set; }
            public long AnchorGrid { get; set; }
            public double AnchorRow { get; set; }
            public double AnchorCol { get; set; }
            public bool MouseEnabled { get; set; }
            public long Zindex { get; set; }
            public long Compindex { get; set; }
            public long ScreenRow { get; set; }
            public long ScreenCol { get; set; }
        }

        public class WinExternalPosEventArgs : EventArgs
        {
            public long Grid { get; set; }
            public NvimWindow Win { get; set; }
        }

        public class WinHideEventArgs : EventArgs
        {
            public long Grid { get; set; }
        }

        public class WinCloseEventArgs : EventArgs
        {
            public long Grid { get; set; }
        }

        public class MsgSetPosEventArgs : EventArgs
        {
            public long Grid { get; set; }
            public long Row { get; set; }
            public bool Scrolled { get; set; }
            public string SepChar { get; set; }
            public long Zindex { get; set; }
            public long Compindex { get; set; }
        }

        public class WinViewportEventArgs : EventArgs
        {
            public long Grid { get; set; }
            public NvimWindow Win { get; set; }
            public long Topline { get; set; }
            public long Botline { get; set; }
            public long Curline { get; set; }
            public long Curcol { get; set; }
            public long LineCount { get; set; }
            public long ScrollDelta { get; set; }
        }

        public class WinViewportMarginsEventArgs : EventArgs
        {
            public long Grid { get; set; }
            public NvimWindow Win { get; set; }
            public long Top { get; set; }
            public long Bottom { get; set; }
            public long Left { get; set; }
            public long Right { get; set; }
        }

        public class WinExtmarkEventArgs : EventArgs
        {
            public long Grid { get; set; }
            public NvimWindow Win { get; set; }
            public long NsId { get; set; }
            public long MarkId { get; set; }
            public long Row { get; set; }
            public long Col { get; set; }
        }

        public class PopupmenuShowEventArgs : EventArgs
        {
            public object[] Items { get; set; }
            public long Selected { get; set; }
            public long Row { get; set; }
            public long Col { get; set; }
            public long Grid { get; set; }
        }

        public class PopupmenuSelectEventArgs : EventArgs
        {
            public long Selected { get; set; }
        }

        public class TablineUpdateEventArgs : EventArgs
        {
            public NvimTabpage Current { get; set; }
            public object[] Tabs { get; set; }
            public NvimBuffer CurrentBuffer { get; set; }
            public object[] Buffers { get; set; }
        }

        public class CmdlineShowEventArgs : EventArgs
        {
            public object[] Content { get; set; }
            public long Pos { get; set; }
            public string Firstc { get; set; }
            public string Prompt { get; set; }
            public long Indent { get; set; }
            public long Level { get; set; }
            public long HlId { get; set; }
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
            public bool Abort { get; set; }
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

        public class MsgShowEventArgs : EventArgs
        {
            public string Kind { get; set; }
            public object[] Content { get; set; }
            public bool ReplaceLast { get; set; }
            public bool History { get; set; }
            public bool Append { get; set; }
            public object Id { get; set; }
            public string Trigger { get; set; }
        }

        public class MsgShowcmdEventArgs : EventArgs
        {
            public object[] Content { get; set; }
        }

        public class MsgShowmodeEventArgs : EventArgs
        {
            public object[] Content { get; set; }
        }

        public class MsgRulerEventArgs : EventArgs
        {
            public object[] Content { get; set; }
        }

        public class MsgHistoryShowEventArgs : EventArgs
        {
            public object[] Entries { get; set; }
            public bool PrevCmd { get; set; }
        }

        public class ErrorExitEventArgs : EventArgs
        {
            public long Status { get; set; }
        }

        private void CallUIEventHandler(string eventName, object[] args)
        {
            switch (eventName)
            {
                case "mode_info_set":
                    ModeInfoSetEvent?.Invoke(
                        this,
                        new ModeInfoSetEventArgs
                        {
                            Enabled = (bool)args[0],
                            CursorStyles = (object[])args[1],
                        }
                    );
                    break;

                case "update_menu":
                    UpdateMenuEvent?.Invoke(this, EventArgs.Empty);
                    break;

                case "busy_start":
                    BusyStartEvent?.Invoke(this, EventArgs.Empty);
                    break;

                case "busy_stop":
                    BusyStopEvent?.Invoke(this, EventArgs.Empty);
                    break;

                case "mouse_on":
                    MouseOnEvent?.Invoke(this, EventArgs.Empty);
                    break;

                case "mouse_off":
                    MouseOffEvent?.Invoke(this, EventArgs.Empty);
                    break;

                case "mode_change":
                    ModeChangeEvent?.Invoke(
                        this,
                        new ModeChangeEventArgs { Mode = (string)args[0], ModeIdx = (long)args[1] }
                    );
                    break;

                case "bell":
                    BellEvent?.Invoke(this, EventArgs.Empty);
                    break;

                case "visual_bell":
                    VisualBellEvent?.Invoke(this, EventArgs.Empty);
                    break;

                case "flush":
                    FlushEvent?.Invoke(this, EventArgs.Empty);
                    break;

                case "connect":
                    ConnectEvent?.Invoke(
                        this,
                        new ConnectEventArgs { ServerAddr = (string)args[0] }
                    );
                    break;

                case "restart":
                    RestartEvent?.Invoke(
                        this,
                        new RestartEventArgs { ListenAddr = (string)args[0] }
                    );
                    break;

                case "suspend":
                    SuspendEvent?.Invoke(this, EventArgs.Empty);
                    break;

                case "set_title":
                    SetTitleEvent?.Invoke(this, new SetTitleEventArgs { Title = (string)args[0] });
                    break;

                case "set_icon":
                    SetIconEvent?.Invoke(this, new SetIconEventArgs { Icon = (string)args[0] });
                    break;

                case "screenshot":
                    ScreenshotEvent?.Invoke(
                        this,
                        new ScreenshotEventArgs { Path = (string)args[0] }
                    );
                    break;

                case "option_set":
                    OptionSetEvent?.Invoke(
                        this,
                        new OptionSetEventArgs { Name = (string)args[0], Value = (object)args[1] }
                    );
                    break;

                case "chdir":
                    ChdirEvent?.Invoke(this, new ChdirEventArgs { Path = (string)args[0] });
                    break;

                case "ui_send":
                    UiSendEvent?.Invoke(this, new UiSendEventArgs { Content = (string)args[0] });
                    break;

                case "update_fg":
                    UpdateFgEvent?.Invoke(this, new UpdateFgEventArgs { Fg = (long)args[0] });
                    break;

                case "update_bg":
                    UpdateBgEvent?.Invoke(this, new UpdateBgEventArgs { Bg = (long)args[0] });
                    break;

                case "update_sp":
                    UpdateSpEvent?.Invoke(this, new UpdateSpEventArgs { Sp = (long)args[0] });
                    break;

                case "resize":
                    ResizeEvent?.Invoke(
                        this,
                        new ResizeEventArgs { Width = (long)args[0], Height = (long)args[1] }
                    );
                    break;

                case "clear":
                    ClearEvent?.Invoke(this, EventArgs.Empty);
                    break;

                case "eol_clear":
                    EolClearEvent?.Invoke(this, EventArgs.Empty);
                    break;

                case "cursor_goto":
                    CursorGotoEvent?.Invoke(
                        this,
                        new CursorGotoEventArgs { Row = (long)args[0], Col = (long)args[1] }
                    );
                    break;

                case "highlight_set":
                    HighlightSetEvent?.Invoke(
                        this,
                        new HighlightSetEventArgs { Attrs = (IDictionary)args[0] }
                    );
                    break;

                case "put":
                    PutEvent?.Invoke(this, new PutEventArgs { Str = (string)args[0] });
                    break;

                case "set_scroll_region":
                    SetScrollRegionEvent?.Invoke(
                        this,
                        new SetScrollRegionEventArgs
                        {
                            Top = (long)args[0],
                            Bot = (long)args[1],
                            Left = (long)args[2],
                            Right = (long)args[3],
                        }
                    );
                    break;

                case "scroll":
                    ScrollEvent?.Invoke(this, new ScrollEventArgs { Count = (long)args[0] });
                    break;

                case "default_colors_set":
                    DefaultColorsSetEvent?.Invoke(
                        this,
                        new DefaultColorsSetEventArgs
                        {
                            RgbFg = (long)args[0],
                            RgbBg = (long)args[1],
                            RgbSp = (long)args[2],
                            CtermFg = (long)args[3],
                            CtermBg = (long)args[4],
                        }
                    );
                    break;

                case "hl_attr_define":
                    HlAttrDefineEvent?.Invoke(
                        this,
                        new HlAttrDefineEventArgs
                        {
                            Id = (long)args[0],
                            RgbAttrs = (IDictionary)args[1],
                            CtermAttrs = (IDictionary)args[2],
                            Info = (object[])args[3],
                        }
                    );
                    break;

                case "hl_group_set":
                    HlGroupSetEvent?.Invoke(
                        this,
                        new HlGroupSetEventArgs { Name = (string)args[0], Id = (long)args[1] }
                    );
                    break;

                case "grid_resize":
                    GridResizeEvent?.Invoke(
                        this,
                        new GridResizeEventArgs
                        {
                            Grid = (long)args[0],
                            Width = (long)args[1],
                            Height = (long)args[2],
                        }
                    );
                    break;

                case "grid_clear":
                    GridClearEvent?.Invoke(this, new GridClearEventArgs { Grid = (long)args[0] });
                    break;

                case "grid_cursor_goto":
                    GridCursorGotoEvent?.Invoke(
                        this,
                        new GridCursorGotoEventArgs
                        {
                            Grid = (long)args[0],
                            Row = (long)args[1],
                            Col = (long)args[2],
                        }
                    );
                    break;

                case "grid_line":
                    GridLineEvent?.Invoke(
                        this,
                        new GridLineEventArgs
                        {
                            Grid = (long)args[0],
                            Row = (long)args[1],
                            ColStart = (long)args[2],
                            Data = (object[])args[3],
                            Wrap = (bool)args[4],
                        }
                    );
                    break;

                case "grid_scroll":
                    GridScrollEvent?.Invoke(
                        this,
                        new GridScrollEventArgs
                        {
                            Grid = (long)args[0],
                            Top = (long)args[1],
                            Bot = (long)args[2],
                            Left = (long)args[3],
                            Right = (long)args[4],
                            Rows = (long)args[5],
                            Cols = (long)args[6],
                        }
                    );
                    break;

                case "grid_destroy":
                    GridDestroyEvent?.Invoke(
                        this,
                        new GridDestroyEventArgs { Grid = (long)args[0] }
                    );
                    break;

                case "win_pos":
                    WinPosEvent?.Invoke(
                        this,
                        new WinPosEventArgs
                        {
                            Grid = (long)args[0],
                            Win = (NvimWindow)args[1],
                            Startrow = (long)args[2],
                            Startcol = (long)args[3],
                            Width = (long)args[4],
                            Height = (long)args[5],
                        }
                    );
                    break;

                case "win_float_pos":
                    WinFloatPosEvent?.Invoke(
                        this,
                        new WinFloatPosEventArgs
                        {
                            Grid = (long)args[0],
                            Win = (NvimWindow)args[1],
                            Anchor = (string)args[2],
                            AnchorGrid = (long)args[3],
                            AnchorRow = (double)args[4],
                            AnchorCol = (double)args[5],
                            MouseEnabled = (bool)args[6],
                            Zindex = (long)args[7],
                            Compindex = (long)args[8],
                            ScreenRow = (long)args[9],
                            ScreenCol = (long)args[10],
                        }
                    );
                    break;

                case "win_external_pos":
                    WinExternalPosEvent?.Invoke(
                        this,
                        new WinExternalPosEventArgs
                        {
                            Grid = (long)args[0],
                            Win = (NvimWindow)args[1],
                        }
                    );
                    break;

                case "win_hide":
                    WinHideEvent?.Invoke(this, new WinHideEventArgs { Grid = (long)args[0] });
                    break;

                case "win_close":
                    WinCloseEvent?.Invoke(this, new WinCloseEventArgs { Grid = (long)args[0] });
                    break;

                case "msg_set_pos":
                    MsgSetPosEvent?.Invoke(
                        this,
                        new MsgSetPosEventArgs
                        {
                            Grid = (long)args[0],
                            Row = (long)args[1],
                            Scrolled = (bool)args[2],
                            SepChar = (string)args[3],
                            Zindex = (long)args[4],
                            Compindex = (long)args[5],
                        }
                    );
                    break;

                case "win_viewport":
                    WinViewportEvent?.Invoke(
                        this,
                        new WinViewportEventArgs
                        {
                            Grid = (long)args[0],
                            Win = (NvimWindow)args[1],
                            Topline = (long)args[2],
                            Botline = (long)args[3],
                            Curline = (long)args[4],
                            Curcol = (long)args[5],
                            LineCount = (long)args[6],
                            ScrollDelta = (long)args[7],
                        }
                    );
                    break;

                case "win_viewport_margins":
                    WinViewportMarginsEvent?.Invoke(
                        this,
                        new WinViewportMarginsEventArgs
                        {
                            Grid = (long)args[0],
                            Win = (NvimWindow)args[1],
                            Top = (long)args[2],
                            Bottom = (long)args[3],
                            Left = (long)args[4],
                            Right = (long)args[5],
                        }
                    );
                    break;

                case "win_extmark":
                    WinExtmarkEvent?.Invoke(
                        this,
                        new WinExtmarkEventArgs
                        {
                            Grid = (long)args[0],
                            Win = (NvimWindow)args[1],
                            NsId = (long)args[2],
                            MarkId = (long)args[3],
                            Row = (long)args[4],
                            Col = (long)args[5],
                        }
                    );
                    break;

                case "popupmenu_show":
                    PopupmenuShowEvent?.Invoke(
                        this,
                        new PopupmenuShowEventArgs
                        {
                            Items = (object[])args[0],
                            Selected = (long)args[1],
                            Row = (long)args[2],
                            Col = (long)args[3],
                            Grid = (long)args[4],
                        }
                    );
                    break;

                case "popupmenu_hide":
                    PopupmenuHideEvent?.Invoke(this, EventArgs.Empty);
                    break;

                case "popupmenu_select":
                    PopupmenuSelectEvent?.Invoke(
                        this,
                        new PopupmenuSelectEventArgs { Selected = (long)args[0] }
                    );
                    break;

                case "tabline_update":
                    TablineUpdateEvent?.Invoke(
                        this,
                        new TablineUpdateEventArgs
                        {
                            Current = (NvimTabpage)args[0],
                            Tabs = (object[])args[1],
                            CurrentBuffer = (NvimBuffer)args[2],
                            Buffers = (object[])args[3],
                        }
                    );
                    break;

                case "cmdline_show":
                    CmdlineShowEvent?.Invoke(
                        this,
                        new CmdlineShowEventArgs
                        {
                            Content = (object[])args[0],
                            Pos = (long)args[1],
                            Firstc = (string)args[2],
                            Prompt = (string)args[3],
                            Indent = (long)args[4],
                            Level = (long)args[5],
                            HlId = (long)args[6],
                        }
                    );
                    break;

                case "cmdline_pos":
                    CmdlinePosEvent?.Invoke(
                        this,
                        new CmdlinePosEventArgs { Pos = (long)args[0], Level = (long)args[1] }
                    );
                    break;

                case "cmdline_special_char":
                    CmdlineSpecialCharEvent?.Invoke(
                        this,
                        new CmdlineSpecialCharEventArgs
                        {
                            C = (string)args[0],
                            Shift = (bool)args[1],
                            Level = (long)args[2],
                        }
                    );
                    break;

                case "cmdline_hide":
                    CmdlineHideEvent?.Invoke(
                        this,
                        new CmdlineHideEventArgs { Level = (long)args[0], Abort = (bool)args[1] }
                    );
                    break;

                case "cmdline_block_show":
                    CmdlineBlockShowEvent?.Invoke(
                        this,
                        new CmdlineBlockShowEventArgs { Lines = (object[])args[0] }
                    );
                    break;

                case "cmdline_block_append":
                    CmdlineBlockAppendEvent?.Invoke(
                        this,
                        new CmdlineBlockAppendEventArgs { Lines = (object[])args[0] }
                    );
                    break;

                case "cmdline_block_hide":
                    CmdlineBlockHideEvent?.Invoke(this, EventArgs.Empty);
                    break;

                case "wildmenu_show":
                    WildmenuShowEvent?.Invoke(
                        this,
                        new WildmenuShowEventArgs { Items = (object[])args[0] }
                    );
                    break;

                case "wildmenu_select":
                    WildmenuSelectEvent?.Invoke(
                        this,
                        new WildmenuSelectEventArgs { Selected = (long)args[0] }
                    );
                    break;

                case "wildmenu_hide":
                    WildmenuHideEvent?.Invoke(this, EventArgs.Empty);
                    break;

                case "msg_show":
                    MsgShowEvent?.Invoke(
                        this,
                        new MsgShowEventArgs
                        {
                            Kind = (string)args[0],
                            Content = (object[])args[1],
                            ReplaceLast = (bool)args[2],
                            History = (bool)args[3],
                            Append = (bool)args[4],
                            Id = (object)args[5],
                            Trigger = (string)args[6],
                        }
                    );
                    break;

                case "msg_clear":
                    MsgClearEvent?.Invoke(this, EventArgs.Empty);
                    break;

                case "msg_showcmd":
                    MsgShowcmdEvent?.Invoke(
                        this,
                        new MsgShowcmdEventArgs { Content = (object[])args[0] }
                    );
                    break;

                case "msg_showmode":
                    MsgShowmodeEvent?.Invoke(
                        this,
                        new MsgShowmodeEventArgs { Content = (object[])args[0] }
                    );
                    break;

                case "msg_ruler":
                    MsgRulerEvent?.Invoke(
                        this,
                        new MsgRulerEventArgs { Content = (object[])args[0] }
                    );
                    break;

                case "msg_history_show":
                    MsgHistoryShowEvent?.Invoke(
                        this,
                        new MsgHistoryShowEventArgs
                        {
                            Entries = (object[])args[0],
                            PrevCmd = (bool)args[1],
                        }
                    );
                    break;

                case "error_exit":
                    ErrorExitEvent?.Invoke(this, new ErrorExitEventArgs { Status = (long)args[0] });
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
                        $"Unknown extension type id {msgPackExtObj.TypeCode}"
                    );
            }
        }
    }
}
