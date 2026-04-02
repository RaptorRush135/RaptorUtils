namespace RaptorUtils.AspNet.Applications;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

using RaptorUtils.Threading.Tasks;

/// <summary>
/// Provides an abstract definition for a web application that supports plugins.
/// This class extends <see cref="WebAppDefinition"/> to allow dynamic behavior through plugins during
/// the application lifecycle.
/// </summary>
public abstract class PluginEnabledWebAppDefinition : WebAppDefinition
{
    private ICollection<WebAppPlugin> Plugins { get; set; } = null!;

    /// <summary>
    /// Retrieves the collection of plugins to be used in the application.
    /// This method must be implemented in derived classes to provide the specific plugins.
    /// </summary>
    /// <returns>A collection of <see cref="WebAppPlugin"/> instances or null if no plugins are available.</returns>
    protected abstract ICollection<WebAppPlugin>? GetPlugins();

    /// <summary>
    /// Invoked after the application builder is created, filtering the plugins to include only enabled ones
    /// and invoking their post-creation logic.
    /// </summary>
    /// <inheritdoc/>
    protected override async ValueTask OnAfterCreateBuilder(WebApplicationBuilder builder, ILogger logger)
    {
        this.Plugins = this.GetPlugins() ?? [];

        this.Plugins = await this.Plugins
            .ToAsyncEnumerable()
            .Where((p, _) => p.IsEnabled(builder).UnderlyingTask)
            .ToArrayAsync();

        await this.InvokePlugins(p => p.OnAfterCreateBuilder(builder, logger));
    }

    /// <summary>
    /// Invoked to configure services for the application,
    /// allowing plugins to contribute their own service configurations.
    /// </summary>
    /// <inheritdoc/>
    protected override ValueTask OnConfigureServices(WebApplicationBuilder builder, ILogger logger)
        => this.InvokePlugins(p => p.OnConfigureServices(builder, logger));

    /// <summary>
    /// Invoked after services have been configured, allowing plugins to execute additional configuration logic.
    /// </summary>
    /// <inheritdoc/>
    protected override ValueTask OnAfterConfigureServices(WebApplicationBuilder builder, ILogger logger)
        => this.InvokePlugins(p => p.OnAfterConfigureServices(builder, logger));

    /// <summary>
    /// Configures the web application's request pipeline, allowing plugins to add middleware or other configurations.
    /// </summary>
    /// <inheritdoc/>
    protected override ValueTask OnConfigure(WebApplication app)
        => this.InvokePlugins(p => p.OnConfigure(app));

    /// <summary>
    /// Invoked after the application has been configured, allowing plugins to perform additional configuration steps.
    /// </summary>
    /// <inheritdoc/>
    protected override ValueTask OnAfterConfigure(WebApplication app)
        => this.InvokePlugins(p => p.OnAfterConfigure(app));

    /// <summary>
    /// Invoked immediately before the application starts running,
    /// allowing plugins to execute custom logic after the application
    /// has been fully configured and built, but before
    /// <see cref="WebApplication.RunAsync"/> is called.
    /// </summary>
    /// <inheritdoc/>
    protected override ValueTask OnBeforeStartup(WebApplication app)
        => this.InvokePlugins(p => p.OnBeforeStartup(app));

    /// <summary>
    /// Handles exceptions that occur during the application's execution by invoking each plugin's exception handler.
    /// </summary>
    /// <returns>An integer exit code if handled by a plugin, or null if the exception should be re-thrown.</returns>
    /// <inheritdoc/>
    protected override async TaskOrValue<int?> OnException(Exception exception, WebApplication? app, ILogger logger)
    {
        int? exitCode = null;

        foreach (var plugin in this.Plugins)
        {
            exitCode ??= await plugin.OnException(exception, app, logger);
        }

        return exitCode;
    }

    /// <summary>
    /// Hook method called after the application has finished running,
    /// allowing plugins to perform cleanup or finalization.
    /// </summary>
    /// <inheritdoc/>
    protected override ValueTask OnFinally(WebApplication? app, ILogger logger)
        => this.InvokePlugins(p => p.OnFinally(app, logger));

    /// <summary>
    /// Invokes a specified function on each plugin in the collection.
    /// </summary>
    /// <param name="pluginFunc">A function that takes a <see cref="WebAppPlugin"/> and returns a task.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected async ValueTask InvokePlugins(Func<WebAppPlugin, ValueTask> pluginFunc)
    {
        foreach (var plugin in this.Plugins)
        {
            await pluginFunc.Invoke(plugin);
        }
    }
}
