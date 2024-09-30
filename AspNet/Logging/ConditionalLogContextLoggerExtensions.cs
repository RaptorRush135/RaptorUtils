namespace RaptorUtils.AspNet.Logging;

using Microsoft.Extensions.Logging;

/// <summary>
/// Provides extension methods for logging that enable conditional logging based on log levels.
/// </summary>
public static class ConditionalLogContextLoggerExtensions
{
    /// <summary>
    /// Attempts to create a <see cref="ConditionalLogContext"/> for the specified log level.
    /// Returns null if the logger is not enabled for the specified level.
    /// </summary>
    /// <param name="logger">The logger instance to extend.</param>
    /// <param name="logLevel">The log level to check.</param>
    /// <returns>
    /// A <see cref="ConditionalLogContext"/> if logging is enabled for the specified level; otherwise, null.
    /// </returns>
    public static ConditionalLogContext? Try(this ILogger logger, LogLevel logLevel)
    {
        if (!logger.IsEnabled(logLevel))
        {
            return null;
        }

        return new ConditionalLogContext(logger, logLevel);
    }

    /// <summary>
    /// Attempts to create a <see cref="ConditionalLogContext"/> for the Trace log level.
    /// Returns null if the logger is not enabled for Trace.
    /// </summary>
    /// <param name="logger">The logger instance to extend.</param>
    /// <returns>A <see cref="ConditionalLogContext"/> for Trace if enabled; otherwise, null.</returns>
    public static ConditionalLogContext? TryTrace(this ILogger logger) => logger.Try(LogLevel.Trace);

    /// <summary>
    /// Attempts to create a <see cref="ConditionalLogContext"/> for the Debug log level.
    /// Returns null if the logger is not enabled for Debug.
    /// </summary>
    /// <param name="logger">The logger instance to extend.</param>
    /// <returns>A <see cref="ConditionalLogContext"/> for Debug if enabled; otherwise, null.</returns>
    public static ConditionalLogContext? TryDebug(this ILogger logger) => logger.Try(LogLevel.Debug);

    /// <summary>
    /// Attempts to create a <see cref="ConditionalLogContext"/> for the Information log level.
    /// Returns null if the logger is not enabled for Information.
    /// </summary>
    /// <param name="logger">The logger instance to extend.</param>
    /// <returns>A <see cref="ConditionalLogContext"/> for Information if enabled; otherwise, null.</returns>
    public static ConditionalLogContext? TryInformation(this ILogger logger) => logger.Try(LogLevel.Information);

    /// <summary>
    /// Attempts to create a <see cref="ConditionalLogContext"/> for the Warning log level.
    /// Returns null if the logger is not enabled for Warning.
    /// </summary>
    /// <param name="logger">The logger instance to extend.</param>
    /// <returns>A <see cref="ConditionalLogContext"/> for Warning if enabled; otherwise, null.</returns>
    public static ConditionalLogContext? TryWarning(this ILogger logger) => logger.Try(LogLevel.Warning);

    /// <summary>
    /// Attempts to create a <see cref="ConditionalLogContext"/> for the Error log level.
    /// Returns null if the logger is not enabled for Error.
    /// </summary>
    /// <param name="logger">The logger instance to extend.</param>
    /// <returns>A <see cref="ConditionalLogContext"/> for Error if enabled; otherwise, null.</returns>
    public static ConditionalLogContext? TryError(this ILogger logger) => logger.Try(LogLevel.Error);

    /// <summary>
    /// Attempts to create a <see cref="ConditionalLogContext"/> for the Critical log level.
    /// Returns null if the logger is not enabled for Critical.
    /// </summary>
    /// <param name="logger">The logger instance to extend.</param>
    /// <returns>A <see cref="ConditionalLogContext"/> for Critical if enabled; otherwise, null.</returns>
    public static ConditionalLogContext? TryCritical(this ILogger logger) => logger.Try(LogLevel.Critical);
}
