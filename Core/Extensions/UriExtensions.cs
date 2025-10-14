namespace RaptorUtils.Extensions;

using System.Text;

using RaptorUtils.CodeAnalysis;

/// <summary>
/// Provides extension methods for the <see cref="Uri"/> class.
/// </summary>
public static class UriExtensions
{
    /// <summary>
    /// Appends path segments to the given <see cref="Uri"/>.
    /// </summary>
    /// <param name="uri">The base URI to append to.</param>
    /// <param name="withTrailingSlash">
    /// Whether to append a trailing slash to the end of the resulting path.
    /// </param>
    /// <param name="paths">Path segments to append.</param>
    /// <returns>A new <see cref="Uri"/> with the additional path segments appended.</returns>
    [MustUseReturnValue]
    [PerformanceSensitive]
    public static Uri Append(this Uri uri, bool withTrailingSlash = true, params ReadOnlySpan<string?> paths)
    {
        ArgumentNullException.ThrowIfNull(uri);

        var pathBuilder = new StringBuilder(uri.AbsolutePath.TrimEnd('/'));

        foreach (string? path in paths)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                continue;
            }

            var pathSpan = path.AsSpan().Trim().Trim('/');
            foreach (Range pathRange in pathSpan.Split('/'))
            {
                pathBuilder.Append('/');
                string encoded = Uri.EscapeDataString(pathSpan[pathRange]);
                pathBuilder.Append(encoded);
            }
        }

        if (withTrailingSlash)
        {
            pathBuilder.Append('/');
        }

        var builder = new UriBuilder(uri)
        {
            Path = pathBuilder.ToString(),
        };

        return builder.Uri;
    }
}
