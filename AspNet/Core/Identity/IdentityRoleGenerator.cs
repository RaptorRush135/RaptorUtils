namespace RaptorUtils.AspNet.Identity;

using Microsoft.AspNetCore.Identity;

/// <summary>
/// A specialized <see cref="IdentityRoleGenerator{TRole, TKey}"/> that generates roles
/// with <see cref="Guid"/> identifiers.
/// </summary>
/// <typeparam name="TRole">
/// The role type to generate. Must derive from <see cref="IdentityRole{Guid}"/>
/// and have a public parameterless constructor.
/// </typeparam>
public class IdentityRoleGenerator<TRole>(
    Guid namespaceId,
    ILookupNormalizer? normalizer = null)
    : IdentityRoleGenerator<TRole, Guid>(
        namespaceId,
        id => id,
        normalizer)
    where TRole : IdentityRole<Guid>, new();
