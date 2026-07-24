using System.Collections.Generic;

namespace Nvim.Client;

internal static partial class NvimUiEventFactory
{
  internal static IReadOnlyList<NvimUiEvent> Decode(
    IReadOnlyList<NvimValue> batch
  )
  {
    IReadOnlyList<NvimUiEvent> events = [];
    Decode(batch, ref events);
    return events;
  }

  static partial void Decode(
    IReadOnlyList<NvimValue> batch,
    ref IReadOnlyList<NvimUiEvent> events
  );
}
