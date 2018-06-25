
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public Task Command(string @command) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_command",
        Arguments = GetRequestArguments(
            @command)
      });

    public Task<IDictionary> GetHlByName(string @name, bool @rgb) =>
      SendAndReceive<IDictionary>(new NvimRequest
      {
        Method = "nvim_get_hl_by_name",
        Arguments = GetRequestArguments(
            @name, @rgb)
      });

    public Task<IDictionary> GetHlById(long @hlId, bool @rgb) =>
      SendAndReceive<IDictionary>(new NvimRequest
      {
        Method = "nvim_get_hl_by_id",
        Arguments = GetRequestArguments(
            @hlId, @rgb)
      });

    public Task Feedkeys(string @keys, string @mode, bool @escapeCsi) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_feedkeys",
        Arguments = GetRequestArguments(
            @keys, @mode, @escapeCsi)
      });

    public Task<long> Input(string @keys) =>
      SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_input",
        Arguments = GetRequestArguments(
            @keys)
      });

    public Task<string> ReplaceTermcodes(string @str, bool @fromPart, bool @doLt, bool @special) =>
      SendAndReceive<string>(new NvimRequest
      {
        Method = "nvim_replace_termcodes",
        Arguments = GetRequestArguments(
            @str, @fromPart, @doLt, @special)
      });

    public Task<string> CommandOutput(string @command) =>
      SendAndReceive<string>(new NvimRequest
      {
        Method = "nvim_command_output",
        Arguments = GetRequestArguments(
            @command)
      });

    public Task<object> Eval(string @expr) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_eval",
        Arguments = GetRequestArguments(
            @expr)
      });

    public Task<object> ExecuteLua(string @code, object[] @args) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_execute_lua",
        Arguments = GetRequestArguments(
            @code, @args)
      });

    public Task<object> CallFunction(string @fn, object[] @args) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_call_function",
        Arguments = GetRequestArguments(
            @fn, @args)
      });

    public Task<object> CallDictFunction(object @dict, string @fn, object[] @args) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_call_dict_function",
        Arguments = GetRequestArguments(
            @dict, @fn, @args)
      });

    public Task<long> Strwidth(string @text) =>
      SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_strwidth",
        Arguments = GetRequestArguments(
            @text)
      });

    public Task<string[]> ListRuntimePaths() =>
      SendAndReceive<string[]>(new NvimRequest
      {
        Method = "nvim_list_runtime_paths",
        Arguments = GetRequestArguments(
            )
      });

    public Task SetCurrentDir(string @dir) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_current_dir",
        Arguments = GetRequestArguments(
            @dir)
      });

    public Task<string> GetCurrentLine() =>
      SendAndReceive<string>(new NvimRequest
      {
        Method = "nvim_get_current_line",
        Arguments = GetRequestArguments(
            )
      });

    public Task SetCurrentLine(string @line) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_current_line",
        Arguments = GetRequestArguments(
            @line)
      });

    public Task DelCurrentLine() =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_del_current_line",
        Arguments = GetRequestArguments(
            )
      });

    public Task<object> GetVar(string @name) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_get_var",
        Arguments = GetRequestArguments(
            @name)
      });

    public Task SetVar(string @name, object @value) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_var",
        Arguments = GetRequestArguments(
            @name, @value)
      });

    public Task DelVar(string @name) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_del_var",
        Arguments = GetRequestArguments(
            @name)
      });

    public Task<object> GetVvar(string @name) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_get_vvar",
        Arguments = GetRequestArguments(
            @name)
      });

    public Task<object> GetOption(string @name) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_get_option",
        Arguments = GetRequestArguments(
            @name)
      });

    public Task SetOption(string @name, object @value) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_option",
        Arguments = GetRequestArguments(
            @name, @value)
      });

    public Task OutWrite(string @str) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_out_write",
        Arguments = GetRequestArguments(
            @str)
      });

    public Task ErrWrite(string @str) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_err_write",
        Arguments = GetRequestArguments(
            @str)
      });

    public Task ErrWriteln(string @str) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_err_writeln",
        Arguments = GetRequestArguments(
            @str)
      });

    public Task<NvimBuffer[]> ListBufs() =>
      SendAndReceive<NvimBuffer[]>(new NvimRequest
      {
        Method = "nvim_list_bufs",
        Arguments = GetRequestArguments(
            )
      });

    public Task<NvimBuffer> GetCurrentBuf() =>
      SendAndReceive<NvimBuffer>(new NvimRequest
      {
        Method = "nvim_get_current_buf",
        Arguments = GetRequestArguments(
            )
      });

    public Task SetCurrentBuf(NvimBuffer @buffer) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_current_buf",
        Arguments = GetRequestArguments(
            @buffer)
      });

    public Task<NvimWindow[]> ListWins() =>
      SendAndReceive<NvimWindow[]>(new NvimRequest
      {
        Method = "nvim_list_wins",
        Arguments = GetRequestArguments(
            )
      });

    public Task<NvimWindow> GetCurrentWin() =>
      SendAndReceive<NvimWindow>(new NvimRequest
      {
        Method = "nvim_get_current_win",
        Arguments = GetRequestArguments(
            )
      });

    public Task SetCurrentWin(NvimWindow @window) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_current_win",
        Arguments = GetRequestArguments(
            @window)
      });

    public Task<NvimTabpage[]> ListTabpages() =>
      SendAndReceive<NvimTabpage[]>(new NvimRequest
      {
        Method = "nvim_list_tabpages",
        Arguments = GetRequestArguments(
            )
      });

    public Task<NvimTabpage> GetCurrentTabpage() =>
      SendAndReceive<NvimTabpage>(new NvimRequest
      {
        Method = "nvim_get_current_tabpage",
        Arguments = GetRequestArguments(
            )
      });

    public Task SetCurrentTabpage(NvimTabpage @tabpage) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_current_tabpage",
        Arguments = GetRequestArguments(
            @tabpage)
      });

    public Task Subscribe(string @event) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_subscribe",
        Arguments = GetRequestArguments(
            @event)
      });

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

    public Task<IDictionary> GetMode() =>
      SendAndReceive<IDictionary>(new NvimRequest
      {
        Method = "nvim_get_mode",
        Arguments = GetRequestArguments(
            )
      });

    public Task<IDictionary[]> GetKeymap(string @mode) =>
      SendAndReceive<IDictionary[]>(new NvimRequest
      {
        Method = "nvim_get_keymap",
        Arguments = GetRequestArguments(
            @mode)
      });

    public Task<IDictionary> GetCommands(IDictionary @opts) =>
      SendAndReceive<IDictionary>(new NvimRequest
      {
        Method = "nvim_get_commands",
        Arguments = GetRequestArguments(
            @opts)
      });

    public Task<object[]> GetApiInfo() =>
      SendAndReceive<object[]>(new NvimRequest
      {
        Method = "nvim_get_api_info",
        Arguments = GetRequestArguments(
            )
      });

    public Task SetClientInfo(string @name, IDictionary @version, string @type, IDictionary @methods, IDictionary @attributes) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_client_info",
        Arguments = GetRequestArguments(
            @name, @version, @type, @methods, @attributes)
      });

    public Task<IDictionary> GetChanInfo(long @chan) =>
      SendAndReceive<IDictionary>(new NvimRequest
      {
        Method = "nvim_get_chan_info",
        Arguments = GetRequestArguments(
            @chan)
      });

    public Task<object[]> ListChans() =>
      SendAndReceive<object[]>(new NvimRequest
      {
        Method = "nvim_list_chans",
        Arguments = GetRequestArguments(
            )
      });

    public Task<object[]> CallAtomic(object[] @calls) =>
      SendAndReceive<object[]>(new NvimRequest
      {
        Method = "nvim_call_atomic",
        Arguments = GetRequestArguments(
            @calls)
      });

    public Task<IDictionary> ParseExpression(string @expr, string @flags, bool @highlight) =>
      SendAndReceive<IDictionary>(new NvimRequest
      {
        Method = "nvim_parse_expression",
        Arguments = GetRequestArguments(
            @expr, @flags, @highlight)
      });

    public Task<object[]> ListUis() =>
      SendAndReceive<object[]>(new NvimRequest
      {
        Method = "nvim_list_uis",
        Arguments = GetRequestArguments(
            )
      });

    public Task<object[]> GetProcChildren(long @pid) =>
      SendAndReceive<object[]>(new NvimRequest
      {
        Method = "nvim_get_proc_children",
        Arguments = GetRequestArguments(
            @pid)
      });

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
    public NvimBuffer(NvimAPI api) => _api = api;
    
    public Task<long> LineCount(NvimBuffer @buffer) =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_buf_line_count",
        Arguments = GetRequestArguments(
            @buffer)
      });

    public Task<bool> Attach(NvimBuffer @buffer, bool @sendBuffer, IDictionary @opts) =>
      _api.SendAndReceive<bool>(new NvimRequest
      {
        Method = "nvim_buf_attach",
        Arguments = GetRequestArguments(
            @buffer, @sendBuffer, @opts)
      });

    public Task<bool> Detach(NvimBuffer @buffer) =>
      _api.SendAndReceive<bool>(new NvimRequest
      {
        Method = "nvim_buf_detach",
        Arguments = GetRequestArguments(
            @buffer)
      });

    public Task<string[]> GetLines(NvimBuffer @buffer, long @start, long @end, bool @strictIndexing) =>
      _api.SendAndReceive<string[]>(new NvimRequest
      {
        Method = "nvim_buf_get_lines",
        Arguments = GetRequestArguments(
            @buffer, @start, @end, @strictIndexing)
      });

    public Task SetLines(NvimBuffer @buffer, long @start, long @end, bool @strictIndexing, string[] @replacement) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_set_lines",
        Arguments = GetRequestArguments(
            @buffer, @start, @end, @strictIndexing, @replacement)
      });

    public Task<object> GetVar(NvimBuffer @buffer, string @name) =>
      _api.SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_buf_get_var",
        Arguments = GetRequestArguments(
            @buffer, @name)
      });

    public Task<long> GetChangedtick(NvimBuffer @buffer) =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_buf_get_changedtick",
        Arguments = GetRequestArguments(
            @buffer)
      });

    public Task<IDictionary[]> GetKeymap(NvimBuffer @buffer, string @mode) =>
      _api.SendAndReceive<IDictionary[]>(new NvimRequest
      {
        Method = "nvim_buf_get_keymap",
        Arguments = GetRequestArguments(
            @buffer, @mode)
      });

    public Task<IDictionary> GetCommands(NvimBuffer @buffer, IDictionary @opts) =>
      _api.SendAndReceive<IDictionary>(new NvimRequest
      {
        Method = "nvim_buf_get_commands",
        Arguments = GetRequestArguments(
            @buffer, @opts)
      });

    public Task SetVar(NvimBuffer @buffer, string @name, object @value) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_set_var",
        Arguments = GetRequestArguments(
            @buffer, @name, @value)
      });

    public Task DelVar(NvimBuffer @buffer, string @name) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_del_var",
        Arguments = GetRequestArguments(
            @buffer, @name)
      });

    public Task<object> GetOption(NvimBuffer @buffer, string @name) =>
      _api.SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_buf_get_option",
        Arguments = GetRequestArguments(
            @buffer, @name)
      });

    public Task SetOption(NvimBuffer @buffer, string @name, object @value) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_set_option",
        Arguments = GetRequestArguments(
            @buffer, @name, @value)
      });

    public Task<string> GetName(NvimBuffer @buffer) =>
      _api.SendAndReceive<string>(new NvimRequest
      {
        Method = "nvim_buf_get_name",
        Arguments = GetRequestArguments(
            @buffer)
      });

    public Task SetName(NvimBuffer @buffer, string @name) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_set_name",
        Arguments = GetRequestArguments(
            @buffer, @name)
      });

    public Task<bool> IsValid(NvimBuffer @buffer) =>
      _api.SendAndReceive<bool>(new NvimRequest
      {
        Method = "nvim_buf_is_valid",
        Arguments = GetRequestArguments(
            @buffer)
      });

    public Task<long[]> GetMark(NvimBuffer @buffer, string @name) =>
      _api.SendAndReceive<long[]>(new NvimRequest
      {
        Method = "nvim_buf_get_mark",
        Arguments = GetRequestArguments(
            @buffer, @name)
      });

    public Task<long> AddHighlight(NvimBuffer @buffer, long @srcId, string @hlGroup, long @line, long @colStart, long @colEnd) =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_buf_add_highlight",
        Arguments = GetRequestArguments(
            @buffer, @srcId, @hlGroup, @line, @colStart, @colEnd)
      });

    public Task ClearHighlight(NvimBuffer @buffer, long @srcId, long @lineStart, long @lineEnd) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_clear_highlight",
        Arguments = GetRequestArguments(
            @buffer, @srcId, @lineStart, @lineEnd)
      });

  }
  public class NvimWindow
  {
    private readonly NvimAPI _api;
    public NvimWindow(NvimAPI api) => _api = api;
    
    public Task<NvimBuffer> GetBuf(NvimWindow @window) =>
      _api.SendAndReceive<NvimBuffer>(new NvimRequest
      {
        Method = "nvim_win_get_buf",
        Arguments = GetRequestArguments(
            @window)
      });

    public Task<long[]> GetCursor(NvimWindow @window) =>
      _api.SendAndReceive<long[]>(new NvimRequest
      {
        Method = "nvim_win_get_cursor",
        Arguments = GetRequestArguments(
            @window)
      });

    public Task SetCursor(NvimWindow @window, long[] @pos) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_set_cursor",
        Arguments = GetRequestArguments(
            @window, @pos)
      });

    public Task<long> GetHeight(NvimWindow @window) =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_win_get_height",
        Arguments = GetRequestArguments(
            @window)
      });

    public Task SetHeight(NvimWindow @window, long @height) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_set_height",
        Arguments = GetRequestArguments(
            @window, @height)
      });

    public Task<long> GetWidth(NvimWindow @window) =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_win_get_width",
        Arguments = GetRequestArguments(
            @window)
      });

    public Task SetWidth(NvimWindow @window, long @width) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_set_width",
        Arguments = GetRequestArguments(
            @window, @width)
      });

    public Task<object> GetVar(NvimWindow @window, string @name) =>
      _api.SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_win_get_var",
        Arguments = GetRequestArguments(
            @window, @name)
      });

    public Task SetVar(NvimWindow @window, string @name, object @value) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_set_var",
        Arguments = GetRequestArguments(
            @window, @name, @value)
      });

    public Task DelVar(NvimWindow @window, string @name) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_del_var",
        Arguments = GetRequestArguments(
            @window, @name)
      });

    public Task<object> GetOption(NvimWindow @window, string @name) =>
      _api.SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_win_get_option",
        Arguments = GetRequestArguments(
            @window, @name)
      });

    public Task SetOption(NvimWindow @window, string @name, object @value) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_set_option",
        Arguments = GetRequestArguments(
            @window, @name, @value)
      });

    public Task<long[]> GetPosition(NvimWindow @window) =>
      _api.SendAndReceive<long[]>(new NvimRequest
      {
        Method = "nvim_win_get_position",
        Arguments = GetRequestArguments(
            @window)
      });

    public Task<NvimTabpage> GetTabpage(NvimWindow @window) =>
      _api.SendAndReceive<NvimTabpage>(new NvimRequest
      {
        Method = "nvim_win_get_tabpage",
        Arguments = GetRequestArguments(
            @window)
      });

    public Task<long> GetNumber(NvimWindow @window) =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_win_get_number",
        Arguments = GetRequestArguments(
            @window)
      });

    public Task<bool> IsValid(NvimWindow @window) =>
      _api.SendAndReceive<bool>(new NvimRequest
      {
        Method = "nvim_win_is_valid",
        Arguments = GetRequestArguments(
            @window)
      });

  }
  public class NvimTabpage
  {
    private readonly NvimAPI _api;
    public NvimTabpage(NvimAPI api) => _api = api;
    
    public Task<NvimWindow[]> ListWins(NvimTabpage @tabpage) =>
      _api.SendAndReceive<NvimWindow[]>(new NvimRequest
      {
        Method = "nvim_tabpage_list_wins",
        Arguments = GetRequestArguments(
            @tabpage)
      });

    public Task<object> GetVar(NvimTabpage @tabpage, string @name) =>
      _api.SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_tabpage_get_var",
        Arguments = GetRequestArguments(
            @tabpage, @name)
      });

    public Task SetVar(NvimTabpage @tabpage, string @name, object @value) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_tabpage_set_var",
        Arguments = GetRequestArguments(
            @tabpage, @name, @value)
      });

    public Task DelVar(NvimTabpage @tabpage, string @name) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_tabpage_del_var",
        Arguments = GetRequestArguments(
            @tabpage, @name)
      });

    public Task<NvimWindow> GetWin(NvimTabpage @tabpage) =>
      _api.SendAndReceive<NvimWindow>(new NvimRequest
      {
        Method = "nvim_tabpage_get_win",
        Arguments = GetRequestArguments(
            @tabpage)
      });

    public Task<long> GetNumber(NvimTabpage @tabpage) =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_tabpage_get_number",
        Arguments = GetRequestArguments(
            @tabpage)
      });

    public Task<bool> IsValid(NvimTabpage @tabpage) =>
      _api.SendAndReceive<bool>(new NvimRequest
      {
        Method = "nvim_tabpage_is_valid",
        Arguments = GetRequestArguments(
            @tabpage)
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
  }
}