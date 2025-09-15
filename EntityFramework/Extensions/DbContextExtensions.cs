namespace RaptorUtils.EntityFramework.Extensions;

using System.Runtime.CompilerServices;

using Microsoft.EntityFrameworkCore;

using RaptorUtils.Collections.Extensions;
using RaptorUtils.Common;
using RaptorUtils.Results;

/// <summary>
/// Provides extension methods for <see cref="DbContext"/> to simplify common database operations.
/// </summary>
public static class DbContextExtensions
{
    /// <summary>
    /// Synchronizes a collection with the requested IDs by removing entities not in the requested list
    /// and adding missing entities from the database.
    /// </summary>
    /// <typeparam name="T">The entity type that implements IIdentifiable.</typeparam>
    /// <typeparam name="TId">The ID type.</typeparam>
    /// <param name="dbContext">The database context.</param>
    /// <param name="collection">The collection to synchronize.</param>
    /// <param name="requestedIds">The IDs that should be present in the collection.</param>
    /// <param name="queryCustomizer">
    /// An optional function to customize the query used to fetch missing entities, e.g., adding includes.
    /// </param>
    /// <param name="paramName">The parameter name for validation error reporting.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure (contained invalid IDs).</returns>
    public static async Task<ValidationResult> TrySyncCollection<T, TId>(
        this DbContext dbContext,
        IList<T> collection,
        IReadOnlyList<TId> requestedIds,
        Func<IQueryable<T>, IQueryable<T>>? queryCustomizer = null,
        [CallerArgumentExpression(nameof(requestedIds))] string paramName = null!)
        where T : class, IIdentifiable<TId>
        where TId : notnull
    {
        var currentIds = collection.Select(e => e.Id);

        collection.RemoveAll(e => !requestedIds.Contains(e.Id));

        var missingIds = requestedIds.Except(currentIds).ToList();

        IQueryable<T> query = dbContext.Set<T>().AsTracking();

        if (queryCustomizer is not null)
        {
            query = queryCustomizer(query);
        }

        var missingEntities = await query
            .Where(e => missingIds.Contains(e.Id))
            .ToListAsync();

        var foundIds = missingEntities.Select(e => e.Id);

        var invalidIds = missingIds.Except(foundIds);

        if (invalidIds.Any())
        {
            return ValidationResult.Failed((paramName, "Invalid reference."));
        }

        collection.AddRange(missingEntities);

        return ValidationResult.Success;
    }
}
