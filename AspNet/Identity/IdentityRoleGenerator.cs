namespace RaptorUtils.AspNet.Identity;

using Microsoft.AspNetCore.Identity;

using NGuid;

/// <summary>
/// Generates <see cref="IdentityRole"/> instances based on a given namespace and optional name normalizer.
/// </summary>
/// <param name="namespaceId">A unique identifier used as the namespace for generating role ids.</param>
/// <param name="normalizer">
/// An optional <see cref="ILookupNormalizer"/> instance for normalizing role names.
/// If not provided, <see cref="UpperInvariantLookupNormalizer"/> is used by default.
/// </param>
public class IdentityRoleGenerator(
    Guid namespaceId,
    ILookupNormalizer? normalizer = null)
{
    /// <summary>
    /// Gets the normalizer used for normalizing role names.
    /// Defaults to <see cref="UpperInvariantLookupNormalizer"/> if no normalizer was provided.
    /// </summary>
    public ILookupNormalizer Normalizer => normalizer ??= new UpperInvariantLookupNormalizer();

    /// <summary>
    /// Generates a collection of <see cref="IdentityRole"/> instances based on the names of an enumeration type.
    /// </summary>
    /// <typeparam name="TEnum">
    /// The enumeration type whose names are used to generate roles. Must be a struct representing an <see langword="enum"/>.
    /// </typeparam>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <see cref="IdentityRole"/> objects,
    /// each corresponding to a name in the specified enumeration.
    /// </returns>
    public IEnumerable<IdentityRole> Generate<TEnum>()
        where TEnum : struct, Enum
    {
        return this.Generate(Enum.GetNames<TEnum>());
    }

    /// <summary>
    /// Generates a collection of <see cref="IdentityRole"/> instances based on the specified role names.
    /// </summary>
    /// <param name="names">An array of role names to generate <see cref="IdentityRole"/> instances for.</param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <see cref="IdentityRole"/> objects, each corresponding to a specified name.
    /// </returns>
    public IEnumerable<IdentityRole> Generate(params string[] names)
    {
        return this.Generate((IEnumerable<string>)names);
    }

    /// <summary>
    /// Generates a collection of <see cref="IdentityRole"/> instances based on the specified collection of role names.
    /// </summary>
    /// <param name="names">A collection of role names to generate <see cref="IdentityRole"/> instances for.</param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <see cref="IdentityRole"/> objects, each corresponding to a specified name.
    /// </returns>
    public IEnumerable<IdentityRole> Generate(IEnumerable<string> names)
    {
        return names.Select(name => this.Generate(name));
    }

    /// <summary>
    /// Generates a single <see cref="IdentityRole"/> instance for a given role name.
    /// </summary>
    /// <param name="name">The name of the role to generate.</param>
    /// <returns>
    /// An <see cref="IdentityRole"/> object with a unique id, the specified name, and a normalized name.
    /// </returns>
    public IdentityRole Generate(string name)
    {
        Guid id = GuidHelpers.CreateFromName(namespaceId, name);
        string normalizedName = this.Normalizer.NormalizeName(name);

        return new()
        {
            Id = id.ToString(),
            Name = name,
            NormalizedName = normalizedName,
        };
    }
}
