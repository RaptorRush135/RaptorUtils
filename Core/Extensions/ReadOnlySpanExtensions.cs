namespace RaptorUtils.Extensions;

/// <summary>
/// Provides extension methods for <see cref="ReadOnlySpan{T}"/>.
/// </summary>
public static class ReadOnlySpanExtensions
{
    /// <summary>
    /// Removes the specified <paramref name="suffix"/> from the end of <paramref name="str"/> if it exists.
    /// </summary>
    /// <param name="str">The span to remove the suffix from.</param>
    /// <param name="suffix">The suffix to remove. Cannot be empty.</param>
    /// <param name="comparer">
    /// An optional <see cref="IEqualityComparer{Char}"/> to use for character comparison.
    /// </param>
    /// <returns>
    /// A <see cref="ReadOnlySpan{Char}"/> representing the span without the specified suffix.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="suffix"/> is empty.</exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when <paramref name="str"/> does not end with the specified <paramref name="suffix"/>.
    /// </exception>
    public static ReadOnlySpan<char> RemoveRequiredSuffix(
        this ReadOnlySpan<char> str,
        ReadOnlySpan<char> suffix,
        IEqualityComparer<char>? comparer = null)
    {
        if (suffix.IsEmpty)
        {
            throw new ArgumentException("Suffix cannot be empty.", nameof(suffix));
        }

        if (str.Length < suffix.Length
            || !str[^suffix.Length..].SequenceEqual(suffix, comparer))
        {
            throw new InvalidOperationException($"The string '{str}' does not end with the '{suffix}' suffix.");
        }

        return str[..^suffix.Length];
    }
}
