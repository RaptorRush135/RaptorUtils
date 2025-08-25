namespace RaptorUtils.Net.Http.Extensions;

using RaptorUtils.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="HttpClient"/> class.
/// </summary>
public static class HttpClientExtensions
{
    /// <summary>
    /// Appends path segments to the <see cref="HttpClient.BaseAddress"/>.
    /// </summary>
    /// <param name="client">
    /// The <see cref="HttpClient"/> whose <see cref="HttpClient.BaseAddress"/> will be updated.
    /// </param>
    /// <param name="paths">
    /// The path segments to append to the current <see cref="HttpClient.BaseAddress"/>.
    /// </param>
    /// <remarks>
    /// If <see cref="HttpClient.BaseAddress"/> is <see langword="null"/>, the method does nothing.
    /// </remarks>
    public static void AppendToBaseAddress(
        this HttpClient client,
        params ReadOnlySpan<string?> paths)
    {
        ArgumentNullException.ThrowIfNull(client);

        client.BaseAddress = client.BaseAddress?.Append(paths: paths);
    }
}
