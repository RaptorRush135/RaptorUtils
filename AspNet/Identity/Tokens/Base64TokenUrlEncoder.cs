namespace RaptorUtils.AspNet.Identity.Tokens;

using System.Buffers;
using System.Buffers.Text;
using System.Text;

/// <summary>
/// Provides a URL-safe Base64 implementation of <see cref="ITokenUrlEncoder"/>.
/// </summary>
public sealed class Base64TokenUrlEncoder : ITokenUrlEncoder
{
    private const int MaxStackBufferSize = 256;

    /// <summary>
    /// Encodes the specified <paramref name="token"/> into a URL-safe Base64 string.
    /// </summary>
    /// <param name="token">
    /// The token to encode.
    /// </param>
    /// <returns>
    /// A URL-safe Base64 encoded representation of the token.
    /// </returns>
    public string Encode(string token)
    {
        int byteCount = Encoding.UTF8.GetByteCount(token);

        if (byteCount <= MaxStackBufferSize)
        {
            Span<byte> stackBuffer = stackalloc byte[MaxStackBufferSize];

            return EncodeToBase64(token, stackBuffer[..byteCount]);
        }

        return ExecuteWithPooledArray(EncodeToBase64, token, byteCount);

        static string EncodeToBase64(string token, Span<byte> buffer)
        {
            Encoding.UTF8.GetBytes(token, buffer);
            return Base64Url.EncodeToString(buffer);
        }
    }

    /// <summary>
    /// Decodes a URL-safe Base64 <paramref name="encodedToken"/> back to its original string.
    /// </summary>
    /// <param name="encodedToken">
    /// The URL-safe Base64 encoded token to decode.
    /// </param>
    /// <returns>
    /// The original token string before encoding.
    /// </returns>
    /// <exception cref="FormatException">
    /// Thrown when the <paramref name="encodedToken"/> is not a valid Base64 string.
    /// </exception>
    public string Decode(string encodedToken)
    {
        int byteCount = Base64Url.GetMaxDecodedLength(encodedToken.Length);

        if (byteCount <= MaxStackBufferSize)
        {
            Span<byte> stackBuffer = stackalloc byte[MaxStackBufferSize];
            return DecodeFromBase64(encodedToken, stackBuffer[..byteCount]);
        }

        return ExecuteWithPooledArray(DecodeFromBase64, encodedToken, byteCount);

        static string DecodeFromBase64(string encoded, Span<byte> buffer)
        {
            int written = Base64Url.DecodeFromChars(encoded, buffer);
            return Encoding.UTF8.GetString(buffer[..written]);
        }
    }

    private static string ExecuteWithPooledArray(
        Func<string, Span<byte>, string> action,
        string token,
        int byteCount)
    {
        byte[] pooledArray = ArrayPool<byte>.Shared.Rent(byteCount);

        try
        {
            return action(token, pooledArray.AsSpan(0, byteCount));
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(pooledArray, clearArray: true);
        }
    }
}
