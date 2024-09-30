namespace RaptorUtils.AspNet.Applications;

using Microsoft.AspNetCore.Builder;

/// <summary>
/// Represents a plugin for a web application that can be enabled or disabled based on a condition.
/// </summary>
/// <param name="isEnabled">
/// A function that determines if the plugin is enabled.
/// </param>
public class WebAppPlugin(Func<WebApplicationBuilder, Task<bool>>? isEnabled)
{
    /// <summary>
    /// Executes logic when the application is run.
    /// This method can be overridden to provide specific behavior when the application is starting.
    /// </summary>
    /// <param name="args">The command line arguments passed to the application.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual Task OnRun(string[] args)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Determines whether the plugin is enabled based on the provided <see cref="WebApplicationBuilder"/>.
    /// </summary>
    /// <param name="builder">The web application builder to evaluate.</param>
    /// <returns>
    /// A task representing the asynchronous operation,
    /// with a boolean indicating whether the plugin is enabled.
    /// </returns>
    public virtual Task<bool> IsEnabled(WebApplicationBuilder builder)
    {
        return isEnabled?.Invoke(builder) ?? Task.FromResult(true);
    }

    /// <summary>
    /// Executes logic after the web application builder has been created.
    /// This method can be overridden to provide specific behavior after the builder is created.
    /// </summary>
    /// <param name="builder">The web application builder that was created.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual Task OnAfterCreateBuilder(WebApplicationBuilder builder)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Executes logic to configure services for the web application.
    /// This method can be overridden to provide specific service configuration behavior.
    /// </summary>
    /// <param name="builder">The web application builder for which services are being configured.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual Task OnConfigureServices(WebApplicationBuilder builder)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Executes logic after the services have been configured.
    /// This method can be overridden to provide specific behavior after service configuration.
    /// </summary>
    /// <param name="builder">The web application builder for which services were configured.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual Task OnAfterConfigureServices(WebApplicationBuilder builder)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Executes logic to configure the web application.
    /// This method can be overridden to provide specific application configuration behavior.
    /// </summary>
    /// <param name="app">The web application being configured.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual Task OnConfigure(WebApplication app)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Executes logic after the web application has been configured.
    /// This method can be overridden to provide specific behavior after application configuration.
    /// </summary>
    /// <param name="app">The web application that was configured.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual Task OnAfterConfigure(WebApplication app)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Executes logic when an exception occurs.
    /// This method can be overridden to provide specific exception handling behavior.
    /// </summary>
    /// <param name="exception">The exception that occurred.</param>
    /// <returns>
    /// A task representing the asynchronous operation, with an optional integer indicating the exit code.
    /// </returns>
    public virtual Task<int?> OnException(Exception exception)
    {
        return Task.FromResult<int?>(null);
    }

    /// <summary>
    /// Executes finalization logic for the plugin.
    /// This method can be overridden to provide specific behavior for cleanup or finalization.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual Task OnFinally()
    {
        return Task.CompletedTask;
    }
}
