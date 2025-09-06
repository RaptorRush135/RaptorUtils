namespace RaptorUtils.AspNet.Identity.Extensions;

using Microsoft.AspNetCore.Identity;

/// <summary>
/// Extension methods for <see cref="IdentityError"/>.
/// </summary>
public static class IdentityErrorExtensions
{
    /// <summary>
    /// Replaces the first occurrence of an <see cref="IdentityError"/> with the specified
    /// <paramref name="code"/> in the sequence with the provided <paramref name="replacement"/>.
    /// Subsequent errors with the same <paramref name="code"/> are skipped (not returned).
    /// </summary>
    /// <param name="errors">
    /// The sequence of <see cref="IdentityError"/> instances to process.
    /// </param>
    /// <param name="code">
    /// The error code to match against existing <see cref="IdentityError.Code"/>.
    /// </param>
    /// <param name="replacement">
    /// The <see cref="IdentityError"/> that replaces the first matching error.
    /// </param>
    /// <returns>
    /// A sequence of <see cref="IdentityError"/> instances with the first match replaced
    /// and any subsequent matches with the same <paramref name="code"/> removed.
    /// </returns>
    public static IEnumerable<IdentityError> ReplaceErrors(
        this IEnumerable<IdentityError> errors,
        string code,
        IdentityError replacement)
    {
        bool replaced = false;
        foreach (var error in errors)
        {
            if (error.Code != code)
            {
                yield return error;
                continue;
            }

            if (!replaced)
            {
                replaced = true;
                yield return replacement;
            }
        }
    }
}
