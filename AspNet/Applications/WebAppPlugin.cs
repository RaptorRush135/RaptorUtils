namespace RaptorUtils.AspNet.Applications;

using Microsoft.AspNetCore.Builder;

public class WebAppPlugin(Func<WebApplicationBuilder, Task<bool>>? isEnabled)
{
    public virtual Task<bool> IsEnabled(WebApplicationBuilder builder)
    {
        return isEnabled?.Invoke(builder)
            ?? Task.FromResult(true);
    }

    public virtual Task OnAfterCreateBuilder(WebApplicationBuilder builder)
    {
        return Task.CompletedTask;
    }

    public virtual Task OnConfigureServices(WebApplicationBuilder builder)
    {
        return Task.CompletedTask;
    }

    public virtual Task OnAfterConfigureServices(WebApplicationBuilder builder)
    {
        return Task.CompletedTask;
    }

    public virtual Task OnConfigure(WebApplication app)
    {
        return Task.CompletedTask;
    }

    public virtual Task OnAfterConfigure(WebApplication app)
    {
        return Task.CompletedTask;
    }

    public virtual Task OnRun(string[] args)
    {
        return Task.CompletedTask;
    }

    public virtual Task<int?> OnException(Exception exception)
    {
        return Task.FromResult<int?>(null);
    }

    public virtual Task OnFinally()
    {
        return Task.CompletedTask;
    }
}
