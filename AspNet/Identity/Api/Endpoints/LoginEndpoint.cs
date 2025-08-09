namespace RaptorUtils.AspNet.Identity.Api.Endpoints;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using RaptorUtils.AspNet.Identity.Api.Services;

/// <summary>
/// Provides an HTTP endpoint for handling user login requests.
/// </summary>
public static class LoginEndpoint
{
    /// <summary>
    /// Processes a login request by validating the provided credentials
    /// using the specified <see cref="IUserLoginService{TRequest}"/>.
    /// </summary>
    /// <typeparam name="TRequest">
    /// The type representing the login request payload.
    /// </typeparam>
    /// <param name="request">
    /// The login request data, provided in the HTTP request body.
    /// </param>
    /// <param name="userLoginService">
    /// The login service used to authenticate the request.
    /// </param>
    /// <returns>
    /// Returns <see cref="Ok"/> if the login succeeds,
    /// or <see cref="ProblemHttpResult"/> with a 401 status code if authentication fails.
    /// </returns>
    public static async Task<Results<Ok, ProblemHttpResult>> Handle<TRequest>(
        [FromBody] TRequest request,
        [FromServices] IUserLoginService<TRequest> userLoginService)
    {
        var result = await userLoginService.Login(request);

        if (!result.Succeeded)
        {
            return TypedResults.Problem(
                result.ToString(),
                statusCode: StatusCodes.Status401Unauthorized);
        }

        return TypedResults.Ok();
    }
}
