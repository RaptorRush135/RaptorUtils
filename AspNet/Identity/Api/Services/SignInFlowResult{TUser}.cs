namespace RaptorUtils.AspNet.Identity.Api.Services;

using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Identity;

/// <summary>
/// Represents the result of a sign-in flow, which can either be a failure
/// with a <see cref="SignInResult"/> or a successful context containing
/// sign-in details.
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
    /// <param name="failResult">
    /// The failed <see cref="SignInResult"/> instance.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if <paramref name="failResult"/> indicates success.
    /// </exception>
    public SignInFlowResult(SignInResult failResult)
    {
        ArgumentNullException.ThrowIfNull(failResult);
        if (failResult.Succeeded)
        {
            throw new InvalidOperationException(
                "Expected a failed SignInResult.");
        }

        this.FailResult = failResult;
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
    /// Gets the failure result if the sign-in flow failed; otherwise, <see langword="null"/>.
    /// </summary>
    public SignInResult? FailResult { get; }

    /// <summary>
    /// Gets the sign-in context if the sign-in flow succeeded; otherwise,<see langword="null"/>.
    /// </summary>
    public SignInContext<TUser>? Context { get; }

    public static implicit operator SignInFlowResult<TUser>(
        SignInResult failResult) => new(failResult);

    public static implicit operator SignInFlowResult<TUser>(
        SignInContext<TUser> context) => new(context);

    /// <summary>
    /// Determines whether the sign-in flow represents a failure.
    /// </summary>
    /// <param name="result">
    /// When this method returns, contains the failure <see cref="SignInResult"/> if it failed;
    /// otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the sign-in flow failed; otherwise, <see langword="false"/>.
    /// </returns>
    [MemberNotNullWhen(false, nameof(Context))]
    public bool IsFail(
        [MaybeNullWhen(false)] out SignInResult result)
    {
        result = this.FailResult;
        return result != null;
    }
}
