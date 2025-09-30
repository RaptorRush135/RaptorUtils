namespace RaptorUtils.AspNet.Applications;

using Microsoft.AspNetCore.Builder;

using RaptorUtils.Threading.Tasks;

/// <summary>
/// Represents a plugin for a web application that can be enabled or disabled based on a condition.
/// </summary>
/// <param name="isEnabled">
/// A function that determines if the plugin is enabled.
/// </param>
public class WebAppPlugin(
    Func<WebApplicationBuilder, TaskOrValue<bool>>? isEnabled)
{
    /// <summary>
    /// Executes logic when the application is run.
    /// This method can be overridden to provide specific behavior when the application is starting.
    /// </summary>
    /// <param name="args">The command line arguments passed to the application.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual ValueTask OnRun(string[] args) => ValueTask.CompletedTask;

    /// <summary>
    /// Determines whether the plugin is enabled based on the provided <see cref="WebApplicationBuilder"/>.
    /// </summary>
    /// <param name="builder">The web application builder to evaluate.</param>
    /// <returns>
    /// A task representing the asynchronous operation,
    /// with a boolean indicating whether the plugin is enabled.
    /// </returns>
    public virtual TaskOrValue<bool> IsEnabled(WebApplicationBuilder builder) => isEnabled?.Invoke(builder) ?? true;

    /// <summary>
    /// Executes logic after the web application builder has been created.
    /// This method can be overridden to provide specific behavior after the builder is created.
    /// </summary>
    /// <param name="builder">The web application builder that was created.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual ValueTask OnAfterCreateBuilder(WebApplicationBuilder builder) => ValueTask.CompletedTask;

    /// <summary>
    /// Executes logic to configure services for the web application.
    /// This method can be overridden to provide specific service configuration behavior.
    /// </summary>
    /// <param name="builder">The web application builder for which services are being configured.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual ValueTask OnConfigureServices(WebApplicationBuilder builder) => ValueTask.CompletedTask;

    /// <summary>
    /// Executes logic after the services have been configured.
    /// This method can be overridden to provide specific behavior after service configuration.
    /// </summary>
    /// <param name="builder">The web application builder for which services were configured.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual ValueTask OnAfterConfigureServices(WebApplicationBuilder builder) => ValueTask.CompletedTask;

    /// <summary>
    /// Executes logic to configure the web application.
    /// This method can be overridden to provide specific application configuration behavior.
    /// </summary>
    /// <param name="app">The web application being configured.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual ValueTask OnConfigure(WebApplication app) => ValueTask.CompletedTask;

    /// <summary>
    /// Executes logic after the web application has been configured.
    /// This method can be overridden to provide specific behavior after application configuration.
    /// </summary>
    /// <param name="app">The web application that was configured.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual ValueTask OnAfterConfigure(WebApplication app) => ValueTask.CompletedTask;

    /// <summary>
    /// Executes logic immediately before the web application starts running.
    /// This method can be overridden to provide specific behavior after the application
    /// has been fully configured and built, but before <see cref="WebApplication.RunAsync"/> is invoked.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual ValueTask OnBeforeStartup(WebApplication app) => ValueTask.CompletedTask;

    /// <summary>
    /// Executes logic when an exception occurs.
    /// This method can be overridden to provide specific exception handling behavior.
    /// </summary>
    /// <param name="exception">The exception that occurred.</param>
    /// <param name="app">
    /// The <see cref="WebApplication"/> instance,
    /// or <see langword="null"/> if the application could not be fully constructed.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation, with an optional integer indicating the exit code.
    /// </returns>
    public virtual TaskOrValue<int?> OnException(Exception exception, WebApplication? app) => (int?)null;

    /// <summary>
    /// Executes finalization logic for the plugin.
    /// This method can be overridden to provide specific behavior for cleanup or finalization.
    /// </summary>
    /// <param name="app">
    /// The <see cref="WebApplication"/> instance,
    /// or <see langword="null"/> if the application could not be fully constructed.
    /// </param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual ValueTask OnFinally(WebApplication? app) => ValueTask.CompletedTask;
}
