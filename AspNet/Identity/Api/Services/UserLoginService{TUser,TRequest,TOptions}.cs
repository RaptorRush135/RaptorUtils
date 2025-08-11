namespace RaptorUtils.AspNet.Identity.Api.Services;

using Microsoft.AspNetCore.Identity;

using RaptorUtils.AspNet.Identity.Api.Results;

/// <summary>
/// Provides a base implementation of <see cref="IUserLoginService{TUser, TRequest, TOptions}"/> that
/// uses <see cref="SignInManager{TUser}"/> to perform user login operations.
/// </summary>
/// <typeparam name="TUser">
/// The type representing the application user entity.
/// </typeparam>
/// <typeparam name="TRequest">
/// The type representing the login request payload.
/// </typeparam>
/// <typeparam name="TOptions">
/// The type representing additional options or settings that influence the login process.
/// </typeparam>
/// <param name="signInManager">
/// The <see cref="SignInManager{TUser}"/> used to manage sign-in operations.
/// </param>
public abstract class UserLoginService<TUser, TRequest, TOptions>(
    SignInManager<TUser> signInManager)
    : IUserLoginService<TUser, TRequest, TOptions>
    where TUser : class
{
    /// <inheritdoc />
    public async Task<LoginResult<TUser>> Login(TRequest request, TOptions options)
    {
        var flow = await this.SignInFlow(request, options);

        if (flow.IsFail(out var error))
        {
            return LoginResult<TUser>.FromError(error);
        }

        var context = flow.Context.Value;

        var signInResult = await signInManager.PasswordSignInAsync(
            context.User,
            context.Password,
            context.IsPersistent,
            context.LockoutOnFailure);

        return LoginResult<TUser>.FromSignIn(signInResult, context.User);
    }

    /// <summary>
    /// Processes the login request and returns a <see cref="SignInFlowResult{TUser}"/>
    /// describing whether the sign-in should proceed or fail.
    /// </summary>
    /// <param name="request">
    /// The login request to process.
    /// </param>
    /// <param name="options">
    /// The options or settings that influence the login process.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation
    /// containing a <see cref="SignInFlowResult{TUser}"/> result.
    /// </returns>
    protected abstract Task<SignInFlowResult<TUser>> SignInFlow(TRequest request, TOptions options);
}
