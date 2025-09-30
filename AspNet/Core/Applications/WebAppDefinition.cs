namespace RaptorUtils.AspNet.Applications;

using Microsoft.AspNetCore.Builder;

using RaptorUtils.Threading.Tasks;

/// <summary>
/// Provides an abstract definition for configuring and running a web application using the .NET minimal hosting model.
/// This class includes hooks for configuring services, middleware, and handling exceptions and finalization.
/// </summary>
public abstract class WebAppDefinition
{
    private WebApplication? application;

    /// <summary>
    /// Runs the web application with the specified arguments.
    /// Handles the entire lifecycle, including exception handling and finalization logic.
    /// </summary>
    /// <param name="args">The command-line arguments passed to the application.</param>
    /// <returns>The exit code of the application, where 0 typically indicates success.</returns>
    public async Task<int> Run(string[] args)
    {
        try
        {
            await this.OnRun(args);
        }
        catch (Exception ex)
        {
            if (await this.OnException(ex, this.application) is { } exitCode)
            {
                return exitCode;
            }

            throw;
        }
        finally
        {
            await this.OnFinally(this.application);
        }

        return 0;
    }

    /// <summary>
    /// The core logic to run the web application, responsible for creating the builder,
    /// configuring services, building the application, and running it.
    /// This method can be overridden to customize the application's setup.
    /// </summary>
    /// <param name="args">The command-line arguments passed to the application.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected virtual async Task OnRun(string[] args)
    {
        var builder = this.CreateBuilder(args);
        await this.AfterCreateBuilder(builder);

        await this.ConfigureServices(builder);
        await this.AfterConfigureServices(builder);

        var app = builder.Build();
        this.application = app;

        await this.Configure(app);
        await this.AfterConfigure(app);

        await this.BeforeStartup(app);
        await app.RunAsync();
    }

    /// <summary>
    /// Creates a <see cref="WebApplicationBuilder"/> instance using the provided arguments.
    /// Override this method to customize the application builder creation.
    /// </summary>
    /// <param name="args">The command-line arguments passed to the application.</param>
    /// <returns>The created <see cref="WebApplicationBuilder"/>.</returns>
    protected virtual WebApplicationBuilder CreateBuilder(string[] args) => WebApplication.CreateBuilder(args);

    /// <summary>
    /// Hook method called after the application builder is created.
    /// Override this method to customize behavior after builder creation.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected virtual ValueTask AfterCreateBuilder(WebApplicationBuilder builder) => ValueTask.CompletedTask;

    /// <summary>
    /// Configures services for the application. This method must be implemented in derived classes.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance to configure services for.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected abstract ValueTask ConfigureServices(WebApplicationBuilder builder);

    /// <summary>
    /// Hook method called after services have been configured.
    /// Override this method to add additional behavior after service configuration.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected virtual ValueTask AfterConfigureServices(WebApplicationBuilder builder) => ValueTask.CompletedTask;

    /// <summary>
    /// Builds the application using the configured <see cref="WebApplicationBuilder"/>.
    /// Override this method to customize the building process.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance.</param>
    /// <returns>The built <see cref="WebApplication"/>.</returns>
    protected virtual WebApplication Build(WebApplicationBuilder builder) => builder.Build();

    /// <summary>
    /// Configures the web application's request pipeline. This method must be implemented in derived classes.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected abstract ValueTask Configure(WebApplication app);

    /// <summary>
    /// Hook method called after the application has been configured.
    /// Override this method to add additional behavior after application configuration.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected virtual ValueTask AfterConfigure(WebApplication app) => ValueTask.CompletedTask;

    /// <summary>
    /// Hook method called immediately before the application starts running.
    /// Override this method to add custom logic that should execute after
    /// the application has been configured and built, but before
    /// <see cref="WebApplication.RunAsync"/> is invoked.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected virtual ValueTask BeforeStartup(WebApplication app) => ValueTask.CompletedTask;

    /// <summary>
    /// Handles any exceptions that occur during the application's execution.
    /// Override this method to customize exception handling behavior.
    /// </summary>
    /// <param name="exception">The exception that occurred.</param>
    /// <param name="app">
    /// The <see cref="WebApplication"/> instance,
    /// or <see langword="null"/> if the application could not be fully constructed.
    /// </param>
    /// <returns>An integer exit code if handled, or null if the exception should be re-thrown.</returns>
    protected virtual TaskOrValue<int?> OnException(Exception exception, WebApplication? app) => (int?)null;

    /// <summary>
    /// Hook method called after the application has finished running, regardless of whether an exception occurred.
    /// Override this method to add custom finalization logic.
    /// </summary>
    /// <param name="app">
    /// The <see cref="WebApplication"/> instance,
    /// or <see langword="null"/> if the application could not be fully constructed.
    /// </param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected virtual ValueTask OnFinally(WebApplication? app) => ValueTask.CompletedTask;
}
