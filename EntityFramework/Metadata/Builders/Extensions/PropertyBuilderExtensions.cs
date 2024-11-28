namespace RaptorUtils.EntityFramework.Metadata.Builders.Extensions;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RaptorUtils.EntityFramework.ChangeTracking;
using RaptorUtils.EntityFramework.Storage.ValueConversion;

/// <summary>
/// Provides extension methods for configuring JSON conversion and comparison
/// for collection properties in Entity Framework models.
/// </summary>
public static class PropertyBuilderExtensions
{
    /// <summary>
    /// Configures the property represented by the <see cref="PropertyBuilder{TProperty}"/>
    /// to use JSON conversion and sequence comparison for a collection of type <typeparamref name="TItem"/>.
    /// </summary>
    /// <typeparam name="TItem">
    /// The type of the items contained in the collection.
    /// </typeparam>
    /// <param name="builder">
    /// The <see cref="PropertyBuilder{TProperty}"/> for the collection property being configured.
    /// </param>
    /// <returns>
    /// The same <see cref="PropertyBuilder{TProperty}"/> instance, allowing for further configuration chaining.
    /// </returns>
    public static PropertyBuilder<ICollection<TItem>> HasJsonConversion<TItem>(
        this PropertyBuilder<ICollection<TItem>> builder)
    {
        return builder.HasJsonConversion<ICollection<TItem>, TItem>();
    }

    /// <summary>
    /// Configures the property represented by the <see cref="PropertyBuilder{TProperty}"/>
    /// to use JSON conversion and sequence comparison for a collection property of type <typeparamref name="TProperty"/>.
    /// </summary>
    /// <typeparam name="TProperty">
    /// The type of the property being configured, which must implement <see cref="IEnumerable{TItem}"/>.
    /// </typeparam>
    /// <typeparam name="TItem">
    /// The type of the items contained in the collection.
    /// </typeparam>
    /// <param name="builder">
    /// The <see cref="PropertyBuilder{TProperty}"/> for the property being configured.
    /// </param>
    /// <returns>
    /// The same <see cref="PropertyBuilder{TProperty}"/> instance, allowing for further configuration chaining.
    /// </returns>
    public static PropertyBuilder<TProperty> HasJsonConversion<TProperty, TItem>(
        this PropertyBuilder<TProperty> builder)
        where TProperty : IEnumerable<TItem>
    {
        return builder.HasConversion<
            CollectionToJsonConverter<TProperty, TItem>,
            CollectionSequenceComparer<TProperty, TItem>>();
    }
}
