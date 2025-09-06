namespace RaptorUtils.AspNet.Identity.Api.Data;

/// <summary>
/// Represents a request containing user credentials, specifically
/// an email address and a password, typically used for authentication.
/// </summary>
public class EmailPasswordRequest
{
    /// <summary>
    /// Gets the email address associated with the request.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Gets the password associated with the request.
    /// </summary>
    public required string Password { get; init; }
}
