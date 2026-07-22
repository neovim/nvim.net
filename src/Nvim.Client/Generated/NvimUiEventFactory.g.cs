#nullable enable
using System.Collections.Generic;

namespace Nvim.Client
{
  internal static partial class NvimUiEventFactory
  {
    static partial void Decode(
      IReadOnlyList<NvimValue> batch,
      ref IReadOnlyList<NvimUiEvent> events
    )
    {
      events = (UiEventDecoder.Decode(batch, Create));
    }

    private static NvimUiEvent? Create(
      string name,
      IReadOnlyList<NvimValue> values
    ) =>
      name switch
      {
        "mode_info_set" => CreateModeInfoSet(values),
        "update_menu" => CreateUpdateMenu(values),
        "busy_start" => CreateBusyStart(values),
        "busy_stop" => CreateBusyStop(values),
        "mouse_on" => CreateMouseOn(values),
        "mouse_off" => CreateMouseOff(values),
        "mode_change" => CreateModeChange(values),
        "bell" => CreateBell(values),
        "visual_bell" => CreateVisualBell(values),
        "flush" => CreateFlush(values),
        "connect" => CreateConnect(values),
        "restart" => CreateRestart(values),
        "suspend" => CreateSuspend(values),
        "set_title" => CreateSetTitle(values),
        "set_icon" => CreateSetIcon(values),
        "screenshot" => CreateScreenshot(values),
        "option_set" => CreateOptionSet(values),
        "chdir" => CreateChdir(values),
        "ui_send" => CreateUiSend(values),
        "update_fg" => CreateUpdateFg(values),
        "update_bg" => CreateUpdateBg(values),
        "update_sp" => CreateUpdateSp(values),
        "resize" => CreateResize(values),
        "clear" => CreateClear(values),
        "eol_clear" => CreateEolClear(values),
        "cursor_goto" => CreateCursorGoto(values),
        "highlight_set" => CreateHighlightSet(values),
        "put" => CreatePut(values),
        "set_scroll_region" => CreateSetScrollRegion(values),
        "scroll" => CreateScroll(values),
        "default_colors_set" => CreateDefaultColorsSet(values),
        "hl_attr_define" => CreateHlAttrDefine(values),
        "hl_group_set" => CreateHlGroupSet(values),
        "grid_resize" => CreateGridResize(values),
        "grid_clear" => CreateGridClear(values),
        "grid_cursor_goto" => CreateGridCursorGoto(values),
        "grid_line" => CreateGridLine(values),
        "grid_scroll" => CreateGridScroll(values),
        "grid_destroy" => CreateGridDestroy(values),
        "win_pos" => CreateWinPos(values),
        "win_float_pos" => CreateWinFloatPos(values),
        "win_external_pos" => CreateWinExternalPos(values),
        "win_hide" => CreateWinHide(values),
        "win_close" => CreateWinClose(values),
        "msg_set_pos" => CreateMsgSetPos(values),
        "win_viewport" => CreateWinViewport(values),
        "win_viewport_margins" => CreateWinViewportMargins(values),
        "win_extmark" => CreateWinExtmark(values),
        "popupmenu_show" => CreatePopupmenuShow(values),
        "popupmenu_hide" => CreatePopupmenuHide(values),
        "popupmenu_select" => CreatePopupmenuSelect(values),
        "tabline_update" => CreateTablineUpdate(values),
        "cmdline_show" => CreateCmdlineShow(values),
        "cmdline_pos" => CreateCmdlinePos(values),
        "cmdline_special_char" => CreateCmdlineSpecialChar(values),
        "cmdline_hide" => CreateCmdlineHide(values),
        "cmdline_block_show" => CreateCmdlineBlockShow(values),
        "cmdline_block_append" => CreateCmdlineBlockAppend(values),
        "cmdline_block_hide" => CreateCmdlineBlockHide(values),
        "wildmenu_show" => CreateWildmenuShow(values),
        "wildmenu_select" => CreateWildmenuSelect(values),
        "wildmenu_hide" => CreateWildmenuHide(values),
        "msg_show" => CreateMsgShow(values),
        "msg_clear" => CreateMsgClear(values),
        "msg_showcmd" => CreateMsgShowcmd(values),
        "msg_showmode" => CreateMsgShowmode(values),
        "msg_ruler" => CreateMsgRuler(values),
        "msg_history_show" => CreateMsgHistoryShow(values),
        "error_exit" => CreateErrorExit(values),
        _ => null,
      };

