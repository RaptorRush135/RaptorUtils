namespace RaptorUtils.AspNet.Identity;

using Microsoft.AspNetCore.Identity;

using NGuid;

/// <summary>
/// Provides functionality to generate strongly typed role instances of type <typeparamref name="TRole"/>.
/// Roles are created deterministically from a given namespace identifier and optional role name normalizer.
/// </summary>
/// <typeparam name="TRole">
/// The role type to generate. Must derive from <see cref="IdentityRole{TKey}"/>
/// and have a public parameterless constructor.
/// </typeparam>
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
public class IdentityRoleGenerator<TRole, TKey>(
    Guid namespaceId,
    Func<Guid, TKey> keyConverter,
    ILookupNormalizer? normalizer = null)
    where TRole : IdentityRole<TKey>, new()
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Gets the normalizer used for role name normalization.
    /// Defaults to <see cref="UpperInvariantLookupNormalizer"/> when no normalizer was provided.
    /// </summary>
    public ILookupNormalizer Normalizer => normalizer ??= new UpperInvariantLookupNormalizer();

    /// <summary>
    /// Generates a collection of roles of type <typeparamref name="TRole"/>
    /// based on the names of an <see langword="enum"/> type.
    /// </summary>
    /// <typeparam name="TEnum">
    /// The <see langword="enum"/> type whose names are used to generate roles.
    /// </typeparam>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <typeparamref name="TRole"/> objects,
    /// each corresponding to a value name in the specified enumeration.
    /// </returns>
    public IEnumerable<TRole> Generate<TEnum>()
        where TEnum : struct, Enum
    {
        return this.Generate(Enum.GetNames<TEnum>());
    }

    /// <summary>
    /// Generates a collection of roles of type <typeparamref name="TRole"/>
    /// from the specified collection of role names.
    /// </summary>
    /// <param name="names">
    /// A collection of role names for which to generate <typeparamref name="TRole"/> instances.
    /// </param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <typeparamref name="TRole"/> objects,
    /// one for each specified name.
    /// </returns>
    public IEnumerable<TRole> Generate(params IEnumerable<string> names)
    {
        return names.Select(name => this.Generate(name));
    }

    /// <summary>
    /// Generates a single role of type <typeparamref name="TRole"/> for a given role name.
    /// </summary>
    /// <param name="name">The role name to generate.</param>
    /// <returns>
    /// A <typeparamref name="TRole"/> with a deterministic identifier,
    /// the specified name, and its normalized equivalent.
    /// </returns>
    public TRole Generate(string name)
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
