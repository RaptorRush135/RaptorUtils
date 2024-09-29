namespace RaptorUtils.AspNet.Applications;

using Microsoft.AspNetCore.Builder;

public abstract class WebAppDefinition
{
    public async Task<int> Run(string[] args)
    {
        try
        {
            await this.OnRun(args);
        }
        catch (Exception ex)
        {
            if (await this.OnException(ex) is { } exitCode)
            {
                return exitCode;
            }

            throw;
        }
        finally
        {
            await this.OnFinally();
        }

        return 0;
    }

    protected virtual async Task OnRun(string[] args)
    {
        var builder = this.CreateBuilder(args);
        await this.AfterCreateBuilder(builder);

        await this.ConfigureServices(builder);
        await this.AfterConfigureServices(builder);

        var app = builder.Build();

        await this.Configure(app);
        await this.AfterConfigure(app);

        await app.RunAsync();
    }

    protected virtual WebApplicationBuilder CreateBuilder(string[] args)
    {
        return WebApplication.CreateBuilder(args);
    }

    protected virtual Task AfterCreateBuilder(WebApplicationBuilder builder)
    {
        return Task.CompletedTask;
    }

    protected abstract Task ConfigureServices(WebApplicationBuilder builder);

    protected virtual Task AfterConfigureServices(WebApplicationBuilder builder)
    {
        return Task.CompletedTask;
    }

    protected virtual WebApplication Build(WebApplicationBuilder builder)
    {
        return builder.Build();
    }

    protected abstract Task Configure(WebApplication app);

    protected virtual Task AfterConfigure(WebApplication app)
    {
        return Task.CompletedTask;
    }

    protected virtual Task<int?> OnException(Exception exception)
    {
        return Task.FromResult<int?>(null);
    }

    protected virtual Task OnFinally()
    {
        return Task.CompletedTask;
    }
}
