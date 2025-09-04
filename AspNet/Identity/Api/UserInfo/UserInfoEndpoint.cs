namespace RaptorUtils.AspNet.Identity.Api.UserInfo;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

/// <summary>
/// Provides an HTTP endpoint for retrieving information about the currently authenticated user.
/// </summary>
public static class UserInfoEndpoint
{
    /// <summary>
    /// Retrieves the user information for the currently authenticated user
    /// using the specified <see cref="IUserInfoService{TResponse}"/>.
    /// </summary>
    /// <typeparam name="TResponse">
    /// The type representing the response model containing user information.
    /// </typeparam>
    /// <param name="loggerFactory">
    /// The factory used to create loggers for logging diagnostic messages.
    /// </param>
    /// <param name="httpContextAccessor">
    /// Provides access to the current <see cref="HttpContext"/> to identify the authenticated user.
    /// </param>
    /// <param name="userInfoService">
    /// The service used to attempt retrieval of user information.
    /// </param>
    /// <returns>
    /// Returns <see cref="Ok{TResponse}"/> containing the user information
    /// if the user is authenticated and the information is available;
    /// otherwise, returns <see cref="UnauthorizedHttpResult"/>.
    /// </returns>
    public static async Task<Results<Ok<TResponse>, UnauthorizedHttpResult>> HandleGet<TResponse>(
        [FromServices] ILoggerFactory loggerFactory,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        [FromServices] IUserInfoService<TResponse> userInfoService)
        where TResponse : class
    {
        if (httpContextAccessor.HttpContext is not { } context)
        {
            ILogger logger = loggerFactory.CreateLogger(nameof(UserInfoEndpoint));
            logger.LogWarning("HttpContext was not available");

            return TypedResults.Unauthorized();
        }

        TResponse? info = await userInfoService.TryGetInfo(context.User);
        if (info == null)
        {
            return TypedResults.Unauthorized();
        }

        return TypedResults.Ok(info);
    }
}
