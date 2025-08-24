namespace RaptorUtils.AspNet.Identity.Tokens;

/// <summary>
/// Defines methods for encoding and decoding tokens in a URL-safe format.
/// This is typically used for tokens that need to be transmitted via query strings or URLs
/// without introducing characters that are unsafe for URLs.
/// </summary>
public interface ITokenUrlEncoder
{
    /// <summary>
    /// Encodes the given <paramref name="token"/> into a URL-safe string.
    /// </summary>
    /// <param name="token">
    /// The token to encode.
    /// </param>
    /// <returns>
    /// A URL-safe encoded representation of the token.
    /// </returns>
    string Encode(string token);

    /// <summary>
    /// Decodes a previously URL-safe encoded token back into its original form.
    /// </summary>
    /// <param name="encodedToken">
    /// The URL-safe encoded token to decode.
    /// </param>
    /// <returns>
    /// The original token before URL encoding.
    /// </returns>
    /// <exception cref="FormatException">
    /// Thrown when the <paramref name="encodedToken"/> is not in a valid format.
    /// </exception>
    string Decode(string encodedToken);
}
