namespace RaptorUtils.AspNet.Identity.Api.Services;

using RaptorUtils.AspNet.Identity.Api.Results;

/// <summary>
/// Defines a service for handling user login requests.
/// </summary>
/// <typeparam name="TRequest">
/// The type representing the login request payload.
/// </typeparam>
public interface IUserLoginService<TRequest>
{
    /// <summary>
    /// Attempts to log in a user using the provided login request data.
    /// </summary>
    /// <param name="request">
    /// The login request containing the user credentials.
    /// </param>
    /// <returns>
    /// A <see cref="Task{LoginResult}"/> representing the asynchronous login operation,
    /// containing the result of the sign-in attempt.
    /// </returns>
    Task<LoginResult> Login(TRequest request);
}
