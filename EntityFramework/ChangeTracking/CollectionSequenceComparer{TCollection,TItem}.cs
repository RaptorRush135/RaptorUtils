namespace RaptorUtils.EntityFramework.ChangeTracking;

using Microsoft.EntityFrameworkCore.ChangeTracking;

/// <summary>
/// Compares two collections of type <typeparamref name="TCollection"/>
/// for equality by comparing their sequence of elements.
/// </summary>
/// <typeparam name="TCollection">
/// The type of the collection being compared. Must implement <see cref="IEnumerable{TItem}"/>.
/// </typeparam>
/// <typeparam name="TItem">
/// The type of the items contained within the collection.
/// </typeparam>
public class CollectionSequenceComparer<TCollection, TItem>()
    : ValueComparer<TCollection>(
        (c1, c2) => c1!.SequenceEqual(c2!),
        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
        c => (TCollection)c.ToList().AsEnumerable())
    where TCollection : IEnumerable<TItem>;
