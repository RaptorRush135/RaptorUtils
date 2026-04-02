namespace RaptorUtils.AspNet.Applications;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

using RaptorUtils.Threading.Tasks;

/// <summary>
/// Provides an abstract definition for configuring and running a web application using the .NET minimal hosting model.
/// This class includes hooks for configuring services, middleware, and handling exceptions and finalization.
/// </summary>
public abstract class WebAppDefinition
{
    /// <summary>
    /// Runs the web application with the specified arguments.
    /// Handles the entire lifecycle, including exception handling and finalization logic.
    /// </summary>
    /// <param name="args">The command-line arguments passed to the application.</param>
    /// <returns>The exit code of the application, where 0 typically indicates success.</returns>
    public async Task<int> Run(string[] args)
    {
        var logger = this.CreateBootstrapLogger();
        var builder = this.CreateBuilder(args);

        WebApplication? app = null;

        try
        {
            app = await this.Build(builder, logger);
            logger = app.Logger;

            await this.Configure(app);

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            if (await this.OnException(ex, app, logger) is { } exitCode)
            {
                return exitCode;
            }

            throw;
        }
        finally
        {
            await this.OnFinally(app, logger);
        }

        return 0;
    }

    /// <summary>
    /// Builds and configures a new <see cref="WebApplication"/> instance.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance.</param>
    /// <param name="logger">Bootstrap logger.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected async Task<WebApplication> Build(WebApplicationBuilder builder, ILogger logger)
    {
        await this.OnAfterCreateBuilder(builder, logger);

        await this.OnConfigureServices(builder, logger);
        await this.OnAfterConfigureServices(builder, logger);

        return builder.Build();
    }

    /// <summary>
    /// Performs application configuration by invoking the configuration pipeline steps.
    /// </summary>
    /// <remarks>
    /// This method executes the configuration steps in order: OnConfigure, OnAfterConfigure, and OnBeforeStartup.
    /// </remarks>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    /// <returns>A task that represents the asynchronous configuration operation.</returns>
    protected async Task Configure(WebApplication app)
    {
        await this.OnConfigure(app);
        await this.OnAfterConfigure(app);
        await this.OnBeforeStartup(app);
    }

    /// <summary>
    /// Creates a <see cref="WebApplicationBuilder"/> instance using the provided arguments.
    /// Override this method to customize the application builder creation.
    /// </summary>
    /// <param name="args">The command-line arguments passed to the application.</param>
    /// <returns>The created <see cref="WebApplicationBuilder"/>.</returns>
    protected virtual WebApplicationBuilder CreateBuilder(string[] args) => WebApplication.CreateBuilder(args);

    /// <summary>
    /// Creates a logger instance for use during application startup
    /// before the main logging configuration is established.
    /// </summary>
    /// <returns>
    /// An instance of <see cref="ILogger"/> to be used for logging during the application's bootstrap phase.
    /// </returns>
    protected abstract ILogger CreateBootstrapLogger();

    /// <summary>
    /// Hook method called after the application builder is created.
    /// Override this method to customize behavior after builder creation.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance.</param>
    /// <param name="logger">Bootstrap logger.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected virtual ValueTask OnAfterCreateBuilder(WebApplicationBuilder builder, ILogger logger)
        => ValueTask.CompletedTask;

    /// <summary>
    /// Configures services for the application. This method must be implemented in derived classes.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance to configure services for.</param>
    /// <param name="logger">Bootstrap logger.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected abstract ValueTask OnConfigureServices(WebApplicationBuilder builder, ILogger logger);

    /// <summary>
    /// Hook method called after services have been configured.
    /// Override this method to add additional behavior after service configuration.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance.</param>
    /// <param name="logger">Bootstrap logger.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected virtual ValueTask OnAfterConfigureServices(WebApplicationBuilder builder, ILogger logger)
        => ValueTask.CompletedTask;

    /// <summary>
    /// Configures the web application's request pipeline. This method must be implemented in derived classes.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected abstract ValueTask OnConfigure(WebApplication app);

    /// <summary>
    /// Hook method called after the application has been configured.
    /// Override this method to add additional behavior after application configuration.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected virtual ValueTask OnAfterConfigure(WebApplication app) => ValueTask.CompletedTask;

    /// <summary>
    /// Hook method called immediately before the application starts running.
    /// Override this method to add custom logic that should execute after
    /// the application has been configured and built, but before
    /// <see cref="WebApplication.RunAsync"/> is invoked.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected virtual ValueTask OnBeforeStartup(WebApplication app) => ValueTask.CompletedTask;

    /// <summary>
    /// Handles any exceptions that occur during the application's execution.
    /// Override this method to customize exception handling behavior.
    /// </summary>
    /// <param name="exception">The exception that occurred.</param>
    /// <param name="app">
    /// The <see cref="WebApplication"/> instance,
    /// or <see langword="null"/> if the application could not be fully constructed.
    /// </param>
    /// <param name="logger">The logger to use for recording exception details.</param>
    /// <returns>An integer exit code if handled, or null if the exception should be re-thrown.</returns>
    protected virtual TaskOrValue<int?> OnException(Exception exception, WebApplication? app, ILogger logger)
        => (int?)null;

    /// <summary>
    /// Hook method called after the application has finished running, regardless of whether an exception occurred.
    /// Override this method to add custom finalization logic.
    /// </summary>
    /// <param name="app">
    /// The <see cref="WebApplication"/> instance,
    /// or <see langword="null"/> if the application could not be fully constructed.
    /// </param>
    /// <param name="logger">The logger to use for recording finalization events.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected virtual ValueTask OnFinally(WebApplication? app, ILogger logger)
        => ValueTask.CompletedTask;
}
