namespace RaptorUtils.EntityFramework.Seeding;

using System.Text.Json;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Provides functionality to seed EF Core entities from a JSON container.
/// </summary>
/// <param name="builder">
/// The <see cref="ModelBuilder"/> used to configure entity seeding.
/// </param>
/// <param name="container">
/// The <see cref="JsonElement"/> containing entity collections keyed by property name.
/// </param>
/// <param name="options">
/// Optional <see cref="JsonSerializerOptions"/> used during JSON deserialization.
/// </param>
public sealed class JsonEntitySeeder(
    ModelBuilder builder,
    JsonElement container,
    JsonSerializerOptions? options = null)
{
    /// <summary>
    /// Seeds the specified entity type using data from the JSON container.
    /// </summary>
    /// <typeparam name="T">
    /// The entity type to seed.
    /// </typeparam>
    /// <param name="name">
    /// The property name within the JSON container that contains the entity collection.
    /// </param>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when the property with the given <paramref name="name"/> does not exist in the JSON container.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the property exists but its value is <see langword="null"/>.
    /// </exception>
    public void Seed<T>(string name)
        where T : class
    {
        if (!container.TryGetProperty(name, out JsonElement value))
        {
            throw new KeyNotFoundException(
                $"Property '{name}' was not found in the JSON container.");
        }

        var data = value.Deserialize<T[]>(options)
            ?? throw new InvalidOperationException(
                $"Property '{name}' contains null value.");

        builder.Entity<T>().HasData(data);
    }
}
