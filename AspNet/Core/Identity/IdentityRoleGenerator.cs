namespace RaptorUtils.AspNet.Identity;

using Microsoft.AspNetCore.Identity;

using NGuid;

/// <summary>
/// A specialized <see cref="IdentityRoleGenerator{TKey}"/> that generates roles
/// using <see cref="Guid"/> as the role identifier type.
/// </summary>
public class IdentityRoleGenerator(
    Guid namespaceId,
    ILookupNormalizer? normalizer = null)
    : IdentityRoleGenerator<Guid>(
        namespaceId,
        id => id,
        normalizer);
