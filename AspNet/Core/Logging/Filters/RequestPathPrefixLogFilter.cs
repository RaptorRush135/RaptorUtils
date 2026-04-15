namespace RaptorUtils.AspNet.Logging.Filters;

using Serilog.Events;

/// <summary>
/// A log filter that conditionally enables logging based on the request path prefix,
/// or unconditionally when the log level meets or exceeds a specified minimum.
/// </summary>
/// <param name="prefix">Request path prefix to match.</param>
/// <param name="exclude">
/// If <see langword="true"/>, excludes matching paths; if <see langword="false"/>, includes only matching paths.
/// </param>
/// <param name="comparison">String comparison used for prefix matching.</param>
/// <param name="minimumLevel">
/// Log level threshold above which events are always included.
/// </param>
public class RequestPathPrefixLogFilter(
    string prefix,
    bool exclude = false,
    StringComparison comparison = StringComparison.InvariantCulture,
    LogEventLevel minimumLevel = LogEventLevel.Warning)
    : RequestPathLogFilter(
        path => path.StartsWith(prefix, comparison) != exclude,
        minimumLevel);
