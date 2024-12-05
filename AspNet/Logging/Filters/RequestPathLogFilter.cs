namespace RaptorUtils.AspNet.Logging.Filters;

using Serilog.Core;
using Serilog.Events;

/// <summary>
/// A filter that determines whether a log event should be logged based on the request path.
/// </summary>
/// <param name="pathFilter">A predicate function to evaluate the request path.</param>
public class RequestPathLogFilter(
    Predicate<string> pathFilter)
    : ILogEventFilter
{
    /// <summary>
    /// Determines if the specified log event should be logged based on the request path filter.
    /// If the log event does not contain a request path, it is considered enabled.
    /// </summary>
    /// <param name="logEvent">The log event to evaluate.</param>
    /// <returns>
    /// <see langword="true"/> if the log event should be logged; otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsEnabled(LogEvent logEvent)
    {
        if (logEvent.Level >= LogEventLevel.Warning // TODO
            || !logEvent.Properties.TryGetValue("RequestPath", out var path))
        {
            return true;
        }

        return pathFilter.Invoke(path.ToString());
    }
}
