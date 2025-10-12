namespace RaptorUtils.AspNet.Identity.Api.Login;

using Microsoft.AspNetCore.Identity;

using RaptorUtils.AspNet.Identity.Api.Login.Results;

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
    /// <summary>
    /// Gets the <see cref="SignInManager{TUser}"/> instance used to perform sign-in operations.
    /// </summary>
    protected SignInManager<TUser> SignInManager { get; } = signInManager;

    /// <inheritdoc />
    public virtual async Task<LoginResult<TUser>> Login(TRequest request, TOptions options)
    {
        var flow = await this.SignInFlow(request, options);

        if (flow.IsFail(out var error))
        {
            return LoginResult<TUser>.FromError(error);
        }

        var context = flow.Context.Value;

        var signInResult = await this.SignIn(context);

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

    /// <summary>
    /// Signs in a user using the provided sign-in context.
    /// </summary>
    /// This method uses the <see cref="SignInManager{TUser}.PasswordSignInAsync(TUser, string, bool, bool)"/> method
    /// to perform the sign-in operation.
    /// Override this method to customize the sign-in behavior.
    /// <param name="context">
    /// The sign-in context containing the user, password, and additional sign-in options.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a  <see cref="SignInResult"/>
    /// indicating the outcome of the sign-in attempt.
    /// </returns>
    protected virtual Task<SignInResult> SignIn(SignInContext<TUser> context)
    {
        return this.SignInManager.PasswordSignInAsync(
            context.User,
            context.Password,
            context.IsPersistent,
            context.LockoutOnFailure);
    }
}
