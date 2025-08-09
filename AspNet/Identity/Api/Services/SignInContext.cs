namespace RaptorUtils.AspNet.Identity.Api.Services;

/// <summary>
/// Represents the context information required to attempt a user sign-in.
/// </summary>
/// <typeparam name="TUser">
/// The type representing the application user entity.
/// </typeparam>
/// <param name="User">
/// The user attempting to sign in.
/// </param>
/// <param name="Password">
/// The password provided for the sign-in attempt.
/// </param>
/// <param name="IsPersistent">
/// Indicates whether the authentication session should persist across browser sessions.
/// </param>
/// <param name="LockoutOnFailure">
/// Indicates whether to lock the user account on consecutive failed sign-in attempts.
/// </param>
public readonly record struct SignInContext<TUser>(
    TUser User,
    string Password,
    bool IsPersistent,
    bool LockoutOnFailure);
