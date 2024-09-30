namespace RaptorUtils.AspNet.Identity;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using RaptorUtils.AspNet.Logging;

/// <summary>
/// Provides a context for accessing the logged-in user's information.
/// </summary>
/// <typeparam name="TUser">The type of the user managed by the UserManager.</typeparam>
/// <param name="httpContextAccessor">The accessor for HTTP context information.</param>
/// <param name="userManager">The user manager used for retrieving user details.</param>
/// <param name="logger">The logger used for logging warnings and errors.</param>
public class UserContext<TUser>(
    IHttpContextAccessor httpContextAccessor,
    UserManager<TUser> userManager,
    ILogger<UserContext<TUser>> logger)
    where TUser : class
{
    /// <summary>
    /// Attempts to retrieve the currently logged-in user.
    /// Returns null if the HttpContext is unavailable or if the user cannot be retrieved.
    /// Logs a warning if the HttpContext is not available.
    /// </summary>
    /// <returns>The logged-in user, or null if not available.</returns>
    public async Task<TUser?> TryGetLoggedInUser()
    {
        HttpContext? context = httpContextAccessor.HttpContext;
        if (context == null)
        {
            logger.TryWarning()?.Log("HttpContext was not available");
            return null;
        }

        return await userManager.GetUserAsync(context.User);
    }

    /// <summary>
    /// Attempts to retrieve the currently logged-in user as a tracked user.
    /// Returns null if the user is not logged in or cannot be retrieved.
    /// </summary>
    /// <returns>A tracked user, or null if not available.</returns>
    public async Task<UserTracker<TUser>?> TryGetLoggedInUserAsTracked()
    {
        if (!(await this.TryGetLoggedInUser() is { } loggedUser))
        {
            return null;
        }

        return new UserTracker<TUser>(loggedUser, userManager);
    }

    // TODO: Remove
    public async Task<TUser> GetLoggedInUser()
    {
        var user = await this.TryGetLoggedInUser();
        return GetUserValueOrThrow(user);
    }

    // TODO: Remove
    public async Task<UserTracker<TUser>> GetLoggedInUserAsTracked()
    {
        var user = await this.TryGetLoggedInUserAsTracked();
        return GetUserValueOrThrow(user);
    }

    // TODO: Remove
    private static TUserResult GetUserValueOrThrow<TUserResult>(TUserResult? user)
        where TUserResult : class
    {
        return user
            ?? throw new InvalidOperationException("No logged in user");
    }
}
