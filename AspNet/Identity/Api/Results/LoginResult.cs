namespace RaptorUtils.AspNet.Identity.Api.Results;

using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Identity;

/// <summary>
/// Represents the result of a login attempt, indicating success or providing an associated <see cref="LoginError"/>.
/// </summary>
/// <param name="error">
/// The error associated with a failed login attempt, or <see langword="null"/> if the login succeeded.
/// </param>
public sealed class LoginResult(LoginError? error)
{
    /// <summary>
    /// Gets a successful login result with no associated error.
    /// </summary>
    public static LoginResult Success { get; } = new(null);

    /// <summary>
    /// Gets a value indicating whether the login attempt succeeded.
    /// </summary>
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Succeeded => this.Error == null;

    /// <summary>
    /// Gets the error associated with a failed login attempt, or <see langword="null"/> if the login succeeded.
    /// </summary>
    public LoginError? Error { get; } = error;

    /// <summary>
    /// Creates a <see cref="LoginResult"/> representing a failed login with the specified error.
    /// </summary>
    /// <param name="error">The error describing why the login failed.</param>
    /// <returns>A <see cref="LoginResult"/> representing the failure.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="error"/> is <see langword="null"/>.
    /// </exception>
    public static LoginResult FromError(LoginError error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return new(error);
    }

    /// <summary>
    /// Creates a <see cref="LoginResult"/> from a <see cref="SignInResult"/>.
    /// </summary>
    /// <param name="signIn">The sign-in result to evaluate.</param>
    /// <returns>
    /// <see cref="Success"/> if <paramref name="signIn"/> succeeded,
    /// otherwise a failure result containing a new <see cref="LoginError"/>.
    /// </returns>
    public static LoginResult FromSignIn(SignInResult signIn)
    {
        return signIn.Succeeded
            ? Success
            : FromError(new LoginError(signIn.ToString()));
    }
}
