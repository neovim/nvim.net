using System.Collections.Generic;

namespace Nvim.Client
{
  /// <summary>
  /// Represents the <c>mode_info_set</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record ModeInfoSet(
    NvimBoolean Enabled,
    IReadOnlyList<NvimValue> CursorStyles
  ) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>update_menu</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record UpdateMenu() : NvimUiEvent;

  /// <summary>
  /// Represents the <c>busy_start</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record BusyStart() : NvimUiEvent;

  /// <summary>
  /// Represents the <c>busy_stop</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record BusyStop() : NvimUiEvent;

  /// <summary>
  /// Represents the <c>mouse_on</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record MouseOn() : NvimUiEvent;

  /// <summary>
  /// Represents the <c>mouse_off</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record MouseOff() : NvimUiEvent;

  /// <summary>
  /// Represents the <c>mode_change</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record ModeChange(NvimString Mode, NvimInteger ModeIdx)
    : NvimUiEvent;

  /// <summary>
  /// Represents the <c>bell</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record Bell() : NvimUiEvent;

  /// <summary>
  /// Represents the <c>visual_bell</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record VisualBell() : NvimUiEvent;

  /// <summary>
  /// Represents the <c>flush</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record Flush() : NvimUiEvent;

  /// <summary>
  /// Represents the <c>connect</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record Connect(NvimString ServerAddr) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>restart</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record Restart(NvimString ListenAddr) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>suspend</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record Suspend() : NvimUiEvent;

  /// <summary>
  /// Represents the <c>set_title</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record SetTitle(NvimString Title) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>set_icon</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record SetIcon(NvimString Icon) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>screenshot</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record Screenshot(NvimString Path) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>option_set</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record OptionSet(NvimString Name, NvimValue Value)
    : NvimUiEvent;

  /// <summary>
  /// Represents the <c>chdir</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record Chdir(NvimString Path) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>ui_send</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record UiSend(NvimString Content) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>update_fg</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record UpdateFg(NvimInteger Fg) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>update_bg</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record UpdateBg(NvimInteger Bg) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>update_sp</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record UpdateSp(NvimInteger Sp) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>resize</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record Resize(NvimInteger Width, NvimInteger Height)
    : NvimUiEvent;

  /// <summary>
  /// Represents the <c>clear</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record Clear() : NvimUiEvent;

  /// <summary>
  /// Represents the <c>eol_clear</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record EolClear() : NvimUiEvent;

  /// <summary>
  /// Represents the <c>cursor_goto</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record CursorGoto(NvimInteger Row, NvimInteger Col)
    : NvimUiEvent;

  /// <summary>
  /// Represents the <c>highlight_set</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record HighlightSet(IReadOnlyList<NvimMapEntry> Attrs)
    : NvimUiEvent;

  /// <summary>
  /// Represents the <c>put</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record Put(NvimString Str) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>set_scroll_region</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record SetScrollRegion(
    NvimInteger Top,
    NvimInteger Bot,
    NvimInteger Left,
    NvimInteger Right
  ) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>scroll</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record Scroll(NvimInteger Count) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>default_colors_set</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record DefaultColorsSet(
    NvimInteger RgbFg,
    NvimInteger RgbBg,
    NvimInteger RgbSp,
    NvimInteger CtermFg,
    NvimInteger CtermBg
  ) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>hl_attr_define</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record HlAttrDefine(
    NvimInteger Id,
    IReadOnlyList<NvimMapEntry> RgbAttrs,
    IReadOnlyList<NvimMapEntry> CtermAttrs,
    IReadOnlyList<NvimValue> Info
  ) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>hl_group_set</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record HlGroupSet(NvimString Name, NvimInteger Id)
    : NvimUiEvent;

  /// <summary>
  /// Represents the <c>grid_resize</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record GridResize(
    NvimInteger Grid,
    NvimInteger Width,
    NvimInteger Height
  ) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>grid_clear</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record GridClear(NvimInteger Grid) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>grid_cursor_goto</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record GridCursorGoto(
    NvimInteger Grid,
    NvimInteger Row,
    NvimInteger Col
  ) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>grid_line</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record GridLine(
    NvimInteger Grid,
    NvimInteger Row,
    NvimInteger ColStart,
    IReadOnlyList<NvimValue> Data,
    NvimBoolean Wrap
  ) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>grid_scroll</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record GridScroll(
    NvimInteger Grid,
    NvimInteger Top,
    NvimInteger Bot,
    NvimInteger Left,
    NvimInteger Right,
    NvimInteger Rows,
    NvimInteger Cols
  ) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>grid_destroy</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record GridDestroy(NvimInteger Grid) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>win_pos</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record WinPos(
    NvimInteger Grid,
    Window Win,
    NvimInteger Startrow,
    NvimInteger Startcol,
    NvimInteger Width,
    NvimInteger Height
  ) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>win_float_pos</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record WinFloatPos(
    NvimInteger Grid,
    Window Win,
    NvimString Anchor,
    NvimInteger AnchorGrid,
    NvimFloat AnchorRow,
    NvimFloat AnchorCol,
    NvimBoolean MouseEnabled,
    NvimInteger Zindex,
    NvimInteger Compindex,
    NvimInteger ScreenRow,
    NvimInteger ScreenCol
  ) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>win_external_pos</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record WinExternalPos(NvimInteger Grid, Window Win)
    : NvimUiEvent;

  /// <summary>
  /// Represents the <c>win_hide</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record WinHide(NvimInteger Grid) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>win_close</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record WinClose(NvimInteger Grid) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>msg_set_pos</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record MsgSetPos(
    NvimInteger Grid,
    NvimInteger Row,
    NvimBoolean Scrolled,
    NvimString SepChar,
    NvimInteger Zindex,
    NvimInteger Compindex
  ) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>win_viewport</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record WinViewport(
    NvimInteger Grid,
    Window Win,
    NvimInteger Topline,
    NvimInteger Botline,
    NvimInteger Curline,
    NvimInteger Curcol,
    NvimInteger LineCount,
    NvimInteger ScrollDelta
  ) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>win_viewport_margins</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record WinViewportMargins(
    NvimInteger Grid,
    Window Win,
    NvimInteger Top,
    NvimInteger Bottom,
    NvimInteger Left,
    NvimInteger Right
  ) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>win_extmark</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record WinExtmark(
    NvimInteger Grid,
    Window Win,
    NvimInteger NsId,
    NvimInteger MarkId,
    NvimInteger Row,
    NvimInteger Col
  ) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>popupmenu_show</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record PopupmenuShow(
    IReadOnlyList<NvimValue> Items,
    NvimInteger Selected,
    NvimInteger Row,
    NvimInteger Col,
    NvimInteger Grid
  ) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>popupmenu_hide</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record PopupmenuHide() : NvimUiEvent;

  /// <summary>
  /// Represents the <c>popupmenu_select</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record PopupmenuSelect(NvimInteger Selected) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>tabline_update</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record TablineUpdate(
    Tabpage Current,
    IReadOnlyList<NvimValue> Tabs,
    Buffer CurrentBuffer,
    IReadOnlyList<NvimValue> Buffers
  ) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>cmdline_show</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record CmdlineShow(
    IReadOnlyList<NvimValue> Content,
    NvimInteger Pos,
    NvimString Firstc,
    NvimString Prompt,
    NvimInteger Indent,
    NvimInteger Level,
    NvimInteger HlId
  ) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>cmdline_pos</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record CmdlinePos(NvimInteger Pos, NvimInteger Level)
    : NvimUiEvent;

  /// <summary>
  /// Represents the <c>cmdline_special_char</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record CmdlineSpecialChar(
    NvimString C,
    NvimBoolean Shift,
    NvimInteger Level
  ) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>cmdline_hide</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record CmdlineHide(NvimInteger Level, NvimBoolean Abort)
    : NvimUiEvent;

  /// <summary>
  /// Represents the <c>cmdline_block_show</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record CmdlineBlockShow(IReadOnlyList<NvimValue> Lines)
    : NvimUiEvent;

  /// <summary>
  /// Represents the <c>cmdline_block_append</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record CmdlineBlockAppend(IReadOnlyList<NvimValue> Lines)
    : NvimUiEvent;

  /// <summary>
  /// Represents the <c>cmdline_block_hide</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record CmdlineBlockHide() : NvimUiEvent;

  /// <summary>
  /// Represents the <c>wildmenu_show</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record WildmenuShow(IReadOnlyList<NvimValue> Items)
    : NvimUiEvent;

  /// <summary>
  /// Represents the <c>wildmenu_select</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record WildmenuSelect(NvimInteger Selected) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>wildmenu_hide</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record WildmenuHide() : NvimUiEvent;

  /// <summary>
  /// Represents the <c>msg_show</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record MsgShow(
    NvimString Kind,
    IReadOnlyList<NvimValue> Content,
    NvimBoolean ReplaceLast,
    NvimBoolean History,
    NvimBoolean Append,
    NvimValue Id,
    NvimString Trigger
  ) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>msg_clear</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record MsgClear() : NvimUiEvent;

  /// <summary>
  /// Represents the <c>msg_showcmd</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record MsgShowcmd(IReadOnlyList<NvimValue> Content)
    : NvimUiEvent;

  /// <summary>
  /// Represents the <c>msg_showmode</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record MsgShowmode(IReadOnlyList<NvimValue> Content)
    : NvimUiEvent;

  /// <summary>
  /// Represents the <c>msg_ruler</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record MsgRuler(IReadOnlyList<NvimValue> Content) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>msg_history_show</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record MsgHistoryShow(
    IReadOnlyList<NvimValue> Entries,
    NvimBoolean PrevCmd
  ) : NvimUiEvent;

  /// <summary>
  /// Represents the <c>error_exit</c> UI redraw event.
  /// <see href="https://neovim.io/doc/user/api-ui-events.html">
  /// See the Neovim UI event documentation for more.
  /// </see>
  /// </summary>
  public sealed record ErrorExit(NvimInteger Status) : NvimUiEvent;
}
