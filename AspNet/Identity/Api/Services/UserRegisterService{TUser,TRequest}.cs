namespace RaptorUtils.AspNet.Identity.Api.Services;

using Microsoft.AspNetCore.Identity;

/// <summary>
/// Provides a base implementation of <see cref="IUserRegisterService{TRequest}"/> that
/// uses <see cref="UserManager{TUser}"/> to perform user registration operations.
/// </summary>
/// <typeparam name="TUser">
/// The type representing the application user entity.
/// </typeparam>
/// <typeparam name="TRequest">
/// The type representing the registration request payload.
/// </typeparam>
/// <param name="userManager">
/// The <see cref="UserManager{TUser}"/> used to manage user creation.
/// </param>
public abstract class UserRegisterService<TUser, TRequest>(
    UserManager<TUser> userManager)
    : IUserRegisterService<TRequest>
    where TUser : class
{
    /// <inheritdoc />
    public async Task<IdentityResult> Register(TRequest request)
    {
        TUser user = this.CreateUser(request, out string password);

        return await userManager.CreateAsync(user, password);
    }

    /// <summary>
    /// Creates a new user instance from the registration request and outputs the password.
    /// </summary>
    /// <param name="request">
    /// The registration request data.
    /// </param>
    /// <param name="password">
    /// The password extracted from the request to use for the new user.
    /// </param>
    /// <returns>
    /// A new user instance based on the registration request.
    /// </returns>
    protected abstract TUser CreateUser(TRequest request, out string password);
}
