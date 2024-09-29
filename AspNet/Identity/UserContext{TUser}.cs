namespace RaptorUtils.AspNet.Identity;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using RaptorUtils.AspNet.Logging;

public class UserContext<TUser>(
    IHttpContextAccessor httpContextAccessor,
    UserManager<TUser> userManager,
    ILogger<UserContext<TUser>> logger)
    where TUser : class
{
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

    public async Task<UserTracker<TUser>?> TryGetLoggedInUserAsTracked()
    {
        if (!(await this.TryGetLoggedInUser() is { } loggedUser))
        {
            return null;
        }

        return new UserTracker<TUser>(loggedUser, userManager);
    }

    public async Task<TUser> GetLoggedInUser()
    {
        var user = await this.TryGetLoggedInUser();
        return GetUserValueOrThrow(user);
    }

    public async Task<UserTracker<TUser>> GetLoggedInUserAsTracked()
    {
        var user = await this.TryGetLoggedInUserAsTracked();
        return GetUserValueOrThrow(user);
    }

    private static TUserResult GetUserValueOrThrow<TUserResult>(TUserResult? user)
        where TUserResult : class
    {
        return user
            ?? throw new InvalidOperationException("No logged in user");
    }
}
