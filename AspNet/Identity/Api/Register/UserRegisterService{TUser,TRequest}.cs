namespace RaptorUtils.AspNet.Identity.Api.Register;

using Microsoft.AspNetCore.Identity;

/// <summary>
/// Provides a base implementation of <see cref="IUserRegisterService{TUser, TRequest}"/> that
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
    : IUserRegisterService<TUser, TRequest>
    where TUser : class
{
    /// <summary>
    /// Gets the <see cref="UserManager{TUser}"/> used to manage user accounts.
    /// </summary>
    protected UserManager<TUser> UserManager { get; } = userManager;

    /// <inheritdoc />
    public virtual async Task<UserIdentityResult<TUser>> Register(TRequest request)
    {
        TUser user = this.CreateUser(request, out string? password);

        IdentityResult result = await (password != null
            ? this.UserManager.CreateAsync(user, password)
            : this.UserManager.CreateAsync(user));

        return !result.Succeeded
            ? UserIdentityResult<TUser>.Failed(result)
            : UserIdentityResult<TUser>.Success(result, user);
    }

    /// <summary>
    /// Creates a new user instance from the registration request and outputs the password.
    /// If <paramref name="password"/> is <see langword="null"/>, the user will be created without a password.
    /// </summary>
    /// <param name="request">
    /// The registration request data.
    /// </param>
    /// <param name="password">
    /// The password extracted from the request, or <see langword="null"/> if no password is supplied.
    /// </param>
    /// <returns>
    /// A new user instance based on the registration request.
    /// </returns>
    protected abstract TUser CreateUser(TRequest request, out string? password);
}
