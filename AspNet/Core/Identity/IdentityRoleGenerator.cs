namespace RaptorUtils.AspNet.Identity;

using Microsoft.AspNetCore.Identity;

using NGuid;

/// <summary>
/// Provides functionality to generate <see cref="IdentityRole{TKey}"/> instances
/// from a given namespace and optional role name normalizer.
/// </summary>
/// <typeparam name="TKey">
/// The type of the role identifier. Must implement <see cref="IEquatable{T}"/>.
/// </typeparam>
/// <param name="namespaceId">
/// A unique <see cref="Guid"/> used as the namespace for generating deterministic role identifiers.
/// </param>
/// <param name="keyConverter">
/// A delegate that converts the generated <see cref="Guid"/>
/// into the role identifier of type <typeparamref name="TKey"/>.
/// </param>
/// <param name="normalizer">
/// An optional <see cref="ILookupNormalizer"/> instance for normalizing role names.
/// If not provided, <see cref="UpperInvariantLookupNormalizer"/> is used by default.
/// </param>
public class IdentityRoleGenerator<TKey>(
    Guid namespaceId,
    Func<Guid, TKey> keyConverter,
    ILookupNormalizer? normalizer = null)
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Gets the normalizer used for role name normalization.
    /// Defaults to <see cref="UpperInvariantLookupNormalizer"/> when no normalizer was provided.
    /// </summary>
    public ILookupNormalizer Normalizer => normalizer ??= new UpperInvariantLookupNormalizer();

    /// <summary>
    /// Generates a collection of <see cref="IdentityRole{TKey}"/> instances
    /// based on the names of an <see langword="enum"/> type.
    /// </summary>
    /// <typeparam name="TEnum">
    /// The <see langword="enum"/> type whose names are used to generate roles.
    /// </typeparam>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <see cref="IdentityRole{TKey}"/> objects,
    /// each corresponding to a value name in the specified enumeration.
    /// </returns>
    public IEnumerable<IdentityRole<TKey>> Generate<TEnum>()
        where TEnum : struct, Enum
    {
        return this.Generate(Enum.GetNames<TEnum>());
    }

    /// <summary>
    /// Generates a collection of <see cref="IdentityRole{TKey}"/> instances
    /// from the specified role names.
    /// </summary>
    /// <param name="names">
    /// An array of role names for which to generate <see cref="IdentityRole{TKey}"/> instances.
    /// </param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <see cref="IdentityRole{TKey}"/> objects,
    /// one for each specified name.
    /// </returns>
    public IEnumerable<IdentityRole<TKey>> Generate(params string[] names)
    {
        return this.Generate((IEnumerable<string>)names);
    }

    /// <summary>
    /// Generates a collection of <see cref="IdentityRole{TKey}"/> instances
    /// from the specified collection of role names.
    /// </summary>
    /// <param name="names">
    /// A collection of role names for which to generate <see cref="IdentityRole{TKey}"/> instances.
    /// </param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <see cref="IdentityRole{TKey}"/> objects,
    /// one for each specified name.
    /// </returns>
    public IEnumerable<IdentityRole<TKey>> Generate(IEnumerable<string> names)
    {
        return names.Select(name => this.Generate(name));
    }

    /// <summary>
    /// Generates a single <see cref="IdentityRole{TKey}"/> instance for a given role name.
    /// </summary>
    /// <param name="name">The role name to generate.</param>
    /// <returns>
    /// An <see cref="IdentityRole{TKey}"/> with a deterministic identifier,
    /// the specified name, and its normalized equivalent.
    /// </returns>
    public IdentityRole<TKey> Generate(string name)
    {
        Guid id = GuidHelpers.CreateFromName(namespaceId, name);
        string normalizedName = this.Normalizer.NormalizeName(name);

        return new()
        {
            Id = keyConverter.Invoke(id),
            Name = name,
            NormalizedName = normalizedName,
        };
    }
}
