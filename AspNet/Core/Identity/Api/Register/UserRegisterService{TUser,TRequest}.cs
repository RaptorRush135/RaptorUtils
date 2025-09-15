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
    public virtual UserIdentityResult<TUser> GenericError()
    {
        return UserIdentityResult<TUser>.Failed(
            this.UserManager.ErrorDescriber.DefaultError());
    }

    /// <inheritdoc />
    public virtual async Task<UserIdentityResult<TUser>> Register(TRequest request)
    {
        var (user, password) = await this.CreateUser(request);

        IdentityResult result = await (password != null
            ? this.UserManager.CreateAsync(user, password)
            : this.UserManager.CreateAsync(user));

        return !result.Succeeded
            ? UserIdentityResult<TUser>.Failed(result)
            : UserIdentityResult<TUser>.Success(result, user);
    }

    /// <summary>
    /// Creates a new user instance from the registration request.
    /// </summary>
    /// <param name="request">
    /// The registration request data.
    /// </param>
    /// <returns>
    /// A <see cref="ValueTask{TResult}"/> representing the asynchronous operation,
    /// containing a <see cref="UserCreationInfo{TUser}"/> with the new user instance
    /// and the optional password.
    /// </returns>
    protected abstract ValueTask<UserCreationInfo<TUser>> CreateUser(TRequest request);
}
