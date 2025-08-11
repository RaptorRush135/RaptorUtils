namespace RaptorUtils.AspNet.Identity.Api.Services;

using RaptorUtils.AspNet.Identity.Api.Results;

/// <summary>
/// Defines a service for handling user login requests.
/// </summary>
/// <typeparam name="TUser">
/// The type representing the user entity.
/// </typeparam>
/// <typeparam name="TRequest">
/// The type representing the login request payload.
/// </typeparam>
/// <typeparam name="TOptions">
/// The type representing additional options or settings that influence the login process.
/// </typeparam>
public interface IUserLoginService<TUser, TRequest, TOptions>
    where TUser : class
{
    /// <summary>
    /// Attempts to log in a user using the provided login request data.
    /// </summary>
    /// <param name="request">
    /// The login request containing the user credentials or other necessary data.
    /// </param>
    /// <param name="options">
    /// The options that configure or influence the login process.
    /// </param>
    /// <returns>
    /// A <see cref="Task{LoginResult}"/> representing the asynchronous login operation,
    /// containing the result of the sign-in attempt.
    /// </returns>
    Task<LoginResult<TUser>> Login(TRequest request, TOptions options);
}
