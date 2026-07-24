using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nvim.Client
{
  public partial class NvimClient
  {
    /// <inheritdoc/>
    public async Task<
      IReadOnlyList<IReadOnlyList<NvimMapEntry>>
    > GetAutocmdsAsync(
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimArray)
          await RequestAsync(
            "nvim_get_autocmds",
            [new NvimMap(Opts)],
            cancellationToken
          )
      )
        .Items.Select(item => ((NvimMap)item).Entries)
        .ToArray();

    /// <inheritdoc/>
    public async Task<NvimInteger> CreateAutocmdAsync(
      NvimValue Event,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimInteger)
        await RequestAsync(
          "nvim_create_autocmd",
          [Event, new NvimMap(Opts)],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task DelAutocmdAsync(
      NvimInteger Id,
      CancellationToken cancellationToken = default(CancellationToken)
    ) => await RequestAsync("nvim_del_autocmd", [Id], cancellationToken);

    /// <inheritdoc/>
    public async Task ClearAutocmdsAsync(
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_clear_autocmds",
        [new NvimMap(Opts)],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<NvimInteger> CreateAugroupAsync(
      NvimString Name,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimInteger)
        await RequestAsync(
          "nvim_create_augroup",
          [Name, new NvimMap(Opts)],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task DelAugroupByIdAsync(
      NvimInteger Id,
      CancellationToken cancellationToken = default(CancellationToken)
    ) => await RequestAsync("nvim_del_augroup_by_id", [Id], cancellationToken);

    /// <inheritdoc/>
    public async Task DelAugroupByNameAsync(
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync("nvim_del_augroup_by_name", [Name], cancellationToken);

    /// <inheritdoc/>
    public async Task ExecAutocmdsAsync(
      NvimValue Event,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_exec_autocmds",
        [Event, new NvimMap(Opts)],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<NvimInteger> BufLineCountAsync(
      Buffer Buf,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimInteger)
        await RequestAsync(
          "nvim_buf_line_count",
          [Buf.Extension],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task<NvimBoolean> BufAttachAsync(
      Buffer Buf,
      NvimBoolean SendBuffer,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimBoolean)
        await RequestAsync(
          "nvim_buf_attach",
          [Buf.Extension, SendBuffer, new NvimMap(Opts)],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task<NvimBoolean> BufDetachAsync(
      Buffer Buf,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimBoolean)
        await RequestAsync(
          "nvim_buf_detach",
          [Buf.Extension],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimString>> BufGetLinesAsync(
      Buffer Buf,
      NvimInteger Start,
      NvimInteger End,
      NvimBoolean StrictIndexing,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimArray)
          await RequestAsync(
            "nvim_buf_get_lines",
            [Buf.Extension, Start, End, StrictIndexing],
            cancellationToken
          )
      )
        .Items.Select(item => (NvimString)item)
        .ToArray();

    /// <inheritdoc/>
    public async Task BufSetLinesAsync(
      Buffer Buf,
      NvimInteger Start,
      NvimInteger End,
      NvimBoolean StrictIndexing,
      IReadOnlyList<NvimString> Replacement,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_buf_set_lines",
        [
          Buf.Extension,
          Start,
          End,
          StrictIndexing,
          new NvimArray(Replacement.Select(item => item)),
        ],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task BufSetTextAsync(
      Buffer Buf,
      NvimInteger StartRow,
      NvimInteger StartCol,
      NvimInteger EndRow,
      NvimInteger EndCol,
      IReadOnlyList<NvimString> Replacement,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_buf_set_text",
        [
          Buf.Extension,
          StartRow,
          StartCol,
          EndRow,
          EndCol,
          new NvimArray(Replacement.Select(item => item)),
        ],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimString>> BufGetTextAsync(
      Buffer Buf,
      NvimInteger StartRow,
      NvimInteger StartCol,
      NvimInteger EndRow,
      NvimInteger EndCol,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimArray)
          await RequestAsync(
            "nvim_buf_get_text",
            [
              Buf.Extension,
              StartRow,
              StartCol,
              EndRow,
              EndCol,
              new NvimMap(Opts),
            ],
            cancellationToken
          )
      )
        .Items.Select(item => (NvimString)item)
        .ToArray();

    /// <inheritdoc/>
    public async Task<NvimInteger> BufGetOffsetAsync(
      Buffer Buf,
      NvimInteger Index,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimInteger)
        await RequestAsync(
          "nvim_buf_get_offset",
          [Buf.Extension, Index],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task<NvimValue> BufGetVarAsync(
      Buffer Buf,
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimValue)
        await RequestAsync(
          "nvim_buf_get_var",
          [Buf.Extension, Name],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task<NvimInteger> BufGetChangedtickAsync(
      Buffer Buf,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimInteger)
        await RequestAsync(
          "nvim_buf_get_changedtick",
          [Buf.Extension],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task<
      IReadOnlyList<IReadOnlyList<NvimMapEntry>>
    > BufGetKeymapAsync(
      Buffer Buf,
      NvimString Mode,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimArray)
          await RequestAsync(
            "nvim_buf_get_keymap",
            [Buf.Extension, Mode],
            cancellationToken
          )
      )
        .Items.Select(item => ((NvimMap)item).Entries)
        .ToArray();

    /// <inheritdoc/>
    public async Task BufSetKeymapAsync(
      Buffer Buf,
      NvimString Mode,
      NvimString Lhs,
      NvimString Rhs,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_buf_set_keymap",
        [Buf.Extension, Mode, Lhs, Rhs, new NvimMap(Opts)],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task BufDelKeymapAsync(
      Buffer Buf,
      NvimString Mode,
      NvimString Lhs,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_buf_del_keymap",
        [Buf.Extension, Mode, Lhs],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task BufSetVarAsync(
      Buffer Buf,
      NvimString Name,
      NvimValue Value,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_buf_set_var",
        [Buf.Extension, Name, Value],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task BufDelVarAsync(
      Buffer Buf,
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_buf_del_var",
        [Buf.Extension, Name],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<NvimString> BufGetNameAsync(
      Buffer Buf,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimString)
        await RequestAsync(
          "nvim_buf_get_name",
          [Buf.Extension],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task BufSetNameAsync(
      Buffer Buf,
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_buf_set_name",
        [Buf.Extension, Name],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<NvimBoolean> BufIsLoadedAsync(
      Buffer Buf,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimBoolean)
        await RequestAsync(
          "nvim_buf_is_loaded",
          [Buf.Extension],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task BufDeleteAsync(
      Buffer Buf,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_buf_delete",
        [Buf.Extension, new NvimMap(Opts)],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<NvimBoolean> BufIsValidAsync(
      Buffer Buf,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimBoolean)
        await RequestAsync(
          "nvim_buf_is_valid",
          [Buf.Extension],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task<NvimBoolean> BufDelMarkAsync(
      Buffer Buf,
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimBoolean)
        await RequestAsync(
          "nvim_buf_del_mark",
          [Buf.Extension, Name],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task<NvimBoolean> BufSetMarkAsync(
      Buffer Buf,
      NvimString Name,
      NvimInteger Line,
      NvimInteger Col,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimBoolean)
        await RequestAsync(
          "nvim_buf_set_mark",
          [Buf.Extension, Name, Line, Col, new NvimMap(Opts)],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimInteger>> BufGetMarkAsync(
      Buffer Buf,
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimArray)
          await RequestAsync(
            "nvim_buf_get_mark",
            [Buf.Extension, Name],
            cancellationToken
          )
      )
        .Items.Select(item => (NvimInteger)item)
        .ToArray();

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimMapEntry>> ParseCmdAsync(
      NvimString Str,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimMap)
          await RequestAsync(
            "nvim_parse_cmd",
            [Str, new NvimMap(Opts)],
            cancellationToken
          )
      ).Entries;

    /// <inheritdoc/>
    public async Task<NvimString> CmdAsync(
      IReadOnlyList<NvimMapEntry> Cmd,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimString)
        await RequestAsync(
          "nvim_cmd",
          [new NvimMap(Cmd), new NvimMap(Opts)],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task CreateUserCommandAsync(
      NvimString Name,
      NvimValue Cmd,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_create_user_command",
        [Name, Cmd, new NvimMap(Opts)],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task DelUserCommandAsync(
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    ) => await RequestAsync("nvim_del_user_command", [Name], cancellationToken);

    /// <inheritdoc/>
    public async Task BufCreateUserCommandAsync(
      Buffer Buf,
      NvimString Name,
      NvimValue Cmd,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_buf_create_user_command",
        [Buf.Extension, Name, Cmd, new NvimMap(Opts)],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task BufDelUserCommandAsync(
      Buffer Buf,
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_buf_del_user_command",
        [Buf.Extension, Name],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimMapEntry>> GetCommandsAsync(
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimMap)
          await RequestAsync(
            "nvim_get_commands",
            [new NvimMap(Opts)],
            cancellationToken
          )
      ).Entries;

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimMapEntry>> BufGetCommandsAsync(
      Buffer Buf,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimMap)
          await RequestAsync(
            "nvim_buf_get_commands",
            [Buf.Extension, new NvimMap(Opts)],
            cancellationToken
          )
      ).Entries;

    /// <inheritdoc/>
    public async Task UiTermEventAsync(
      NvimString Event,
      NvimValue Value,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_ui_term_event",
        [Event, Value],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<NvimInteger> CreateNamespaceAsync(
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimInteger)
        await RequestAsync("nvim_create_namespace", [Name], cancellationToken);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimMapEntry>> GetNamespacesAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimMap)
          await RequestAsync("nvim_get_namespaces", [], cancellationToken)
      ).Entries;

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimValue>> BufGetExtmarkByIdAsync(
      Buffer Buf,
      NvimInteger NsId,
      NvimInteger Id,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimArray)
          await RequestAsync(
            "nvim_buf_get_extmark_by_id",
            [Buf.Extension, NsId, Id, new NvimMap(Opts)],
            cancellationToken
          )
      )
        .Items.Select(item => (NvimValue)item)
        .ToArray();

    /// <inheritdoc/>
    public async Task<
      IReadOnlyList<IReadOnlyList<NvimMapEntry>>
    > BufGetExtmarksAsync(
      Buffer Buf,
      NvimInteger NsId,
      NvimValue Start,
      NvimValue End,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimArray)
          await RequestAsync(
            "nvim_buf_get_extmarks",
            [Buf.Extension, NsId, Start, End, new NvimMap(Opts)],
            cancellationToken
          )
      )
        .Items.Select(item => ((NvimMap)item).Entries)
        .ToArray();

    /// <inheritdoc/>
    public async Task<NvimInteger> BufSetExtmarkAsync(
      Buffer Buf,
      NvimInteger NsId,
      NvimInteger Line,
      NvimInteger Col,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimInteger)
        await RequestAsync(
          "nvim_buf_set_extmark",
          [Buf.Extension, NsId, Line, Col, new NvimMap(Opts)],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task<NvimBoolean> BufDelExtmarkAsync(
      Buffer Buf,
      NvimInteger NsId,
      NvimInteger Id,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimBoolean)
        await RequestAsync(
          "nvim_buf_del_extmark",
          [Buf.Extension, NsId, Id],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task BufClearNamespaceAsync(
      Buffer Buf,
      NvimInteger NsId,
      NvimInteger LineStart,
      NvimInteger LineEnd,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_buf_clear_namespace",
        [Buf.Extension, NsId, LineStart, LineEnd],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task SetDecorationProviderAsync(
      NvimInteger NsId,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_set_decoration_provider",
        [NsId, new NvimMap(Opts)],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<NvimValue> GetOptionValueAsync(
      NvimString Name,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimValue)
        await RequestAsync(
          "nvim_get_option_value",
          [Name, new NvimMap(Opts)],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task SetOptionValueAsync(
      NvimString Name,
      NvimValue Value,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_set_option_value",
        [Name, Value, new NvimMap(Opts)],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimMapEntry>> GetAllOptionsInfoAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimMap)
          await RequestAsync("nvim_get_all_options_info", [], cancellationToken)
      ).Entries;

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimMapEntry>> GetOptionInfo2Async(
      NvimString Name,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimMap)
          await RequestAsync(
            "nvim_get_option_info2",
            [Name, new NvimMap(Opts)],
            cancellationToken
          )
      ).Entries;

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Window>> TabpageListWinsAsync(
      Tabpage Tabpage,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimArray)
          await RequestAsync(
            "nvim_tabpage_list_wins",
            [Tabpage.Extension],
            cancellationToken
          )
      )
        .Items.Select(item => new Window((NvimExtension)item))
        .ToArray();

    /// <inheritdoc/>
    public async Task<NvimValue> TabpageGetVarAsync(
      Tabpage Tabpage,
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimValue)
        await RequestAsync(
          "nvim_tabpage_get_var",
          [Tabpage.Extension, Name],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task TabpageSetVarAsync(
      Tabpage Tabpage,
      NvimString Name,
      NvimValue Value,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_tabpage_set_var",
        [Tabpage.Extension, Name, Value],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task TabpageDelVarAsync(
      Tabpage Tabpage,
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_tabpage_del_var",
        [Tabpage.Extension, Name],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<Window> TabpageGetWinAsync(
      Tabpage Tabpage,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      new Window(
        (NvimExtension)
          await RequestAsync(
            "nvim_tabpage_get_win",
            [Tabpage.Extension],
            cancellationToken
          )
      );

    /// <inheritdoc/>
    public async Task TabpageSetWinAsync(
      Tabpage Tabpage,
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_tabpage_set_win",
        [Tabpage.Extension, Win.Extension],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<NvimInteger> TabpageGetNumberAsync(
      Tabpage Tabpage,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimInteger)
        await RequestAsync(
          "nvim_tabpage_get_number",
          [Tabpage.Extension],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task<NvimBoolean> TabpageIsValidAsync(
      Tabpage Tabpage,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimBoolean)
        await RequestAsync(
          "nvim_tabpage_is_valid",
          [Tabpage.Extension],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task<Tabpage> OpenTabpageAsync(
      Buffer Buf,
      NvimBoolean Enter,
      IReadOnlyList<NvimMapEntry> Config,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      new Tabpage(
        (NvimExtension)
          await RequestAsync(
            "nvim_open_tabpage",
            [Buf.Extension, Enter, new NvimMap(Config)],
            cancellationToken
          )
      );

    /// <inheritdoc/>
    public async Task UiAttachAsync(
      NvimInteger Width,
      NvimInteger Height,
      IReadOnlyList<NvimMapEntry> Options,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_ui_attach",
        [Width, Height, new NvimMap(Options)],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task UiSetFocusAsync(
      NvimBoolean Gained,
      CancellationToken cancellationToken = default(CancellationToken)
    ) => await RequestAsync("nvim_ui_set_focus", [Gained], cancellationToken);

    /// <inheritdoc/>
    public async Task UiDetachAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    ) => await RequestAsync("nvim_ui_detach", [], cancellationToken);

    /// <inheritdoc/>
    public async Task UiTryResizeAsync(
      NvimInteger Width,
      NvimInteger Height,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_ui_try_resize",
        [Width, Height],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task UiSetOptionAsync(
      NvimString Name,
      NvimValue Value,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_ui_set_option",
        [Name, Value],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task UiTryResizeGridAsync(
      NvimInteger Grid,
      NvimInteger Width,
      NvimInteger Height,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_ui_try_resize_grid",
        [Grid, Width, Height],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task UiPumSetHeightAsync(
      NvimInteger Height,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync("nvim_ui_pum_set_height", [Height], cancellationToken);

    /// <inheritdoc/>
    public async Task UiPumSetBoundsAsync(
      NvimFloat Width,
      NvimFloat Height,
      NvimFloat Row,
      NvimFloat Col,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_ui_pum_set_bounds",
        [Width, Height, Row, Col],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task UiSendAsync(
      NvimString Content,
      CancellationToken cancellationToken = default(CancellationToken)
    ) => await RequestAsync("nvim_ui_send", [Content], cancellationToken);

    /// <inheritdoc/>
    public async Task<NvimInteger> GetHlIdByNameAsync(
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimInteger)
        await RequestAsync("nvim_get_hl_id_by_name", [Name], cancellationToken);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimMapEntry>> GetHlAsync(
      NvimInteger NsId,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimMap)
          await RequestAsync(
            "nvim_get_hl",
            [NsId, new NvimMap(Opts)],
            cancellationToken
          )
      ).Entries;

    /// <inheritdoc/>
    public async Task SetHlAsync(
      NvimInteger NsId,
      NvimString Name,
      IReadOnlyList<NvimMapEntry> Val,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_set_hl",
        [NsId, Name, new NvimMap(Val)],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<NvimInteger> GetHlNsAsync(
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimInteger)
        await RequestAsync(
          "nvim_get_hl_ns",
          [new NvimMap(Opts)],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task SetHlNsAsync(
      NvimInteger NsId,
      CancellationToken cancellationToken = default(CancellationToken)
    ) => await RequestAsync("nvim_set_hl_ns", [NsId], cancellationToken);

    /// <inheritdoc/>
    public async Task SetHlNsFastAsync(
      NvimInteger NsId,
      CancellationToken cancellationToken = default(CancellationToken)
    ) => await RequestAsync("nvim_set_hl_ns_fast", [NsId], cancellationToken);

    /// <inheritdoc/>
    public async Task FeedkeysAsync(
      NvimString Keys,
      NvimString Mode,
      NvimBoolean EscapeKs,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_feedkeys",
        [Keys, Mode, EscapeKs],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<NvimInteger> InputAsync(
      NvimString Keys,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimInteger)await RequestAsync("nvim_input", [Keys], cancellationToken);

    /// <inheritdoc/>
    public async Task InputMouseAsync(
      NvimString Button,
      NvimString Action,
      NvimString Modifier,
      NvimInteger Grid,
      NvimInteger Row,
      NvimInteger Col,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_input_mouse",
        [Button, Action, Modifier, Grid, Row, Col],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<NvimString> ReplaceTermcodesAsync(
      NvimString Str,
      NvimBoolean FromPart,
      NvimBoolean DoLt,
      NvimBoolean Special,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimString)
        await RequestAsync(
          "nvim_replace_termcodes",
          [Str, FromPart, DoLt, Special],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task<NvimValue> ExecLuaAsync(
      NvimString Code,
      IReadOnlyList<NvimValue> Args,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimValue)
        await RequestAsync(
          "nvim_exec_lua",
          [Code, new NvimArray(Args.Select(item => item))],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task<NvimInteger> StrwidthAsync(
      NvimString Text,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimInteger)
        await RequestAsync("nvim_strwidth", [Text], cancellationToken);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimString>> ListRuntimePathsAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimArray)
          await RequestAsync("nvim_list_runtime_paths", [], cancellationToken)
      )
        .Items.Select(item => (NvimString)item)
        .ToArray();

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimString>> GetRuntimeFileAsync(
      NvimString Name,
      NvimBoolean All,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimArray)
          await RequestAsync(
            "nvim_get_runtime_file",
            [Name, All],
            cancellationToken
          )
      )
        .Items.Select(item => (NvimString)item)
        .ToArray();

    /// <inheritdoc/>
    public async Task SetCurrentDirAsync(
      NvimString Dir,
      CancellationToken cancellationToken = default(CancellationToken)
    ) => await RequestAsync("nvim_set_current_dir", [Dir], cancellationToken);

    /// <inheritdoc/>
    public async Task<NvimString> GetCurrentLineAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimString)
        await RequestAsync("nvim_get_current_line", [], cancellationToken);

    /// <inheritdoc/>
    public async Task SetCurrentLineAsync(
      NvimString Line,
      CancellationToken cancellationToken = default(CancellationToken)
    ) => await RequestAsync("nvim_set_current_line", [Line], cancellationToken);

    /// <inheritdoc/>
    public async Task DelCurrentLineAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    ) => await RequestAsync("nvim_del_current_line", [], cancellationToken);

    /// <inheritdoc/>
    public async Task<NvimValue> GetVarAsync(
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimValue)await RequestAsync("nvim_get_var", [Name], cancellationToken);

    /// <inheritdoc/>
    public async Task SetVarAsync(
      NvimString Name,
      NvimValue Value,
      CancellationToken cancellationToken = default(CancellationToken)
    ) => await RequestAsync("nvim_set_var", [Name, Value], cancellationToken);

    /// <inheritdoc/>
    public async Task DelVarAsync(
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    ) => await RequestAsync("nvim_del_var", [Name], cancellationToken);

    /// <inheritdoc/>
    public async Task<NvimValue> GetVvarAsync(
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimValue)await RequestAsync("nvim_get_vvar", [Name], cancellationToken);

    /// <inheritdoc/>
    public async Task SetVvarAsync(
      NvimString Name,
      NvimValue Value,
      CancellationToken cancellationToken = default(CancellationToken)
    ) => await RequestAsync("nvim_set_vvar", [Name, Value], cancellationToken);

    /// <inheritdoc/>
    public async Task<NvimValue> EchoAsync(
      IReadOnlyList<IReadOnlyList<NvimValue>> Chunks,
      NvimBoolean History,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimValue)
        await RequestAsync(
          "nvim_echo",
          [
            new NvimArray(
              Chunks.Select(item => new NvimArray(item.Select(item => item)))
            ),
            History,
            new NvimMap(Opts),
          ],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Buffer>> ListBufsAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      ((NvimArray)await RequestAsync("nvim_list_bufs", [], cancellationToken))
        .Items.Select(item => new Buffer((NvimExtension)item))
        .ToArray();

    /// <inheritdoc/>
    public async Task<Buffer> GetCurrentBufAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      new Buffer(
        (NvimExtension)
          await RequestAsync("nvim_get_current_buf", [], cancellationToken)
      );

    /// <inheritdoc/>
    public async Task SetCurrentBufAsync(
      Buffer Buf,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_set_current_buf",
        [Buf.Extension],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Window>> ListWinsAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      ((NvimArray)await RequestAsync("nvim_list_wins", [], cancellationToken))
        .Items.Select(item => new Window((NvimExtension)item))
        .ToArray();

    /// <inheritdoc/>
    public async Task<Window> GetCurrentWinAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      new Window(
        (NvimExtension)
          await RequestAsync("nvim_get_current_win", [], cancellationToken)
      );

    /// <inheritdoc/>
    public async Task SetCurrentWinAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_set_current_win",
        [Win.Extension],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<Buffer> CreateBufAsync(
      NvimBoolean Listed,
      NvimBoolean Scratch,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      new Buffer(
        (NvimExtension)
          await RequestAsync(
            "nvim_create_buf",
            [Listed, Scratch],
            cancellationToken
          )
      );

    /// <inheritdoc/>
    public async Task<NvimInteger> OpenTermAsync(
      Buffer Buf,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimInteger)
        await RequestAsync(
          "nvim_open_term",
          [Buf.Extension, new NvimMap(Opts)],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task ChanSendAsync(
      NvimInteger Chan,
      NvimString Data,
      CancellationToken cancellationToken = default(CancellationToken)
    ) => await RequestAsync("nvim_chan_send", [Chan, Data], cancellationToken);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Tabpage>> ListTabpagesAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimArray)
          await RequestAsync("nvim_list_tabpages", [], cancellationToken)
      )
        .Items.Select(item => new Tabpage((NvimExtension)item))
        .ToArray();

    /// <inheritdoc/>
    public async Task<Tabpage> GetCurrentTabpageAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      new Tabpage(
        (NvimExtension)
          await RequestAsync("nvim_get_current_tabpage", [], cancellationToken)
      );

    /// <inheritdoc/>
    public async Task SetCurrentTabpageAsync(
      Tabpage Tabpage,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_set_current_tabpage",
        [Tabpage.Extension],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<NvimBoolean> PasteAsync(
      NvimString Data,
      NvimBoolean Crlf,
      NvimInteger Phase,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimBoolean)
        await RequestAsync(
          "nvim_paste",
          [Data, Crlf, Phase],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task PutAsync(
      IReadOnlyList<NvimString> Lines,
      NvimString Type,
      NvimBoolean After,
      NvimBoolean Follow,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_put",
        [new NvimArray(Lines.Select(item => item)), Type, After, Follow],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<NvimInteger> GetColorByNameAsync(
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimInteger)
        await RequestAsync("nvim_get_color_by_name", [Name], cancellationToken);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimMapEntry>> GetColorMapAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimMap)await RequestAsync("nvim_get_color_map", [], cancellationToken)
      ).Entries;

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimMapEntry>> GetContextAsync(
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimMap)
          await RequestAsync(
            "nvim_get_context",
            [new NvimMap(Opts)],
            cancellationToken
          )
      ).Entries;

    /// <inheritdoc/>
    public async Task<NvimValue> LoadContextAsync(
      IReadOnlyList<NvimMapEntry> Dict,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimValue)
        await RequestAsync(
          "nvim_load_context",
          [new NvimMap(Dict)],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimMapEntry>> GetModeAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimMap)await RequestAsync("nvim_get_mode", [], cancellationToken)
      ).Entries;

    /// <inheritdoc/>
    public async Task<
      IReadOnlyList<IReadOnlyList<NvimMapEntry>>
    > GetKeymapAsync(
      NvimString Mode,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimArray)
          await RequestAsync("nvim_get_keymap", [Mode], cancellationToken)
      )
        .Items.Select(item => ((NvimMap)item).Entries)
        .ToArray();

    /// <inheritdoc/>
    public async Task SetKeymapAsync(
      NvimString Mode,
      NvimString Lhs,
      NvimString Rhs,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_set_keymap",
        [Mode, Lhs, Rhs, new NvimMap(Opts)],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task DelKeymapAsync(
      NvimString Mode,
      NvimString Lhs,
      CancellationToken cancellationToken = default(CancellationToken)
    ) => await RequestAsync("nvim_del_keymap", [Mode, Lhs], cancellationToken);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimValue>> GetApiInfoAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimArray)
          await RequestAsync("nvim_get_api_info", [], cancellationToken)
      )
        .Items.Select(item => (NvimValue)item)
        .ToArray();

    /// <inheritdoc/>
    public async Task SetClientInfoAsync(
      NvimString Name,
      IReadOnlyList<NvimMapEntry> Version,
      NvimString Type,
      IReadOnlyList<NvimMapEntry> Methods,
      IReadOnlyList<NvimMapEntry> Attributes,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_set_client_info",
        [
          Name,
          new NvimMap(Version),
          Type,
          new NvimMap(Methods),
          new NvimMap(Attributes),
        ],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimMapEntry>> GetChanInfoAsync(
      NvimInteger Chan,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimMap)
          await RequestAsync("nvim_get_chan_info", [Chan], cancellationToken)
      ).Entries;

    /// <inheritdoc/>
    public async Task<
      IReadOnlyList<IReadOnlyList<NvimMapEntry>>
    > ListChansAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      ((NvimArray)await RequestAsync("nvim_list_chans", [], cancellationToken))
        .Items.Select(item => ((NvimMap)item).Entries)
        .ToArray();

    /// <inheritdoc/>
    public async Task<IReadOnlyList<IReadOnlyList<NvimMapEntry>>> ListUisAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      ((NvimArray)await RequestAsync("nvim_list_uis", [], cancellationToken))
        .Items.Select(item => ((NvimMap)item).Entries)
        .ToArray();

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimValue>> GetProcChildrenAsync(
      NvimInteger Pid,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimArray)
          await RequestAsync("nvim_get_proc_children", [Pid], cancellationToken)
      )
        .Items.Select(item => (NvimValue)item)
        .ToArray();

    /// <inheritdoc/>
    public async Task<NvimValue> GetProcAsync(
      NvimInteger Pid,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimValue)await RequestAsync("nvim_get_proc", [Pid], cancellationToken);

    /// <inheritdoc/>
    public async Task SelectPopupmenuItemAsync(
      NvimInteger Item,
      NvimBoolean Insert,
      NvimBoolean Finish,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_select_popupmenu_item",
        [Item, Insert, Finish, new NvimMap(Opts)],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<NvimBoolean> DelMarkAsync(
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimBoolean)
        await RequestAsync("nvim_del_mark", [Name], cancellationToken);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimValue>> GetMarkAsync(
      NvimString Name,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimArray)
          await RequestAsync(
            "nvim_get_mark",
            [Name, new NvimMap(Opts)],
            cancellationToken
          )
      )
        .Items.Select(item => (NvimValue)item)
        .ToArray();

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimMapEntry>> EvalStatuslineAsync(
      NvimString Str,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimMap)
          await RequestAsync(
            "nvim_eval_statusline",
            [Str, new NvimMap(Opts)],
            cancellationToken
          )
      ).Entries;

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimMapEntry>> Exec2Async(
      NvimString Src,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimMap)
          await RequestAsync(
            "nvim_exec2",
            [Src, new NvimMap(Opts)],
            cancellationToken
          )
      ).Entries;

    /// <inheritdoc/>
    public async Task CommandAsync(
      NvimString Cmd,
      CancellationToken cancellationToken = default(CancellationToken)
    ) => await RequestAsync("nvim_command", [Cmd], cancellationToken);

    /// <inheritdoc/>
    public async Task<NvimValue> EvalAsync(
      NvimString Expr,
      CancellationToken cancellationToken = default(CancellationToken)
    ) => (NvimValue)await RequestAsync("nvim_eval", [Expr], cancellationToken);

    /// <inheritdoc/>
    public async Task<NvimValue> CallFunctionAsync(
      NvimString Fn,
      IReadOnlyList<NvimValue> Args,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimValue)
        await RequestAsync(
          "nvim_call_function",
          [Fn, new NvimArray(Args.Select(item => item))],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task<NvimValue> CallDictFunctionAsync(
      NvimValue Dict,
      NvimString Fn,
      IReadOnlyList<NvimValue> Args,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimValue)
        await RequestAsync(
          "nvim_call_dict_function",
          [Dict, Fn, new NvimArray(Args.Select(item => item))],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimMapEntry>> ParseExpressionAsync(
      NvimString Expr,
      NvimString Flags,
      NvimBoolean Hl,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimMap)
          await RequestAsync(
            "nvim_parse_expression",
            [Expr, Flags, Hl],
            cancellationToken
          )
      ).Entries;

    /// <inheritdoc/>
    public async Task<Window> OpenWinAsync(
      Buffer Buf,
      NvimBoolean Enter,
      IReadOnlyList<NvimMapEntry> Config,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      new Window(
        (NvimExtension)
          await RequestAsync(
            "nvim_open_win",
            [Buf.Extension, Enter, new NvimMap(Config)],
            cancellationToken
          )
      );

    /// <inheritdoc/>
    public async Task WinSetConfigAsync(
      Window Win,
      IReadOnlyList<NvimMapEntry> Config,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_win_set_config",
        [Win.Extension, new NvimMap(Config)],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimMapEntry>> WinGetConfigAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimMap)
          await RequestAsync(
            "nvim_win_get_config",
            [Win.Extension],
            cancellationToken
          )
      ).Entries;

    /// <inheritdoc/>
    public async Task<Buffer> WinGetBufAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      new Buffer(
        (NvimExtension)
          await RequestAsync(
            "nvim_win_get_buf",
            [Win.Extension],
            cancellationToken
          )
      );

    /// <inheritdoc/>
    public async Task WinSetBufAsync(
      Window Win,
      Buffer Buf,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_win_set_buf",
        [Win.Extension, Buf.Extension],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimInteger>> WinGetCursorAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimArray)
          await RequestAsync(
            "nvim_win_get_cursor",
            [Win.Extension],
            cancellationToken
          )
      )
        .Items.Select(item => (NvimInteger)item)
        .ToArray();

    /// <inheritdoc/>
    public async Task WinSetCursorAsync(
      Window Win,
      IReadOnlyList<NvimInteger> Pos,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_win_set_cursor",
        [Win.Extension, new NvimArray(Pos.Select(item => item))],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<NvimInteger> WinGetHeightAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimInteger)
        await RequestAsync(
          "nvim_win_get_height",
          [Win.Extension],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task WinSetHeightAsync(
      Window Win,
      NvimInteger Height,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_win_set_height",
        [Win.Extension, Height],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<NvimInteger> WinGetWidthAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimInteger)
        await RequestAsync(
          "nvim_win_get_width",
          [Win.Extension],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task WinSetWidthAsync(
      Window Win,
      NvimInteger Width,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_win_set_width",
        [Win.Extension, Width],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<NvimValue> WinGetVarAsync(
      Window Win,
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimValue)
        await RequestAsync(
          "nvim_win_get_var",
          [Win.Extension, Name],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task WinSetVarAsync(
      Window Win,
      NvimString Name,
      NvimValue Value,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_win_set_var",
        [Win.Extension, Name, Value],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task WinDelVarAsync(
      Window Win,
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_win_del_var",
        [Win.Extension, Name],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimInteger>> WinGetPositionAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimArray)
          await RequestAsync(
            "nvim_win_get_position",
            [Win.Extension],
            cancellationToken
          )
      )
        .Items.Select(item => (NvimInteger)item)
        .ToArray();

    /// <inheritdoc/>
    public async Task<Tabpage> WinGetTabpageAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      new Tabpage(
        (NvimExtension)
          await RequestAsync(
            "nvim_win_get_tabpage",
            [Win.Extension],
            cancellationToken
          )
      );

    /// <inheritdoc/>
    public async Task<NvimInteger> WinGetNumberAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimInteger)
        await RequestAsync(
          "nvim_win_get_number",
          [Win.Extension],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task<NvimBoolean> WinIsValidAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (NvimBoolean)
        await RequestAsync(
          "nvim_win_is_valid",
          [Win.Extension],
          cancellationToken
        );

    /// <inheritdoc/>
    public async Task WinHideAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync("nvim_win_hide", [Win.Extension], cancellationToken);

    /// <inheritdoc/>
    public async Task WinCloseAsync(
      Window Win,
      NvimBoolean Force,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_win_close",
        [Win.Extension, Force],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task WinSetHlNsAsync(
      Window Win,
      NvimInteger NsId,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      await RequestAsync(
        "nvim_win_set_hl_ns",
        [Win.Extension, NsId],
        cancellationToken
      );

    /// <inheritdoc/>
    public async Task<IReadOnlyList<NvimMapEntry>> WinTextHeightAsync(
      Window Win,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    ) =>
      (
        (NvimMap)
          await RequestAsync(
            "nvim_win_text_height",
            [Win.Extension, new NvimMap(Opts)],
            cancellationToken
          )
      ).Entries;
  }
}
