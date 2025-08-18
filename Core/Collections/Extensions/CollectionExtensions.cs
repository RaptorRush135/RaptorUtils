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
    /// Thrown if either <paramref name="destination"/> or <paramref name="collection"/> is <see langword="null"/>.
    /// </exception>
    public static void AddRange<T>(this ICollection<T> destination, params IEnumerable<T> collection)
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

    /// <summary>
    /// Removes all elements from the specified <see cref="ICollection{T}"/>
    /// that match the conditions defined by the provided predicate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="destination">The collection from which elements will be removed.</param>
    /// <param name="predicate">
    /// A function that defines the conditions of the elements to remove.
    /// The function should return <see langword="true"/> for elements that should be removed,
    /// and <see langword="false"/> otherwise.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="destination"/> or <paramref name="predicate"/> is <see langword="null"/>.
    /// </exception>
    public static void RemoveAll<T>(this ICollection<T> destination, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(destination);
        ArgumentNullException.ThrowIfNull(predicate);

        if (destination is List<T> list)
        {
            list.RemoveAll(new Predicate<T>(predicate));
            return;
        }

        foreach (T item in destination.Where(predicate).ToList())
        {
            destination.Remove(item);
        }
    }
}
