
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

    /// <summary>
    /// EventHandler for <c>mode_info_set</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.ModeInfoSetEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `ModeInfoSetEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="ModeInfoSetEventArgs"/>
    public event EventHandler<ModeInfoSetEventArgs> ModeInfoSetEvent;

    /// <summary>
    /// EventHandler for <c>update_menu</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.UpdateMenuEvent += (sender, args) =>
    /// {
    ///     // `args` contains no data for this event.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    public event EventHandler UpdateMenuEvent;

    /// <summary>
    /// EventHandler for <c>busy_start</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.BusyStartEvent += (sender, args) =>
    /// {
    ///     // `args` contains no data for this event.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    public event EventHandler BusyStartEvent;

    /// <summary>
    /// EventHandler for <c>busy_stop</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.BusyStopEvent += (sender, args) =>
    /// {
    ///     // `args` contains no data for this event.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    public event EventHandler BusyStopEvent;

    /// <summary>
    /// EventHandler for <c>mouse_on</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.MouseOnEvent += (sender, args) =>
    /// {
    ///     // `args` contains no data for this event.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    public event EventHandler MouseOnEvent;

    /// <summary>
    /// EventHandler for <c>mouse_off</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.MouseOffEvent += (sender, args) =>
    /// {
    ///     // `args` contains no data for this event.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    public event EventHandler MouseOffEvent;

    /// <summary>
    /// EventHandler for <c>mode_change</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.ModeChangeEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `ModeChangeEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="ModeChangeEventArgs"/>
    public event EventHandler<ModeChangeEventArgs> ModeChangeEvent;

    /// <summary>
    /// EventHandler for <c>bell</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.BellEvent += (sender, args) =>
    /// {
    ///     // `args` contains no data for this event.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    public event EventHandler BellEvent;

    /// <summary>
    /// EventHandler for <c>visual_bell</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.VisualBellEvent += (sender, args) =>
    /// {
    ///     // `args` contains no data for this event.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    public event EventHandler VisualBellEvent;

    /// <summary>
    /// EventHandler for <c>flush</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.FlushEvent += (sender, args) =>
    /// {
    ///     // `args` contains no data for this event.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    public event EventHandler FlushEvent;

    /// <summary>
    /// EventHandler for <c>suspend</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.SuspendEvent += (sender, args) =>
    /// {
    ///     // `args` contains no data for this event.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    public event EventHandler SuspendEvent;

    /// <summary>
    /// EventHandler for <c>set_title</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.SetTitleEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `SetTitleEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="SetTitleEventArgs"/>
    public event EventHandler<SetTitleEventArgs> SetTitleEvent;

    /// <summary>
    /// EventHandler for <c>set_icon</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.SetIconEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `SetIconEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="SetIconEventArgs"/>
    public event EventHandler<SetIconEventArgs> SetIconEvent;

    /// <summary>
    /// EventHandler for <c>screenshot</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.ScreenshotEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `ScreenshotEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="ScreenshotEventArgs"/>
    public event EventHandler<ScreenshotEventArgs> ScreenshotEvent;

    /// <summary>
    /// EventHandler for <c>option_set</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.OptionSetEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `OptionSetEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="OptionSetEventArgs"/>
    public event EventHandler<OptionSetEventArgs> OptionSetEvent;

    /// <summary>
    /// EventHandler for <c>update_fg</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.UpdateFgEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `UpdateFgEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="UpdateFgEventArgs"/>
    public event EventHandler<UpdateFgEventArgs> UpdateFgEvent;

    /// <summary>
    /// EventHandler for <c>update_bg</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.UpdateBgEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `UpdateBgEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="UpdateBgEventArgs"/>
    public event EventHandler<UpdateBgEventArgs> UpdateBgEvent;

    /// <summary>
    /// EventHandler for <c>update_sp</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.UpdateSpEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `UpdateSpEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="UpdateSpEventArgs"/>
    public event EventHandler<UpdateSpEventArgs> UpdateSpEvent;

    /// <summary>
    /// EventHandler for <c>resize</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.ResizeEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `ResizeEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="ResizeEventArgs"/>
    public event EventHandler<ResizeEventArgs> ResizeEvent;

    /// <summary>
    /// EventHandler for <c>clear</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.ClearEvent += (sender, args) =>
    /// {
    ///     // `args` contains no data for this event.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    public event EventHandler ClearEvent;

    /// <summary>
    /// EventHandler for <c>eol_clear</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.EolClearEvent += (sender, args) =>
    /// {
    ///     // `args` contains no data for this event.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    public event EventHandler EolClearEvent;

    /// <summary>
    /// EventHandler for <c>cursor_goto</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.CursorGotoEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `CursorGotoEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="CursorGotoEventArgs"/>
    public event EventHandler<CursorGotoEventArgs> CursorGotoEvent;

    /// <summary>
    /// EventHandler for <c>highlight_set</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.HighlightSetEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `HighlightSetEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="HighlightSetEventArgs"/>
    public event EventHandler<HighlightSetEventArgs> HighlightSetEvent;

    /// <summary>
    /// EventHandler for <c>put</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.PutEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `PutEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="PutEventArgs"/>
    public event EventHandler<PutEventArgs> PutEvent;

    /// <summary>
    /// EventHandler for <c>set_scroll_region</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.SetScrollRegionEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `SetScrollRegionEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="SetScrollRegionEventArgs"/>
    public event EventHandler<SetScrollRegionEventArgs> SetScrollRegionEvent;

    /// <summary>
    /// EventHandler for <c>scroll</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.ScrollEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `ScrollEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="ScrollEventArgs"/>
    public event EventHandler<ScrollEventArgs> ScrollEvent;

    /// <summary>
    /// EventHandler for <c>default_colors_set</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.DefaultColorsSetEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `DefaultColorsSetEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="DefaultColorsSetEventArgs"/>
    public event EventHandler<DefaultColorsSetEventArgs> DefaultColorsSetEvent;

    /// <summary>
    /// EventHandler for <c>hl_attr_define</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.HlAttrDefineEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `HlAttrDefineEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="HlAttrDefineEventArgs"/>
    public event EventHandler<HlAttrDefineEventArgs> HlAttrDefineEvent;

    /// <summary>
    /// EventHandler for <c>hl_group_set</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.HlGroupSetEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `HlGroupSetEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="HlGroupSetEventArgs"/>
    public event EventHandler<HlGroupSetEventArgs> HlGroupSetEvent;

    /// <summary>
    /// EventHandler for <c>grid_resize</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.GridResizeEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `GridResizeEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="GridResizeEventArgs"/>
    public event EventHandler<GridResizeEventArgs> GridResizeEvent;

    /// <summary>
    /// EventHandler for <c>grid_clear</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.GridClearEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `GridClearEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="GridClearEventArgs"/>
    public event EventHandler<GridClearEventArgs> GridClearEvent;

    /// <summary>
    /// EventHandler for <c>grid_cursor_goto</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.GridCursorGotoEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `GridCursorGotoEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="GridCursorGotoEventArgs"/>
    public event EventHandler<GridCursorGotoEventArgs> GridCursorGotoEvent;

    /// <summary>
    /// EventHandler for <c>grid_line</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.GridLineEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `GridLineEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="GridLineEventArgs"/>
    public event EventHandler<GridLineEventArgs> GridLineEvent;

    /// <summary>
    /// EventHandler for <c>grid_scroll</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.GridScrollEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `GridScrollEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="GridScrollEventArgs"/>
    public event EventHandler<GridScrollEventArgs> GridScrollEvent;

    /// <summary>
    /// EventHandler for <c>grid_destroy</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.GridDestroyEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `GridDestroyEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="GridDestroyEventArgs"/>
    public event EventHandler<GridDestroyEventArgs> GridDestroyEvent;

    /// <summary>
    /// EventHandler for <c>win_pos</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.WinPosEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `WinPosEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="WinPosEventArgs"/>
    public event EventHandler<WinPosEventArgs> WinPosEvent;

    /// <summary>
    /// EventHandler for <c>win_float_pos</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.WinFloatPosEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `WinFloatPosEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="WinFloatPosEventArgs"/>
    public event EventHandler<WinFloatPosEventArgs> WinFloatPosEvent;

    /// <summary>
    /// EventHandler for <c>win_external_pos</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.WinExternalPosEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `WinExternalPosEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="WinExternalPosEventArgs"/>
    public event EventHandler<WinExternalPosEventArgs> WinExternalPosEvent;

    /// <summary>
    /// EventHandler for <c>win_hide</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.WinHideEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `WinHideEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="WinHideEventArgs"/>
    public event EventHandler<WinHideEventArgs> WinHideEvent;

    /// <summary>
    /// EventHandler for <c>win_close</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.WinCloseEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `WinCloseEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="WinCloseEventArgs"/>
    public event EventHandler<WinCloseEventArgs> WinCloseEvent;

    /// <summary>
    /// EventHandler for <c>msg_set_pos</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.MsgSetPosEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `MsgSetPosEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="MsgSetPosEventArgs"/>
    public event EventHandler<MsgSetPosEventArgs> MsgSetPosEvent;

    /// <summary>
    /// EventHandler for <c>win_viewport</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.WinViewportEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `WinViewportEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="WinViewportEventArgs"/>
    public event EventHandler<WinViewportEventArgs> WinViewportEvent;

    /// <summary>
    /// EventHandler for <c>popupmenu_show</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.PopupmenuShowEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `PopupmenuShowEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="PopupmenuShowEventArgs"/>
    public event EventHandler<PopupmenuShowEventArgs> PopupmenuShowEvent;

    /// <summary>
    /// EventHandler for <c>popupmenu_hide</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.PopupmenuHideEvent += (sender, args) =>
    /// {
    ///     // `args` contains no data for this event.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    public event EventHandler PopupmenuHideEvent;

    /// <summary>
    /// EventHandler for <c>popupmenu_select</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.PopupmenuSelectEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `PopupmenuSelectEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="PopupmenuSelectEventArgs"/>
    public event EventHandler<PopupmenuSelectEventArgs> PopupmenuSelectEvent;

    /// <summary>
    /// EventHandler for <c>tabline_update</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.TablineUpdateEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `TablineUpdateEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="TablineUpdateEventArgs"/>
    public event EventHandler<TablineUpdateEventArgs> TablineUpdateEvent;

    /// <summary>
    /// EventHandler for <c>cmdline_show</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.CmdlineShowEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `CmdlineShowEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="CmdlineShowEventArgs"/>
    public event EventHandler<CmdlineShowEventArgs> CmdlineShowEvent;

    /// <summary>
    /// EventHandler for <c>cmdline_pos</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.CmdlinePosEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `CmdlinePosEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="CmdlinePosEventArgs"/>
    public event EventHandler<CmdlinePosEventArgs> CmdlinePosEvent;

    /// <summary>
    /// EventHandler for <c>cmdline_special_char</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.CmdlineSpecialCharEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `CmdlineSpecialCharEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="CmdlineSpecialCharEventArgs"/>
    public event EventHandler<CmdlineSpecialCharEventArgs> CmdlineSpecialCharEvent;

    /// <summary>
    /// EventHandler for <c>cmdline_hide</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.CmdlineHideEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `CmdlineHideEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="CmdlineHideEventArgs"/>
    public event EventHandler<CmdlineHideEventArgs> CmdlineHideEvent;

    /// <summary>
    /// EventHandler for <c>cmdline_block_show</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.CmdlineBlockShowEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `CmdlineBlockShowEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="CmdlineBlockShowEventArgs"/>
    public event EventHandler<CmdlineBlockShowEventArgs> CmdlineBlockShowEvent;

    /// <summary>
    /// EventHandler for <c>cmdline_block_append</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.CmdlineBlockAppendEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `CmdlineBlockAppendEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="CmdlineBlockAppendEventArgs"/>
    public event EventHandler<CmdlineBlockAppendEventArgs> CmdlineBlockAppendEvent;

    /// <summary>
    /// EventHandler for <c>cmdline_block_hide</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.CmdlineBlockHideEvent += (sender, args) =>
    /// {
    ///     // `args` contains no data for this event.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    public event EventHandler CmdlineBlockHideEvent;

    /// <summary>
    /// EventHandler for <c>wildmenu_show</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.WildmenuShowEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `WildmenuShowEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="WildmenuShowEventArgs"/>
    public event EventHandler<WildmenuShowEventArgs> WildmenuShowEvent;

    /// <summary>
    /// EventHandler for <c>wildmenu_select</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.WildmenuSelectEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `WildmenuSelectEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="WildmenuSelectEventArgs"/>
    public event EventHandler<WildmenuSelectEventArgs> WildmenuSelectEvent;

    /// <summary>
    /// EventHandler for <c>wildmenu_hide</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.WildmenuHideEvent += (sender, args) =>
    /// {
    ///     // `args` contains no data for this event.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    public event EventHandler WildmenuHideEvent;

    /// <summary>
    /// EventHandler for <c>msg_show</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.MsgShowEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `MsgShowEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="MsgShowEventArgs"/>
    public event EventHandler<MsgShowEventArgs> MsgShowEvent;

    /// <summary>
    /// EventHandler for <c>msg_clear</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.MsgClearEvent += (sender, args) =>
    /// {
    ///     // `args` contains no data for this event.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    public event EventHandler MsgClearEvent;

    /// <summary>
    /// EventHandler for <c>msg_showcmd</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.MsgShowcmdEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `MsgShowcmdEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="MsgShowcmdEventArgs"/>
    public event EventHandler<MsgShowcmdEventArgs> MsgShowcmdEvent;

    /// <summary>
    /// EventHandler for <c>msg_showmode</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.MsgShowmodeEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `MsgShowmodeEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="MsgShowmodeEventArgs"/>
    public event EventHandler<MsgShowmodeEventArgs> MsgShowmodeEvent;

    /// <summary>
    /// EventHandler for <c>msg_ruler</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.MsgRulerEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `MsgRulerEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="MsgRulerEventArgs"/>
    public event EventHandler<MsgRulerEventArgs> MsgRulerEvent;

    /// <summary>
    /// EventHandler for <c>msg_history_show</c> UI event (see corresponding
    /// docs in `:help ui-events` in nvim).
    /// </summary>
    /// <example>
    /// <code>
    /// var api = new NvimAPI();
    /// api.MsgHistoryShowEvent += (sender, args) =>
    /// {
    ///     // `args` is of type `MsgHistoryShowEventArgs`.
    ///     // Handler code goes here.
    /// }
    /// // This handler will be executed whenever the event is emitted after attaching the UI.
    /// </code>
    /// </example>
    /// <seealso cref="MsgHistoryShowEventArgs"/>
    public event EventHandler<MsgHistoryShowEventArgs> MsgHistoryShowEvent;

    /// <summary>
    /// <para>
    /// Deprecated
    /// </para>
    /// </summary>
    public Task<string> CommandOutput(string @command) =>
      SendAndReceive<string>(new NvimRequest
      {
        Method = "nvim_command_output",
        Arguments = GetRequestArguments(
          @command)
      });

    /// <summary>
    /// <para>
    /// DeprecatedUse nvim_exec_lua() instead. 
    /// </para>
    /// </summary>
    public Task<object> ExecuteLua(string @code, object[] @args) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_execute_lua",
        Arguments = GetRequestArguments(
          @code, @args)
      });

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
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_ui_attach",
        Arguments = GetRequestArguments(
          @width, @height, @options)
      });

    /// <summary>
    /// <para>
    /// Deactivates UI events on the channel.
    /// </para>
    /// </summary>
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
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_ui_try_resize_grid",
        Arguments = GetRequestArguments(
          @grid, @width, @height)
      });

    /// <summary>
    /// <para>
    /// Tells Nvim the number of elements displaying in the popumenu, to decide &lt;PageUp&gt; and &lt;PageDown&gt; movement.
    /// </para>
    /// </summary>
    /// <param name="height">
    /// <para>
    /// Popupmenu height, must be greater than zero. 
    /// </para>
    /// </param>
    public Task UiPumSetHeight(long @height) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_ui_pum_set_height",
        Arguments = GetRequestArguments(
          @height)
      });

    /// <summary>
    /// <para>
    /// Tells Nvim the geometry of the popumenu, to align floating windows with an external popup menu.
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
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_ui_pum_set_bounds",
        Arguments = GetRequestArguments(
          @width, @height, @row, @col)
      });

    /// <summary>
    /// <para>
    /// Executes Vimscript (multiline block of Ex-commands), like anonymous |:source|.
    /// </para>
    /// </summary>
    /// <param name="src">
    /// <para>
    /// Vimscript code 
    /// </para>
    /// </param>
    /// <param name="output">
    /// <para>
    /// Capture and return all (non-error, non-shell |:!|) output 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Output (non-error, non-shell |:!|) if 
    /// </para>
    /// </returns>
    public Task<string> Exec(string @src, bool @output) =>
      SendAndReceive<string>(new NvimRequest
      {
        Method = "nvim_exec",
        Arguments = GetRequestArguments(
          @src, @output)
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
    /// Gets a highlight group by name
    /// </para>
    /// </summary>
    public Task<long> GetHlIdByName(string @name) =>
      SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_get_hl_id_by_name",
        Arguments = GetRequestArguments(
          @name)
      });

    /// <summary>
    /// <para>
    /// Set a highlight group.
    /// </para>
    /// </summary>
    /// <param name="nsId">
    /// <para>
    /// number of namespace for this highlight 
    /// </para>
    /// </param>
    /// <param name="name">
    /// <para>
    /// highlight group name, like ErrorMsg 
    /// </para>
    /// </param>
    /// <param name="val">
    /// <para>
    /// highlight definition map, like |nvim_get_hl_by_name|. in addition the following keys are also recognized: 
    /// </para>
    /// </param>
    public Task SetHl(long @nsId, string @name, IDictionary @val) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_hl",
        Arguments = GetRequestArguments(
          @nsId, @name, @val)
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
    /// Send mouse event from GUI.
    /// </para>
    /// </summary>
    /// <param name="button">
    /// <para>
    /// Mouse button: one of &quot;left&quot;, &quot;right&quot;, &quot;middle&quot;, &quot;wheel&quot;. 
    /// </para>
    /// </param>
    /// <param name="action">
    /// <para>
    /// For ordinary buttons, one of &quot;press&quot;, &quot;drag&quot;, &quot;release&quot;. For the wheel, one of &quot;up&quot;, &quot;down&quot;, &quot;left&quot;, &quot;right&quot;. 
    /// </para>
    /// </param>
    /// <param name="modifier">
    /// <para>
    /// String of modifiers each represented by a single char. The same specifiers are used as for a key press, except that the &quot;-&quot; separator is optional, so &quot;C-A-&quot;, &quot;c-a&quot; and &quot;CA&quot; can all be used to specify Ctrl+Alt+click. 
    /// </para>
    /// </param>
    /// <param name="grid">
    /// <para>
    /// Grid number if the client uses |ui-multigrid|, else 0. 
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
    /// Currently this doesn&apos;t support &quot;scripting&quot; multiple mouse events by calling it multiple times in a loop: the intermediate mouse positions will be ignored. It should be used to implement real-time mouse input in a GUI. The deprecated pseudokey form (&quot;&lt;LeftMouse&gt;&lt;col,row&gt;&quot;) of |nvim_input()| has the same limitation.
    /// </remarks>
    public Task InputMouse(string @button, string @action, string @modifier, long @grid, long @row, long @col) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_input_mouse",
        Arguments = GetRequestArguments(
          @button, @action, @modifier, @grid, @row, @col)
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
    /// Evaluates a VimL |expression|. Dictionaries and Lists are recursively expanded.
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
    /// Execute Lua code. Parameters (if any) are available as 
    /// </para>
    /// </summary>
    /// <param name="code">
    /// <para>
    /// Lua code to execute 
    /// </para>
    /// </param>
    /// <param name="args">
    /// <para>
    /// Arguments to the code 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Return value of Lua code if present or NIL. 
    /// </para>
    /// </returns>
    public Task<object> ExecLua(string @code, object[] @args) =>
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_exec_lua",
        Arguments = GetRequestArguments(
          @code, @args)
      });

    /// <summary>
    /// <para>
    /// Notify the user with a message
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
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_notify",
        Arguments = GetRequestArguments(
          @msg, @logLevel, @opts)
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
    /// Find files in runtime directories
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
    public Task<string[]> GetRuntimeFile(string @name, bool @all) =>
      SendAndReceive<string[]>(new NvimRequest
      {
        Method = "nvim_get_runtime_file",
        Arguments = GetRequestArguments(
          @name, @all)
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
    /// Gets the current line.
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
    /// Sets the current line.
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
    /// Deletes the current line.
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
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_get_var",
        Arguments = GetRequestArguments(
          @name)
      });

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
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_var",
        Arguments = GetRequestArguments(
          @name, @value)
      });

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
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_del_var",
        Arguments = GetRequestArguments(
          @name)
      });

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
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_get_vvar",
        Arguments = GetRequestArguments(
          @name)
      });

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
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_vvar",
        Arguments = GetRequestArguments(
          @name, @value)
      });

    /// <summary>
    /// <para>
    /// Gets an option value string.
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
    /// Gets the option information for all options.
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// dictionary of all options 
    /// </para>
    /// </returns>
    public Task<IDictionary> GetAllOptionsInfo() =>
      SendAndReceive<IDictionary>(new NvimRequest
      {
        Method = "nvim_get_all_options_info",
        Arguments = GetRequestArguments(
          )
      });

    /// <summary>
    /// <para>
    /// Gets the option information for one option
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Option name 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Option Information 
    /// </para>
    /// </returns>
    public Task<IDictionary> GetOptionInfo(string @name) =>
      SendAndReceive<IDictionary>(new NvimRequest
      {
        Method = "nvim_get_option_info",
        Arguments = GetRequestArguments(
          @name)
      });

    /// <summary>
    /// <para>
    /// Sets an option value.
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
    /// Echo a message.
    /// </para>
    /// </summary>
    /// <param name="chunks">
    /// <para>
    /// A list of [text, hl_group] arrays, each representing a text chunk with specified highlight. 
    /// </para>
    /// </param>
    /// <param name="history">
    /// <para>
    /// if true, add to |message-history|. 
    /// </para>
    /// </param>
    /// <param name="opts">
    /// <para>
    /// Optional parameters. Reserved for future use. 
    /// </para>
    /// </param>
    public Task Echo(object[] @chunks, bool @history, IDictionary @opts) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_echo",
        Arguments = GetRequestArguments(
          @chunks, @history, @opts)
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
    /// Gets the current buffer.
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
    /// Sets the current buffer.
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
    /// Gets the current list of window handles.
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
    /// Gets the current window.
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
    /// Sets the current window.
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
    /// Buffer handle, or 0 on error
    /// </para>
    /// </returns>
    public Task<NvimBuffer> CreateBuf(bool @listed, bool @scratch) =>
      SendAndReceive<NvimBuffer>(new NvimRequest
      {
        Method = "nvim_create_buf",
        Arguments = GetRequestArguments(
          @listed, @scratch)
      });

    /// <summary>
    /// <para>
    /// Open a terminal instance in a buffer
    /// </para>
    /// </summary>
    /// <param name="buffer">
    /// <para>
    /// the buffer to use (expected to be empty) 
    /// </para>
    /// </param>
    /// <param name="opts">
    /// <para>
    /// Optional parameters. Reserved for future use. 
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// Channel id, or 0 on error 
    /// </para>
    /// </returns>
    public Task<long> OpenTerm(NvimBuffer @buffer, IDictionary @opts) =>
      SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_open_term",
        Arguments = GetRequestArguments(
          @buffer, @opts)
      });

    /// <summary>
    /// <para>
    /// Send data to channel 
    /// </para>
    /// </summary>
    /// <param name="chan">
    /// <para>
    /// id of the channel 
    /// </para>
    /// </param>
    /// <param name="data">
    /// <para>
    /// data to write. 8-bit clean: can contain NUL bytes. 
    /// </para>
    /// </param>
    public Task ChanSend(long @chan, string @data) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_chan_send",
        Arguments = GetRequestArguments(
          @chan, @data)
      });

    /// <summary>
    /// <para>
    /// Open a new window.
    /// </para>
    /// </summary>
    /// <param name="buffer">
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
    /// Window handle, or 0 on error 
    /// </para>
    /// </returns>
    public Task<NvimWindow> OpenWin(NvimBuffer @buffer, bool @enter, IDictionary @config) =>
      SendAndReceive<NvimWindow>(new NvimRequest
      {
        Method = "nvim_open_win",
        Arguments = GetRequestArguments(
          @buffer, @enter, @config)
      });

    /// <summary>
    /// <para>
    /// Gets the current list of tabpage handles.
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
    /// Gets the current tabpage.
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
    /// Sets the current tabpage.
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
    /// Creates a new namespace, or gets an existing one.
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
      SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_create_namespace",
        Arguments = GetRequestArguments(
          @name)
      });

    /// <summary>
    /// <para>
    /// Gets existing, non-anonymous namespaces.
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// dict that maps from names to namespace ids. 
    /// </para>
    /// </returns>
    public Task<IDictionary> GetNamespaces() =>
      SendAndReceive<IDictionary>(new NvimRequest
      {
        Method = "nvim_get_namespaces",
        Arguments = GetRequestArguments(
          )
      });

    /// <summary>
    /// <para>
    /// Pastes at cursor, in any mode.
    /// </para>
    /// </summary>
    /// <param name="data">
    /// <para>
    /// Multiline input. May be binary (containing NUL bytes). 
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
      SendAndReceive<bool>(new NvimRequest
      {
        Method = "nvim_paste",
        Arguments = GetRequestArguments(
          @data, @crlf, @phase)
      });

    /// <summary>
    /// <para>
    /// Puts text at cursor, in any mode.
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
    public Task Put(string[] @lines, string @type, bool @after, bool @follow) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_put",
        Arguments = GetRequestArguments(
          @lines, @type, @after, @follow)
      });

    /// <summary>
    /// <para>
    /// Subscribes to event broadcasts.
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
    /// Unsubscribes to event broadcasts.
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
      SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_get_color_by_name",
        Arguments = GetRequestArguments(
          @name)
      });

    /// <summary>
    /// <para>
    /// Returns a map of color names and RGB values.
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// Map of color names and RGB values. 
    /// </para>
    /// </returns>
    public Task<IDictionary> GetColorMap() =>
      SendAndReceive<IDictionary>(new NvimRequest
      {
        Method = "nvim_get_color_map",
        Arguments = GetRequestArguments(
          )
      });

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
      SendAndReceive<IDictionary>(new NvimRequest
      {
        Method = "nvim_get_context",
        Arguments = GetRequestArguments(
          @opts)
      });

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
      SendAndReceive<object>(new NvimRequest
      {
        Method = "nvim_load_context",
        Arguments = GetRequestArguments(
          @dict)
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
    /// Sets a global |mapping| for the given mode.
    /// </para>
    /// </summary>
    /// <param name="mode">
    /// <para>
    /// Mode short-name (map command prefix: &quot;n&quot;, &quot;i&quot;, &quot;v&quot;, &quot;x&quot;, …) or &quot;!&quot; for |:map!|, or empty string for |:map|. 
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
    /// Optional parameters map. Accepts all |:map-arguments| as keys excluding |&lt;buffer&gt;| but including |noremap|. Values are Booleans. Unknown key is an error. 
    /// </para>
    /// </param>
    public Task SetKeymap(string @mode, string @lhs, string @rhs, IDictionary @opts) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_keymap",
        Arguments = GetRequestArguments(
          @mode, @lhs, @rhs, @opts)
      });

    /// <summary>
    /// <para>
    /// Unmaps a global |mapping| for the given mode.
    /// </para>
    /// </summary>
    public Task DelKeymap(string @mode, string @lhs) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_del_keymap",
        Arguments = GetRequestArguments(
          @mode, @lhs)
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
    /// Self-identifies the client.
    /// </para>
    /// </summary>
    /// <param name="name">
    /// <para>
    /// Short name for the connected client 
    /// </para>
    /// </param>
    /// <param name="version">
    /// <para>
    /// Dictionary describing the version, with these (optional) keys:
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
    /// <remarks>
    /// &quot;Something is better than nothing&quot;. You don&apos;t need to include all the fields.
    /// </remarks>
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
    /// Dictionary describing a channel, with these keys:
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
    /// Array of Dictionaries, each describing a channel with the format specified at |nvim_get_chan_info()|. 
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
    /// Array of two elements. The first is an array of return values. The second is NIL if all calls succeeded. If a call resulted in an error, it is a three-element array with the zero-based index of the call which resulted in an error, the error type and the error message. If an error occurred, the values from all preceding calls will still be returned. 
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
    /// Parse a VimL expression.
    /// </para>
    /// </summary>
    /// <param name="expr">
    /// <para>
    /// Expression to parse. Always treated as a single line. 
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
    /// <list type="bullet">
    /// <item><description>
    /// AST: top-level dictionary with these keys:&quot;error&quot;: Dictionary with error, present only if parser saw some error. Contains the following keys:&quot;message&quot;: String, error message in printf format, translated. Must contain exactly one &quot;%.*s&quot;.&quot;arg&quot;: String, error message argument.&quot;len&quot;: Amount of bytes successfully parsed. With flags equal to &quot;&quot; that should be equal to the length of expr string. (“Successfully parsed” here means “participated in AST creation”, not “till the first error”.)&quot;ast&quot;: AST, either nil or a dictionary with these keys:&quot;type&quot;: node type, one of the value names from ExprASTNodeType stringified without &quot;kExprNode&quot; prefix.&quot;start&quot;: a pair [line, column] describing where node is &quot;started&quot; where &quot;line&quot; is always 0 (will not be 0 if you will be using nvim_parse_viml() on e.g. &quot;:let&quot;, but that is not present yet). Both elements are Integers.&quot;len&quot;: “length” of the node. This and &quot;start&quot; are there for debugging purposes primary (debugging parser and providing debug information).&quot;children&quot;: a list of nodes described in top/&quot;ast&quot;. There always is zero, one or two children, key will not be present if node has no children. Maximum number of children may be found in node_maxchildren array.
    /// </description></item>
    /// </list>
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
    /// Array of UI dictionaries, each with these keys:
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

    /// <summary>
    /// <para>
    /// Selects an item in the completion popupmenu.
    /// </para>
    /// </summary>
    /// <param name="item">
    /// <para>
    /// Index (zero-based) of the item to select. Value of -1 selects nothing and restores the original text. 
    /// </para>
    /// </param>
    /// <param name="insert">
    /// <para>
    /// Whether the selection should be inserted in the buffer. 
    /// </para>
    /// </param>
    /// <param name="finish">
    /// <para>
    /// Finish the completion and dismiss the popupmenu. Implies 
    /// </para>
    /// </param>
    /// <param name="opts">
    /// <para>
    /// Optional parameters. Reserved for future use. 
    /// </para>
    /// </param>
    public Task SelectPopupmenuItem(long @item, bool @insert, bool @finish, IDictionary @opts) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_select_popupmenu_item",
        Arguments = GetRequestArguments(
          @item, @insert, @finish, @opts)
      });

    /// <summary>
    /// <para>
    /// Set or change decoration provider for a namespace
    /// </para>
    /// </summary>
    /// <param name="nsId">
    /// <para>
    /// Namespace id from |nvim_create_namespace()| 
    /// </para>
    /// </param>
    /// <param name="opts">
    /// <para>
    /// Callbacks invoked during redraw:
    /// </para>
    /// </param>
    public Task SetDecorationProvider(long @nsId, IDictionary @opts) =>
      SendAndReceive(new NvimRequest
      {
        Method = "nvim_set_decoration_provider",
        Arguments = GetRequestArguments(
          @nsId, @opts)
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
    /// Line count, or 0 for unloaded buffer. |api-buffer| 
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
    /// Activates buffer-update events on a channel, or as Lua callbacks.
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
    /// False if attach failed (invalid parameter, or buffer isn&apos;t loaded); otherwise True. TODO: LUA_API_NO_EVAL 
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
    /// Deactivates buffer-update events on the channel.
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// False if detach failed (because the buffer isn&apos;t loaded); otherwise True. 
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
    /// Array of lines, or empty array for unloaded buffer. 
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
    /// Sets (replaces) a range in the buffer
    /// </para>
    /// </summary>
    /// <param name="startRow">
    /// <para>
    /// First line index 
    /// </para>
    /// </param>
    /// <param name="endRow">
    /// <para>
    /// Last line index 
    /// </para>
    /// </param>
    /// <param name="replacement">
    /// <para>
    /// Array of lines to use as replacement 
    /// </para>
    /// </param>
    public Task SetText(long @startRow, long @startCol, long @endRow, long @endCol, string[] @replacement) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_set_text",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @startRow, @startCol, @endRow, @endCol, @replacement)
      });

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
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_buf_get_offset",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @index)
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
    /// Sets a buffer-local |mapping| for the given mode.
    /// </para>
    /// </summary>
    public Task SetKeymap(string @mode, string @lhs, string @rhs, IDictionary @opts) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_set_keymap",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @mode, @lhs, @rhs, @opts)
      });

    /// <summary>
    /// <para>
    /// Unmaps a buffer-local |mapping| for the given mode.
    /// </para>
    /// </summary>
    public Task DelKeymap(string @mode, string @lhs) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_del_keymap",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @mode, @lhs)
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
    /// Checks if a buffer is valid and loaded. See |api-buffer| for more info about unloaded buffers.
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// true if the buffer is valid and loaded, false otherwise. 
    /// </para>
    /// </returns>
    public Task<bool> IsLoaded() =>
      _api.SendAndReceive<bool>(new NvimRequest
      {
        Method = "nvim_buf_is_loaded",
        Arguments = GetRequestArguments(
          _msgPackExtObj)
      });

    /// <summary>
    /// <para>
    /// Deletes the buffer. See |:bwipeout|
    /// </para>
    /// </summary>
    /// <param name="opts">
    /// <para>
    /// Optional parameters. Keys:
    /// </para>
    /// </param>
    public Task Delete(IDictionary @opts) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_delete",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @opts)
      });

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
      _api.SendAndReceive<bool>(new NvimRequest
      {
        Method = "nvim_buf_is_valid",
        Arguments = GetRequestArguments(
          _msgPackExtObj)
      });

    /// <summary>
    /// <para>
    /// Return a tuple (row,col) representing the position of the named mark.
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
    /// Returns position for a given extmark id
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
    /// <param name="opts">
    /// <para>
    /// Optional parameters. Keys:
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// (row, col) tuple or empty list () if extmark id was absent 
    /// </para>
    /// </returns>
    public Task<long[]> GetExtmarkById(long @nsId, long @id, IDictionary @opts) =>
      _api.SendAndReceive<long[]>(new NvimRequest
      {
        Method = "nvim_buf_get_extmark_by_id",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @nsId, @id, @opts)
      });

    /// <summary>
    /// <para>
    /// Gets extmarks in &quot;traversal order&quot; from a |charwise| region defined by buffer positions (inclusive, 0-indexed |api-indexing|).
    /// </para>
    /// </summary>
    /// <param name="nsId">
    /// <para>
    /// Namespace id from |nvim_create_namespace()| 
    /// </para>
    /// </param>
    /// <param name="start">
    /// <para>
    /// Start of range, given as (row, col) or valid extmark id (whose position defines the bound) 
    /// </para>
    /// </param>
    /// <param name="end">
    /// <para>
    /// End of range, given as (row, col) or valid extmark id (whose position defines the bound) 
    /// </para>
    /// </param>
    /// <param name="opts">
    /// <para>
    /// Optional parameters. Keys:
    /// </para>
    /// </param>
    /// <returns>
    /// <para>
    /// List of [extmark_id, row, col] tuples in &quot;traversal order&quot;. 
    /// </para>
    /// </returns>
    public Task<object[]> GetExtmarks(long @nsId, object @start, object @end, IDictionary @opts) =>
      _api.SendAndReceive<object[]>(new NvimRequest
      {
        Method = "nvim_buf_get_extmarks",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @nsId, @start, @end, @opts)
      });

    /// <summary>
    /// <para>
    /// Creates or updates an extmark.
    /// </para>
    /// </summary>
    /// <param name="nsId">
    /// <para>
    /// Namespace id from |nvim_create_namespace()| 
    /// </para>
    /// </param>
    /// <param name="line">
    /// <para>
    /// Line where to place the mark, 0-based 
    /// </para>
    /// </param>
    /// <param name="col">
    /// <para>
    /// Column where to place the mark, 0-based 
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
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_buf_set_extmark",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @nsId, @line, @col, @opts)
      });

    /// <summary>
    /// <para>
    /// Removes an extmark.
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
      _api.SendAndReceive<bool>(new NvimRequest
      {
        Method = "nvim_buf_del_extmark",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @nsId, @id)
      });

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
    public Task<long> AddHighlight(long @nsId, string @hlGroup, long @line, long @colStart, long @colEnd) =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_buf_add_highlight",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @nsId, @hlGroup, @line, @colStart, @colEnd)
      });

    /// <summary>
    /// <para>
    /// Clears namespaced objects (highlights, extmarks, virtual text) from a region.
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
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_clear_namespace",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @nsId, @lineStart, @lineEnd)
      });

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
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_buf_clear_highlight",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @nsId, @lineStart, @lineEnd)
      });

    /// <summary>
    /// <para>
    /// Set the virtual text (annotation) for a buffer line.
    /// </para>
    /// </summary>
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
    public Task<long> SetVirtualText(long @srcId, long @line, object[] @chunks, IDictionary @opts) =>
      _api.SendAndReceive<long>(new NvimRequest
      {
        Method = "nvim_buf_set_virtual_text",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @srcId, @line, @chunks, @opts)
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
    /// Sets the current buffer in a window, without side-effects
    /// </para>
    /// </summary>
    /// <param name="buffer">
    /// <para>
    /// Buffer handle 
    /// </para>
    /// </param>
    public Task SetBuf(NvimBuffer @buffer) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_set_buf",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @buffer)
      });

    /// <summary>
    /// <para>
    /// Gets the (1,0)-indexed cursor position in the window. |api-indexing|
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
    /// Sets the (1,0)-indexed cursor position in the window. |api-indexing|
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

    /// <summary>
    /// <para>
    /// Configures window layout. Currently only for floating and external windows (including changing a split window to those layouts).
    /// </para>
    /// </summary>
    /// <param name="config">
    /// <para>
    /// Map defining the window configuration, see |nvim_open_win()| 
    /// </para>
    /// </param>
    public Task SetConfig(IDictionary @config) =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_set_config",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @config)
      });

    /// <summary>
    /// <para>
    /// Gets window configuration.
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// Map defining the window configuration, see |nvim_open_win()| 
    /// </para>
    /// </returns>
    public Task<IDictionary> GetConfig() =>
      _api.SendAndReceive<IDictionary>(new NvimRequest
      {
        Method = "nvim_win_get_config",
        Arguments = GetRequestArguments(
          _msgPackExtObj)
      });

    /// <summary>
    /// <para>
    /// Closes the window and hide the buffer it contains (like |:hide| with a |window-ID|).
    /// </para>
    /// </summary>
    public Task Hide() =>
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_hide",
        Arguments = GetRequestArguments(
          _msgPackExtObj)
      });

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
      _api.SendAndReceive(new NvimRequest
      {
        Method = "nvim_win_close",
        Arguments = GetRequestArguments(
          _msgPackExtObj, @force)
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
    public bool Focusable { get; set; }
    public long Zindex { get; set; }

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

  }
  public class WinViewportEventArgs : EventArgs
  {
    public long Grid { get; set; }
    public NvimWindow Win { get; set; }
    public long Topline { get; set; }
    public long Botline { get; set; }
    public long Curline { get; set; }
    public long Curcol { get; set; }

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
  public class MsgShowEventArgs : EventArgs
  {
    public string Kind { get; set; }
    public object[] Content { get; set; }
    public bool ReplaceLast { get; set; }

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

  }
    private void CallUIEventHandler(string eventName, object[] args)
    {
      switch (eventName)
      {
  
      case "mode_info_set":
          ModeInfoSetEvent?.Invoke(this, new ModeInfoSetEventArgs
          {
            Enabled = (bool) args[0],
            CursorStyles = (object[]) args[1]
          });
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
          ModeChangeEvent?.Invoke(this, new ModeChangeEventArgs
          {
            Mode = (string) args[0],
            ModeIdx = (long) args[1]
          });
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

      case "suspend":
          SuspendEvent?.Invoke(this, EventArgs.Empty);
          break;

      case "set_title":
          SetTitleEvent?.Invoke(this, new SetTitleEventArgs
          {
            Title = (string) args[0]
          });
          break;

      case "set_icon":
          SetIconEvent?.Invoke(this, new SetIconEventArgs
          {
            Icon = (string) args[0]
          });
          break;

      case "screenshot":
          ScreenshotEvent?.Invoke(this, new ScreenshotEventArgs
          {
            Path = (string) args[0]
          });
          break;

      case "option_set":
          OptionSetEvent?.Invoke(this, new OptionSetEventArgs
          {
            Name = (string) args[0],
            Value = (object) args[1]
          });
          break;

      case "update_fg":
          UpdateFgEvent?.Invoke(this, new UpdateFgEventArgs
          {
            Fg = (long) args[0]
          });
          break;

      case "update_bg":
          UpdateBgEvent?.Invoke(this, new UpdateBgEventArgs
          {
            Bg = (long) args[0]
          });
          break;

      case "update_sp":
          UpdateSpEvent?.Invoke(this, new UpdateSpEventArgs
          {
            Sp = (long) args[0]
          });
          break;

      case "resize":
          ResizeEvent?.Invoke(this, new ResizeEventArgs
          {
            Width = (long) args[0],
            Height = (long) args[1]
          });
          break;

      case "clear":
          ClearEvent?.Invoke(this, EventArgs.Empty);
          break;

      case "eol_clear":
          EolClearEvent?.Invoke(this, EventArgs.Empty);
          break;

      case "cursor_goto":
          CursorGotoEvent?.Invoke(this, new CursorGotoEventArgs
          {
            Row = (long) args[0],
            Col = (long) args[1]
          });
          break;

      case "highlight_set":
          HighlightSetEvent?.Invoke(this, new HighlightSetEventArgs
          {
            Attrs = (IDictionary) args[0]
          });
          break;

      case "put":
          PutEvent?.Invoke(this, new PutEventArgs
          {
            Str = (string) args[0]
          });
          break;

      case "set_scroll_region":
          SetScrollRegionEvent?.Invoke(this, new SetScrollRegionEventArgs
          {
            Top = (long) args[0],
            Bot = (long) args[1],
            Left = (long) args[2],
            Right = (long) args[3]
          });
          break;

      case "scroll":
          ScrollEvent?.Invoke(this, new ScrollEventArgs
          {
            Count = (long) args[0]
          });
          break;

      case "default_colors_set":
          DefaultColorsSetEvent?.Invoke(this, new DefaultColorsSetEventArgs
          {
            RgbFg = (long) args[0],
            RgbBg = (long) args[1],
            RgbSp = (long) args[2],
            CtermFg = (long) args[3],
            CtermBg = (long) args[4]
          });
          break;

      case "hl_attr_define":
          HlAttrDefineEvent?.Invoke(this, new HlAttrDefineEventArgs
          {
            Id = (long) args[0],
            RgbAttrs = (IDictionary) args[1],
            CtermAttrs = (IDictionary) args[2],
            Info = (object[]) args[3]
          });
          break;

      case "hl_group_set":
          HlGroupSetEvent?.Invoke(this, new HlGroupSetEventArgs
          {
            Name = (string) args[0],
            Id = (long) args[1]
          });
          break;

      case "grid_resize":
          GridResizeEvent?.Invoke(this, new GridResizeEventArgs
          {
            Grid = (long) args[0],
            Width = (long) args[1],
            Height = (long) args[2]
          });
          break;

      case "grid_clear":
          GridClearEvent?.Invoke(this, new GridClearEventArgs
          {
            Grid = (long) args[0]
          });
          break;

      case "grid_cursor_goto":
          GridCursorGotoEvent?.Invoke(this, new GridCursorGotoEventArgs
          {
            Grid = (long) args[0],
            Row = (long) args[1],
            Col = (long) args[2]
          });
          break;

      case "grid_line":
          GridLineEvent?.Invoke(this, new GridLineEventArgs
          {
            Grid = (long) args[0],
            Row = (long) args[1],
            ColStart = (long) args[2],
            Data = (object[]) args[3]
          });
          break;

      case "grid_scroll":
          GridScrollEvent?.Invoke(this, new GridScrollEventArgs
          {
            Grid = (long) args[0],
            Top = (long) args[1],
            Bot = (long) args[2],
            Left = (long) args[3],
            Right = (long) args[4],
            Rows = (long) args[5],
            Cols = (long) args[6]
          });
          break;

      case "grid_destroy":
          GridDestroyEvent?.Invoke(this, new GridDestroyEventArgs
          {
            Grid = (long) args[0]
          });
          break;

      case "win_pos":
          WinPosEvent?.Invoke(this, new WinPosEventArgs
          {
            Grid = (long) args[0],
            Win = (NvimWindow) args[1],
            Startrow = (long) args[2],
            Startcol = (long) args[3],
            Width = (long) args[4],
            Height = (long) args[5]
          });
          break;

      case "win_float_pos":
          WinFloatPosEvent?.Invoke(this, new WinFloatPosEventArgs
          {
            Grid = (long) args[0],
            Win = (NvimWindow) args[1],
            Anchor = (string) args[2],
            AnchorGrid = (long) args[3],
            AnchorRow = (double) args[4],
            AnchorCol = (double) args[5],
            Focusable = (bool) args[6],
            Zindex = (long) args[7]
          });
          break;

      case "win_external_pos":
          WinExternalPosEvent?.Invoke(this, new WinExternalPosEventArgs
          {
            Grid = (long) args[0],
            Win = (NvimWindow) args[1]
          });
          break;

      case "win_hide":
          WinHideEvent?.Invoke(this, new WinHideEventArgs
          {
            Grid = (long) args[0]
          });
          break;

      case "win_close":
          WinCloseEvent?.Invoke(this, new WinCloseEventArgs
          {
            Grid = (long) args[0]
          });
          break;

      case "msg_set_pos":
          MsgSetPosEvent?.Invoke(this, new MsgSetPosEventArgs
          {
            Grid = (long) args[0],
            Row = (long) args[1],
            Scrolled = (bool) args[2],
            SepChar = (string) args[3]
          });
          break;

      case "win_viewport":
          WinViewportEvent?.Invoke(this, new WinViewportEventArgs
          {
            Grid = (long) args[0],
            Win = (NvimWindow) args[1],
            Topline = (long) args[2],
            Botline = (long) args[3],
            Curline = (long) args[4],
            Curcol = (long) args[5]
          });
          break;

      case "popupmenu_show":
          PopupmenuShowEvent?.Invoke(this, new PopupmenuShowEventArgs
          {
            Items = (object[]) args[0],
            Selected = (long) args[1],
            Row = (long) args[2],
            Col = (long) args[3],
            Grid = (long) args[4]
          });
          break;

      case "popupmenu_hide":
          PopupmenuHideEvent?.Invoke(this, EventArgs.Empty);
          break;

      case "popupmenu_select":
          PopupmenuSelectEvent?.Invoke(this, new PopupmenuSelectEventArgs
          {
            Selected = (long) args[0]
          });
          break;

      case "tabline_update":
          TablineUpdateEvent?.Invoke(this, new TablineUpdateEventArgs
          {
            Current = (NvimTabpage) args[0],
            Tabs = (object[]) args[1],
            CurrentBuffer = (NvimBuffer) args[2],
            Buffers = (object[]) args[3]
          });
          break;

      case "cmdline_show":
          CmdlineShowEvent?.Invoke(this, new CmdlineShowEventArgs
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
          CmdlinePosEvent?.Invoke(this, new CmdlinePosEventArgs
          {
            Pos = (long) args[0],
            Level = (long) args[1]
          });
          break;

      case "cmdline_special_char":
          CmdlineSpecialCharEvent?.Invoke(this, new CmdlineSpecialCharEventArgs
          {
            C = (string) args[0],
            Shift = (bool) args[1],
            Level = (long) args[2]
          });
          break;

      case "cmdline_hide":
          CmdlineHideEvent?.Invoke(this, new CmdlineHideEventArgs
          {
            Level = (long) args[0]
          });
          break;

      case "cmdline_block_show":
          CmdlineBlockShowEvent?.Invoke(this, new CmdlineBlockShowEventArgs
          {
            Lines = (object[]) args[0]
          });
          break;

      case "cmdline_block_append":
          CmdlineBlockAppendEvent?.Invoke(this, new CmdlineBlockAppendEventArgs
          {
            Lines = (object[]) args[0]
          });
          break;

      case "cmdline_block_hide":
          CmdlineBlockHideEvent?.Invoke(this, EventArgs.Empty);
          break;

      case "wildmenu_show":
          WildmenuShowEvent?.Invoke(this, new WildmenuShowEventArgs
          {
            Items = (object[]) args[0]
          });
          break;

      case "wildmenu_select":
          WildmenuSelectEvent?.Invoke(this, new WildmenuSelectEventArgs
          {
            Selected = (long) args[0]
          });
          break;

      case "wildmenu_hide":
          WildmenuHideEvent?.Invoke(this, EventArgs.Empty);
          break;

      case "msg_show":
          MsgShowEvent?.Invoke(this, new MsgShowEventArgs
          {
            Kind = (string) args[0],
            Content = (object[]) args[1],
            ReplaceLast = (bool) args[2]
          });
          break;

      case "msg_clear":
          MsgClearEvent?.Invoke(this, EventArgs.Empty);
          break;

      case "msg_showcmd":
          MsgShowcmdEvent?.Invoke(this, new MsgShowcmdEventArgs
          {
            Content = (object[]) args[0]
          });
          break;

      case "msg_showmode":
          MsgShowmodeEvent?.Invoke(this, new MsgShowmodeEventArgs
          {
            Content = (object[]) args[0]
          });
          break;

      case "msg_ruler":
          MsgRulerEvent?.Invoke(this, new MsgRulerEventArgs
          {
            Content = (object[]) args[0]
          });
          break;

      case "msg_history_show":
          MsgHistoryShowEvent?.Invoke(this, new MsgHistoryShowEventArgs
          {
            Entries = (object[]) args[0]
          });
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