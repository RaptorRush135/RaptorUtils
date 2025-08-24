namespace RaptorUtils.AspNet.Identity.Api.UserInfo;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Provides an HTTP endpoint for retrieving information about the currently authenticated user.
/// </summary>
public static class UserInfoEndpoint
{
    /// <summary>
    /// Retrieves the user information for the currently authenticated user
    /// using the specified <see cref="IUserInfoService{TUser, TResponse}"/>.
    /// </summary>
    /// <typeparam name="TUser">
    /// The type representing the application user entity.
    /// </typeparam>
    /// <typeparam name="TResponse">
    /// The type representing the response model containing user information.
    /// </typeparam>
    /// <param name="userContext">
    /// The current user context, used to identify the authenticated user.
    /// </param>
    /// <param name="userInfoService">
    /// The service used to retrieve user information.
    /// </param>
    /// <returns>
    /// Returns <see cref="Ok{TResponse}"/> containing
    /// the user information if the user is authenticated;
    /// otherwise, returns <see cref="NotFound"/>.
    /// </returns>
    public static async Task<Results<Ok<TResponse>, NotFound>> HandleGet<TUser, TResponse>(
        [FromServices] UserContext<TUser> userContext,
        [FromServices] IUserInfoService<TUser, TResponse> userInfoService)
        where TUser : class
    {
        if (await userContext.TryGetLoggedInUser() is not { } user)
        {
            return TypedResults.NotFound();
        }

        var info = userInfoService.GetInfo(user);

        return TypedResults.Ok(info);
    }
}
