namespace RaptorUtils.Extensions;

/// <summary>
/// Provides extension methods for <see cref="string"/>.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Removes the specified <paramref name="suffix"/> from the end of <paramref name="str"/> if it exists.
    /// </summary>
    /// <param name="str">
    /// The string to remove the suffix from. Cannot be <see langword="null"/>.
    /// </param>
    /// <param name="suffix">
    /// The suffix to remove. Cannot be <see langword="null"/> or empty.
    /// </param>
    /// <param name="comparisonType">
    /// Specifies the rules for the string comparison.
    /// </param>
    /// <returns>
    /// A new string with the specified suffix removed.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="str"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="suffix"/> is <see langword="null"/> or empty.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when <paramref name="str"/> does not end with the specified <paramref name="suffix"/>.
    /// </exception>
    public static string RemoveRequiredSuffix(
        this string str,
        string suffix,
        StringComparison comparisonType)
    {
        ArgumentNullException.ThrowIfNull(str);
        ArgumentException.ThrowIfNullOrEmpty(suffix);

        if (!str.EndsWith(suffix, comparisonType))
        {
            throw new InvalidOperationException($"The string '{str}' does not end with the specified suffix '{suffix}'.");
        }

        return str[..^suffix.Length];
    }
}
