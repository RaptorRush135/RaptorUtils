namespace RaptorUtils.AspNet.Logging.Filters;

/// <summary>
/// A log filter that enables logging for events where the request path starts with a specified prefix.
/// </summary>
/// <param name="prefix">The prefix that the request path must start with to enable logging.</param>
/// <param name="comparison">
/// The string comparison option to use when checking the prefix (default is <see cref="StringComparison.InvariantCulture"/>).
/// </param>
public class RequestPathPrefixLogFilter(
    string prefix,
    StringComparison comparison = StringComparison.InvariantCulture)
    : RequestPathLogFilter(path => path.StartsWith($"\"{prefix}", comparison));
