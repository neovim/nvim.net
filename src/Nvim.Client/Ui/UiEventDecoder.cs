using System;
using System.Collections.Generic;
using System.Linq;

namespace Nvim.Client;

internal static class UiEventDecoder
{
  internal static IReadOnlyList<NvimUiEvent> Decode(
    IReadOnlyList<NvimValue> batch,
    Func<string, IReadOnlyList<NvimValue>, NvimUiEvent?> factory
  )
  {
    List<NvimUiEvent> events = [];

    foreach (var encodedEvent in batch)
    {
      if (encodedEvent is not NvimArray uiEvent || uiEvent.Items.Count == 0)
        throw new NvimProtocolException("Invalid redraw event.");

      if (uiEvent.Items[0] is not NvimString name)
        throw new NvimProtocolException("Invalid redraw event name.");

      foreach (var encodedArguments in uiEvent.Items.Skip(1))
      {
        if (encodedArguments is not NvimArray arguments)
          throw new NvimProtocolException("Invalid redraw event arguments.");

        events.Add(
          factory(name.Value, arguments.Items)
            ?? throw new NvimProtocolException(
              $"Unknown redraw event: {name.Value}."
            )
        );
      }
    }

    return events;
  }

  internal static void RequireArity(IReadOnlyList<NvimValue> values, int count)
  {
    if (values.Count != count)
      throw new NvimProtocolException("Invalid redraw event argument count.");
  }

  internal static T Require<T>(NvimValue value)
    where T : NvimValue =>
    value as T
    ?? throw new NvimProtocolException("Invalid redraw event argument type.");

  internal static IReadOnlyList<NvimMapEntry> RequireMap(NvimValue value) =>
    value is NvimMap map
      ? map.Entries
      : throw new NvimProtocolException("Invalid redraw map argument.");

  internal static IReadOnlyList<T> RequireArray<T>(
    NvimValue value,
    Func<NvimValue, T> convert
  ) =>
    value is NvimArray array
      ? array.Items.Select(convert).ToArray()
      : throw new NvimProtocolException("Invalid redraw array argument.");
}
