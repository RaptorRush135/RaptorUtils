namespace RaptorUtils.AspNet.Identity.Api.Endpoints;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using RaptorUtils.AspNet.Identity.Api.Services;
using RaptorUtils.AspNet.Identity.Extensions;

/// <summary>
/// Provides an HTTP endpoint for registering a new user account.
/// </summary>
public static class RegisterEndpoint
{
    /// <summary>
    /// Processes a registration request by creating a new user account
    /// using the specified <see cref="IUserRegisterService{TUser, TRequest}"/>.
    /// </summary>
    /// <typeparam name="TUser">
    /// The type representing the application user entity.
    /// </typeparam>
    /// <typeparam name="TRequest">
    /// The type representing the registration request payload.
    /// </typeparam>
    /// <param name="request">
    /// The registration request data, provided in the HTTP request body.
    /// </param>
    /// <param name="userRegisterService">
    /// The service used to perform the user registration process.
    /// </param>
    /// <returns>
    /// Returns <see cref="Ok"/> if the registration succeeds,
    /// or a <see cref="ValidationProblem"/> containing validation errors if it fails.
    /// </returns>
    public static async Task<Results<Ok, ValidationProblem>> Handle<TUser, TRequest>(
        [FromBody] TRequest request,
        [FromServices] IUserRegisterService<TUser, TRequest> userRegisterService)
        where TUser : class
    {
        IdentityResult result = await userRegisterService.Register(request);

        if (!result.Succeeded)
        {
            return result.ToValidationProblem();
        }

        return TypedResults.Ok();
    }
}
