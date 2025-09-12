namespace RaptorUtils.EntityFramework.Metadata.Extensions;

using Microsoft.EntityFrameworkCore.Metadata;

/// <summary>
/// Provides extension methods for entity/property configuration.
/// </summary>
public static class EntityFrameworkMetadataExtensions
{
    /// <summary>
    /// Applies the specified <paramref name="convention"/> action to each entity type in the collection.
    /// </summary>
    /// <param name="entityTypes">
    /// The collection of entity types to configure.
    /// </param>
    /// <param name="convention">
    /// An action that applies a convention to an individual <see cref="IMutableEntityType"/>.
    /// </param>
    public static void Configure(
        this IEnumerable<IMutableEntityType> entityTypes,
        Action<IMutableEntityType> convention)
    {
        foreach (var entityType in entityTypes)
        {
            convention(entityType);
        }
    }

    /// <summary>
    /// Applies the specified <paramref name="convention"/> action to each property in the collection.
    /// </summary>
    /// <param name="propertyTypes">
    /// The collection of properties to configure.
    /// </param>
    /// <param name="convention">
    /// An action that applies a convention to an individual <see cref="IMutableProperty"/>.
    /// </param>
    public static void Configure(
        this IEnumerable<IMutableProperty> propertyTypes,
        Action<IMutableProperty> convention)
    {
        foreach (var propertyType in propertyTypes)
        {
            convention(propertyType);
        }
    }
}
