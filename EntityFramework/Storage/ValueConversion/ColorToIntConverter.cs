namespace RaptorUtils.EntityFramework.Storage.ValueConversion;

using System.Drawing;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="Color"/> to its <see langword="int"/> ARGB representation
/// and back, for use with EF Core value conversions.
/// </summary>
public sealed class ColorToIntConverter()
    : ValueConverter<Color, int>(
        v => v.ToArgb(),
        v => Color.FromArgb(v));
