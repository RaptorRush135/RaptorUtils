namespace RaptorUtils.AspNet.Logging.Filters;

public class RequestPathPrefixLogFilter(
    string prefix,
    StringComparison comparison = StringComparison.InvariantCulture)
    : RequestPathLogFilter(path => path.StartsWith($"\"{prefix}", comparison));
