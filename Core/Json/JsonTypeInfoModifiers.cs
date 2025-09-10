namespace RaptorUtils.Json;

using System.Text.Json.Serialization.Metadata;

/// <summary>
/// Provides helpers for modifying <see cref="JsonTypeInfo"/> instances
/// used in <see cref="System.Text.Json"/> serialization.
/// </summary>
public static class JsonTypeInfoModifiers
{
    /// <summary>
    /// Marks all properties in the specified <see cref="JsonTypeInfo"/>
    /// as optional by setting <see cref="JsonPropertyInfo.IsRequired"/> to <see langword="false"/>.
    /// </summary>
    /// <param name="typeInfo">
    /// The <see cref="JsonTypeInfo"/> whose property metadata will be modified.
    /// </param>
    public static void MakeAllPropertiesOptional(JsonTypeInfo typeInfo)
    {
        foreach (var property in typeInfo.Properties)
        {
            property.IsRequired = false;
        }
    }
}
