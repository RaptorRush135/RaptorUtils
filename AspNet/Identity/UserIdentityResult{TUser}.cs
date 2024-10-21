namespace RaptorUtils.AspNet.Identity;

using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

using Microsoft.AspNetCore.Identity;

/// <summary>
/// A wrapper for <see cref="IdentityResult"/> that includes a user object.
/// </summary>
/// <typeparam name="TUser">The type of the user object.</typeparam>
public class UserIdentityResult<TUser>(IdentityResult result, TUser? user)
    where TUser : class
{
    /// <summary>
    /// Gets the <see cref="IdentityResult"/> associated with this result.
    /// </summary>
    public IdentityResult Result { get; } = result;

    /// <summary>
    /// Gets the user object if the operation succeeded; otherwise, <see langword="null"/>.
    /// </summary>
    /// <remarks>
    /// If <see cref="Succeeded"/> is <see langword="false"/>, this property will be <see langword="null"/>.
    /// </remarks>
    public TUser? User { get; } = result.Succeeded ? user : null;

    /// <inheritdoc cref="IdentityResult.Succeeded" />
    [MemberNotNullWhen(true, nameof(User))]
    public bool Succeeded => this.Result.Succeeded;

    /// <inheritdoc cref="IdentityResult.Errors" />
    public IEnumerable<IdentityError> Errors => this.Result.Errors;

    /// <summary>
    /// Implicitly converts a <see cref="UserIdentityResult{TUser}"/> to an <see cref="IdentityResult"/>.
    /// </summary>
    /// <param name="result">The <see cref="UserIdentityResult{TUser}"/> to convert.</param>
    /// <returns>The underlying <see cref="IdentityResult"/>.</returns>
    public static implicit operator IdentityResult(UserIdentityResult<TUser> result) => result.Result;

    /// <summary>
    /// Creates a successful <see cref="UserIdentityResult{TUser}"/> with the specified user.
    /// </summary>
    /// <param name="result">The successful <see cref="IdentityResult"/>.</param>
    /// <param name="user">The user object to include.</param>
    /// <returns>A successful <see cref="UserIdentityResult{TUser}"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if <paramref name="result"/> is not successful.
    /// </exception>
    [Pure]
    public static UserIdentityResult<TUser> Success(IdentityResult result, TUser user)
    {
        if (!result.Succeeded)
        {
            throw new InvalidOperationException("IdentityResult must have succeeded");
        }

        ArgumentNullException.ThrowIfNull(user);

        return new UserIdentityResult<TUser>(result, user);
    }

    /// <summary>
    /// Creates a failed <see cref="UserIdentityResult{TUser}"/> with no user.
    /// </summary>
    /// <param name="result">The failed <see cref="IdentityResult"/>.</param>
    /// <returns>A failed <see cref="UserIdentityResult{TUser}"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if <paramref name="result"/> is successful.
    /// </exception>
    [Pure]
    public static UserIdentityResult<TUser> Failed(IdentityResult result)
    {
        if (result.Succeeded)
        {
            throw new InvalidOperationException("IdentityResult must not have succeeded");
        }

        return new UserIdentityResult<TUser>(result, null);
    }

    /// <inheritdoc cref="IdentityResult.Failed" />
    [Pure]
    public static UserIdentityResult<TUser> Failed(params IdentityError[] errors)
    {
        return Failed(IdentityResult.Failed(errors));
    }

    /// <inheritdoc cref="IdentityResult.ToString" />
    [Pure]
    public override string ToString() => this.Result.ToString();
}
