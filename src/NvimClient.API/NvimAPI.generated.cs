
using System.Threading.Tasks;
using MsgPack;
using NvimClient.NvimMsgpack.Models;

namespace NvimClient.API
{
  public partial class NvimAPI
  {

    public Task NvimUiAttach(long @width, long @height, MessagePackObject @options) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_ui_attach",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@width), MessagePackObject.FromObject(@height), MessagePackObject.FromObject(@options)
        })
      });

    public Task NvimUiDetach() =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_ui_detach",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            
        })
      });

    public Task NvimUiTryResize(long @width, long @height) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_ui_try_resize",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@width), MessagePackObject.FromObject(@height)
        })
      });

    public Task NvimUiSetOption(string @name, object @value) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_ui_set_option",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@name), MessagePackObject.FromObject(@value)
        })
      });

    public Task NvimCommand(string @command) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_command",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@command)
        })
      });

    public Task<MessagePackObject> NvimGetHlByName(string @name, bool @rgb) =>
      SendAndReceive<MessagePackObject>(new NvimRequest
      {
        Method = "nvim_get_hl_by_name",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@name), MessagePackObject.FromObject(@rgb)
        })
      });

    public Task<MessagePackObject> NvimGetHlById(long @hlId, bool @rgb) =>
      SendAndReceive<MessagePackObject>(new NvimRequest
      {
        Method = "nvim_get_hl_by_id",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@hlId), MessagePackObject.FromObject(@rgb)
        })
      });

    public Task NvimFeedkeys(string @keys, string @mode, bool @escapeCsi) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_feedkeys",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@keys), MessagePackObject.FromObject(@mode), MessagePackObject.FromObject(@escapeCsi)
        })
      });

    public Task<long> NvimInput(string @keys) =>
      SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_input",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@keys)
        })
      });

    public Task<string> NvimReplaceTermcodes(string @str, bool @fromPart, bool @doLt, bool @special) =>
      SendAndReceive<string>(new NvimRequest
      {
        Method = "nvim_replace_termcodes",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@str), MessagePackObject.FromObject(@fromPart), MessagePackObject.FromObject(@doLt), MessagePackObject.FromObject(@special)
        })
      });

    public Task<string> NvimCommandOutput(string @str) =>
      SendAndReceive<string>(new NvimRequest
      {
        Method = "nvim_command_output",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@str)
        })
      });

    public Task<object> NvimEval(string @expr) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_eval",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@expr)
        })
      });

    public Task<object> NvimCallFunction(string @fname, MessagePackObject[] @args) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_call_function",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@fname), MessagePackObject.FromObject(@args)
        })
      });

    public Task<object> NvimExecuteLua(string @code, MessagePackObject[] @args) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_execute_lua",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@code), MessagePackObject.FromObject(@args)
        })
      });

    public Task<long> NvimStrwidth(string @text) =>
      SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_strwidth",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@text)
        })
      });

    public Task<string[]> NvimListRuntimePaths() =>
      SendAndReceive<string[]>(new NvimRequest
      {
        Method = "nvim_list_runtime_paths",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            
        })
      });

    public Task NvimSetCurrentDir(string @dir) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_current_dir",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@dir)
        })
      });

    public Task<string> NvimGetCurrentLine() =>
      SendAndReceive<string>(new NvimRequest
      {
        Method = "nvim_get_current_line",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            
        })
      });

    public Task NvimSetCurrentLine(string @line) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_current_line",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@line)
        })
      });

    public Task NvimDelCurrentLine() =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_del_current_line",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            
        })
      });

    public Task<object> NvimGetVar(string @name) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_get_var",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@name)
        })
      });

    public Task NvimSetVar(string @name, object @value) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_var",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@name), MessagePackObject.FromObject(@value)
        })
      });

    public Task NvimDelVar(string @name) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_del_var",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@name)
        })
      });

    public Task<object> NvimGetVvar(string @name) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_get_vvar",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@name)
        })
      });

    public Task<object> NvimGetOption(string @name) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_get_option",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@name)
        })
      });

    public Task NvimSetOption(string @name, object @value) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_option",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@name), MessagePackObject.FromObject(@value)
        })
      });

    public Task NvimOutWrite(string @str) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_out_write",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@str)
        })
      });

    public Task NvimErrWrite(string @str) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_err_write",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@str)
        })
      });

    public Task NvimErrWriteln(string @str) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_err_writeln",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@str)
        })
      });

    public Task<NvimBuffer[]> NvimListBufs() =>
      SendAndReceive<NvimBuffer[]>(new NvimRequest
      {
        Method = "nvim_list_bufs",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            
        })
      });

    public Task<NvimBuffer> NvimGetCurrentBuf() =>
      SendAndReceive<NvimBuffer>(new NvimRequest
      {
        Method = "nvim_get_current_buf",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            
        })
      });

    public Task NvimSetCurrentBuf(NvimBuffer @buffer) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_current_buf",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@buffer)
        })
      });

    public Task<NvimWindow[]> NvimListWins() =>
      SendAndReceive<NvimWindow[]>(new NvimRequest
      {
        Method = "nvim_list_wins",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            
        })
      });

    public Task<NvimWindow> NvimGetCurrentWin() =>
      SendAndReceive<NvimWindow>(new NvimRequest
      {
        Method = "nvim_get_current_win",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            
        })
      });

    public Task NvimSetCurrentWin(NvimWindow @window) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_current_win",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@window)
        })
      });

    public Task<NvimTabpage[]> NvimListTabpages() =>
      SendAndReceive<NvimTabpage[]>(new NvimRequest
      {
        Method = "nvim_list_tabpages",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            
        })
      });

    public Task<NvimTabpage> NvimGetCurrentTabpage() =>
      SendAndReceive<NvimTabpage>(new NvimRequest
      {
        Method = "nvim_get_current_tabpage",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            
        })
      });

    public Task NvimSetCurrentTabpage(NvimTabpage @tabpage) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_current_tabpage",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@tabpage)
        })
      });

    public Task NvimSubscribe(string @event) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_subscribe",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@event)
        })
      });

    public Task NvimUnsubscribe(string @event) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_unsubscribe",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@event)
        })
      });

    public Task<long> NvimGetColorByName(string @name) =>
      SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_get_color_by_name",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@name)
        })
      });

    public Task<MessagePackObject> NvimGetColorMap() =>
      SendAndReceive<MessagePackObject>(new NvimRequest
      {
        Method = "nvim_get_color_map",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            
        })
      });

    public Task<MessagePackObject> NvimGetMode() =>
      SendAndReceive<MessagePackObject>(new NvimRequest
      {
        Method = "nvim_get_mode",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            
        })
      });

    public Task<MessagePackObject[]> NvimGetKeymap(string @mode) =>
      SendAndReceive<MessagePackObject[]>(new NvimRequest
      {
        Method = "nvim_get_keymap",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@mode)
        })
      });

    public Task<MessagePackObject[]> NvimGetApiInfo() =>
      SendAndReceive<MessagePackObject[]>(new NvimRequest
      {
        Method = "nvim_get_api_info",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            
        })
      });

    public Task<MessagePackObject[]> NvimCallAtomic(MessagePackObject[] @calls) =>
      SendAndReceive<MessagePackObject[]>(new NvimRequest
      {
        Method = "nvim_call_atomic",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@calls)
        })
      });


  public class NvimBuffer
  {
    private readonly NvimAPI _api;
    public NvimBuffer(NvimAPI api) => _api = api;
    
    public Task<long> LineCount(NvimBuffer @buffer) =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_buf_line_count",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@buffer)
        })
      });

    public Task<string[]> GetLines(NvimBuffer @buffer, long @start, long @end, bool @strictIndexing) =>
      _api.SendAndReceive<string[]>(new NvimRequest
      {
        Method = "nvim_buf_get_lines",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@buffer), MessagePackObject.FromObject(@start), MessagePackObject.FromObject(@end), MessagePackObject.FromObject(@strictIndexing)
        })
      });

    public Task SetLines(NvimBuffer @buffer, long @start, long @end, bool @strictIndexing, string[] @replacement) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_set_lines",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@buffer), MessagePackObject.FromObject(@start), MessagePackObject.FromObject(@end), MessagePackObject.FromObject(@strictIndexing), MessagePackObject.FromObject(@replacement)
        })
      });

    public Task<object> GetVar(NvimBuffer @buffer, string @name) =>
      _api.SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_buf_get_var",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@buffer), MessagePackObject.FromObject(@name)
        })
      });

    public Task<long> GetChangedtick(NvimBuffer @buffer) =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_buf_get_changedtick",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@buffer)
        })
      });

    public Task<MessagePackObject[]> GetKeymap(NvimBuffer @buffer, string @mode) =>
      _api.SendAndReceive<MessagePackObject[]>(new NvimRequest
      {
        Method = "nvim_buf_get_keymap",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@buffer), MessagePackObject.FromObject(@mode)
        })
      });

    public Task SetVar(NvimBuffer @buffer, string @name, object @value) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_set_var",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@buffer), MessagePackObject.FromObject(@name), MessagePackObject.FromObject(@value)
        })
      });

    public Task DelVar(NvimBuffer @buffer, string @name) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_del_var",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@buffer), MessagePackObject.FromObject(@name)
        })
      });

    public Task<object> GetOption(NvimBuffer @buffer, string @name) =>
      _api.SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_buf_get_option",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@buffer), MessagePackObject.FromObject(@name)
        })
      });

    public Task SetOption(NvimBuffer @buffer, string @name, object @value) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_set_option",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@buffer), MessagePackObject.FromObject(@name), MessagePackObject.FromObject(@value)
        })
      });

    public Task<string> GetName(NvimBuffer @buffer) =>
      _api.SendAndReceive<string>(new NvimRequest
      {
        Method = "nvim_buf_get_name",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@buffer)
        })
      });

    public Task SetName(NvimBuffer @buffer, string @name) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_set_name",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@buffer), MessagePackObject.FromObject(@name)
        })
      });

    public Task<bool> IsValid(NvimBuffer @buffer) =>
      _api.SendAndReceive<bool>(new NvimRequest
      {
        Method = "nvim_buf_is_valid",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@buffer)
        })
      });

    public Task<long[]> GetMark(NvimBuffer @buffer, string @name) =>
      _api.SendAndReceive<long[]>(new NvimRequest
      {
        Method = "nvim_buf_get_mark",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@buffer), MessagePackObject.FromObject(@name)
        })
      });

    public Task<long> AddHighlight(NvimBuffer @buffer, long @srcId, string @hlGroup, long @line, long @colStart, long @colEnd) =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_buf_add_highlight",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@buffer), MessagePackObject.FromObject(@srcId), MessagePackObject.FromObject(@hlGroup), MessagePackObject.FromObject(@line), MessagePackObject.FromObject(@colStart), MessagePackObject.FromObject(@colEnd)
        })
      });

    public Task ClearHighlight(NvimBuffer @buffer, long @srcId, long @lineStart, long @lineEnd) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_clear_highlight",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@buffer), MessagePackObject.FromObject(@srcId), MessagePackObject.FromObject(@lineStart), MessagePackObject.FromObject(@lineEnd)
        })
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
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@window)
        })
      });

    public Task<long[]> GetCursor(NvimWindow @window) =>
      _api.SendAndReceive<long[]>(new NvimRequest
      {
        Method = "nvim_win_get_cursor",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@window)
        })
      });

    public Task SetCursor(NvimWindow @window, long[] @pos) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_set_cursor",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@window), MessagePackObject.FromObject(@pos)
        })
      });

    public Task<long> GetHeight(NvimWindow @window) =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_win_get_height",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@window)
        })
      });

    public Task SetHeight(NvimWindow @window, long @height) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_set_height",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@window), MessagePackObject.FromObject(@height)
        })
      });

    public Task<long> GetWidth(NvimWindow @window) =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_win_get_width",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@window)
        })
      });

    public Task SetWidth(NvimWindow @window, long @width) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_set_width",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@window), MessagePackObject.FromObject(@width)
        })
      });

    public Task<object> GetVar(NvimWindow @window, string @name) =>
      _api.SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_win_get_var",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@window), MessagePackObject.FromObject(@name)
        })
      });

    public Task SetVar(NvimWindow @window, string @name, object @value) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_set_var",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@window), MessagePackObject.FromObject(@name), MessagePackObject.FromObject(@value)
        })
      });

    public Task DelVar(NvimWindow @window, string @name) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_del_var",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@window), MessagePackObject.FromObject(@name)
        })
      });

    public Task<object> GetOption(NvimWindow @window, string @name) =>
      _api.SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_win_get_option",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@window), MessagePackObject.FromObject(@name)
        })
      });

    public Task SetOption(NvimWindow @window, string @name, object @value) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_set_option",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@window), MessagePackObject.FromObject(@name), MessagePackObject.FromObject(@value)
        })
      });

    public Task<long[]> GetPosition(NvimWindow @window) =>
      _api.SendAndReceive<long[]>(new NvimRequest
      {
        Method = "nvim_win_get_position",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@window)
        })
      });

    public Task<NvimTabpage> GetTabpage(NvimWindow @window) =>
      _api.SendAndReceive<NvimTabpage>(new NvimRequest
      {
        Method = "nvim_win_get_tabpage",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@window)
        })
      });

    public Task<long> GetNumber(NvimWindow @window) =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_win_get_number",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@window)
        })
      });

    public Task<bool> IsValid(NvimWindow @window) =>
      _api.SendAndReceive<bool>(new NvimRequest
      {
        Method = "nvim_win_is_valid",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@window)
        })
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
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@tabpage)
        })
      });

    public Task<object> GetVar(NvimTabpage @tabpage, string @name) =>
      _api.SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_tabpage_get_var",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@tabpage), MessagePackObject.FromObject(@name)
        })
      });

    public Task SetVar(NvimTabpage @tabpage, string @name, object @value) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_tabpage_set_var",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@tabpage), MessagePackObject.FromObject(@name), MessagePackObject.FromObject(@value)
        })
      });

    public Task DelVar(NvimTabpage @tabpage, string @name) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_tabpage_del_var",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@tabpage), MessagePackObject.FromObject(@name)
        })
      });

    public Task<NvimWindow> GetWin(NvimTabpage @tabpage) =>
      _api.SendAndReceive<NvimWindow>(new NvimRequest
      {
        Method = "nvim_tabpage_get_win",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@tabpage)
        })
      });

    public Task<long> GetNumber(NvimTabpage @tabpage) =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_tabpage_get_number",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@tabpage)
        })
      });

    public Task<bool> IsValid(NvimTabpage @tabpage) =>
      _api.SendAndReceive<bool>(new NvimRequest
      {
        Method = "nvim_tabpage_is_valid",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {
            MessagePackObject.FromObject(@tabpage)
        })
      });

  }
  }
}