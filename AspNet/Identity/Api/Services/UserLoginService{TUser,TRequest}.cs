namespace RaptorUtils.AspNet.Identity.Api.Services;

using Microsoft.AspNetCore.Identity;

/// <summary>
/// Provides a base implementation of <see cref="IUserLoginService{TRequest}"/> that
/// uses <see cref="SignInManager{TUser}"/> to perform user login operations.
/// </summary>
/// <typeparam name="TUser">
/// The type representing the application user entity.
/// </typeparam>
/// <typeparam name="TRequest">
/// The type representing the login request payload.
/// </typeparam>
/// <param name="signInManager">
/// The <see cref="SignInManager{TUser}"/> used to manage sign-in operations.
/// </param>
public abstract class UserLoginService<TUser, TRequest>(
    SignInManager<TUser> signInManager)
    : IUserLoginService<TRequest>
    where TUser : class
{
    /// <summary>
    /// Attempts to log in a user based on the provided login request.
    /// </summary>
    /// <param name="request">
    /// The login request containing user credentials.
    /// </param>
    /// <returns>
    /// A <see cref="Task{SignInResult}"/> representing the asynchronous login operation,
    /// containing the result of the sign-in attempt.
    /// </returns>
    public async Task<SignInResult> Login(TRequest request)
    {
        var flow = await this.SignInFlow(request);

        if (flow.IsFail(out var result))
        {
            return result;
        }

        var context = flow.Context.Value;

        return await signInManager.PasswordSignInAsync(
            context.User,
            context.Password,
            context.IsPersistent,
            context.LockoutOnFailure);
    }

    /// <summary>
    /// Processes the login request and returns a <see cref="SignInFlowResult{TUser}"/>
    /// describing whether the sign-in should proceed or fail.
    /// </summary>
    /// <param name="request">
    /// The login request to process.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation
    /// containing a <see cref="SignInFlowResult{TUser}"/> result.
    /// </returns>
    protected abstract Task<SignInFlowResult<TUser>> SignInFlow(TRequest request);
}
