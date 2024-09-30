﻿namespace RaptorUtils.Extensions.Configuration;

using Microsoft.Extensions.Configuration;

/// <summary>
/// Provides extension methods for simplifying common configuration access patterns.
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Determines whether a configuration setting is enabled, based on a specific key.
    /// If the key is not found, it defaults to false.
    /// </summary>
    /// <param name="configuration">The configuration object to query.</param>
    /// <param name="key">The configuration key to check.</param>
    /// <returns>True if the setting is enabled, otherwise false.</returns>
    public static bool IsEnabled(this IConfiguration configuration, string key)
    {
        return configuration.IsEnabled(key, false);
    }

    /// <summary>
    /// Determines whether a configuration setting is enabled, based on a specific key.
    /// If the key is not found, it returns the specified default value.
    /// </summary>
    /// <param name="configuration">The configuration object to query.</param>
    /// <param name="key">The configuration key to check.</param>
    /// <param name="defaultValue">The default value to return if the key is not found.</param>
    /// <returns>True if the setting is enabled, otherwise the provided default value.</returns>
    public static bool IsEnabled(this IConfiguration configuration, string key, bool defaultValue)
    {
        return configuration.GetEnabledState(key) ?? defaultValue;
    }

    /// <summary>
    /// Retrieves the enabled state of a configuration setting as a nullable boolean.
    /// Returns true if the value is "1" or "true" (case-insensitive), otherwise false,
    /// or null if the key does not exist.
    /// </summary>
    /// <param name="configuration">The configuration object to query.</param>
    /// <param name="key">The configuration key to check.</param>
    /// <returns>A nullable boolean representing the enabled state, or null if the key does not exist.</returns>
    public static bool? GetEnabledState(this IConfiguration configuration, string key)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(key);

        string? value = configuration[key.Trim()];

        return value is null
            ? (bool?)null
            : IsEnabledString(value);
    }

    /// <summary>
    /// Retrieves a required connection string from the configuration.
    /// Throws an exception if the connection string is not found.
    /// </summary>
    /// <param name="configuration">The configuration object to query.</param>
    /// <param name="name">The name of the connection string to retrieve.</param>
    /// <returns>The connection string value.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the connection string is not found.
    /// </exception>
    public static string GetRequiredConnectionString(this IConfiguration configuration, string name)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(name);

        return configuration.GetConnectionString(name)
            ?? throw new InvalidOperationException($"Required connection string '{name}' not found.");
    }

    /// <summary>
    /// Retrieves a required configuration value based on a key.
    /// Throws an exception if the key does not exist.
    /// </summary>
    /// <param name="configuration">The configuration object to query.</param>
    /// <param name="key">The configuration key to retrieve.</param>
    /// <returns>The configuration value associated with the specified key.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the key is not found.
    /// </exception>
    public static string GetRequired(this IConfiguration configuration, string key)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(key);

        return configuration[key]
            ?? throw new InvalidOperationException($"Required key '{key}' not found.");
    }

    /// <summary>
    /// Retrieves a required configuration section and binds it to a specified type.
    /// Throws an exception if the binding fails.
    /// </summary>
    /// <typeparam name="T">The type to bind the configuration section to.</typeparam>
    /// <param name="configuration">The configuration object to query.</param>
    /// <returns>An object of type <typeparamref name="T"/> with the configuration values bound to it.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the configuration could not be bound to the specified type.
    /// </exception>
    public static T GetRequired<T>(this IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        return configuration.Get<T>()
            ?? throw new InvalidOperationException($"Configuration could not be bound to type {typeof(T).FullName}.");
    }

    /// <summary>
    /// Helper method to determine if a string represents an enabled state.
    /// Returns true if the value is "1" or "true" (case-insensitive), otherwise false.
    /// </summary>
    /// <param name="value">The string value to evaluate.</param>
    /// <returns>True if the string represents an enabled state, otherwise false.</returns>
    private static bool IsEnabledString(string? value)
    {
        return value is "1"
            || string.Equals(value, "true", StringComparison.InvariantCultureIgnoreCase);
    }
}
