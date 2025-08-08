namespace RaptorUtils.AspNet.Logging.Filters;

using Serilog.Events;

/// <summary>
/// A log filter that enables logging for events where the request path starts with a specified prefix,
/// or the log level is greater than or equal to a specified minimum.
/// </summary>
/// <param name="prefix">The prefix that the request path must start with to enable logging.</param>
/// <param name="comparison">
/// The string comparison option to use when checking the prefix.
/// </param>
/// <param name="minimumLevel">
/// The minimum log level at which events are always logged, regardless of their request path.
/// </param>
public class RequestPathPrefixLogFilter(
    string prefix,
    StringComparison comparison = StringComparison.InvariantCulture,
    LogEventLevel minimumLevel = LogEventLevel.Warning)
    : RequestPathLogFilter(
        path => path.StartsWith($"\"{prefix}", comparison),
        minimumLevel);
