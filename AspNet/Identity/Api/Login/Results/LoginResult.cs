namespace RaptorUtils.AspNet.Identity.Api.Login.Results;

using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Identity;

/// <summary>
/// Represents the result of a login attempt for a user of type <typeparamref name="TUser"/>.
/// </summary>
/// <typeparam name="TUser">The type representing the user entity.</typeparam>
public sealed class LoginResult<TUser>
    where TUser : class
{
    /// <summary>
    /// Gets a value indicating whether the login attempt succeeded.
    /// </summary>
    [MemberNotNullWhen(true, nameof(User))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Succeeded => this.User != null;

    /// <summary>
    /// Gets the user associated with a successful login, or <see langword="null"/> if the login failed.
    /// </summary>
    public TUser? User { get; private init; }

    /// <summary>
    /// Gets the error associated with a failed login attempt, or <see langword="null"/> if the login succeeded.
    /// </summary>
    public LoginError? Error { get; private init; }

    /// <summary>
    /// Creates a successful <see cref="LoginResult{TUser}"/> containing the specified user.
    /// </summary>
    /// <param name="user">The successfully logged-in user.</param>
    /// <returns>A successful login result.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="user"/> is <see langword="null"/>.</exception>
    public static LoginResult<TUser> Success(TUser user)
    {
        ArgumentNullException.ThrowIfNull(user);
        return new() { User = user };
    }

    /// <summary>
    /// Creates a <see cref="LoginResult{TUser}"/> representing a failed login with the specified error.
    /// </summary>
    /// <param name="error">The error describing why the login failed.</param>
    /// <returns>A <see cref="LoginResult{TUser}"/> representing the failure.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="error"/> is <see langword="null"/>.
    /// </exception>
    public static LoginResult<TUser> FromError(LoginError error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return new() { Error = error };
    }

    /// <summary>
    /// Creates a <see cref="LoginResult{TUser}"/> based on an <see cref="SignInResult"/> and user.
    /// </summary>
    /// <param name="signIn">The sign-in result.</param>
    /// <param name="user">The user associated with the sign-in attempt.</param>
    /// <returns>
    /// A successful login result if <paramref name="signIn"/> succeeded;
    /// otherwise, a failed login result with the sign-in error.
    /// </returns>
    public static LoginResult<TUser> FromSignIn(SignInResult signIn, TUser user)
    {
        return signIn.Succeeded
            ? Success(user)
            : FromError(new LoginError(signIn.ToString()));
    }
}
