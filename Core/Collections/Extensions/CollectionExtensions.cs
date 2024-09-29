namespace RaptorUtils.Collections.Extensions;

public static class CollectionExtensions
{
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
