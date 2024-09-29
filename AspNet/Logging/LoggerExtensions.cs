namespace RaptorUtils.AspNet.Logging;

using Microsoft.Extensions.Logging;

public static class LoggerExtensions
{
    public static ConditionalLogContext? Try(this ILogger logger, LogLevel logLevel)
    {
        if (!logger.IsEnabled(logLevel))
        {
            return null;
        }

        return new ConditionalLogContext(logger, logLevel);
    }

    public static ConditionalLogContext? TryTrace(this ILogger logger) => logger.Try(LogLevel.Trace);

    public static ConditionalLogContext? TryDebug(this ILogger logger) => logger.Try(LogLevel.Debug);

    public static ConditionalLogContext? TryInformation(this ILogger logger) => logger.Try(LogLevel.Information);

    public static ConditionalLogContext? TryWarning(this ILogger logger) => logger.Try(LogLevel.Warning);

    public static ConditionalLogContext? TryError(this ILogger logger) => logger.Try(LogLevel.Error);

    public static ConditionalLogContext? TryCritical(this ILogger logger) => logger.Try(LogLevel.Critical);
}
