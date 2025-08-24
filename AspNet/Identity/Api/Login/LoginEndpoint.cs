namespace RaptorUtils.AspNet.Identity.Api.Login;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using RaptorUtils.AspNet.Identity.Api.Login.Results;

/// <summary>
/// Provides an HTTP endpoint for handling user login requests.
/// </summary>
public static class LoginEndpoint
{
    /// <summary>
    /// Processes a login request by validating the provided credentials
    /// using the specified <see cref="IUserLoginService{TUser, TRequest, TOptions}"/>.
    /// </summary>
    /// <typeparam name="TUser">The user type.</typeparam>
    /// <typeparam name="TRequest">The type of the login request.</typeparam>
    /// <typeparam name="TOptions">
    /// The type representing additional options or settings that influence the login process.
    /// </typeparam>
    /// <param name="request">The login request payload.</param>
    /// <param name="userLoginService">The login service to process the request.</param>
    /// <returns>
    /// Returns an <see cref="Ok"/> result if login succeeds; otherwise,
    /// returns a <see cref="ProblemHttpResult"/> with status code 401 Unauthorized.
    /// </returns>
    public static async Task<Results<Ok, ProblemHttpResult>> Handle<TUser, TRequest, TOptions>(
        [FromBody] TRequest request,
        [FromServices] IUserLoginService<TUser, TRequest, TOptions> userLoginService)
        where TUser : class
        where TOptions : new()
    {
        LoginResult<TUser> result = await userLoginService.Login(request, new TOptions());

        if (!result.Succeeded)
        {
            return result.Error.ToProblemHttpResult(
                defaultStatus: StatusCodes.Status401Unauthorized);
        }

        return TypedResults.Ok();
    }
}
