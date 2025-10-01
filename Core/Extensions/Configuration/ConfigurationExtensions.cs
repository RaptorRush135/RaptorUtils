namespace RaptorUtils.Extensions.Configuration;

using System.Diagnostics.Contracts;

using Microsoft.Extensions.Configuration;

/// <summary>
/// Provides extension methods for simplifying common configuration access patterns.
/// </summary>
public static class ConfigurationExtensions
{
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
        where T : class
    {
        ArgumentNullException.ThrowIfNull(configuration);

        return configuration.Get<T>()
            ?? throw BindException<T>();
    }

    /// <summary>
    /// Retrieves a required <paramref name="configuration"/> value based on a <paramref name="key"/>.
    /// <para>
    /// Throws an exception if the <paramref name="key"/> does not exist.
    /// </para>
    /// </summary>
    /// <param name="configuration">The configuration object to query.</param>
    /// <param name="key">The configuration key to retrieve.</param>
    /// <returns>
    /// The <paramref name="configuration"/> value associated with the specified <paramref name="key"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the <paramref name="key"/> is not found.
    /// </exception>
    public static string GetRequiredValue(this IConfiguration configuration, string key)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(key);

        return configuration[key]
            ?? throw KeyNotFoundException(key);
    }

    /// <summary>
    /// Retrieves a required <paramref name="configuration"/> value based on a <paramref name="key"/>.
    /// </summary>
    /// <typeparam name="T">The type to bind the configuration section to.</typeparam>
    /// <param name="configuration">The configuration object to query.</param>
    /// <param name="key">The configuration key to retrieve.</param>
    /// <returns>An object of type <typeparamref name="T"/> with the configuration values bound to it.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the <paramref name="key"/> is not found
    /// or if the configuration could not be bound to the specified type.
    /// </exception>
    public static T GetRequiredValue<T>(this IConfiguration configuration, string key)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(key);

        IConfigurationSection section = configuration.GetSection(key);
        if (!section.Exists())
        {
            throw KeyNotFoundException(key);
        }

        return section.Get<T>()
            ?? throw BindException<T>();
    }

    /// <summary>
    /// Shorthand for GetSection("ConnectionStrings")[name].
    /// <para>
    /// Throws an exception if the connection string is not found.
    /// </para>
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
    /// Retrieves the value of a service endpoint from configuration.
    /// <para>
    /// Follows the .NET Aspire convention for service endpoint keys:
    /// <c>services__{serviceName}__{endpointName}__{index}</c>.
    /// </para>
    /// </summary>
    /// <param name="configuration">The configuration object to query.</param>
    /// <param name="serviceName">The name of the service.</param>
    /// <param name="endpointName">The name of the endpoint within the service.</param>
    /// <param name="index">
    /// The zero-based index of the endpoint.
    /// Defaults to 0 for the first endpoint.
    /// </param>
    /// <returns>
    /// The endpoint value if found; otherwise <see langword="null"/>.
    /// </returns>
    public static string? GetServiceEndpoint(
        this IConfiguration configuration,
        string serviceName,
        string endpointName,
        int index = 0)
    {
        string key = GetServiceEndpointKey(serviceName, endpointName, index);
        return configuration[key];
    }

    /// <summary>
    /// Retrieves the value of a required service endpoint from configuration.
    /// <para>
    /// Follows the .NET Aspire convention for service endpoint keys:
    /// <c>services__{serviceName}__{endpointName}__{index}</c>.
    /// </para>
    /// </summary>
    /// <param name="configuration">The configuration object to query.</param>
    /// <param name="serviceName">The name of the service.</param>
    /// <param name="endpointName">The name of the endpoint within the service.</param>
    /// <param name="index">
    /// The zero-based index of the endpoint.
    /// Defaults to 0 for the first endpoint.
    /// </param>
    /// <returns>The endpoint value.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the endpoint value is not found in the configuration.
    /// </exception>
    public static string GetRequiredServiceEndpoint(
        this IConfiguration configuration,
        string serviceName,
        string endpointName,
        int index = 0)
    {
        string key = GetServiceEndpointKey(serviceName, endpointName, index);
        return configuration[key]
            ?? throw KeyNotFoundException(key);
    }

    [Pure]
    private static string GetServiceEndpointKey(
        string serviceName,
        string endpointName,
        int index = 0)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(serviceName);
        ArgumentException.ThrowIfNullOrWhiteSpace(endpointName);
        ArgumentOutOfRangeException.ThrowIfNegative(0);

        return $"services:{serviceName}:{endpointName}:{index}";
    }

    [Pure]
    private static InvalidOperationException BindException<T>()
        => new($"Configuration could not be bound to type {typeof(T).FullName}.");

    [Pure]
    private static InvalidOperationException KeyNotFoundException(string key)
       => new($"Required key '{key}' not found.");
}
