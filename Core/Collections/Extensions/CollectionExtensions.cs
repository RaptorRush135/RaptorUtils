namespace RaptorUtils.Collections.Extensions;

/// <summary>
/// Provides extension methods for collections.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Adds the elements of the specified collection to the end of the <see cref="ICollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collection.
    /// </typeparam>
    /// <param name="destination">
    /// The collection to which the elements will be added.
    /// </param>
    /// <param name="collection">
    /// The collection whose elements should be added to the <paramref name="destination"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if either <paramref name="destination"/> or <paramref name="collection"/> is null.
    /// </exception>
    public static void AddRange<T>(this ICollection<T> destination, IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(destination);
        ArgumentNullException.ThrowIfNull(collection);

        if (destination is List<T> list)
        {
            list.AddRange(collection);
            return;
        }

        foreach (var item in collection)
        {
            destination.Add(item);
        }
    }
}
