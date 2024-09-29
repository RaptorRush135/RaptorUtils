namespace RaptorUtils.AspNet.Identity;

using Microsoft.AspNetCore.Identity;

public class UserTracker<TUser>
    where TUser : class
{
    public UserTracker(TUser user, UserManager<TUser> userManager)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(userManager);

        this.User = user;
        this.UserManager = userManager;
    }

    public TUser User { get; }

    public bool RequiresUpdate { get; private set; }

    private UserManager<TUser> UserManager { get; }

    public void ScheduleUpdate()
    {
        this.RequiresUpdate = true;
    }

    public Task UpdateAsync()
    {
        return this.UserManager.UpdateAsync(this.User);
    }

    public Task UpdateIfRequiredAsync()
    {
        if (this.RequiresUpdate)
        {
            return this.UpdateAsync();
        }

        return Task.CompletedTask;
    }
}
