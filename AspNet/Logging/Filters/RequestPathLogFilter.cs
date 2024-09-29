namespace RaptorUtils.AspNet.Logging.Filters;

using Serilog.Core;
using Serilog.Events;

public class RequestPathLogFilter(
    Predicate<string> pathFilter)
    : ILogEventFilter
{
    public bool IsEnabled(LogEvent logEvent)
    {
        if (!logEvent.Properties.TryGetValue("RequestPath", out var path))
        {
            return true;
        }

        return pathFilter.Invoke(path.ToString());
    }
}
