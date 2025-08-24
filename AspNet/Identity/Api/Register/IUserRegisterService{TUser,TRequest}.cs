namespace RaptorUtils.AspNet.Identity.Api.Register;

/// <summary>
/// Defines a service for handling user registration requests.
/// </summary>
/// <typeparam name="TUser">
/// The type representing the application user entity.
/// </typeparam>
/// <typeparam name="TRequest">
/// The type representing the registration request payload.
/// </typeparam>
public interface IUserRegisterService<TUser, TRequest>
    where TUser : class
{
    /// <summary>
    /// Attempts to register a new user using the provided registration data.
    /// </summary>
    /// <param name="request">
    /// The registration request containing user details.
    /// </param>
    /// <returns>
    /// A <see cref="Task{IdentityResult}"/> representing the asynchronous registration operation,
    /// containing the result of the registration attempt.
    /// </returns>
    Task<UserIdentityResult<TUser>> Register(TRequest request);
}
