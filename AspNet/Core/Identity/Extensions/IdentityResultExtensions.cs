namespace RaptorUtils.AspNet.Identity.Extensions;

using System.Diagnostics.Contracts;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// Extension methods for <see cref="IdentityResult"/>.
/// </summary>
public static class IdentityResultExtensions
{
    /// <summary>
    /// Creates a <see cref="ValidationProblem"/> from the specified <paramref name="result"/>.
    /// </summary>
    /// <param name="result">The <see cref="IdentityResult"/> containing the errors.</param>
    /// <returns>A <see cref="ValidationProblem"/> containing the identity errors.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="result"/> represents a successful operation.
    /// </exception>
    [Pure]
    public static ValidationProblem ToValidationProblem(this IdentityResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result.Succeeded)
        {
            throw new ArgumentException("A ValidationProblem should only be created on not succeeded.");
        }

        var errorDictionary = new Dictionary<string, string[]>(1);

        foreach (IdentityError error in result.Errors)
        {
            errorDictionary[error.Code] = GetDescriptions(errorDictionary, error);
        }

        return TypedResults.ValidationProblem(errorDictionary);
    }

    [Pure]
    private static string[] GetDescriptions(
        Dictionary<string, string[]> errorDictionary,
        IdentityError error)
    {
        return errorDictionary.TryGetValue(error.Code, out string[]? descriptions)
            ? [.. descriptions, error.Description]
            : [error.Description];
    }
}
