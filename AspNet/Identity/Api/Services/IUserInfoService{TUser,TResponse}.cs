namespace RaptorUtils.AspNet.Identity.Api.Services;

/// <summary>
/// Defines a service for retrieving information about a specific user.
/// </summary>
/// <typeparam name="TUser">
/// The type representing the application user entity.
/// </typeparam>
/// <typeparam name="TResponse">
/// The type representing the response model containing user information.
/// </typeparam>
public interface IUserInfoService<in TUser, out TResponse>
{
    /// <summary>
    /// Retrieves the information associated with the specified user.
    /// </summary>
    /// <param name="user">
    /// The user for whom to retrieve information.
    /// </param>
    /// <returns>
    /// A <typeparamref name="TResponse"/> instance containing the user's information.
    /// </returns>
    TResponse GetInfo(TUser user);
}
