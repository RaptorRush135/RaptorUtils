namespace RaptorUtils.Collections.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IEnumerable{T}"/>.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Determines whether the <see cref="IEnumerable{T}"/> contains any duplicate elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source collection.</typeparam>
    /// <param name="source">The source collection to check for duplicates.</param>
    /// <returns>
    /// <see langword="true"/> if the source collection contains duplicate elements;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool ContainsDuplicates<T>(this IEnumerable<T> source)
    {
        var valueSet = new HashSet<T>();
        return source.Any(item => !valueSet.Add(item));
    }
}
