namespace RaptorUtils.AspNet.Identity.Api.Register;

/// <summary>
/// Represents the result of creating a new user, including the user instance
/// and an optional password.
/// </summary>
/// <typeparam name="TUser">
/// The type of the user entity.
/// </typeparam>
/// <param name="User">
/// The created user instance.
/// </param>
/// <param name="Password">
/// The password associated with the user, or <see langword="null"/> if no password was provided.
/// </param>
public readonly record struct UserCreationInfo<TUser>(
    TUser User,
    string? Password)
{
    /// <summary>
    /// Creates a <see cref="UserCreationInfo{TUser}"/> instance for a user without a password.
    /// </summary>
    /// <param name="user">
    /// The created user instance.
    /// </param>
    /// <returns>
    /// A <see cref="UserCreationInfo{TUser}"/> containing the user instance with no password.
    /// </returns>
    public static UserCreationInfo<TUser> NoPassword(TUser user) => new(user, Password: null);

    /// <summary>
    /// Deconstructs the <see cref="UserCreationInfo{TUser}"/> into its component values.
    /// </summary>
    /// <param name="user">
    /// The created user instance.
    /// </param>
    /// <param name="password">
    /// The password associated with the user, or <see langword="null"/> if none was provided.
    /// </param>
    public void Deconstruct(out TUser user, out string? password)
    {
        user = this.User;
        password = this.Password;
    }
}