    private static ModeInfoSet CreateModeInfoSet(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 2);
      return new ModeInfoSet(
        UiEventDecoder.Require<NvimBoolean>(values[0]),
        UiEventDecoder.RequireArray(
          values[1],
          item => UiEventDecoder.Require<NvimValue>(item)
        )
      );
    }

    private static UpdateMenu CreateUpdateMenu(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 0);
      return new UpdateMenu();
    }

    private static BusyStart CreateBusyStart(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 0);
      return new BusyStart();
    }

    private static BusyStop CreateBusyStop(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 0);
      return new BusyStop();
    }

    private static MouseOn CreateMouseOn(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 0);
      return new MouseOn();
    }

    private static MouseOff CreateMouseOff(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 0);
      return new MouseOff();
    }

    private static ModeChange CreateModeChange(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 2);
      return new ModeChange(
        UiEventDecoder.Require<NvimString>(values[0]),
        UiEventDecoder.Require<NvimInteger>(values[1])
      );
    }

    private static Bell CreateBell(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 0);
      return new Bell();
    }

    private static VisualBell CreateVisualBell(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 0);
      return new VisualBell();
    }

    private static Flush CreateFlush(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 0);
      return new Flush();
    }

    private static Connect CreateConnect(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 1);
      return new Connect(UiEventDecoder.Require<NvimString>(values[0]));
    }

    private static Restart CreateRestart(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 1);
      return new Restart(UiEventDecoder.Require<NvimString>(values[0]));
    }

    private static Suspend CreateSuspend(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 0);
      return new Suspend();
    }

    private static SetTitle CreateSetTitle(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 1);
      return new SetTitle(UiEventDecoder.Require<NvimString>(values[0]));
    }

    private static SetIcon CreateSetIcon(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 1);
      return new SetIcon(UiEventDecoder.Require<NvimString>(values[0]));
    }

    private static Screenshot CreateScreenshot(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 1);
      return new Screenshot(UiEventDecoder.Require<NvimString>(values[0]));
    }

    private static OptionSet CreateOptionSet(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 2);
      return new OptionSet(
        UiEventDecoder.Require<NvimString>(values[0]),
        UiEventDecoder.Require<NvimValue>(values[1])
      );
    }

    private static Chdir CreateChdir(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 1);
      return new Chdir(UiEventDecoder.Require<NvimString>(values[0]));
    }

    private static UiSend CreateUiSend(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 1);
      return new UiSend(UiEventDecoder.Require<NvimString>(values[0]));
    }

    private static UpdateFg CreateUpdateFg(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 1);
      return new UpdateFg(UiEventDecoder.Require<NvimInteger>(values[0]));
    }

    private static UpdateBg CreateUpdateBg(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 1);
      return new UpdateBg(UiEventDecoder.Require<NvimInteger>(values[0]));
    }

    private static UpdateSp CreateUpdateSp(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 1);
      return new UpdateSp(UiEventDecoder.Require<NvimInteger>(values[0]));
    }

    private static Resize CreateResize(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 2);
      return new Resize(
        UiEventDecoder.Require<NvimInteger>(values[0]),
        UiEventDecoder.Require<NvimInteger>(values[1])
      );
    }

    private static Clear CreateClear(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 0);
      return new Clear();
    }

    private static EolClear CreateEolClear(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 0);
      return new EolClear();
    }

    private static CursorGoto CreateCursorGoto(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 2);
      return new CursorGoto(
        UiEventDecoder.Require<NvimInteger>(values[0]),
        UiEventDecoder.Require<NvimInteger>(values[1])
      );
    }

    private static HighlightSet CreateHighlightSet(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 1);
      return new HighlightSet(UiEventDecoder.RequireMap(values[0]));
    }

    private static Put CreatePut(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 1);
      return new Put(UiEventDecoder.Require<NvimString>(values[0]));
    }

    private static SetScrollRegion CreateSetScrollRegion(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 4);
      return new SetScrollRegion(
        UiEventDecoder.Require<NvimInteger>(values[0]),
        UiEventDecoder.Require<NvimInteger>(values[1]),
        UiEventDecoder.Require<NvimInteger>(values[2]),
        UiEventDecoder.Require<NvimInteger>(values[3])
      );
    }

    private static Scroll CreateScroll(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 1);
      return new Scroll(UiEventDecoder.Require<NvimInteger>(values[0]));
    }

    private static DefaultColorsSet CreateDefaultColorsSet(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 5);
      return new DefaultColorsSet(
        UiEventDecoder.Require<NvimInteger>(values[0]),
        UiEventDecoder.Require<NvimInteger>(values[1]),
        UiEventDecoder.Require<NvimInteger>(values[2]),
        UiEventDecoder.Require<NvimInteger>(values[3]),
        UiEventDecoder.Require<NvimInteger>(values[4])
      );
    }

    private static HlAttrDefine CreateHlAttrDefine(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 4);
      return new HlAttrDefine(
        UiEventDecoder.Require<NvimInteger>(values[0]),
        UiEventDecoder.RequireMap(values[1]),
        UiEventDecoder.RequireMap(values[2]),
        UiEventDecoder.RequireArray(
          values[3],
          item => UiEventDecoder.Require<NvimValue>(item)
        )
      );
    }

    private static HlGroupSet CreateHlGroupSet(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 2);
      return new HlGroupSet(
        UiEventDecoder.Require<NvimString>(values[0]),
        UiEventDecoder.Require<NvimInteger>(values[1])
      );
    }

    private static GridResize CreateGridResize(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 3);
      return new GridResize(
        UiEventDecoder.Require<NvimInteger>(values[0]),
        UiEventDecoder.Require<NvimInteger>(values[1]),
        UiEventDecoder.Require<NvimInteger>(values[2])
      );
    }

    private static GridClear CreateGridClear(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 1);
      return new GridClear(UiEventDecoder.Require<NvimInteger>(values[0]));
    }

    private static GridCursorGoto CreateGridCursorGoto(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 3);
      return new GridCursorGoto(
        UiEventDecoder.Require<NvimInteger>(values[0]),
        UiEventDecoder.Require<NvimInteger>(values[1]),
        UiEventDecoder.Require<NvimInteger>(values[2])
      );
    }

    private static GridLine CreateGridLine(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 5);
      return new GridLine(
        UiEventDecoder.Require<NvimInteger>(values[0]),
        UiEventDecoder.Require<NvimInteger>(values[1]),
        UiEventDecoder.Require<NvimInteger>(values[2]),
        UiEventDecoder.RequireArray(
          values[3],
          item => UiEventDecoder.Require<NvimValue>(item)
        ),
        UiEventDecoder.Require<NvimBoolean>(values[4])
      );
    }

    private static GridScroll CreateGridScroll(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 7);
      return new GridScroll(
        UiEventDecoder.Require<NvimInteger>(values[0]),
        UiEventDecoder.Require<NvimInteger>(values[1]),
        UiEventDecoder.Require<NvimInteger>(values[2]),
        UiEventDecoder.Require<NvimInteger>(values[3]),
        UiEventDecoder.Require<NvimInteger>(values[4]),
        UiEventDecoder.Require<NvimInteger>(values[5]),
        UiEventDecoder.Require<NvimInteger>(values[6])
      );
    }

    private static GridDestroy CreateGridDestroy(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 1);
      return new GridDestroy(UiEventDecoder.Require<NvimInteger>(values[0]));
    }

    private static WinPos CreateWinPos(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 6);
      return new WinPos(
        UiEventDecoder.Require<NvimInteger>(values[0]),
        new Window(UiEventDecoder.Require<NvimExtension>(values[1])),
        UiEventDecoder.Require<NvimInteger>(values[2]),
        UiEventDecoder.Require<NvimInteger>(values[3]),
        UiEventDecoder.Require<NvimInteger>(values[4]),
        UiEventDecoder.Require<NvimInteger>(values[5])
      );
    }

    private static WinFloatPos CreateWinFloatPos(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 11);
      return new WinFloatPos(
        UiEventDecoder.Require<NvimInteger>(values[0]),
        new Window(UiEventDecoder.Require<NvimExtension>(values[1])),
        UiEventDecoder.Require<NvimString>(values[2]),
        UiEventDecoder.Require<NvimInteger>(values[3]),
        UiEventDecoder.Require<NvimFloat>(values[4]),
        UiEventDecoder.Require<NvimFloat>(values[5]),
        UiEventDecoder.Require<NvimBoolean>(values[6]),
        UiEventDecoder.Require<NvimInteger>(values[7]),
        UiEventDecoder.Require<NvimInteger>(values[8]),
        UiEventDecoder.Require<NvimInteger>(values[9]),
        UiEventDecoder.Require<NvimInteger>(values[10])
      );
    }

    private static WinExternalPos CreateWinExternalPos(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 2);
      return new WinExternalPos(
        UiEventDecoder.Require<NvimInteger>(values[0]),
        new Window(UiEventDecoder.Require<NvimExtension>(values[1]))
      );
    }

    private static WinHide CreateWinHide(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 1);
      return new WinHide(UiEventDecoder.Require<NvimInteger>(values[0]));
    }

    private static WinClose CreateWinClose(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 1);
      return new WinClose(UiEventDecoder.Require<NvimInteger>(values[0]));
    }

    private static MsgSetPos CreateMsgSetPos(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 6);
      return new MsgSetPos(
        UiEventDecoder.Require<NvimInteger>(values[0]),
        UiEventDecoder.Require<NvimInteger>(values[1]),
        UiEventDecoder.Require<NvimBoolean>(values[2]),
        UiEventDecoder.Require<NvimString>(values[3]),
        UiEventDecoder.Require<NvimInteger>(values[4]),
        UiEventDecoder.Require<NvimInteger>(values[5])
      );
    }

    private static WinViewport CreateWinViewport(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 8);
      return new WinViewport(
        UiEventDecoder.Require<NvimInteger>(values[0]),
        new Window(UiEventDecoder.Require<NvimExtension>(values[1])),
        UiEventDecoder.Require<NvimInteger>(values[2]),
        UiEventDecoder.Require<NvimInteger>(values[3]),
        UiEventDecoder.Require<NvimInteger>(values[4]),
        UiEventDecoder.Require<NvimInteger>(values[5]),
        UiEventDecoder.Require<NvimInteger>(values[6]),
        UiEventDecoder.Require<NvimInteger>(values[7])
      );
    }

    private static WinViewportMargins CreateWinViewportMargins(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 6);
      return new WinViewportMargins(
        UiEventDecoder.Require<NvimInteger>(values[0]),
        new Window(UiEventDecoder.Require<NvimExtension>(values[1])),
        UiEventDecoder.Require<NvimInteger>(values[2]),
        UiEventDecoder.Require<NvimInteger>(values[3]),
        UiEventDecoder.Require<NvimInteger>(values[4]),
        UiEventDecoder.Require<NvimInteger>(values[5])
      );
    }

    private static WinExtmark CreateWinExtmark(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 6);
      return new WinExtmark(
        UiEventDecoder.Require<NvimInteger>(values[0]),
        new Window(UiEventDecoder.Require<NvimExtension>(values[1])),
        UiEventDecoder.Require<NvimInteger>(values[2]),
        UiEventDecoder.Require<NvimInteger>(values[3]),
        UiEventDecoder.Require<NvimInteger>(values[4]),
        UiEventDecoder.Require<NvimInteger>(values[5])
      );
    }

    private static PopupmenuShow CreatePopupmenuShow(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 5);
      return new PopupmenuShow(
        UiEventDecoder.RequireArray(
          values[0],
          item => UiEventDecoder.Require<NvimValue>(item)
        ),
        UiEventDecoder.Require<NvimInteger>(values[1]),
        UiEventDecoder.Require<NvimInteger>(values[2]),
        UiEventDecoder.Require<NvimInteger>(values[3]),
        UiEventDecoder.Require<NvimInteger>(values[4])
      );
    }

    private static PopupmenuHide CreatePopupmenuHide(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 0);
      return new PopupmenuHide();
    }

    private static PopupmenuSelect CreatePopupmenuSelect(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 1);
      return new PopupmenuSelect(
        UiEventDecoder.Require<NvimInteger>(values[0])
      );
    }

    private static TablineUpdate CreateTablineUpdate(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 4);
      return new TablineUpdate(
        new Tabpage(UiEventDecoder.Require<NvimExtension>(values[0])),
        UiEventDecoder.RequireArray(
          values[1],
          item => UiEventDecoder.Require<NvimValue>(item)
        ),
        new Buffer(UiEventDecoder.Require<NvimExtension>(values[2])),
        UiEventDecoder.RequireArray(
          values[3],
          item => UiEventDecoder.Require<NvimValue>(item)
        )
      );
    }

    private static CmdlineShow CreateCmdlineShow(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 7);
      return new CmdlineShow(
        UiEventDecoder.RequireArray(
          values[0],
          item => UiEventDecoder.Require<NvimValue>(item)
        ),
        UiEventDecoder.Require<NvimInteger>(values[1]),
        UiEventDecoder.Require<NvimString>(values[2]),
        UiEventDecoder.Require<NvimString>(values[3]),
        UiEventDecoder.Require<NvimInteger>(values[4]),
        UiEventDecoder.Require<NvimInteger>(values[5]),
        UiEventDecoder.Require<NvimInteger>(values[6])
      );
    }

    private static CmdlinePos CreateCmdlinePos(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 2);
      return new CmdlinePos(
        UiEventDecoder.Require<NvimInteger>(values[0]),
        UiEventDecoder.Require<NvimInteger>(values[1])
      );
    }

    private static CmdlineSpecialChar CreateCmdlineSpecialChar(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 3);
      return new CmdlineSpecialChar(
        UiEventDecoder.Require<NvimString>(values[0]),
        UiEventDecoder.Require<NvimBoolean>(values[1]),
        UiEventDecoder.Require<NvimInteger>(values[2])
      );
    }

    private static CmdlineHide CreateCmdlineHide(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 2);
      return new CmdlineHide(
        UiEventDecoder.Require<NvimInteger>(values[0]),
        UiEventDecoder.Require<NvimBoolean>(values[1])
      );
    }

    private static CmdlineBlockShow CreateCmdlineBlockShow(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 1);
      return new CmdlineBlockShow(
        UiEventDecoder.RequireArray(
          values[0],
          item => UiEventDecoder.Require<NvimValue>(item)
        )
      );
    }

    private static CmdlineBlockAppend CreateCmdlineBlockAppend(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 1);
      return new CmdlineBlockAppend(
        UiEventDecoder.RequireArray(
          values[0],
          item => UiEventDecoder.Require<NvimValue>(item)
        )
      );
    }

    private static CmdlineBlockHide CreateCmdlineBlockHide(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 0);
      return new CmdlineBlockHide();
    }

    private static WildmenuShow CreateWildmenuShow(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 1);
      return new WildmenuShow(
        UiEventDecoder.RequireArray(
          values[0],
          item => UiEventDecoder.Require<NvimValue>(item)
        )
      );
    }

    private static WildmenuSelect CreateWildmenuSelect(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 1);
      return new WildmenuSelect(UiEventDecoder.Require<NvimInteger>(values[0]));
    }

    private static WildmenuHide CreateWildmenuHide(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 0);
      return new WildmenuHide();
    }

    private static MsgShow CreateMsgShow(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 7);
      return new MsgShow(
        UiEventDecoder.Require<NvimString>(values[0]),
        UiEventDecoder.RequireArray(
          values[1],
          item => UiEventDecoder.Require<NvimValue>(item)
        ),
        UiEventDecoder.Require<NvimBoolean>(values[2]),
        UiEventDecoder.Require<NvimBoolean>(values[3]),
        UiEventDecoder.Require<NvimBoolean>(values[4]),
        UiEventDecoder.Require<NvimValue>(values[5]),
        UiEventDecoder.Require<NvimString>(values[6])
      );
    }

    private static MsgClear CreateMsgClear(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 0);
      return new MsgClear();
    }

    private static MsgShowcmd CreateMsgShowcmd(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 1);
      return new MsgShowcmd(
        UiEventDecoder.RequireArray(
          values[0],
          item => UiEventDecoder.Require<NvimValue>(item)
        )
      );
    }

    private static MsgShowmode CreateMsgShowmode(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 1);
      return new MsgShowmode(
        UiEventDecoder.RequireArray(
          values[0],
          item => UiEventDecoder.Require<NvimValue>(item)
        )
      );
    }

    private static MsgRuler CreateMsgRuler(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 1);
      return new MsgRuler(
        UiEventDecoder.RequireArray(
          values[0],
          item => UiEventDecoder.Require<NvimValue>(item)
        )
      );
    }

    private static MsgHistoryShow CreateMsgHistoryShow(
      IReadOnlyList<NvimValue> values
    )
    {
      UiEventDecoder.RequireArity(values, 2);
      return new MsgHistoryShow(
        UiEventDecoder.RequireArray(
          values[0],
          item => UiEventDecoder.Require<NvimValue>(item)
        ),
        UiEventDecoder.Require<NvimBoolean>(values[1])
      );
    }

    private static ErrorExit CreateErrorExit(IReadOnlyList<NvimValue> values)
    {
      UiEventDecoder.RequireArity(values, 1);
      return new ErrorExit(UiEventDecoder.Require<NvimInteger>(values[0]));
    }
  }
}
