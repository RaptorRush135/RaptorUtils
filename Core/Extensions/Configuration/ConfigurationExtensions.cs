namespace RaptorUtils.Extensions.Configuration;

using Microsoft.Extensions.Configuration;

public static class ConfigurationExtensions
{
    public static bool IsEnabled(this IConfiguration configuration, string key)
    {
        return configuration.IsEnabled(key, false);
    }

    public static bool IsEnabled(this IConfiguration configuration, string key, bool defaultValue)
    {
        return configuration.GetEnabledState(key) ?? defaultValue;
    }

    public static bool? GetEnabledState(this IConfiguration configuration, string key)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(key);

        string? value = configuration[key.Trim()];

        return value is null
            ? null
            : IsEnabledString(value);
    }

    public static string GetRequired(this IConfiguration configuration, string key)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(key);

        return configuration[key]
            ?? throw new InvalidOperationException($"Required key '{key}' not found.");
    }

    public static string GetRequiredConnectionString(this IConfiguration configuration, string name)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(name);

        return configuration.GetConnectionString(name)
            ?? throw new InvalidOperationException($"Required connection string '{name}' not found.");
    }

    private static bool IsEnabledString(string? value)
    {
        return value is "1"
            || string.Equals(value, "true", StringComparison.InvariantCultureIgnoreCase);
    }
}
