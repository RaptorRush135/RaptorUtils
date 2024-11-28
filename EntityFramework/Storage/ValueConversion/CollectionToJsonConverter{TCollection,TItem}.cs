namespace RaptorUtils.EntityFramework.Storage.ValueConversion;

using System.Text.Json;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#pragma warning disable S3427

/// <summary>
/// Converts a collection of type <typeparamref name="TCollection"/> to and from its JSON representation.
/// </summary>
/// <typeparam name="TCollection">
/// The type of the collection being converted. Must implement <see cref="IEnumerable{TItem}"/>.
/// </typeparam>
/// <typeparam name="TItem">
/// The type of items contained within the collection.
/// </typeparam>
/// <param name="serializeOptions">
/// The <see cref="JsonSerializerOptions"/> used for serialization.
/// </param>
/// <param name="deserializeOptions">
/// The <see cref="JsonSerializerOptions"/> used for deserialization.
/// </param>
/// <param name="mappingHints">
/// Optional <see cref="ConverterMappingHints"/> to influence conversion behavior.
/// </param>
public class CollectionToJsonConverter<TCollection, TItem>(
    JsonSerializerOptions? serializeOptions = null,
    JsonSerializerOptions? deserializeOptions = null,
    ConverterMappingHints? mappingHints = null)
    : ValueConverter<TCollection, string>(
        v => JsonSerializer.Serialize(v, serializeOptions),
        v => JsonSerializer.Deserialize<TCollection>(v, deserializeOptions)!,
        mappingHints)
    where TCollection : IEnumerable<TItem>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CollectionToJsonConverter{TCollection, TItem}"/> class
    /// with default serialization and deserialization settings.
    /// </summary>
    public CollectionToJsonConverter()
        : this(null)
    {
    }
}
