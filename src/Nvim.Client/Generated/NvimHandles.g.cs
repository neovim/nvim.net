using System;

namespace Nvim.Client
{
  /// <summary>
  /// Represents Neovim Buffer handle values.
  /// </summary>
  public readonly struct Buffer
  {
    /// <summary>
    /// Gets the underlying MessagePack extension value.
    /// </summary>
    public NvimExtension Extension { get; }

    /// <summary>
    /// Creates a validated handle from a MessagePack extension value.
    /// </summary>
    public Buffer(NvimExtension extension)
    {
      ArgumentNullException.ThrowIfNull(extension);
      if ((extension.Tag) != (0))
      {
        throw new ArgumentException("Expected Buffer extension tag.");
      }

      Extension = extension;
    }
  }

  /// <summary>
  /// Represents Neovim Window handle values.
  /// </summary>
  public readonly struct Window
  {
    /// <summary>
    /// Gets the underlying MessagePack extension value.
    /// </summary>
    public NvimExtension Extension { get; }

    /// <summary>
    /// Creates a validated handle from a MessagePack extension value.
    /// </summary>
    public Window(NvimExtension extension)
    {
      ArgumentNullException.ThrowIfNull(extension);
      if ((extension.Tag) != (1))
      {
        throw new ArgumentException("Expected Window extension tag.");
      }

      Extension = extension;
    }
  }

  /// <summary>
  /// Represents Neovim Tabpage handle values.
  /// </summary>
  public readonly struct Tabpage
  {
    /// <summary>
    /// Gets the underlying MessagePack extension value.
    /// </summary>
    public NvimExtension Extension { get; }

    /// <summary>
    /// Creates a validated handle from a MessagePack extension value.
    /// </summary>
    public Tabpage(NvimExtension extension)
    {
      ArgumentNullException.ThrowIfNull(extension);
      if ((extension.Tag) != (2))
      {
        throw new ArgumentException("Expected Tabpage extension tag.");
      }

      Extension = extension;
    }
  }
}
