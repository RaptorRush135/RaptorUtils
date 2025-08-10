namespace RaptorUtils.AspNet.Identity.Api.Results;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents the result of a sign-in flow, which can either be a failure with a <see cref="LoginError"/>
/// or a successful context containing sign-in details.
/// </summary>
/// <typeparam name="TUser">
/// The type representing the application user entity.
/// </typeparam>
public class SignInFlowResult<TUser>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SignInFlowResult{TUser}"/> class,
    /// representing a failed sign-in result.
    /// </summary>
    /// <param name="error">
    /// The <see cref="LoginError"/> instance.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if <paramref name="error"/> indicates success.
    /// </exception>
    public SignInFlowResult(LoginError error)
    {
        this.Error = error;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SignInFlowResult{TUser}"/> class,
    /// representing a successful sign-in context.
    /// </summary>
    /// <param name="context">
    /// The <see cref="SignInContext{TUser}"/> containing sign-in details.
    /// </param>
    public SignInFlowResult(SignInContext<TUser> context)
    {
        this.Context = context;
    }

    /// <summary>
    /// Gets the login error if the sign-in flow failed; otherwise, <see langword="null"/>.
    /// </summary>
    public LoginError? Error { get; }

    /// <summary>
    /// Gets the sign-in context if the sign-in flow succeeded; otherwise, <see langword="null"/>.
    /// </summary>
    public SignInContext<TUser>? Context { get; }

    public static implicit operator SignInFlowResult<TUser>(
        LoginError error) => new(error);

    public static implicit operator SignInFlowResult<TUser>(
        SignInContext<TUser> context) => new(context);

    /// <summary>
    /// Determines whether the sign-in flow represents a failure.
    /// </summary>
    /// <param name="error">
    /// When this method returns, contains the <see cref="LoginError"/> if it failed;
    /// otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the sign-in flow failed; otherwise, <see langword="false"/>.
    /// </returns>
    [MemberNotNullWhen(false, nameof(Context))]
    public bool IsFail(
        [MaybeNullWhen(false)] out LoginError error)
    {
        error = this.Error;
        return error != null;
    }
}
