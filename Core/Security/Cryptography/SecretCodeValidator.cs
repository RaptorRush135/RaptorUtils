namespace RaptorUtils.Security.Cryptography;

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Provides functionality to store and securely validate secret codes using a hashing algorithm.
/// </summary>
/// <remarks>
/// This class enables associating secret codes with unique names and retrieving validators that check
/// whether a provided code matches the stored hash.
/// The hashing algorithm can be customized by supplying a delegate to the constructor.
/// Secret codes are stored as hashes and compared using fixed-time operations to help prevent timing attacks.
/// Thread safety is not guaranteed; concurrent access should be externally synchronized if required.
/// </remarks>
public sealed class SecretCodeValidator
{
    private readonly Func<string, byte[]> hasher;

    private readonly Dictionary<string, Lazy<byte[]>> codes = new(StringComparer.Ordinal);

    /// <summary>
    /// Initializes a new instance of the <see cref="SecretCodeValidator"/> class that uses SHA256 hashing
    /// to validate secret codes.
    /// </summary>
    public SecretCodeValidator()
        : this(code => SHA256.HashData(Encoding.UTF8.GetBytes(code)))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SecretCodeValidator"/> class with a specified hashing function.
    /// </summary>
    /// <remarks>
    /// The hasher function is expected to take a string input and return a byte array representing the hashed value.
    /// </remarks>
    /// <param name="hasher">The function used to hash the secret code. This cannot be <see langword="null"/>.</param>
    public SecretCodeValidator(Func<string, byte[]> hasher)
    {
        ArgumentNullException.ThrowIfNull(hasher);

        this.hasher = hasher;
    }

    /// <summary>
    /// Adds a new secret code associated with the specified name.
    /// </summary>
    /// <param name="name">
    /// The name of the secret code. This parameter cannot be <see langword="null"/> or empty.
    /// </param>
    /// <param name="code">
    /// The secret code to be added. This parameter cannot be <see langword="null"/> or empty.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if a secret code with the specified name already exists.
    /// </exception>
    public void Add(string name, string code)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(code);

        if (this.codes.ContainsKey(name))
        {
            throw new InvalidOperationException(
                $"A secret code with the name '{name}' has already been added.");
        }

        this.codes.Add(name, new(() => this.hasher(code)));
    }

    /// <summary>
    /// Attempts to retrieve a string validation predicate associated with the specified name.
    /// </summary>
    /// <remarks>
    /// This method uses a fixed-time comparison to prevent timing attacks when validating the string
    /// against the stored value.
    /// </remarks>
    /// <param name="name">
    /// The name of the validator to retrieve. This parameter cannot be <see langword="null"/> or empty.
    /// </param>
    /// <param name="validator">
    /// When this method returns <see langword="true"/>, contains a predicate that validates a string based on the
    /// associated value; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if a validator was found for the specified name;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetValidator(string name, [MaybeNullWhen(false)] out Predicate<string> validator)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        if (!this.codes.TryGetValue(name, out var value))
        {
            validator = null;
            return false;
        }

        validator = code => CryptographicOperations.FixedTimeEquals(this.hasher(code), value.Value);
        return true;
    }
}
