namespace RaptorUtils.AspNet.Logging.Filters;

using Serilog.Core;
using Serilog.Events;

/// <summary>
/// A filter that allows log events to be processed only if the request path associated with the event
/// matches a given predicate, or if their level is greater than or equal to a specified minimum level.
/// </summary>
/// <param name="pathFilter">
/// A predicate function used to evaluate the request path (from the log event's "RequestPath" property).
/// If the log event's level is below the minimum level, this predicate determines whether the event is logged.
/// </param>
/// <param name="minimumLevel">
/// The minimum log level required for a log event to be automatically logged regardless of its request path.
/// </param>
public class RequestPathLogFilter(
    Predicate<string> pathFilter,
    LogEventLevel minimumLevel = LogEventLevel.Warning)
    : ILogEventFilter
{
    /// <summary>
    /// Determines whether the specified log event should be logged.
    /// Events at or above the configured minimum level are always logged.
    /// Events below the minimum level are logged only if their "RequestPath" property
    /// satisfies the provided path predicate.
    /// If the "RequestPath" property is missing, the event is logged by default.
    /// </summary>
    /// <param name="logEvent">The log event to evaluate.</param>
    /// <returns>
    /// <see langword="true"/> if the log event should be logged; otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsEnabled(LogEvent logEvent)
    {
        if (logEvent.Level >= minimumLevel
            || !logEvent.Properties.TryGetValue("RequestPath", out var path))
        {
            return true;
        }

        return pathFilter.Invoke(path.ToString());
    }
}
