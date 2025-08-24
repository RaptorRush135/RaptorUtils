namespace RaptorUtils.AspNet.Identity.Api.Logout;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Provides an HTTP endpoint for logging out the currently authenticated user.
/// </summary>
public static class LogoutEndpoint
{
    /// <summary>
    /// Signs out the currently authenticated user using the provided
    /// <see cref="SignInManager{TUser}"/>.
    /// </summary>
    /// <typeparam name="TUser">
    /// The type representing the application user entity.
    /// </typeparam>
    /// <param name="signInManager">
    /// The <see cref="SignInManager{TUser}"/> used to perform the sign-out operation.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous sign-out operation.
    /// </returns>
    public static Task Handle<TUser>(
        [FromServices] SignInManager<TUser> signInManager)
        where TUser : class
    {
        return signInManager.SignOutAsync();
    }
}
