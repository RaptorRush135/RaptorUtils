namespace RaptorUtils.AspNet.Identity;

using Microsoft.AspNetCore.Identity;

/// <summary>
/// Tracks a user and provides functionality to manage user updates.
/// </summary>
/// <typeparam name="TUser">The type of the user being tracked, which must be a class.</typeparam>
public class UserTracker<TUser>
    where TUser : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserTracker{TUser}"/> class.
    /// </summary>
    /// <param name="user">The user being tracked.</param>
    /// <param name="userManager">The user manager to handle updates for the user.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="user"/> or <paramref name="userManager"/> is null.
    /// </exception>
    public UserTracker(TUser user, UserManager<TUser> userManager)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(userManager);

        this.User = user;
        this.UserManager = userManager;
    }

    /// <summary>
    /// Gets the user being tracked.
    /// </summary>
    public TUser User { get; }

    /// <summary>
    /// Gets a value indicating whether an update is required for the tracked user.
    /// </summary>
    public bool RequiresUpdate { get; private set; }

    private UserManager<TUser> UserManager { get; }

    /// <summary>
    /// Schedules an update for the tracked user.
    /// </summary>
    public void ScheduleUpdate()
    {
        this.RequiresUpdate = true;
    }

    /// <summary>
    /// Updates the tracked user asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous update operation.</returns>
    public Task UpdateAsync()
    {
        return this.UserManager.UpdateAsync(this.User);
    }

    /// <summary>
    /// Updates the tracked user asynchronously if an update is required.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous update operation,
    /// which completes immediately if no update is required.
    /// </returns>
    public Task UpdateIfRequiredAsync()
    {
        if (this.RequiresUpdate)
        {
            return this.UpdateAsync();
        }

        return Task.CompletedTask;
    }
}
