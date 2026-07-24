using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Nvim.Client
{
  public partial interface INvimClient
  {
    /// <summary>
    /// Gets all autocommands matching ALL criteria in the <paramref name = "Opts"/> query.
    /// </summary>
    /// <param name = "Opts">
    /// Dict with at least one of these keys:
    /// <list type="bullet">
    /// <item><description>
    /// <c>buf</c>: (<c>integer[]|integer?</c>) Buffer id or list of buffer ids, for buffer-local autocommands <c>autocmd-buflocal</c>. Not allowed with <c>pattern</c>.
    /// </description></item>
    /// <item><description>
    /// <c>event</c>: (<c>vim.api.keyset.events|vim.api.keyset.events[]?</c>) Event(s) to match <c>autocmd-events</c>.
    /// </description></item>
    /// <item><description>
    /// <c>group</c>: (<c>string|table?</c>) Group name or id to match.
    /// </description></item>
    /// <item><description>
    /// <c>id</c>: (<c>integer?</c>) Autocommand ID to match.
    /// </description></item>
    /// <item><description>
    /// <c>pattern</c>: (<c>string|table?</c>) Pattern(s) to match <c>autocmd-pattern</c>. Not allowed with <c>buf</c>.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Array of matching autocommands, where each item has:
    /// <list type="bullet">
    /// <item><description>
    /// <c>buf</c> (<c>integer?</c>): Buffer id (only for <c>autocmd-buffer-local</c>).
    /// </description></item>
    /// <item><description>
    /// <c>buflocal</c> (<c>boolean?</c>): true if the autocommand is buffer-local <c>autocmd-buffer-local</c>.
    /// </description></item>
    /// <item><description>
    /// <c>callback</c>: (<c>function|string?</c>): Event handler: a Lua function or Vimscript function name.
    /// </description></item>
    /// <item><description>
    /// <c>command</c>: (<c>string</c>) Event handler: an Ex-command. Empty if a <c>callback</c> is set.
    /// </description></item>
    /// <item><description>
    /// <c>desc</c>: (<c>string</c>) Description.
    /// </description></item>
    /// <item><description>
    /// <c>event</c>: (<c>vim.api.keyset.events</c>) Event name(s).
    /// </description></item>
    /// <item><description>
    /// <c>group</c>: (<c>integer</c>) Group id.
    /// </description></item>
    /// <item><description>
    /// <c>group_name</c>: (<c>string</c>) Group name.
    /// </description></item>
    /// <item><description>
    /// <c>id</c>: (<c>integer</c>) Autocommand id (only when defined with the API).
    /// </description></item>
    /// <item><description>
    /// <c>once</c>: (<c>boolean</c>) true if <c>autocmd-once</c> was set.
    /// </description></item>
    /// <item><description>
    /// <c>pattern</c>: (<c>string</c>) Autocommand pattern.
    /// </description></item>
    /// </list>
    /// </returns>
    Task<IReadOnlyList<IReadOnlyList<NvimMapEntry>>> GetAutocmdsAsync(
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Creates an <c>autocommand</c> event handler, defined by <c>callback</c> (Lua function or Vimscript function name string) or <c>command</c> (Ex command string).
    /// </summary>
    /// <param name = "Event">
    /// Event(s) that will trigger the handler (<c>callback</c> or <c>command</c>).
    /// </param>
    /// <param name = "Opts">
    /// Options dict:
    /// <list type="bullet">
    /// <item><description>
    /// <c>buf</c> (<c>integer?</c>) Buffer id for buffer-local autocommands <c>autocmd-buflocal</c>. Not allowed with <c>pattern</c>.
    /// </description></item>
    /// <item><description>
    /// <c>callback</c> (<c>function|string?</c>) Lua function (or Vimscript function name, if string) called when the event(s) is triggered. Lua callback can return <c>lua-truthy</c> to delete the autocommand. Callback receives one argument, a table with keys: <c>event-args</c>
    /// <list type="bullet">
    /// <item><description>
    /// <c>buf</c>: (<c>number</c>) [&lt;abuf&gt;]
    /// </description></item>
    /// <item><description>
    /// <c>data</c>: (<c>any</c>) Arbitrary data passed from <see cref = "ExecAutocmdsAsync"/> <c>event-data</c>
    /// </description></item>
    /// <item><description>
    /// <c>event</c>: (<c>vim.api.keyset.events</c>) Name of the triggered event <c>autocmd-events</c>
    /// </description></item>
    /// <item><description>
    /// <c>file</c>: (<c>string</c>) [&lt;afile&gt;] (not expanded to a full path)
    /// </description></item>
    /// <item><description>
    /// <c>group</c>: (<c>number?</c>) Group id, if any
    /// </description></item>
    /// <item><description>
    /// <c>id</c>: (<c>number</c>) Autocommand id
    /// </description></item>
    /// <item><description>
    /// <c>match</c>: (<c>string</c>) [&lt;amatch&gt;] (expanded to a full path)
    /// </description></item>
    /// </list>
    /// </description></item>
    /// <item><description>
    /// <c>command</c> (string?) Vim command executed on event. Not allowed with <c>callback</c>.
    /// </description></item>
    /// <item><description>
    /// <c>desc</c> (<c>string?</c>) Description (for documentation and troubleshooting).
    /// </description></item>
    /// <item><description>
    /// <c>group</c> (<c>string|integer?</c>) Group name or id to match against.
    /// </description></item>
    /// <item><description>
    /// <c>nested</c> (<c>boolean?</c>, default: false) Run nested autocommands <c>autocmd-nested</c>.
    /// </description></item>
    /// <item><description>
    /// <c>once</c> (<c>boolean?</c>, default: false) Handle the event only once <c>autocmd-once</c>.
    /// </description></item>
    /// <item><description>
    /// <c>pattern</c> (<c>string|array?</c>) Pattern(s) to match literally <c>autocmd-pattern</c>.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Autocommand id (number)
    /// </returns>
    Task<NvimInteger> CreateAutocmdAsync(
      NvimValue Event,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Deletes an autocommand by id.
    /// </summary>
    /// <param name = "Id">
    /// Autocommand id returned by <see cref = "CreateAutocmdAsync"/>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task DelAutocmdAsync(
      NvimInteger Id,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Clears all autocommands matching the <paramref name = "Opts"/> query.
    /// </summary>
    /// <param name = "Opts">
    /// Optional parameters:
    /// <list type="bullet">
    /// <item><description>
    /// <c>buf</c>: (<c>integer?</c>) Select <c>autocmd-buflocal</c> autocommands. Not allowed with <c>pattern</c>.
    /// </description></item>
    /// <item><description>
    /// <c>event</c>: (<c>vim.api.keyset.events|vim.api.keyset.events[]?</c>) Examples:
    /// <list type="bullet">
    /// <item><description>
    /// <c>event</c>: &quot;pat1&quot;
    /// </description></item>
    /// <item><description>
    /// <c>event</c>: { &quot;pat1&quot; }
    /// </description></item>
    /// <item><description>
    /// <c>event</c>: { &quot;pat1&quot;, &quot;pat2&quot;, &quot;pat3&quot; }
    /// </description></item>
    /// </list>
    /// </description></item>
    /// <item><description>
    /// <c>group</c>: (<c>string|int?</c>) Group name or id.
    /// <list type="bullet">
    /// <item><description>
    /// <c>NOTE</c>: If not given, matches autocmds not in any group.
    /// </description></item>
    /// </list>
    /// </description></item>
    /// <item><description>
    /// <c>pattern</c>: (<c>string|table?</c>) Filter by patterns (exact match). Not allowed with <c>buf</c>.
    /// <list type="bullet">
    /// <item><description>
    /// <c>Example</c>: if you have <c>*.py</c> as that pattern for the autocmd, you must pass <c>*.py</c> exactly to clear it. <c>test.py</c> will not match the pattern.
    /// </description></item>
    /// </list>
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task ClearAutocmdsAsync(
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Create or get an autocommand group <c>autocmd-groups</c>.
    /// </summary>
    /// <param name = "Name">
    /// Group name
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters:
    /// <list type="bullet">
    /// <item><description>
    /// <c>clear</c> (<c>boolean?</c>, default: true) Clear existing commands in the group <c>autocmd-groups</c>.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Group id.
    /// </returns>
    Task<NvimInteger> CreateAugroupAsync(
      NvimString Name,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Delete an autocommand group by id.
    /// </summary>
    /// <param name = "Id">
    /// Group id.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task DelAugroupByIdAsync(
      NvimInteger Id,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Delete an autocommand group by name.
    /// </summary>
    /// <param name = "Name">
    /// Group name.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task DelAugroupByNameAsync(
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Executes handlers for <paramref name = "Event"/> that match the corresponding <paramref name = "Opts"/> query.
    /// </summary>
    /// <param name = "Event">
    /// Event(s) to execute.
    /// </param>
    /// <param name = "Opts">
    /// Optional filters:
    /// <list type="bullet">
    /// <item><description>
    /// <c>buf</c> (<c>integer?</c>) Buffer id <c>autocmd-buflocal</c>. Not allowed with <c>pattern</c>.
    /// </description></item>
    /// <item><description>
    /// <c>data</c> (<c>any</c>): Arbitrary data passed to the callback. See <see cref = "CreateAutocmdAsync"/>.
    /// </description></item>
    /// <item><description>
    /// <c>group</c> (<c>string|integer?</c>) Group name or id to match against. <c>autocmd-groups</c>.
    /// </description></item>
    /// <item><description>
    /// <c>modeline</c> (<c>boolean?</c>, default: true) Process the modeline after the autocommands [&lt;nomodeline&gt;].
    /// </description></item>
    /// <item><description>
    /// <c>pattern</c> (<c>string|array?</c>, default: current file name) <c>autocmd-pattern</c>. Not allowed with <c>buf</c>.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task ExecAutocmdsAsync(
      NvimValue Event,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Returns the number of lines in the given buffer.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Line count, or 0 for unloaded buffer. <c>api-buffer</c>
    /// </returns>
    Task<NvimInteger> BufLineCountAsync(
      Buffer Buf,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Activates <c>api-buffer-updates</c> events on a channel, or as Lua callbacks.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "SendBuffer">
    /// True if the initial notification should contain the whole buffer: first notification will be <c>nvim_buf_lines_event</c>. Else the first notification will be <c>nvim_buf_changedtick_event</c>. Not for Lua callbacks.
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters.
    /// <list type="bullet">
    /// <item><description>
    /// <c>on_bytes</c>: Called on granular changes (compared to on_lines). Not called on buffer reload (<c>:checktime</c>, <c>:edit</c>, …), see <c>on_reload:</c>. Return a [lua-truthy] value to detach. Args:
    /// <list type="bullet">
    /// <item><description>
    /// the string &quot;bytes&quot;
    /// </description></item>
    /// <item><description>
    /// buffer id
    /// </description></item>
    /// <item><description>
    /// <c>b</c>:changedtick
    /// </description></item>
    /// <item><description>
    /// start row of the changed text (zero-indexed)
    /// </description></item>
    /// <item><description>
    /// start column of the changed text
    /// </description></item>
    /// <item><description>
    /// byte offset of the changed text (from the start of the buffer)
    /// </description></item>
    /// <item><description>
    /// old end row of the changed text (offset from start row)
    /// </description></item>
    /// <item><description>
    /// old end column of the changed text (if old end row = 0, offset from start column)
    /// </description></item>
    /// <item><description>
    /// old end byte length of the changed text
    /// </description></item>
    /// <item><description>
    /// new end row of the changed text (offset from start row)
    /// </description></item>
    /// <item><description>
    /// new end column of the changed text (if new end row = 0, offset from start column)
    /// </description></item>
    /// <item><description>
    /// new end byte length of the changed text
    /// </description></item>
    /// </list>
    /// </description></item>
    /// <item><description>
    /// <c>on_changedtick</c>: Called on [changetick] increment without text change. Args:
    /// <list type="bullet">
    /// <item><description>
    /// the string &quot;changedtick&quot;
    /// </description></item>
    /// <item><description>
    /// buffer id
    /// </description></item>
    /// <item><description>
    /// <c>b</c>:changedtick
    /// </description></item>
    /// </list>
    /// </description></item>
    /// <item><description>
    /// <c>on_detach</c>: Called on detach. Args:
    /// <list type="bullet">
    /// <item><description>
    /// the string &quot;detach&quot;
    /// </description></item>
    /// <item><description>
    /// buffer id
    /// </description></item>
    /// </list>
    /// </description></item>
    /// <item><description>
    /// <c>on_lines</c>: Called on linewise changes. Not called on buffer reload (<c>:checktime</c>, <c>:edit</c>, …), see <c>on_reload:</c>. Return a [lua-truthy] value to detach. Args:
    /// <list type="bullet">
    /// <item><description>
    /// the string &quot;lines&quot;
    /// </description></item>
    /// <item><description>
    /// buffer id
    /// </description></item>
    /// <item><description>
    /// <c>b</c>:changedtick
    /// </description></item>
    /// <item><description>
    /// first line that changed (zero-indexed)
    /// </description></item>
    /// <item><description>
    /// last line that was changed
    /// </description></item>
    /// <item><description>
    /// last line in the updated range
    /// </description></item>
    /// <item><description>
    /// byte count of previous contents
    /// </description></item>
    /// <item><description>
    /// <c>deleted_codepoints</c> (if <c>utf_sizes</c> is true)
    /// </description></item>
    /// <item><description>
    /// <c>deleted_codeunits</c> (if <c>utf_sizes</c> is true)
    /// </description></item>
    /// </list>
    /// </description></item>
    /// <item><description>
    /// <c>on_reload</c>: Called on whole-buffer load (<c>:checktime</c>, <c>:edit</c>, …). Clients should typically re-fetch the entire buffer contents. Args:
    /// <list type="bullet">
    /// <item><description>
    /// the string &quot;reload&quot;
    /// </description></item>
    /// <item><description>
    /// buffer id
    /// </description></item>
    /// </list>
    /// </description></item>
    /// <item><description>
    /// <c>preview</c>: also attach to command preview (i.e. &apos;inccommand&apos;) events.
    /// </description></item>
    /// <item><description>
    /// <c>utf_sizes</c>: include UTF-32 and UTF-16 size of the replaced region, as args to <c>on_lines</c>.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// False if attach failed (invalid parameter, or buffer isn&apos;t loaded); otherwise True.
    /// </returns>
    Task<NvimBoolean> BufAttachAsync(
      Buffer Buf,
      NvimBoolean SendBuffer,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Deactivates buffer-update events on the channel.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// False if detach failed (because the buffer isn&apos;t loaded); otherwise True.
    /// </returns>
    Task<NvimBoolean> BufDetachAsync(
      Buffer Buf,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets a line-range from the buffer.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "Start">
    /// First line index
    /// </param>
    /// <param name = "End">
    /// Last line index, exclusive
    /// </param>
    /// <param name = "StrictIndexing">
    /// Whether out-of-bounds should be an error.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Array of lines, or empty array for unloaded buffer.
    /// </returns>
    Task<IReadOnlyList<NvimString>> BufGetLinesAsync(
      Buffer Buf,
      NvimInteger Start,
      NvimInteger End,
      NvimBoolean StrictIndexing,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets (replaces) a line-range in the buffer.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "Start">
    /// First line index
    /// </param>
    /// <param name = "End">
    /// Last line index, exclusive
    /// </param>
    /// <param name = "StrictIndexing">
    /// Whether out-of-bounds should be an error.
    /// </param>
    /// <param name = "Replacement">
    /// Array of lines to use as replacement
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task BufSetLinesAsync(
      Buffer Buf,
      NvimInteger Start,
      NvimInteger End,
      NvimBoolean StrictIndexing,
      IReadOnlyList<NvimString> Replacement,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets (replaces) a range in the buffer.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "StartRow">
    /// First line index
    /// </param>
    /// <param name = "StartCol">
    /// Starting column (byte offset) on first line
    /// </param>
    /// <param name = "EndRow">
    /// Last line index, inclusive
    /// </param>
    /// <param name = "EndCol">
    /// Ending column (byte offset) on last line, exclusive
    /// </param>
    /// <param name = "Replacement">
    /// Array of lines to use as replacement
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task BufSetTextAsync(
      Buffer Buf,
      NvimInteger StartRow,
      NvimInteger StartCol,
      NvimInteger EndRow,
      NvimInteger EndCol,
      IReadOnlyList<NvimString> Replacement,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets a range from the buffer (may be partial lines, unlike <see cref = "BufGetLinesAsync"/>).
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "StartRow">
    /// First line index
    /// </param>
    /// <param name = "StartCol">
    /// Starting column (byte offset) on first line
    /// </param>
    /// <param name = "EndRow">
    /// Last line index, inclusive
    /// </param>
    /// <param name = "EndCol">
    /// Ending column (byte offset) on last line, exclusive
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters. Currently unused.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Array of lines, or empty array for unloaded buffer.
    /// </returns>
    Task<IReadOnlyList<NvimString>> BufGetTextAsync(
      Buffer Buf,
      NvimInteger StartRow,
      NvimInteger StartCol,
      NvimInteger EndRow,
      NvimInteger EndCol,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Returns the byte offset of a line (0-indexed).
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "Index">
    /// Line index
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Integer byte offset, or -1 for unloaded buffer.
    /// </returns>
    Task<NvimInteger> BufGetOffsetAsync(
      Buffer Buf,
      NvimInteger Index,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets a buffer-scoped (b:) variable.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "Name">
    /// Variable name
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Variable value
    /// </returns>
    Task<NvimValue> BufGetVarAsync(
      Buffer Buf,
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets a changed tick of a buffer.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// <c>b:changedtick</c> value.
    /// </returns>
    Task<NvimInteger> BufGetChangedtickAsync(
      Buffer Buf,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets a list of buffer-local <c>mapping</c> definitions.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "Mode">
    /// Mode short-name (&quot;n&quot;, &quot;i&quot;, &quot;v&quot;, ...)
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Array of <c>maparg()</c>-like dictionaries describing mappings. The &quot;buf&quot; key holds the associated buffer id.
    /// </returns>
    Task<IReadOnlyList<IReadOnlyList<NvimMapEntry>>> BufGetKeymapAsync(
      Buffer Buf,
      NvimString Mode,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets a buffer-local <c>mapping</c> for the given mode.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "Mode">
    /// The <paramref name = "Mode"/> argument.
    /// </param>
    /// <param name = "Lhs">
    /// The <paramref name = "Lhs"/> argument.
    /// </param>
    /// <param name = "Rhs">
    /// The <paramref name = "Rhs"/> argument.
    /// </param>
    /// <param name = "Opts">
    /// The <paramref name = "Opts"/> argument.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task BufSetKeymapAsync(
      Buffer Buf,
      NvimString Mode,
      NvimString Lhs,
      NvimString Rhs,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Unmaps a buffer-local <c>mapping</c> for the given mode.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "Mode">
    /// The <paramref name = "Mode"/> argument.
    /// </param>
    /// <param name = "Lhs">
    /// The <paramref name = "Lhs"/> argument.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task BufDelKeymapAsync(
      Buffer Buf,
      NvimString Mode,
      NvimString Lhs,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets a buffer-scoped (b:) variable.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "Name">
    /// Variable name
    /// </param>
    /// <param name = "Value">
    /// Variable value
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task BufSetVarAsync(
      Buffer Buf,
      NvimString Name,
      NvimValue Value,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Removes a buffer-scoped (b:) variable.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "Name">
    /// Variable name
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task BufDelVarAsync(
      Buffer Buf,
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the full file name for the buffer.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Buffer name
    /// </returns>
    Task<NvimString> BufGetNameAsync(
      Buffer Buf,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets the full file name for a buffer, like <c>:file_f</c>.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "Name">
    /// Buffer name
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task BufSetNameAsync(
      Buffer Buf,
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Checks if a buffer is valid and loaded.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// true if the buffer is valid and loaded, false otherwise.
    /// </returns>
    Task<NvimBoolean> BufIsLoadedAsync(
      Buffer Buf,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Deletes a buffer and its metadata (like <c>:bwipeout</c>).
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters. Keys:
    /// <list type="bullet">
    /// <item><description>
    /// <c>force</c>: Force deletion, ignore unsaved changes.
    /// </description></item>
    /// <item><description>
    /// <c>unload</c>: Unloaded only (<c>:bunload</c>), do not delete.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task BufDeleteAsync(
      Buffer Buf,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Checks if a buffer is valid.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// true if the buffer is valid, false otherwise.
    /// </returns>
    Task<NvimBoolean> BufIsValidAsync(
      Buffer Buf,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Deletes a named mark in the buffer.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer to set the mark on
    /// </param>
    /// <param name = "Name">
    /// Mark name
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// true if the mark was deleted, else false.
    /// </returns>
    Task<NvimBoolean> BufDelMarkAsync(
      Buffer Buf,
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets a named mark in the given buffer, all marks are allowed file/uppercase, visual, last change, etc.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer to set the mark on
    /// </param>
    /// <param name = "Name">
    /// Mark name
    /// </param>
    /// <param name = "Line">
    /// Line number
    /// </param>
    /// <param name = "Col">
    /// Column/row number
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters. Reserved for future use.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// true if the mark was set, else false.
    /// </returns>
    Task<NvimBoolean> BufSetMarkAsync(
      Buffer Buf,
      NvimString Name,
      NvimInteger Line,
      NvimInteger Col,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Returns a <c>(row,col)</c> tuple representing the position of the named mark.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "Name">
    /// Mark name
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// (row, col) tuple, (0, 0) if the mark is not set, or is an uppercase/file mark set in another buffer.
    /// </returns>
    Task<IReadOnlyList<NvimInteger>> BufGetMarkAsync(
      Buffer Buf,
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Parse command line.
    /// </summary>
    /// <param name = "Str">
    /// Command line string to parse. Cannot contain &quot;\n&quot;.
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters. Reserved for future use.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Dict containing command information, with these keys:
    /// <list type="bullet">
    /// <item><description>
    /// <c>cmd</c>: (string) Command name.
    /// </description></item>
    /// <item><description>
    /// <c>range</c>: (array) (optional) Command range ([&lt;line1&gt;] [&lt;line2&gt;]). Omitted if command doesn&apos;t accept a range. Otherwise, has no elements if no range was specified, one element if only a single range item was specified, or two elements if both range items were specified.
    /// </description></item>
    /// <item><description>
    /// <c>count</c>: (number) (optional) Command [&lt;count&gt;]. Omitted if command cannot take a count.
    /// </description></item>
    /// <item><description>
    /// <c>reg</c>: (string) (optional) Command [&lt;register&gt;]. Omitted if command cannot take a register.
    /// </description></item>
    /// <item><description>
    /// <c>bang</c>: (boolean) Whether command contains a [&lt;bang&gt;] (!) modifier.
    /// </description></item>
    /// <item><description>
    /// <c>args</c>: (array) Command arguments.
    /// </description></item>
    /// <item><description>
    /// <c>addr</c>: (string) Value of <c>:command-addr</c>. Uses short name or &quot;line&quot; for -addr=lines.
    /// </description></item>
    /// <item><description>
    /// <c>nargs</c>: (string) Value of <c>:command-nargs</c>.
    /// </description></item>
    /// <item><description>
    /// <c>nextcmd</c>: (string) Next command if there are multiple commands separated by a <c>:bar</c>. Empty if there isn&apos;t a next command.
    /// </description></item>
    /// <item><description>
    /// <c>magic</c>: (dict) Which characters have special meaning in the command arguments.
    /// <list type="bullet">
    /// <item><description>
    /// <c>file</c>: (boolean) The command expands filenames. Which means characters such as &quot;%&quot;, &quot;#&quot; and wildcards are expanded.
    /// </description></item>
    /// <item><description>
    /// <c>bar</c>: (boolean) The &quot;<c>&quot; character is treated as a command separator and the double quote character (&quot;) is treated as the start of a comment. - mods: (dict) </c>:command-modifiers<c>. - filter: (dict) </c>:filter<c>. - pattern: (string) Filter pattern. Empty string if there is no filter. - force: (boolean) Whether filter is inverted or not. - silent: (boolean) </c>:silent<c>. - emsg_silent: (boolean) </c>:silent!<c>. - unsilent: (boolean) </c>:unsilent<c>. - sandbox: (boolean) </c>:sandbox<c>. - noautocmd: (boolean) </c>:noautocmd<c>. - browse: (boolean) </c>:browse<c>. - confirm: (boolean) </c>:confirm<c>. - hide: (boolean) </c>:hide<c>. - horizontal: (boolean) </c>:horizontal<c>. - keepalt: (boolean) </c>:keepalt<c>. - keepjumps: (boolean) </c>:keepjumps<c>. - keepmarks: (boolean) </c>:keepmarks<c>. - keeppatterns: (boolean) </c>:keeppatterns<c>. - lockmarks: (boolean) </c>:lockmarks<c>. - noswapfile: (boolean) </c>:noswapfile<c>. - tab: (integer) </c>:tab<c>. -1 when omitted. - verbose: (integer) </c>:verbose<c>. -1 when omitted. - vertical: (boolean) </c>:vertical<c>. - split: (string) Split modifier string, is an empty string when there&apos;s no split modifier. If there is a split modifier it can be one of: - &quot;aboveleft&quot;: </c>:aboveleft<c>. - &quot;belowright&quot;: </c>:belowright<c>. - &quot;topleft&quot;: </c>:topleft<c>. - &quot;botright&quot;: </c>:botright|.
    /// </description></item>
    /// </list>
    /// </description></item>
    /// </list>
    /// </returns>
    Task<IReadOnlyList<NvimMapEntry>> ParseCmdAsync(
      NvimString Str,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Executes an Ex command <c>cmd</c>, specified as a Dict with the same structure as returned by <see cref = "ParseCmdAsync"/>.
    /// </summary>
    /// <param name = "Cmd">
    /// Command to execute, a Dict with the same structure as the return value of <see cref = "ParseCmdAsync"/> (except &quot;addr&quot;, &quot;nargs&quot; and &quot;nextcmd&quot; are ignored). All keys except &quot;cmd&quot; are optional.
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters.
    /// <list type="bullet">
    /// <item><description>
    /// <c>output</c>: (boolean, default false) Whether to return command output.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Command output (non-error, non-shell <c>:!</c>) if <c>output</c> is true, else empty string.
    /// </returns>
    Task<NvimString> CmdAsync(
      IReadOnlyList<NvimMapEntry> Cmd,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Creates a global <c>user-commands</c> command.
    /// </summary>
    /// <param name = "Name">
    /// Command name. First char must be uppercase.
    /// </param>
    /// <param name = "Cmd">
    /// Command or Lua function, executed when the command is invoked. Lua function receives a table with keys:
    /// <list type="bullet">
    /// <item><description>
    /// <c>args</c>: (string) Args passed to the command, if any. [&lt;args&gt;]
    /// </description></item>
    /// <item><description>
    /// <c>bang</c>: (boolean) true if the command was executed with &quot;!&quot;. [&lt;bang&gt;]
    /// </description></item>
    /// <item><description>
    /// <c>count</c>: (number) Count, if any. [&lt;count&gt;]
    /// </description></item>
    /// <item><description>
    /// <c>fargs</c>: (table) Args split by unescaped whitespace (when more than one arg is allowed), if any. [&lt;f-args&gt;]
    /// </description></item>
    /// <item><description>
    /// <c>line1</c>: (number) Start of the command range. [&lt;line1&gt;]
    /// </description></item>
    /// <item><description>
    /// <c>line2</c>: (number) End of the command range. [&lt;line2&gt;]
    /// </description></item>
    /// <item><description>
    /// <c>mods</c>: (string) Command modifiers (unstructured string), if any. [&lt;mods&gt;]
    /// </description></item>
    /// <item><description>
    /// <c>name</c>: (string) Command name.
    /// </description></item>
    /// <item><description>
    /// <c>nargs</c>: (string) Number of arguments allowed by the command. <c>:command-nargs</c>
    /// </description></item>
    /// <item><description>
    /// <c>range</c>: (number) Number of items in the command range: 0, 1, or 2. [&lt;range&gt;]
    /// </description></item>
    /// <item><description>
    /// <c>reg</c>: (string) Register name, if any. [&lt;reg&gt;]
    /// </description></item>
    /// <item><description>
    /// <c>smods</c>: (table) Command modifiers (structured), same as &quot;mods&quot; in <see cref = "ParseCmdAsync"/>.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "Opts">
    /// Optional flags:
    /// <list type="bullet">
    /// <item><description>
    /// <c>addr</c> <c>:command-addr</c>
    /// </description></item>
    /// <item><description>
    /// <c>complete</c> <c>:command-complete</c> command or function <c>:command-completion-customlist</c>.
    /// </description></item>
    /// <item><description>
    /// <c>count</c> <c>:command-count</c>
    /// </description></item>
    /// <item><description>
    /// <c>desc</c> (string) Command description.
    /// </description></item>
    /// <item><description>
    /// <c>force</c> (boolean, default true) Override the existing definition, if any.
    /// </description></item>
    /// <item><description>
    /// <c>nargs</c> Number of arguments allowed by the command. <c>:command-nargs</c>
    /// </description></item>
    /// <item><description>
    /// <c>preview</c> (function) Preview handler for &apos;inccommand&apos;. <c>:command-preview</c>
    /// </description></item>
    /// <item><description>
    /// <c>range</c> <c>:command-range</c>
    /// </description></item>
    /// <item><description>
    /// boolean <c>command-attributes</c> such as <c>:command-bang</c> or <c>:command-bar</c> (but not <c>:command-buffer</c>, use <see cref = "BufCreateUserCommandAsync"/> instead).
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task CreateUserCommandAsync(
      NvimString Name,
      NvimValue Cmd,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Delete a user-defined command.
    /// </summary>
    /// <param name = "Name">
    /// Name of the command to delete.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task DelUserCommandAsync(
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Creates a buffer-local command <c>user-commands</c>.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer.
    /// </param>
    /// <param name = "Name">
    /// The <paramref name = "Name"/> argument.
    /// </param>
    /// <param name = "Cmd">
    /// The <paramref name = "Cmd"/> argument.
    /// </param>
    /// <param name = "Opts">
    /// The <paramref name = "Opts"/> argument.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task BufCreateUserCommandAsync(
      Buffer Buf,
      NvimString Name,
      NvimValue Cmd,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Delete a buffer-local user-defined command.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer.
    /// </param>
    /// <param name = "Name">
    /// Name of the command to delete.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task BufDelUserCommandAsync(
      Buffer Buf,
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets a map of global (non-buffer-local) Ex commands.
    /// </summary>
    /// <param name = "Opts">
    /// Optional parameters. Currently only supports {&quot;builtin&quot;:false}
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Map of maps describing commands.
    /// </returns>
    Task<IReadOnlyList<NvimMapEntry>> GetCommandsAsync(
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets a map of buffer-local <c>user-commands</c>.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters. Currently not used.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Map of maps describing commands.
    /// </returns>
    Task<IReadOnlyList<NvimMapEntry>> BufGetCommandsAsync(
      Buffer Buf,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Emitted by the TUI client to signal when a host-terminal event occurred.
    /// </summary>
    /// <param name = "Event">
    /// Event name
    /// </param>
    /// <param name = "Value">
    /// Event payload
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task UiTermEventAsync(
      NvimString Event,
      NvimValue Value,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Creates a new namespace or gets an existing one.
    /// </summary>
    /// <param name = "Name">
    /// Namespace name or empty string
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Namespace id
    /// </returns>
    Task<NvimInteger> CreateNamespaceAsync(
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets existing, non-anonymous <c>namespace</c>s.
    /// </summary>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// dict that maps from names to namespace ids.
    /// </returns>
    Task<IReadOnlyList<NvimMapEntry>> GetNamespacesAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the position (0-indexed) of an <c>extmark</c>.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "NsId">
    /// Namespace id from <see cref = "CreateNamespaceAsync"/>
    /// </param>
    /// <param name = "Id">
    /// Extmark id
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters. Keys:
    /// <list type="bullet">
    /// <item><description>
    /// <c>details</c>: Whether to include the details dict
    /// </description></item>
    /// <item><description>
    /// <c>hl_name</c>: Whether to include highlight group name instead of id, true if omitted
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// 0-indexed (row, col, details?) tuple or empty list () if extmark id was absent. The optional <c>details</c> dictionary contains the same keys as <c>opts</c> in <see cref = "BufSetExtmarkAsync"/>, except for <c>id</c>, <c>conceal_lines</c> and <c>ephemeral</c>. It also contains the following keys:
    /// </returns>
    Task<IReadOnlyList<NvimValue>> BufGetExtmarkByIdAsync(
      Buffer Buf,
      NvimInteger NsId,
      NvimInteger Id,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets <c>extmarks</c> in &quot;traversal order&quot; from a <c>charwise</c> region defined by buffer positions (inclusive, 0-indexed <c>api-indexing</c>).
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "NsId">
    /// Namespace id from <see cref = "CreateNamespaceAsync"/> or -1 for all namespaces
    /// </param>
    /// <param name = "Start">
    /// Start of range: a 0-indexed (row, col) or valid extmark id (whose position defines the bound). <c>api-indexing</c>
    /// </param>
    /// <param name = "End">
    /// End of range (inclusive): a 0-indexed (row, col) or valid extmark id (whose position defines the bound). <c>api-indexing</c>
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters. Keys:
    /// <list type="bullet">
    /// <item><description>
    /// <c>details</c>: Whether to include the details dict
    /// </description></item>
    /// <item><description>
    /// <c>hl_name</c>: Whether to include highlight group name instead of id, true if omitted
    /// </description></item>
    /// <item><description>
    /// <c>limit</c>: Maximum number of marks to return
    /// </description></item>
    /// <item><description>
    /// <c>overlap</c>: Also include marks which overlap the range, even if their start position is less than <c>start</c>
    /// </description></item>
    /// <item><description>
    /// <c>type</c>: Filter marks by type: &quot;highlight&quot;, &quot;sign&quot;, &quot;virt_text&quot; and &quot;virt_lines&quot;
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// List of <c>[extmark_id, row, col, details?]</c> tuples in &quot;traversal order&quot;. For the <c>details</c> dictionary, see <see cref = "BufGetExtmarkByIdAsync"/>.
    /// </returns>
    Task<IReadOnlyList<IReadOnlyList<NvimMapEntry>>> BufGetExtmarksAsync(
      Buffer Buf,
      NvimInteger NsId,
      NvimValue Start,
      NvimValue End,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Creates or updates an <c>extmark</c>.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "NsId">
    /// Namespace id from <see cref = "CreateNamespaceAsync"/>
    /// </param>
    /// <param name = "Line">
    /// Line where to place the mark, 0-based. <c>api-indexing</c>
    /// </param>
    /// <param name = "Col">
    /// Column where to place the mark, 0-based. <c>api-indexing</c>
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters.
    /// <list type="bullet">
    /// <item><description>
    /// <c>conceal</c>: string which should be either empty or a single character. Enable concealing similar to <c>:syn-conceal</c>. When a character is supplied it is used as <c>:syn-cchar</c>. &quot;hl_group&quot; is used as highlight for the cchar if provided, otherwise it defaults to <c>hl-Conceal</c>.
    /// </description></item>
    /// <item><description>
    /// <c>conceal_lines</c>: string which should be empty. When provided, lines in the range are not drawn at all (according to &apos;conceallevel&apos;); the next unconcealed line is drawn instead.
    /// </description></item>
    /// <item><description>
    /// <c>cursorline_hl_group</c>: highlight group used for the sign column text when the cursor is on the same line as the mark and &apos;cursorline&apos; is enabled.
    /// </description></item>
    /// <item><description>
    /// end_col : ending col of the mark, 0-based exclusive, or -1 to extend the range to end of line (if strict=false).
    /// </description></item>
    /// <item><description>
    /// end_right_gravity : boolean that indicates the direction the extmark end position (if it exists) will be shifted in when new text is inserted (true for right, false for left). Defaults to false.
    /// </description></item>
    /// <item><description>
    /// end_row : ending line of the mark, 0-based inclusive.
    /// </description></item>
    /// <item><description>
    /// ephemeral : for use with <see cref = "SetDecorationProviderAsync"/> callbacks. The mark will only be used for the current redraw cycle, and not be permanently stored in the buffer.
    /// </description></item>
    /// <item><description>
    /// hl_eol : when true, for a multiline highlight covering the EOL of a line, continue the highlight for the rest of the screen line (just like for diff and cursorline highlight).
    /// </description></item>
    /// <item><description>
    /// hl_group : highlight group used for the text range. This and below highlight groups can be supplied either as a string or as an integer, the latter of which can be obtained using <see cref = "GetHlIdByNameAsync"/>.
    ///
    /// Multiple highlight groups can be stacked by passing an array (highest priority last).
    /// </description></item>
    /// <item><description>
    /// hl_mode : control how highlights are combined with the highlights of the text. Currently only affects virt_text highlights, but might affect <c>hl_group</c> in later versions.
    /// <list type="bullet">
    /// <item><description>
    /// &quot;replace&quot;: only show the virt_text color. This is the default.
    /// </description></item>
    /// <item><description>
    /// &quot;combine&quot;: combine with background text color.
    /// </description></item>
    /// <item><description>
    /// &quot;blend&quot;: blend with background text color. Not supported for &quot;inline&quot; virt_text.
    /// </description></item>
    /// </list>
    /// </description></item>
    /// <item><description>
    /// id : id of the extmark to edit.
    /// </description></item>
    /// <item><description>
    /// invalidate : boolean that indicates whether to hide the extmark if the entirety of its range is deleted. For hidden marks, an &quot;invalid&quot; key is added to the &quot;details&quot; array of <see cref = "BufGetExtmarksAsync"/> and family. If &quot;undo_restore&quot; is false, the extmark is deleted instead.
    /// </description></item>
    /// <item><description>
    /// <c>line_hl_group</c>: highlight group used for the whole line.
    /// </description></item>
    /// <item><description>
    /// <c>number_hl_group</c>: highlight group used for the number column.
    /// </description></item>
    /// <item><description>
    /// <c>priority</c>: a priority value for the highlight group, sign attribute or virtual text. For virtual text, item with highest priority is drawn last. For example treesitter highlighting uses a value of 100.
    /// </description></item>
    /// <item><description>
    /// right_gravity : boolean that indicates the direction the extmark will be shifted in when new text is inserted (true for right, false for left). Defaults to true.
    /// </description></item>
    /// <item><description>
    /// <c>sign_hl_group</c>: highlight group used for the sign column text.
    /// </description></item>
    /// <item><description>
    /// <c>sign_text</c>: string of length 1-2 used to display in the sign column.
    /// </description></item>
    /// <item><description>
    /// <c>spell</c>: boolean indicating that spell checking should be performed within this extmark
    /// </description></item>
    /// <item><description>
    /// <c>strict</c>: boolean that indicates extmark should not be placed if the line or column value is past the end of the buffer or end of the line respectively. Defaults to true.
    /// </description></item>
    /// <item><description>
    /// <c>ui_watched</c>: boolean that indicates the mark should be drawn by a UI. When set, the UI will receive win_extmark events. Note: the mark is positioned by virt_text attributes. Can be used together with virt_text.
    /// </description></item>
    /// <item><description>
    /// undo_restore : Restore the exact position of the mark if text around the mark was deleted and then restored by undo. Defaults to true.
    /// </description></item>
    /// <item><description>
    /// <c>url</c>: A URL to associate with this extmark. In the TUI, the OSC 8 control sequence is used to generate a clickable hyperlink to this URL.
    /// </description></item>
    /// <item><description>
    /// virt_lines : virtual lines to add next to this mark This should be an array over lines, where each line in turn is an array over <c>[text, highlight]</c> tuples. In general, buffer and window options do not affect the display of the text. In particular &apos;wrap&apos; and &apos;linebreak&apos; options do not take effect, so the number of extra screen lines will always match the size of the array. However the &apos;tabstop&apos; buffer option is still used for hard tabs. By default lines are placed below the buffer line containing the mark.
    /// </description></item>
    /// <item><description>
    /// <c>virt_lines_above</c>: place virtual lines above instead.
    /// </description></item>
    /// <item><description>
    /// <c>virt_lines_leftcol</c>: Place virtual lines in the leftmost column of the window, bypassing sign and number columns.
    /// </description></item>
    /// <item><description>
    /// <c>virt_lines_overflow</c>: controls how to handle virtual lines wider than the window. Currently takes the one of the following values:
    /// <list type="bullet">
    /// <item><description>
    /// &quot;trunc&quot;: truncate virtual lines on the right (default).
    /// </description></item>
    /// <item><description>
    /// &quot;scroll&quot;: virtual lines can scroll horizontally with &apos;nowrap&apos;, otherwise the same as &quot;trunc&quot;.
    /// </description></item>
    /// </list>
    /// </description></item>
    /// <item><description>
    /// virt_text : [](virtual-text) to link to this mark. A list of <c>[text, highlight]</c> tuples, each representing a text chunk with specified highlight. <c>highlight</c> element can either be a single highlight group, or an array of multiple highlight groups that will be stacked (highest priority last).
    /// </description></item>
    /// <item><description>
    /// virt_text_hide : hide the virtual text when the background text is selected or hidden because of scrolling with &apos;nowrap&apos; or &apos;smoothscroll&apos;. Currently only affects &quot;overlay&quot; virt_text.
    /// </description></item>
    /// <item><description>
    /// virt_text_pos : position of virtual text. Possible values:
    /// <list type="bullet">
    /// <item><description>
    /// &quot;eol&quot;: right after eol character (default).
    /// </description></item>
    /// <item><description>
    /// &quot;eol_right_align&quot;: display right aligned in the window unless the virtual text is longer than the space available. If the virtual text is too long, it is truncated to fit in the window after the EOL character. If the line is wrapped, the virtual text is shown after the end of the line rather than the previous screen line.
    /// </description></item>
    /// <item><description>
    /// &quot;overlay&quot;: display over the specified column, without shifting the underlying text.
    /// </description></item>
    /// <item><description>
    /// &quot;right_align&quot;: display right aligned in the window.
    /// </description></item>
    /// <item><description>
    /// &quot;inline&quot;: display at the specified column, and shift the buffer text to the right as needed.
    /// </description></item>
    /// </list>
    /// </description></item>
    /// <item><description>
    /// virt_text_repeat_linebreak : repeat the virtual text on wrapped lines.
    /// </description></item>
    /// <item><description>
    /// virt_text_win_col : position the virtual text at a fixed window column (starting from the first text column of the screen line) instead of &quot;virt_text_pos&quot;.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Id of the created/updated extmark
    /// </returns>
    Task<NvimInteger> BufSetExtmarkAsync(
      Buffer Buf,
      NvimInteger NsId,
      NvimInteger Line,
      NvimInteger Col,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Removes an <c>extmark</c>.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "NsId">
    /// Namespace id from <see cref = "CreateNamespaceAsync"/>
    /// </param>
    /// <param name = "Id">
    /// Extmark id
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// true if the extmark was found, else false
    /// </returns>
    Task<NvimBoolean> BufDelExtmarkAsync(
      Buffer Buf,
      NvimInteger NsId,
      NvimInteger Id,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Clears <c>namespace</c>d objects (highlights, <c>extmarks</c>, virtual text) from a region.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id, or 0 for current buffer
    /// </param>
    /// <param name = "NsId">
    /// Namespace to clear, or -1 to clear all namespaces.
    /// </param>
    /// <param name = "LineStart">
    /// Start of range of lines to clear
    /// </param>
    /// <param name = "LineEnd">
    /// End of range of lines to clear (exclusive) or -1 to clear to end of buffer.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task BufClearNamespaceAsync(
      Buffer Buf,
      NvimInteger NsId,
      NvimInteger LineStart,
      NvimInteger LineEnd,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Set or change decoration provider for a <c>namespace</c>.
    /// </summary>
    /// <param name = "NsId">
    /// Namespace id from <see cref = "CreateNamespaceAsync"/>
    /// </param>
    /// <param name = "Opts">
    /// Table of callbacks:
    /// <list type="bullet">
    /// <item><description>
    /// <c>on_buf</c>: called for each buffer being redrawn (once per edit, before window callbacks) <c>[&quot;buf&quot;, bufnr, tick]</c>
    /// </description></item>
    /// <item><description>
    /// <c>on_end</c>: called at the end of a redraw cycle <c>[&quot;end&quot;, tick]</c>
    /// </description></item>
    /// <item><description>
    /// <c>on_line</c>: (deprecated, use on_range instead) <c>[&quot;line&quot;, winid, bufnr, row]</c>
    /// </description></item>
    /// <item><description>
    /// <c>on_range</c>: called for each buffer range being redrawn. Range is end-exclusive and may span multiple lines. Range bounds point to the first byte of a character. An end position of the form (lnum, 0), including (number of lines, 0), is valid and indicates that EOL of the preceding line is included. <c>[&quot;range&quot;, winid, bufnr, begin_row, begin_col, end_row, end_col]</c>
    ///
    /// In addition to returning a boolean, it is also allowed to return a <c>skip_row, skip_col</c> pair of integers. This implies that this function does not need to be called until a range which continues beyond the skipped position. A single integer return value <c>skip_row</c> is short for <c>skip_row, 0</c>
    /// </description></item>
    /// <item><description>
    /// <c>on_start</c>: called first on each screen redraw <c>[&quot;start&quot;, tick]</c>
    /// </description></item>
    /// <item><description>
    /// <c>on_win</c>: called when starting to redraw a specific window. <c>[&quot;win&quot;, winid, bufnr, toprow, botrow]</c>
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task SetDecorationProviderAsync(
      NvimInteger NsId,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the value of an option.
    /// </summary>
    /// <param name = "Name">
    /// Option name
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters
    /// <list type="bullet">
    /// <item><description>
    /// <c>buf</c>: Buffer number. Used for getting buffer local options. Implies <c>scope</c> is &quot;local&quot;.
    /// </description></item>
    /// <item><description>
    /// <c>filetype</c>: <c>filetype</c>. Used to get the default option for a specific filetype. Cannot be used with any other option. Note: this will trigger <c>ftplugin</c> and all <c>FileType</c> autocommands for the corresponding filetype.
    /// </description></item>
    /// <item><description>
    /// <c>scope</c>: One of &quot;global&quot; or &quot;local&quot;. Analogous to <c>:setglobal</c> and <c>:setlocal</c>, respectively.
    /// </description></item>
    /// <item><description>
    /// <c>win</c>: <c>window-ID</c>. Used for getting window local options.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Option value
    /// </returns>
    Task<NvimValue> GetOptionValueAsync(
      NvimString Name,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets the value of an option.
    /// </summary>
    /// <param name = "Name">
    /// Option name
    /// </param>
    /// <param name = "Value">
    /// New option value
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters
    /// <list type="bullet">
    /// <item><description>
    /// <c>buf</c>: Buffer number. Used for setting buffer local option.
    /// </description></item>
    /// <item><description>
    /// <c>scope</c>: One of &quot;global&quot; or &quot;local&quot;. Analogous to <c>:setglobal</c> and <c>:setlocal</c>, respectively.
    /// </description></item>
    /// <item><description>
    /// <c>win</c>: <c>window-ID</c>. Used for setting window local option.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task SetOptionValueAsync(
      NvimString Name,
      NvimValue Value,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the option information for all options.
    /// </summary>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// dict of all options
    /// </returns>
    Task<IReadOnlyList<NvimMapEntry>> GetAllOptionsInfoAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the option information for one option from arbitrary buffer or window.
    /// </summary>
    /// <param name = "Name">
    /// Option name
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters
    /// <list type="bullet">
    /// <item><description>
    /// <c>buf</c>: Buffer number. Used for getting buffer local options. Implies <c>scope</c> is &quot;local&quot;.
    /// </description></item>
    /// <item><description>
    /// <c>scope</c>: One of &quot;global&quot; or &quot;local&quot;. Analogous to <c>:setglobal</c> and <c>:setlocal</c>, respectively.
    /// </description></item>
    /// <item><description>
    /// <c>win</c>: <c>window-ID</c>. Used for getting window local options.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Option Information
    /// </returns>
    Task<IReadOnlyList<NvimMapEntry>> GetOptionInfo2Async(
      NvimString Name,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the windows in a tabpage.
    /// </summary>
    /// <param name = "Tabpage">
    /// <c>tab-ID</c>, or 0 for current tabpage
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// List of windows in <c>tabpage</c>
    /// </returns>
    Task<IReadOnlyList<Window>> TabpageListWinsAsync(
      Tabpage Tabpage,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets a tab-scoped (t:) variable.
    /// </summary>
    /// <param name = "Tabpage">
    /// <c>tab-ID</c>, or 0 for current tabpage
    /// </param>
    /// <param name = "Name">
    /// Variable name
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Variable value
    /// </returns>
    Task<NvimValue> TabpageGetVarAsync(
      Tabpage Tabpage,
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets a tab-scoped (t:) variable.
    /// </summary>
    /// <param name = "Tabpage">
    /// <c>tab-ID</c>, or 0 for current tabpage
    /// </param>
    /// <param name = "Name">
    /// Variable name
    /// </param>
    /// <param name = "Value">
    /// Variable value
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task TabpageSetVarAsync(
      Tabpage Tabpage,
      NvimString Name,
      NvimValue Value,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Removes a tab-scoped (t:) variable.
    /// </summary>
    /// <param name = "Tabpage">
    /// <c>tab-ID</c>, or 0 for current tabpage
    /// </param>
    /// <param name = "Name">
    /// Variable name
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task TabpageDelVarAsync(
      Tabpage Tabpage,
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the current window in a tabpage.
    /// </summary>
    /// <param name = "Tabpage">
    /// <c>tab-ID</c>, or 0 for current tabpage
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// <c>window-ID</c>
    /// </returns>
    Task<Window> TabpageGetWinAsync(
      Tabpage Tabpage,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets the current window in a tabpage.
    /// </summary>
    /// <param name = "Tabpage">
    /// <c>tab-ID</c>, or 0 for current tabpage
    /// </param>
    /// <param name = "Win">
    /// <c>window-ID</c>, must already belong to <paramref name = "Tabpage"/>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task TabpageSetWinAsync(
      Tabpage Tabpage,
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the tabpage number.
    /// </summary>
    /// <param name = "Tabpage">
    /// <c>tab-ID</c>, or 0 for current tabpage
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Tabpage number
    /// </returns>
    Task<NvimInteger> TabpageGetNumberAsync(
      Tabpage Tabpage,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Checks if a tabpage is valid.
    /// </summary>
    /// <param name = "Tabpage">
    /// <c>tab-ID</c>, or 0 for current tabpage
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// true if the tabpage is valid, false otherwise
    /// </returns>
    Task<NvimBoolean> TabpageIsValidAsync(
      Tabpage Tabpage,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Opens a new tabpage.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer to open in the first window of the new tabpage. Use 0 for current buffer.
    /// </param>
    /// <param name = "Enter">
    /// Enter the tabpage (make it the current tabpage).
    /// </param>
    /// <param name = "Config">
    /// Configuration for the new tabpage. Keys:
    /// <list type="bullet">
    /// <item><description>
    /// <c>after</c>: Position to insert tabpage (default: -1; after current). 0 = first, N = after Nth.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// <c>tab-ID</c> of the new tabpage
    /// </returns>
    Task<Tabpage> OpenTabpageAsync(
      Buffer Buf,
      NvimBoolean Enter,
      IReadOnlyList<NvimMapEntry> Config,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Activates UI events on the channel.
    /// </summary>
    /// <param name = "Width">
    /// Requested screen columns
    /// </param>
    /// <param name = "Height">
    /// Requested screen rows
    /// </param>
    /// <param name = "Options">
    /// <c>ui-option</c> map
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task UiAttachAsync(
      NvimInteger Width,
      NvimInteger Height,
      IReadOnlyList<NvimMapEntry> Options,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Tells the nvim server if focus was gained or lost by the GUI.
    /// </summary>
    /// <param name = "Gained">
    /// The <paramref name = "Gained"/> argument.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task UiSetFocusAsync(
      NvimBoolean Gained,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Deactivates UI events on the channel.
    /// </summary>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task UiDetachAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Invokes Neovim RPC method <c>nvim_ui_try_resize</c>.
    /// </summary>
    /// <param name = "Width">
    /// The <paramref name = "Width"/> argument.
    /// </param>
    /// <param name = "Height">
    /// The <paramref name = "Height"/> argument.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task UiTryResizeAsync(
      NvimInteger Width,
      NvimInteger Height,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Invokes Neovim RPC method <c>nvim_ui_set_option</c>.
    /// </summary>
    /// <param name = "Name">
    /// The <paramref name = "Name"/> argument.
    /// </param>
    /// <param name = "Value">
    /// The <paramref name = "Value"/> argument.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task UiSetOptionAsync(
      NvimString Name,
      NvimValue Value,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Tell Nvim to resize a grid.
    /// </summary>
    /// <param name = "Grid">
    /// The handle of the grid to be changed.
    /// </param>
    /// <param name = "Width">
    /// The new requested width.
    /// </param>
    /// <param name = "Height">
    /// The new requested height.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task UiTryResizeGridAsync(
      NvimInteger Grid,
      NvimInteger Width,
      NvimInteger Height,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Tells Nvim the number of elements displaying in the popupmenu, to decide [&lt;PageUp&gt;] and [&lt;PageDown&gt;] movement.
    /// </summary>
    /// <param name = "Height">
    /// Popupmenu height, must be greater than zero.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task UiPumSetHeightAsync(
      NvimInteger Height,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Tells Nvim the geometry of the popupmenu, to align floating windows with an external popup menu.
    /// </summary>
    /// <param name = "Width">
    /// Popupmenu width.
    /// </param>
    /// <param name = "Height">
    /// Popupmenu height.
    /// </param>
    /// <param name = "Row">
    /// Popupmenu row.
    /// </param>
    /// <param name = "Col">
    /// Popupmenu height.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task UiPumSetBoundsAsync(
      NvimFloat Width,
      NvimFloat Height,
      NvimFloat Row,
      NvimFloat Col,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sends arbitrary data to a UI.
    /// </summary>
    /// <param name = "Content">
    /// Content to write to the TTY
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task UiSendAsync(
      NvimString Content,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets a highlight group by name.
    /// </summary>
    /// <param name = "Name">
    /// The <paramref name = "Name"/> argument.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// The Neovim RPC result.
    /// </returns>
    Task<NvimInteger> GetHlIdByNameAsync(
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets all or specific highlight groups in a namespace.
    /// </summary>
    /// <param name = "NsId">
    /// Get highlight groups for namespace ns_id <see cref = "GetNamespacesAsync"/>. Use 0 to get global highlight groups <c>:highlight</c>.
    /// </param>
    /// <param name = "Opts">
    /// Options dict:
    /// <list type="bullet">
    /// <item><description>
    /// <c>create</c>: (boolean, default true) When highlight group doesn&apos;t exist create it.
    /// </description></item>
    /// <item><description>
    /// <c>id</c>: (integer) Get a highlight definition by id.
    /// </description></item>
    /// <item><description>
    /// <c>link</c>: (boolean, default true) Show linked group name instead of effective definition <c>:hi-link</c>.
    /// </description></item>
    /// <item><description>
    /// <c>name</c>: (string) Get a highlight definition by name.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Highlight groups as a map from group name to a highlight definition map as in <see cref = "SetHlAsync"/>, or only a single highlight definition map if requested by name or id.
    /// </returns>
    Task<IReadOnlyList<NvimMapEntry>> GetHlAsync(
      NvimInteger NsId,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets a highlight group.
    /// </summary>
    /// <param name = "NsId">
    /// Namespace id for this highlight <see cref = "CreateNamespaceAsync"/>. Use 0 to set a highlight group globally <c>:highlight</c>. Highlights from non-global namespaces are not active by default, use <see cref = "SetHlNsAsync"/> or <see cref = "WinSetHlNsAsync"/> to activate them.
    /// </param>
    /// <param name = "Name">
    /// Highlight group name, e.g. &quot;ErrorMsg&quot;
    /// </param>
    /// <param name = "Val">
    /// Highlight definition map, accepts the following keys:
    /// <list type="bullet">
    /// <item><description>
    /// <c>altfont</c>: boolean
    /// </description></item>
    /// <item><description>
    /// <c>bg</c>: color name or &quot;#RRGGBB&quot;, see note.
    /// </description></item>
    /// <item><description>
    /// <c>bg_indexed</c>: boolean. If true, <c>bg</c> is an RGB approximation of <c>ctermbg</c> (a palette index). UIs rendering cterm natively may prefer <c>ctermbg</c>.
    /// </description></item>
    /// <item><description>
    /// <c>blend</c>: integer between 0 and 100
    /// </description></item>
    /// <item><description>
    /// <c>blink</c>: boolean
    /// </description></item>
    /// <item><description>
    /// <c>bold</c>: boolean
    /// </description></item>
    /// <item><description>
    /// <c>conceal</c>: boolean Concealment at the UI level (terminal SGR), unrelated to <c>:syn-conceal</c>.
    /// </description></item>
    /// <item><description>
    /// <c>cterm</c>: cterm attribute map, like <c>highlight-args</c>. If not set, cterm attributes will match those from the attribute map documented above.
    /// </description></item>
    /// <item><description>
    /// <c>ctermbg</c>: Sets background of cterm color <c>ctermbg</c>
    /// </description></item>
    /// <item><description>
    /// <c>ctermfg</c>: Sets foreground of cterm color <c>ctermfg</c>
    /// </description></item>
    /// <item><description>
    /// <c>default</c>: boolean Don&apos;t override existing definition <c>:hi-default</c>
    /// </description></item>
    /// <item><description>
    /// <c>dim</c>: boolean
    /// </description></item>
    /// <item><description>
    /// <c>fg</c>: Color name or &quot;#RRGGBB&quot;, see note.
    /// </description></item>
    /// <item><description>
    /// <c>fg_indexed</c>: boolean. Same as <c>bg_indexed</c>, for <c>fg</c> and <c>ctermfg</c>.
    /// </description></item>
    /// <item><description>
    /// <c>force</c>: boolean (default false) Update the highlight group even if it already exists.
    /// </description></item>
    /// <item><description>
    /// <c>italic</c>: boolean
    /// </description></item>
    /// <item><description>
    /// <c>link</c>: Name of highlight group to link to. <c>:hi-link</c>
    /// </description></item>
    /// <item><description>
    /// <c>link_global</c>: Like &quot;link&quot;, but always resolved in the global namespace (ns=0).
    /// </description></item>
    /// <item><description>
    /// <c>nocombine</c>: boolean
    /// </description></item>
    /// <item><description>
    /// <c>overline</c>: boolean
    /// </description></item>
    /// <item><description>
    /// <c>reverse</c>: boolean
    /// </description></item>
    /// <item><description>
    /// <c>sp</c>: Color name or &quot;#RRGGBB&quot;
    /// </description></item>
    /// <item><description>
    /// <c>standout</c>: boolean
    /// </description></item>
    /// <item><description>
    /// <c>strikethrough</c>: boolean
    /// </description></item>
    /// <item><description>
    /// <c>undercurl</c>: boolean
    /// </description></item>
    /// <item><description>
    /// <c>underdashed</c>: boolean
    /// </description></item>
    /// <item><description>
    /// <c>underdotted</c>: boolean
    /// </description></item>
    /// <item><description>
    /// <c>underdouble</c>: boolean
    /// </description></item>
    /// <item><description>
    /// <c>underline</c>: boolean
    /// </description></item>
    /// <item><description>
    /// <c>update</c>: boolean (default false) Update specified attributes only, leave others unchanged.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task SetHlAsync(
      NvimInteger NsId,
      NvimString Name,
      IReadOnlyList<NvimMapEntry> Val,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the active highlight namespace.
    /// </summary>
    /// <param name = "Opts">
    /// Optional parameters
    /// <list type="bullet">
    /// <item><description>
    /// <c>winid</c>: (number) <c>window-ID</c> for retrieving a window&apos;s highlight namespace. A value of -1 is returned when <see cref = "WinSetHlNsAsync"/> has not been called for the window (or was called with a namespace of -1).
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Namespace id, or -1
    /// </returns>
    Task<NvimInteger> GetHlNsAsync(
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Set active namespace for highlights defined with <see cref = "SetHlAsync"/>.
    /// </summary>
    /// <param name = "NsId">
    /// the namespace to use
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task SetHlNsAsync(
      NvimInteger NsId,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Set active namespace for highlights defined with <see cref = "SetHlAsync"/> while redrawing.
    /// </summary>
    /// <param name = "NsId">
    /// the namespace to activate
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task SetHlNsFastAsync(
      NvimInteger NsId,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sends input-keys to Nvim, subject to various quirks controlled by <c>mode</c> flags.
    /// </summary>
    /// <param name = "Keys">
    /// to be typed
    /// </param>
    /// <param name = "Mode">
    /// behavior flags, see <c>feedkeys()</c>
    /// </param>
    /// <param name = "EscapeKs">
    /// If true, escape K_SPECIAL bytes in <c>keys</c>. This should be false if you already used <see cref = "ReplaceTermcodesAsync"/>, and true otherwise.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task FeedkeysAsync(
      NvimString Keys,
      NvimString Mode,
      NvimBoolean EscapeKs,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Queues raw user-input.
    /// </summary>
    /// <param name = "Keys">
    /// to be typed
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Number of bytes actually written (can be fewer than requested if the buffer becomes full).
    /// </returns>
    Task<NvimInteger> InputAsync(
      NvimString Keys,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Send mouse event from GUI.
    /// </summary>
    /// <param name = "Button">
    /// Mouse button: one of &quot;left&quot;, &quot;right&quot;, &quot;middle&quot;, &quot;wheel&quot;, &quot;move&quot;, &quot;x1&quot;, &quot;x2&quot;.
    /// </param>
    /// <param name = "Action">
    /// For ordinary buttons, one of &quot;press&quot;, &quot;drag&quot;, &quot;release&quot;. For the wheel, one of &quot;up&quot;, &quot;down&quot;, &quot;left&quot;, &quot;right&quot;. Ignored for &quot;move&quot;.
    /// </param>
    /// <param name = "Modifier">
    /// String of modifiers each represented by a single char. The same specifiers are used as for a key press, except that the &quot;-&quot; separator is optional, so &quot;C-A-&quot;, &quot;c-a&quot; and &quot;CA&quot; can all be used to specify Ctrl+Alt+click.
    /// </param>
    /// <param name = "Grid">
    /// Grid number (used by <c>ui-multigrid</c> client), or 0 to let Nvim decide positioning of windows. For more information, see [dev-ui-multigrid]
    /// </param>
    /// <param name = "Row">
    /// Mouse row-position (zero-based, like redraw events)
    /// </param>
    /// <param name = "Col">
    /// Mouse column-position (zero-based, like redraw events)
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task InputMouseAsync(
      NvimString Button,
      NvimString Action,
      NvimString Modifier,
      NvimInteger Grid,
      NvimInteger Row,
      NvimInteger Col,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Replaces terminal codes and <c>keycodes</c> ([&lt;CR&gt;], [&lt;Esc&gt;], ...) in a string with the internal representation.
    /// </summary>
    /// <param name = "Str">
    /// String to be converted.
    /// </param>
    /// <param name = "FromPart">
    /// Legacy Vim parameter. Usually true.
    /// </param>
    /// <param name = "DoLt">
    /// Also translate [&lt;lt&gt;]. Ignored if <c>special</c> is false.
    /// </param>
    /// <param name = "Special">
    /// Replace <c>keycodes</c>, e.g. [&lt;CR&gt;] becomes a &quot;\r&quot; char.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// The Neovim RPC result.
    /// </returns>
    Task<NvimString> ReplaceTermcodesAsync(
      NvimString Str,
      NvimBoolean FromPart,
      NvimBoolean DoLt,
      NvimBoolean Special,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Executes Lua code.
    /// </summary>
    /// <param name = "Code">
    /// Lua code to execute.
    /// </param>
    /// <param name = "Args">
    /// Arguments to the Lua code.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Value returned by the Lua code (if any), or NIL.
    /// </returns>
    Task<NvimValue> ExecLuaAsync(
      NvimString Code,
      IReadOnlyList<NvimValue> Args,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Calculates the number of display cells occupied by <c>text</c>.
    /// </summary>
    /// <param name = "Text">
    /// Some text
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Number of cells
    /// </returns>
    Task<NvimInteger> StrwidthAsync(
      NvimString Text,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the paths contained in <c>runtime-search-path</c>.
    /// </summary>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// List of paths
    /// </returns>
    Task<IReadOnlyList<NvimString>> ListRuntimePathsAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Finds files in runtime directories, in &apos;runtimepath&apos; order.
    /// </summary>
    /// <param name = "Name">
    /// pattern of files to search for
    /// </param>
    /// <param name = "All">
    /// whether to return all matches or only the first
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// list of absolute paths to the found files
    /// </returns>
    Task<IReadOnlyList<NvimString>> GetRuntimeFileAsync(
      NvimString Name,
      NvimBoolean All,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Changes the global working directory.
    /// </summary>
    /// <param name = "Dir">
    /// Directory path
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task SetCurrentDirAsync(
      NvimString Dir,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the current line.
    /// </summary>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Current line string
    /// </returns>
    Task<NvimString> GetCurrentLineAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets the text on the current line.
    /// </summary>
    /// <param name = "Line">
    /// Line contents
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task SetCurrentLineAsync(
      NvimString Line,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Deletes the current line.
    /// </summary>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task DelCurrentLineAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets a global (g:) variable.
    /// </summary>
    /// <param name = "Name">
    /// Variable name
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Variable value
    /// </returns>
    Task<NvimValue> GetVarAsync(
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets a global (g:) variable.
    /// </summary>
    /// <param name = "Name">
    /// Variable name
    /// </param>
    /// <param name = "Value">
    /// Variable value
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task SetVarAsync(
      NvimString Name,
      NvimValue Value,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Removes a global (g:) variable.
    /// </summary>
    /// <param name = "Name">
    /// Variable name
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task DelVarAsync(
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets a v: variable.
    /// </summary>
    /// <param name = "Name">
    /// Variable name
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Variable value
    /// </returns>
    Task<NvimValue> GetVvarAsync(
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets a v: variable, if it is not readonly.
    /// </summary>
    /// <param name = "Name">
    /// Variable name
    /// </param>
    /// <param name = "Value">
    /// Variable value
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task SetVvarAsync(
      NvimString Name,
      NvimValue Value,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Prints a message given by a list of <c>[text, hl_group]</c> &quot;chunks&quot;.
    /// </summary>
    /// <param name = "Chunks">
    /// List of <c>[text, hl_group]</c> pairs, where each is a <c>text</c> string highlighted by the (optional) name or ID <c>hl_group</c>.
    /// </param>
    /// <param name = "History">
    /// if true, add to <c>message-history</c>.
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters.
    /// <list type="bullet">
    /// <item><description>
    /// <c>data</c> (<c>table?</c>) Dict of arbitrary data, available in <c>Progress</c> <c>event-data</c>.
    /// </description></item>
    /// <item><description>
    /// <c>err</c> (<c>boolean?</c>) Treat the message like <c>:echoerr</c>. Sets <c>hl_group</c> to <c>hl-ErrorMsg</c> by default.
    /// </description></item>
    /// <item><description>
    /// <c>id</c> (<c>integer|string?</c>) Message-id returned by a previous <c>nvim_echo</c> call, or a user-defined id (string). If existing message has this id, it will be updated instead of creating a new message.
    /// </description></item>
    /// <item><description>
    /// <c>kind</c> (<c>string?</c>) Decides the <c>ui-messages</c> kind in the emitted message. Set &quot;progress&quot; to emit a <c>progress-message</c>.
    /// </description></item>
    /// <item><description>
    /// <c>percent</c> (<c>integer?</c>) <c>progress-message</c> percentage.
    /// </description></item>
    /// <item><description>
    /// <c>source</c> (<c>string?</c>) <c>progress-message</c> source.
    /// </description></item>
    /// <item><description>
    /// <c>status</c> (<c>string?</c>) <c>progress-message</c> status:
    /// <list type="bullet">
    /// <item><description>
    /// &quot;success&quot;: Process completed successfully.
    /// </description></item>
    /// <item><description>
    /// &quot;running&quot;: Process is ongoing.
    /// </description></item>
    /// <item><description>
    /// &quot;failed&quot;: Process failed.
    /// </description></item>
    /// <item><description>
    /// &quot;cancel&quot;: Process should be cancelled. Progress owner must handle the <c>Progress</c> event to perform the cancellation.
    /// </description></item>
    /// </list>
    /// </description></item>
    /// <item><description>
    /// <c>title</c> (<c>string?</c>) Message title. Only for <c>progress-message</c> currently.
    /// </description></item>
    /// <item><description>
    /// <c>verbose</c> (<c>boolean?</c>) Message is controlled by the &apos;verbose&apos; option. <c>nvim -V3log</c> will write the message to the &quot;log&quot; file instead of standard output.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Message-id, or -1 if message wasn&apos;t shown.
    /// </returns>
    Task<NvimValue> EchoAsync(
      IReadOnlyList<IReadOnlyList<NvimValue>> Chunks,
      NvimBoolean History,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the current list of buffers.
    /// </summary>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// List of buffer ids
    /// </returns>
    Task<IReadOnlyList<Buffer>> ListBufsAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the current buffer.
    /// </summary>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Buffer id
    /// </returns>
    Task<Buffer> GetCurrentBufAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets the current window&apos;s buffer to <c>buf</c>.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer id
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task SetCurrentBufAsync(
      Buffer Buf,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the current list of all <c>window-ID</c>s in all tabpages.
    /// </summary>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// List of <c>window-ID</c>s
    /// </returns>
    Task<IReadOnlyList<Window>> ListWinsAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the current window.
    /// </summary>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// <c>window-ID</c>
    /// </returns>
    Task<Window> GetCurrentWinAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Navigates to the given window (and tabpage, implicitly).
    /// </summary>
    /// <param name = "Win">
    /// <c>window-ID</c> to focus
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task SetCurrentWinAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Creates a new, empty, unnamed buffer.
    /// </summary>
    /// <param name = "Listed">
    /// Sets &apos;buflisted&apos;
    /// </param>
    /// <param name = "Scratch">
    /// Creates a &quot;throwaway&quot; <c>scratch-buffer</c> for temporary work (always &apos;nomodified&apos;). Also sets &apos;nomodeline&apos; on the buffer.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Buffer id, or 0 on error
    /// </returns>
    Task<Buffer> CreateBufAsync(
      NvimBoolean Listed,
      NvimBoolean Scratch,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Open a terminal instance in a buffer.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer to use. Buffer contents (if any) will be written to the PTY.
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters.
    /// <list type="bullet">
    /// <item><description>
    /// <c>force_crlf</c>: (boolean, default true) Convert &quot;\n&quot; to &quot;\r\n&quot;.
    /// </description></item>
    /// <item><description>
    /// <c>on_input</c>: Lua callback for input sent, i e keypresses in terminal mode. Note: keypresses are sent raw as they would be to the pty master end. For instance, a carriage return is sent as a &quot;\r&quot;, not as a &quot;\n&quot;. <c>textlock</c> applies. It is possible to call <see cref = "ChanSendAsync"/> directly in the callback however. <c>[&quot;input&quot;, term, bufnr, data]</c>
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Channel id, or 0 on error
    /// </returns>
    Task<NvimInteger> OpenTermAsync(
      Buffer Buf,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sends raw data to channel <c>chan</c>.
    /// </summary>
    /// <param name = "Chan">
    /// Channel id
    /// </param>
    /// <param name = "Data">
    /// Data to write. 8-bit clean: may contain NUL bytes.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task ChanSendAsync(
      NvimInteger Chan,
      NvimString Data,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the current list of <c>tab-ID</c>s.
    /// </summary>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// List of <c>tab-ID</c>s
    /// </returns>
    Task<IReadOnlyList<Tabpage>> ListTabpagesAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the current tabpage.
    /// </summary>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// <c>tab-ID</c>
    /// </returns>
    Task<Tabpage> GetCurrentTabpageAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets the current tabpage.
    /// </summary>
    /// <param name = "Tabpage">
    /// <c>tab-ID</c> to focus
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task SetCurrentTabpageAsync(
      Tabpage Tabpage,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Pastes at cursor (in any mode), and sets &quot;redo&quot; so dot (|.
    /// </summary>
    /// <param name = "Data">
    /// Multiline input. Lines break at LF (&quot;\n&quot;). May be binary (containing NUL bytes).
    /// </param>
    /// <param name = "Crlf">
    /// Also break lines at CR and CRLF.
    /// </param>
    /// <param name = "Phase">
    /// -1: paste in a single call (i.e. without streaming). To &quot;stream&quot; a paste, call <c>nvim_paste</c> sequentially with these <c>phase</c> values:
    /// <list type="bullet">
    /// <item><description>
    /// 1: starts the paste (exactly once)
    /// </description></item>
    /// <item><description>
    /// 2: continues the paste (zero or more times)
    /// </description></item>
    /// <item><description>
    /// 3: ends the paste (exactly once)
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// <list type="bullet">
    /// <item><description>
    /// <c>true</c>: Client may continue pasting.
    /// </description></item>
    /// <item><description>
    /// <c>false</c>: Client should cancel the paste.
    /// </description></item>
    /// </list>
    /// </returns>
    Task<NvimBoolean> PasteAsync(
      NvimString Data,
      NvimBoolean Crlf,
      NvimInteger Phase,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Puts text at cursor, in any mode.
    /// </summary>
    /// <param name = "Lines">
    /// <c>readfile()</c>-style list of lines. <c>channel-lines</c>
    /// </param>
    /// <param name = "Type">
    /// Edit behavior: any <c>getregtype()</c> result, or:
    /// <list type="bullet">
    /// <item><description>
    /// &quot;b&quot; <c>blockwise-visual</c> mode (may include width, e.g. &quot;b3&quot;)
    /// </description></item>
    /// <item><description>
    /// &quot;c&quot; <c>charwise</c> mode
    /// </description></item>
    /// <item><description>
    /// &quot;l&quot; <c>linewise</c> mode
    /// </description></item>
    /// <item><description>
    /// &quot; &quot; guess by contents, see <c>setreg()</c>
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "After">
    /// If true insert after cursor (like <c>p</c>), or before (like <c>P</c>).
    /// </param>
    /// <param name = "Follow">
    /// If true place cursor at end of inserted text.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task PutAsync(
      IReadOnlyList<NvimString> Lines,
      NvimString Type,
      NvimBoolean After,
      NvimBoolean Follow,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Returns the 24-bit RGB value of a <see cref = "GetColorMapAsync"/> color name or &quot;#rrggbb&quot; hexadecimal string.
    /// </summary>
    /// <param name = "Name">
    /// Color name or &quot;#rrggbb&quot; string
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// 24-bit RGB value, or -1 for invalid argument.
    /// </returns>
    Task<NvimInteger> GetColorByNameAsync(
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Returns a map of color names and RGB values.
    /// </summary>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Map of color names and RGB values.
    /// </returns>
    Task<IReadOnlyList<NvimMapEntry>> GetColorMapAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets a map of the current editor state.
    /// </summary>
    /// <param name = "Opts">
    /// Optional parameters.
    /// <list type="bullet">
    /// <item><description>
    /// <c>types</c>: List of <c>context-types</c> (&quot;regs&quot;, &quot;jumps&quot;, &quot;bufs&quot;, &quot;gvars&quot;, …) to gather, or empty for &quot;all&quot;.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// map of global <c>context</c>.
    /// </returns>
    Task<IReadOnlyList<NvimMapEntry>> GetContextAsync(
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets the current editor state from the given <c>context</c> map.
    /// </summary>
    /// <param name = "Dict">
    /// <c>Context</c> map.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// The Neovim RPC result.
    /// </returns>
    Task<NvimValue> LoadContextAsync(
      IReadOnlyList<NvimMapEntry> Dict,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the current mode.
    /// </summary>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Dict { &quot;mode&quot;: String, &quot;blocking&quot;: Boolean}
    /// </returns>
    Task<IReadOnlyList<NvimMapEntry>> GetModeAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets a list of global (non-buffer-local) <c>mapping</c> definitions.
    /// </summary>
    /// <param name = "Mode">
    /// Mode short-name (&quot;n&quot;, &quot;i&quot;, &quot;v&quot;, ...)
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Array of <c>maparg()</c>-like dictionaries describing mappings. The &quot;buf&quot; key is always zero.
    /// </returns>
    Task<IReadOnlyList<IReadOnlyList<NvimMapEntry>>> GetKeymapAsync(
      NvimString Mode,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets a global <c>mapping</c> for the given mode.
    /// </summary>
    /// <param name = "Mode">
    /// Mode short-name (map command prefix: &quot;n&quot;, &quot;i&quot;, &quot;v&quot;, &quot;x&quot;, …) or &quot;!&quot; for <c>:map!</c>, or empty string for <c>:map</c>. &quot;ia&quot;, &quot;ca&quot; or &quot;!a&quot; for abbreviation in Insert mode, Cmdline mode, or both, respectively
    /// </param>
    /// <param name = "Lhs">
    /// Left-hand-side <c><paramref name = "Lhs"/></c> of the mapping.
    /// </param>
    /// <param name = "Rhs">
    /// Right-hand-side <c><paramref name = "Rhs"/></c> of the mapping.
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters map: Accepts all <c>:map-arguments</c> as keys except [&lt;buffer&gt;], values are booleans (default false). Also:
    /// <list type="bullet">
    /// <item><description>
    /// &quot;callback&quot; Lua function called in place of <paramref name = "Rhs"/>.
    /// </description></item>
    /// <item><description>
    /// &quot;desc&quot; human-readable description.
    /// </description></item>
    /// <item><description>
    /// &quot;noremap&quot; disables <c>recursive_mapping</c>, like <c>:noremap</c>
    /// </description></item>
    /// <item><description>
    /// &quot;replace_keycodes&quot; (boolean) When &quot;expr&quot; is true, replace keycodes in the resulting string (see <see cref = "ReplaceTermcodesAsync"/>). Returning nil from the Lua &quot;callback&quot; is equivalent to returning an empty string.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task SetKeymapAsync(
      NvimString Mode,
      NvimString Lhs,
      NvimString Rhs,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Unmaps a global <c>mapping</c> for the given mode.
    /// </summary>
    /// <param name = "Mode">
    /// The <paramref name = "Mode"/> argument.
    /// </param>
    /// <param name = "Lhs">
    /// The <paramref name = "Lhs"/> argument.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task DelKeymapAsync(
      NvimString Mode,
      NvimString Lhs,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Returns a 2-tuple (Array), where item 0 is the current channel id and item 1 is the <c>api-metadata</c> map (Dict).
    /// </summary>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// 2-tuple <c>[{channel-id}, {api-metadata}]</c>
    /// </returns>
    Task<IReadOnlyList<NvimValue>> GetApiInfoAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Self-identifies the client, and sets optional flags on the channel.
    /// </summary>
    /// <param name = "Name">
    /// Client short-name. Sets the <c>client.name</c> field of <see cref = "GetChanInfoAsync"/>.
    /// </param>
    /// <param name = "Version">
    /// Dict describing the version, with these (optional) keys:
    /// <list type="bullet">
    /// <item><description>
    /// &quot;commit&quot; hash or similar identifier of commit
    /// </description></item>
    /// <item><description>
    /// &quot;major&quot; major version (defaults to 0 if not set, for no release yet)
    /// </description></item>
    /// <item><description>
    /// &quot;minor&quot; minor version
    /// </description></item>
    /// <item><description>
    /// &quot;patch&quot; patch number
    /// </description></item>
    /// <item><description>
    /// &quot;prerelease&quot; string describing a prerelease, like &quot;dev&quot; or &quot;beta1&quot;
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "Type">
    /// Must be one of the following values. Client libraries should default to &quot;remote&quot; unless overridden by the user.
    /// <list type="bullet">
    /// <item><description>
    /// &quot;embedder&quot; application using Nvim as a component (for example, IDE/editor implementing a vim mode).
    /// </description></item>
    /// <item><description>
    /// &quot;host&quot; plugin host, typically started by nvim
    /// </description></item>
    /// <item><description>
    /// &quot;msgpack-rpc&quot; remote client connected to Nvim via fully MessagePack-RPC compliant protocol.
    /// </description></item>
    /// <item><description>
    /// &quot;plugin&quot; single plugin, started by nvim
    /// </description></item>
    /// <item><description>
    /// &quot;remote&quot; remote client connected &quot;Nvim flavored&quot; MessagePack-RPC (responses must be in reverse order of requests). <c>msgpack-rpc</c>
    /// </description></item>
    /// <item><description>
    /// &quot;ui&quot; gui frontend
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "Methods">
    /// Builtin methods in the client. For a host, this does not include plugin methods which will be discovered later. The key should be the method name, the values are dicts with these (optional) keys (more keys may be added in future versions of Nvim, thus unknown keys are ignored. Clients must only use keys defined in this or later versions of Nvim):
    /// <list type="bullet">
    /// <item><description>
    /// &quot;async&quot; if true, send as a notification. If false or unspecified, use a blocking request
    /// </description></item>
    /// <item><description>
    /// &quot;nargs&quot; Number of arguments. Could be a single integer or an array of two integers, minimum and maximum inclusive.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "Attributes">
    /// Arbitrary string:string map of informal client properties. Suggested keys:
    /// <list type="bullet">
    /// <item><description>
    /// &quot;license&quot;: License description (&quot;Apache 2&quot;, &quot;GPLv3&quot;, &quot;MIT&quot;, …)
    /// </description></item>
    /// <item><description>
    /// &quot;logo&quot;: URI or path to image, preferably small logo or icon. .png or .svg format is preferred.
    /// </description></item>
    /// <item><description>
    /// &quot;pid&quot;: Process id.
    /// </description></item>
    /// <item><description>
    /// &quot;website&quot;: Client homepage URL (e.g. GitHub repository)
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task SetClientInfoAsync(
      NvimString Name,
      IReadOnlyList<NvimMapEntry> Version,
      NvimString Type,
      IReadOnlyList<NvimMapEntry> Methods,
      IReadOnlyList<NvimMapEntry> Attributes,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets information about a channel.
    /// </summary>
    /// <param name = "Chan">
    /// channel_id, or 0 for current channel
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Channel info dict with these keys:
    /// <list type="bullet">
    /// <item><description>
    /// &quot;id&quot; Channel id.
    /// </description></item>
    /// <item><description>
    /// &quot;argv&quot; (optional) Job arguments list.
    /// </description></item>
    /// <item><description>
    /// &quot;stream&quot; Stream underlying the channel.
    /// <list type="bullet">
    /// <item><description>
    /// &quot;stdio&quot; stdin and stdout of this Nvim instance
    /// </description></item>
    /// <item><description>
    /// &quot;stderr&quot; stderr of this Nvim instance
    /// </description></item>
    /// <item><description>
    /// &quot;socket&quot; TCP/IP socket or named pipe
    /// </description></item>
    /// <item><description>
    /// &quot;job&quot; Job with communication over its stdio.
    /// </description></item>
    /// </list>
    /// </description></item>
    /// <item><description>
    /// &quot;mode&quot; How data received on the channel is interpreted.
    /// <list type="bullet">
    /// <item><description>
    /// &quot;bytes&quot; Send and receive raw bytes.
    /// </description></item>
    /// <item><description>
    /// &quot;terminal&quot; <c>terminal</c> instance interprets ASCII sequences.
    /// </description></item>
    /// <item><description>
    /// &quot;rpc&quot; <c>RPC</c> communication on the channel is active.
    /// </description></item>
    /// </list>
    /// </description></item>
    /// <item><description>
    /// &quot;pty&quot; (optional) Name of pseudoterminal. On a POSIX system this is a device path like &quot;/dev/pts/1&quot;. If unknown, the key will still be present if a pty is used (e.g. for conpty on Windows).
    /// </description></item>
    /// <item><description>
    /// &quot;buf&quot; (optional) Buffer connected to <c>terminal</c> instance.
    /// </description></item>
    /// <item><description>
    /// &quot;buffer&quot; (optional) Deprecated alias for <c>buf</c>.
    /// </description></item>
    /// <item><description>
    /// &quot;client&quot; (optional) Info about the peer (client on the other end of the channel), as set by <see cref = "SetClientInfoAsync"/>.
    /// </description></item>
    /// <item><description>
    /// &quot;exitcode&quot; (optional) Exit code of the <c>terminal</c> process.
    /// </description></item>
    /// </list>
    /// </returns>
    Task<IReadOnlyList<NvimMapEntry>> GetChanInfoAsync(
      NvimInteger Chan,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Get information about all open channels.
    /// </summary>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Array of Dictionaries, each describing a channel with the format specified at <see cref = "GetChanInfoAsync"/>.
    /// </returns>
    Task<IReadOnlyList<IReadOnlyList<NvimMapEntry>>> ListChansAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets a list of dictionaries representing attached UIs.
    /// </summary>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Array of UI dictionaries, each with these keys:
    /// <list type="bullet">
    /// <item><description>
    /// &quot;height&quot; Requested height of the UI
    /// </description></item>
    /// <item><description>
    /// &quot;width&quot; Requested width of the UI
    /// </description></item>
    /// <item><description>
    /// &quot;rgb&quot; true if the UI uses RGB colors (false implies <c>cterm-colors</c>)
    /// </description></item>
    /// <item><description>
    /// &quot;ext_...&quot; Requested UI extensions, see <c>ui-option</c>
    /// </description></item>
    /// <item><description>
    /// &quot;chan&quot; <c>channel-id</c> of remote UI
    /// </description></item>
    /// </list>
    /// </returns>
    Task<IReadOnlyList<IReadOnlyList<NvimMapEntry>>> ListUisAsync(
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the immediate children of process <c>pid</c>.
    /// </summary>
    /// <param name = "Pid">
    /// The <paramref name = "Pid"/> argument.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Array of child process ids, empty if process not found.
    /// </returns>
    Task<IReadOnlyList<NvimValue>> GetProcChildrenAsync(
      NvimInteger Pid,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets info describing process <c>pid</c>.
    /// </summary>
    /// <param name = "Pid">
    /// The <paramref name = "Pid"/> argument.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Map of process properties, or NIL if process not found.
    /// </returns>
    Task<NvimValue> GetProcAsync(
      NvimInteger Pid,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Selects an item in the completion popup menu.
    /// </summary>
    /// <param name = "Item">
    /// Index (zero-based) of the item to select. Value of -1 selects nothing and restores the original text.
    /// </param>
    /// <param name = "Insert">
    /// For <c>ins-completion</c>, whether the selection should be inserted in the buffer. Ignored for <c>cmdline-completion</c>.
    /// </param>
    /// <param name = "Finish">
    /// Finish the completion and dismiss the popup menu. Implies <paramref name = "Insert"/>.
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters. Reserved for future use.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task SelectPopupmenuItemAsync(
      NvimInteger Item,
      NvimBoolean Insert,
      NvimBoolean Finish,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Deletes an uppercase/file named mark.
    /// </summary>
    /// <param name = "Name">
    /// Mark name
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// true if the mark was deleted, else false.
    /// </returns>
    Task<NvimBoolean> DelMarkAsync(
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Returns a <c>(row, col, buffer, buffername)</c> tuple representing the position of the uppercase/file named mark.
    /// </summary>
    /// <param name = "Name">
    /// Mark name
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters. Reserved for future use.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// 4-tuple (row, col, buffer, buffername), (0, 0, 0, &apos; &apos;) if the mark is not set.
    /// </returns>
    Task<IReadOnlyList<NvimValue>> GetMarkAsync(
      NvimString Name,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Evaluates statusline string.
    /// </summary>
    /// <param name = "Str">
    /// Statusline string (see &apos;statusline&apos;).
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters.
    /// <list type="bullet">
    /// <item><description>
    /// <c>fillchar</c>: (string) Character to fill blank spaces in the statusline (see &apos;fillchars&apos;). Treated as single-width even if it isn&apos;t.
    /// </description></item>
    /// <item><description>
    /// <c>highlights</c>: (boolean) Return highlight information.
    /// </description></item>
    /// <item><description>
    /// <c>maxwidth</c>: (number) Maximum width of statusline.
    /// </description></item>
    /// <item><description>
    /// <c>use_statuscol_lnum</c>: (number) Evaluate statuscolumn for this line number instead of statusline.
    /// </description></item>
    /// <item><description>
    /// <c>use_tabline</c>: (boolean) Evaluate tabline instead of statusline. When true, <c>winid</c> is ignored. Mutually exclusive with <c>use_winbar</c>.
    /// </description></item>
    /// <item><description>
    /// <c>use_winbar</c>: (boolean) Evaluate winbar instead of statusline.
    /// </description></item>
    /// <item><description>
    /// <c>winid</c>: (number) <c>window-ID</c> of the window to use as context for statusline.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Dict containing statusline information, with these keys:
    /// <list type="bullet">
    /// <item><description>
    /// <c>str</c>: (string) Characters that will be displayed on the statusline.
    /// </description></item>
    /// <item><description>
    /// <c>width</c>: (number) Display width of the statusline.
    /// </description></item>
    /// <item><description>
    /// <c>highlights</c>: Array containing highlight information of the statusline. Only included when the &quot;highlights&quot; key in <paramref name = "Opts"/> is true. Each element of the array is a <c>Dict</c> with these keys:
    /// <list type="bullet">
    /// <item><description>
    /// <c>start</c>: (number) Byte index (0-based) of first character that uses the highlight.
    /// </description></item>
    /// <item><description>
    /// <c>group</c>: (string) Deprecated. Use <c>groups</c> instead.
    /// </description></item>
    /// <item><description>
    /// <c>groups</c>: (array) Names of stacked highlight groups (highest priority last).
    /// </description></item>
    /// </list>
    /// </description></item>
    /// </list>
    /// </returns>
    Task<IReadOnlyList<NvimMapEntry>> EvalStatuslineAsync(
      NvimString Str,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Executes Vimscript (multiline block of Ex commands), like anonymous <c>:source</c>.
    /// </summary>
    /// <param name = "Src">
    /// Vimscript code
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters.
    /// <list type="bullet">
    /// <item><description>
    /// <c>output</c>: (boolean, default false) Whether to capture and return all (non-error, non-shell <c>:!</c>) output.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Dict containing information about execution, with these keys:
    /// <list type="bullet">
    /// <item><description>
    /// <c>output</c>: (string|nil) Output if <c>opts.output</c> is true.
    /// </description></item>
    /// </list>
    /// </returns>
    Task<IReadOnlyList<NvimMapEntry>> Exec2Async(
      NvimString Src,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Executes an Ex command.
    /// </summary>
    /// <param name = "Cmd">
    /// Ex command string
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task CommandAsync(
      NvimString Cmd,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Evaluates a Vimscript <c>expression</c>.
    /// </summary>
    /// <param name = "Expr">
    /// Vimscript expression string
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Evaluation result or expanded object
    /// </returns>
    Task<NvimValue> EvalAsync(
      NvimString Expr,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Calls a Vimscript function with the given arguments.
    /// </summary>
    /// <param name = "Fn">
    /// Function to call
    /// </param>
    /// <param name = "Args">
    /// Function arguments packed in an Array
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Result of the function call
    /// </returns>
    Task<NvimValue> CallFunctionAsync(
      NvimString Fn,
      IReadOnlyList<NvimValue> Args,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Calls a Vimscript <c>Dictionary-function</c> with the given arguments.
    /// </summary>
    /// <param name = "Dict">
    /// Dict, or String evaluating to a Vimscript <c>self</c> dict
    /// </param>
    /// <param name = "Fn">
    /// Name of the function defined on the Vimscript dict
    /// </param>
    /// <param name = "Args">
    /// Function arguments packed in an Array
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Result of the function call
    /// </returns>
    Task<NvimValue> CallDictFunctionAsync(
      NvimValue Dict,
      NvimString Fn,
      IReadOnlyList<NvimValue> Args,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Invokes Neovim RPC method <c>nvim_parse_expression</c>.
    /// </summary>
    /// <param name = "Expr">
    /// The <paramref name = "Expr"/> argument.
    /// </param>
    /// <param name = "Flags">
    /// The <paramref name = "Flags"/> argument.
    /// </param>
    /// <param name = "Hl">
    /// The <paramref name = "Hl"/> argument.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// The Neovim RPC result.
    /// </returns>
    Task<IReadOnlyList<NvimMapEntry>> ParseExpressionAsync(
      NvimString Expr,
      NvimString Flags,
      NvimBoolean Hl,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Opens a new split window, floating window, or external window.
    /// </summary>
    /// <param name = "Buf">
    /// Buffer to display, or 0 for current buffer
    /// </param>
    /// <param name = "Enter">
    /// Enter the window (make it the current window)
    /// </param>
    /// <param name = "Config">
    /// Map defining the window configuration. Keys:
    /// <list type="bullet">
    /// <item><description>
    /// <c>anchor</c>: Decides which corner of the float to place at (row,col):
    /// <list type="bullet">
    /// <item><description>
    /// &quot;NW&quot; northwest (default)
    /// </description></item>
    /// <item><description>
    /// &quot;NE&quot; northeast
    /// </description></item>
    /// <item><description>
    /// &quot;SW&quot; southwest
    /// </description></item>
    /// <item><description>
    /// &quot;SE&quot; southeast
    /// </description></item>
    /// </list>
    /// </description></item>
    /// <item><description>
    /// <c>border</c>: (<c>string|string[]</c>) (defaults to &apos;winborder&apos; option) Window border. The string form accepts the same values as the &apos;winborder&apos; option. The array form must have a length of eight or any divisor of eight, specifying the chars that form the border in a clockwise fashion starting from the top-left corner. For example, the double-box style can be specified as: <c>[ &quot;╔&quot;, &quot;═&quot; ,&quot;╗&quot;, &quot;║&quot;, &quot;╝&quot;, &quot;═&quot;, &quot;╚&quot;, &quot;║&quot; ].</c> If fewer than eight chars are given, they will be repeated. An ASCII border could be specified as: <c>[ &quot;/&quot;, &quot;-&quot;, \&quot;\\\\\\\\\\&quot;, \&quot;\&quot; ], &lt;/tt&gt; Or one char for all sides: &lt;tt&gt; [ \&quot;x\&quot; ]. &lt;/tt&gt; Empty string can be used to hide a specific border. This example will show only vertical borders, not horizontal: &lt;tt&gt; [ \&quot;\&quot;, \&quot;\&quot;, \&quot;\&quot;, \&quot;\&gt;\&quot;, \&quot;\&quot;, \&quot;\&quot;, \&quot;\&quot;, \&quot;\&lt;\&quot; ] &lt;/tt&gt; By default, hl-FloatBorder highlight is used, which links to hl-WinSeparator when not defined. Each border side can specify an optional highlight: &lt;tt&gt; [ [\&quot;+\&quot;, \&quot;MyCorner\&quot;], [\&quot;x\&quot;, \&quot;MyBorder\&quot;] ]. &lt;/tt&gt; - bufpos: Places float relative to buffer text (only when relative=&quot;win&quot;). Takes a tuple of zero-indexed &lt;tt&gt;[line, column]&lt;/tt&gt;. &lt;tt&gt;row&lt;/tt&gt; and &lt;tt&gt;col&lt;/tt&gt; if given are applied relative to this position, else they default to: - &lt;tt&gt;row=1&lt;/tt&gt; and &lt;tt&gt;col=0&lt;/tt&gt; if &lt;tt&gt;anchor&lt;/tt&gt; is &quot;NW&quot; or &quot;NE&quot; - &lt;tt&gt;row=0&lt;/tt&gt; and &lt;tt&gt;col=0&lt;/tt&gt; if &lt;tt&gt;anchor&lt;/tt&gt; is &quot;SW&quot; or &quot;SE&quot; (thus like a tooltip near the buffer text). - col: Column position in units of screen cell width, may be fractional. - external: GUI should display the window as an external top-level window. Currently accepts no other positioning configuration together with this. - fixed: If true when anchor is NW or SW, the float window would be kept fixed even if the window would be truncated. - focusable: Enable focus by user actions (wincmds, mouse events). Defaults to true. Non-focusable windows can be entered by nvim_set_current_win(), or, when the &lt;tt&gt;mouse&lt;/tt&gt; field is set to true, by mouse events. See focusable. - footer: (optional) Footer in window border, string or list. List should consist of &lt;tt&gt;[text, highlight]&lt;/tt&gt; tuples. If string, or a tuple lacks a highlight, the default highlight group is &lt;tt&gt;FloatFooter&lt;/tt&gt;. - footer_pos: Footer position. Must be set with &lt;tt&gt;footer&lt;/tt&gt; option. Value can be one of &quot;left&quot;, &quot;center&quot;, or &quot;right&quot;. Default is &lt;tt&gt;\&quot;left\&quot; &lt;/tt&gt;. - height: Window height (in character cells). Minimum of 1. - hide: If true the floating window will be hidden and the cursor will be invisible when focused on it. - mouse: Specify how this window interacts with mouse events. Defaults to &lt;tt&gt;focusable&lt;/tt&gt; value. - If false, mouse events pass through this window. - If true, mouse events interact with this window normally. - noautocmd: Block all autocommands for the duration of the call. Cannot be changed by nvim_win_set_config(). - relative: Sets the window layout to &quot;floating&quot;, placed at (row,col) coordinates relative to: - &quot;cursor&quot; Cursor position in current window. - &quot;editor&quot; The global editor grid. - &quot;laststatus&quot; &apos;laststatus&apos; if present, or last row. - &quot;mouse&quot; Mouse position. - &quot;tabline&quot; Tabline if present, or first row. - &quot;win&quot; Window given by the &lt;tt&gt;win&lt;/tt&gt; field, or current window. - row: Row position in units of &quot;screen cell height&quot;, may be fractional. - split: Split direction: &quot;left&quot;, &quot;right&quot;, &quot;above&quot;, &quot;below&quot;. - style: (optional) Configure the appearance of the window: - &quot; &quot; No special style. - &quot;minimal&quot; Nvim will display the window with many UI options disabled. This is useful when displaying a temporary float where the text should not be edited. Disables &apos;number&apos;, &apos;relativenumber&apos;, &apos;cursorline&apos;, &apos;cursorcolumn&apos;, &apos;foldcolumn&apos;, &apos;spell&apos; and &apos;list&apos; options. &apos;signcolumn&apos; is changed to &lt;tt&gt;auto&lt;/tt&gt; and &apos;colorcolumn&apos; is cleared. &apos;statuscolumn&apos; is changed to empty. The end-of-buffer region is hidden by setting &lt;tt&gt;eob&lt;/tt&gt; flag of &apos;fillchars&apos; to a space char, and clearing the hl-EndOfBuffer region in &apos;winhighlight&apos;. - title: (optional) Title in window border, string or list. List should consist of &lt;tt&gt;[text, highlight]&lt;/tt&gt; tuples. If string, or a tuple lacks a highlight, the default highlight group is &lt;tt&gt;FloatTitle&lt;/tt&gt;. - title_pos: Title position. Must be set with &lt;tt&gt;title&lt;/tt&gt; option. Value can be one of &quot;left&quot;, &quot;center&quot;, or &quot;right&quot;. Default is &lt;tt&gt;\&quot;left\&quot; &lt;/tt&gt;. - vertical: Split vertically :vertical. - width: Window width (in character cells). Minimum of 1. - win: window-ID target window. Can be in a different tab page. Determines the window to split (negative values act like :topleft, :botright|), the relative window for a &lt;tt&gt;relative=\&quot;win\&quot;</c> float, or just the target tab page (inferred from the window) for others.
    /// </description></item>
    /// <item><description>
    /// <c>zindex</c>: Stacking order. floats with higher <c>zindex</c> go on top on floats with lower indices. Must be larger than zero. The following screen elements have hard-coded z-indices:
    /// <list type="bullet">
    /// <item><description>
    /// 100: insert completion popupmenu
    /// </description></item>
    /// <item><description>
    /// 200: message scrollback
    /// </description></item>
    /// <item><description>
    /// 250: cmdline completion popupmenu (when wildoptions+=pum) The default value for floats are 50. In general, values below 100 are recommended, unless there is a good reason to overshadow builtin elements.
    /// </description></item>
    /// </list>
    /// </description></item>
    /// <item><description>
    /// <c>_cmdline_offset</c>: (EXPERIMENTAL) When provided, anchor the <c>cmdline-completion</c> popupmenu to this window, with an offset in screen cell width.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// <c>window-ID</c>, or 0 on error
    /// </returns>
    Task<Window> OpenWinAsync(
      Buffer Buf,
      NvimBoolean Enter,
      IReadOnlyList<NvimMapEntry> Config,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Reconfigures the layout and properties of a window.
    /// </summary>
    /// <param name = "Win">
    /// <c>window-ID</c>, or 0 for current window
    /// </param>
    /// <param name = "Config">
    /// Map defining the window configuration, see <see cref = "OpenWinAsync"/>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task WinSetConfigAsync(
      Window Win,
      IReadOnlyList<NvimMapEntry> Config,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets window configuration in the form of a dict which can be passed as the <c>config</c> parameter of <see cref = "OpenWinAsync"/>.
    /// </summary>
    /// <param name = "Win">
    /// <c>window-ID</c>, or 0 for current window
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Map defining the window configuration, see <see cref = "OpenWinAsync"/>
    /// </returns>
    Task<IReadOnlyList<NvimMapEntry>> WinGetConfigAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the current buffer in a window.
    /// </summary>
    /// <param name = "Win">
    /// <c>window-ID</c>, or 0 for current window
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Buffer id
    /// </returns>
    Task<Buffer> WinGetBufAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets the current buffer in a window.
    /// </summary>
    /// <param name = "Win">
    /// <c>window-ID</c>, or 0 for current window
    /// </param>
    /// <param name = "Buf">
    /// Buffer id
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task WinSetBufAsync(
      Window Win,
      Buffer Buf,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the (1,0)-indexed, buffer-relative cursor position for a given window (different windows showing the same buffer have independent cursor positions).
    /// </summary>
    /// <param name = "Win">
    /// <c>window-ID</c>, or 0 for current window
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// (row, col) tuple
    /// </returns>
    Task<IReadOnlyList<NvimInteger>> WinGetCursorAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets the (1,0)-indexed cursor position (byte offset) in the window.
    /// </summary>
    /// <param name = "Win">
    /// <c>window-ID</c>, or 0 for current window
    /// </param>
    /// <param name = "Pos">
    /// (row, col) tuple representing the new position
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task WinSetCursorAsync(
      Window Win,
      IReadOnlyList<NvimInteger> Pos,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the window height.
    /// </summary>
    /// <param name = "Win">
    /// <c>window-ID</c>, or 0 for current window
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Height as a count of rows
    /// </returns>
    Task<NvimInteger> WinGetHeightAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets the window height.
    /// </summary>
    /// <param name = "Win">
    /// <c>window-ID</c>, or 0 for current window
    /// </param>
    /// <param name = "Height">
    /// Height as a count of rows
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task WinSetHeightAsync(
      Window Win,
      NvimInteger Height,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the window width.
    /// </summary>
    /// <param name = "Win">
    /// <c>window-ID</c>, or 0 for current window
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Width as a count of columns
    /// </returns>
    Task<NvimInteger> WinGetWidthAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets the window width.
    /// </summary>
    /// <param name = "Win">
    /// <c>window-ID</c>, or 0 for current window
    /// </param>
    /// <param name = "Width">
    /// Width as a count of columns
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task WinSetWidthAsync(
      Window Win,
      NvimInteger Width,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets a window-scoped (w:) variable.
    /// </summary>
    /// <param name = "Win">
    /// <c>window-ID</c>, or 0 for current window
    /// </param>
    /// <param name = "Name">
    /// Variable name
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Variable value
    /// </returns>
    Task<NvimValue> WinGetVarAsync(
      Window Win,
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Sets a window-scoped (w:) variable.
    /// </summary>
    /// <param name = "Win">
    /// <c>window-ID</c>, or 0 for current window
    /// </param>
    /// <param name = "Name">
    /// Variable name
    /// </param>
    /// <param name = "Value">
    /// Variable value
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task WinSetVarAsync(
      Window Win,
      NvimString Name,
      NvimValue Value,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Removes a window-scoped (w:) variable.
    /// </summary>
    /// <param name = "Win">
    /// <c>window-ID</c>, or 0 for current window
    /// </param>
    /// <param name = "Name">
    /// Variable name
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task WinDelVarAsync(
      Window Win,
      NvimString Name,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the window position in display cells.
    /// </summary>
    /// <param name = "Win">
    /// <c>window-ID</c>, or 0 for current window
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// (row, col) tuple with the window position
    /// </returns>
    Task<IReadOnlyList<NvimInteger>> WinGetPositionAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the window tabpage.
    /// </summary>
    /// <param name = "Win">
    /// <c>window-ID</c>, or 0 for current window
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Tabpage that contains the window
    /// </returns>
    Task<Tabpage> WinGetTabpageAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Gets the window number.
    /// </summary>
    /// <param name = "Win">
    /// <c>window-ID</c>, or 0 for current window
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Window number
    /// </returns>
    Task<NvimInteger> WinGetNumberAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Checks if a window is valid.
    /// </summary>
    /// <param name = "Win">
    /// <c>window-ID</c>, or 0 for current window
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// true if the window is valid, false otherwise
    /// </returns>
    Task<NvimBoolean> WinIsValidAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Closes the window and hide the buffer it contains (like <c>:hide</c> with a <c>window-ID</c>).
    /// </summary>
    /// <param name = "Win">
    /// <c>window-ID</c>, or 0 for current window
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task WinHideAsync(
      Window Win,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Closes the window (like <c>:close</c> with a <c>window-ID</c>).
    /// </summary>
    /// <param name = "Win">
    /// <c>window-ID</c>, or 0 for current window
    /// </param>
    /// <param name = "Force">
    /// Behave like <c>:close!</c> The last window of a buffer with unwritten changes can be closed. The buffer will become hidden, even if &apos;hidden&apos; is not set.
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task WinCloseAsync(
      Window Win,
      NvimBoolean Force,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Set highlight namespace for a window.
    /// </summary>
    /// <param name = "Win">
    /// The <paramref name = "Win"/> argument.
    /// </param>
    /// <param name = "NsId">
    /// the namespace to use
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    Task WinSetHlNsAsync(
      Window Win,
      NvimInteger NsId,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Computes the number of screen lines occupied by a range of text in a given window.
    /// </summary>
    /// <param name = "Win">
    /// <c>window-ID</c>, or 0 for current window.
    /// </param>
    /// <param name = "Opts">
    /// Optional parameters:
    /// <list type="bullet">
    /// <item><description>
    /// <c>end_row</c>: Ending line index, 0-based inclusive. When omitted end at the very bottom.
    /// </description></item>
    /// <item><description>
    /// <c>end_vcol</c>: Ending virtual column index on &quot;end_row&quot;, 0-based exclusive, rounded up to full screen lines. When 0 only include diff filler and virtual lines above &quot;end_row&quot;. When omitted include the whole line.
    /// </description></item>
    /// <item><description>
    /// <c>max_height</c>: Don&apos;t add the height of lines below the row for which this height is reached. Useful to e.g. limit the height to the window height, avoiding unnecessary work. Or to find out how many buffer lines beyond &quot;start_row&quot; take up a certain number of logical lines (returned in &quot;end_row&quot; and &quot;end_vcol&quot;).
    /// </description></item>
    /// <item><description>
    /// <c>start_row</c>: Starting line index, 0-based inclusive. When omitted start at the very top.
    /// </description></item>
    /// <item><description>
    /// <c>start_vcol</c>: Starting virtual column index on &quot;start_row&quot;, 0-based inclusive, rounded down to full screen lines. When omitted include the whole line.
    /// </description></item>
    /// </list>
    /// </param>
    /// <param name = "cancellationToken">
    /// A token that cancels the RPC request.
    /// </param>
    /// <returns>
    /// Dict containing text height information, with these keys:
    /// <list type="bullet">
    /// <item><description>
    /// <c>all</c>: The total number of screen lines occupied by the range.
    /// </description></item>
    /// <item><description>
    /// <c>fill</c>: The number of diff filler or virtual lines among them.
    /// </description></item>
    /// <item><description>
    /// <c>end_row</c>: The row on which the returned height is reached (first row of a closed fold).
    /// </description></item>
    /// <item><description>
    /// <c>end_vcol</c>: Ending virtual column in &quot;end_row&quot; where &quot;max_height&quot; or the returned height is reached. 0 if &quot;end_row&quot; is a closed fold.
    /// </description></item>
    /// </list>
    /// </returns>
    Task<IReadOnlyList<NvimMapEntry>> WinTextHeightAsync(
      Window Win,
      IReadOnlyList<NvimMapEntry> Opts,
      CancellationToken cancellationToken = default(CancellationToken)
    );
  }
}
