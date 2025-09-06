namespace RaptorUtils.AspNet.Identity.Api.Login.Results;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

/// <summary>
/// Represents a login failure, optionally including an associated HTTP status code.
/// </summary>
public sealed class LoginError
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginError"/> class.
    /// </summary>
    /// <param name="detail">A human-readable description of the login error.</param>
    /// <param name="statusCode">
    /// An optional HTTP status code representing the failure.
    /// Must be between 400 and 499 if provided.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="detail"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="statusCode"/> is outside the range 400–499.
    /// </exception>
    public LoginError(
        string detail,
        int? statusCode = null)
    {
        ArgumentNullException.ThrowIfNull(detail);

        if (statusCode.HasValue && statusCode is < 400 or > 499)
        {
            throw new ArgumentOutOfRangeException(
                nameof(statusCode),
                "Status code must be between 400-499.");
        }

        this.Detail = detail;
        this.StatusCode = statusCode;
    }

    /// <summary>
    /// Gets a generic login failure instance with the default message "Failed".
    /// </summary>
    public static LoginError Failed { get; } = new("Failed");

    /// <summary>
    /// Gets a description of the login error.
    /// </summary>
    public string? Detail { get; }

    /// <summary>
    /// Gets the associated HTTP status code, if provided.
    /// </summary>
    public int? StatusCode { get; }

    /// <summary>
    /// Converts this <see cref="LoginError"/> into a <see cref="ProblemHttpResult"/>.
    /// </summary>
    /// <param name="defaultStatus">
    /// The status code to use if <see cref="StatusCode"/> is <see langword="null"/>.
    /// </param>
    /// <returns>
    /// A <see cref="ProblemHttpResult"/> representing this error,
    /// with <see cref="StatusCode"/> or <paramref name="defaultStatus"/> as its HTTP status.
    /// </returns>
    public ProblemHttpResult ToProblemHttpResult(int defaultStatus)
    {
        return TypedResults.Problem(
            this.Detail,
            statusCode: this.StatusCode ?? defaultStatus);
    }
}
