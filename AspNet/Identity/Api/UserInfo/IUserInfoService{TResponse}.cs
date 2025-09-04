namespace RaptorUtils.AspNet.Identity.Api.UserInfo;

using System.Security.Claims;

/// <summary>
/// Defines a service for retrieving information about a user
/// based on their <see cref="ClaimsPrincipal"/>.
/// </summary>
/// <typeparam name="TResponse">
/// The type representing the response model containing user information.
/// </typeparam>
public interface IUserInfoService<TResponse>
{
    /// <summary>
    /// Attempts to retrieve the information associated with the specified user.
    /// </summary>
    /// <param name="user">
    /// The <see cref="ClaimsPrincipal"/> representing the authenticated user.
    /// </param>
    /// <returns>
    /// A <typeparamref name="TResponse"/> instance containing the user's information
    /// if available; otherwise, <see langword="null"/>.
    /// </returns>
    Task<TResponse?> TryGetInfo(ClaimsPrincipal user);
}
