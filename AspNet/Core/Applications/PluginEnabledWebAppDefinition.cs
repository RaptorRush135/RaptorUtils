namespace RaptorUtils.AspNet.Applications;

using Microsoft.AspNetCore.Builder;

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
    /// Executes the application run logic, including invoking the plugins' run logic before the base run logic.
    /// </summary>
    /// <param name="args">The command-line arguments passed to the application.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected override async Task OnRun(string[] args)
    {
        this.Plugins = this.GetPlugins() ?? Array.Empty<WebAppPlugin>();

        await this.InvokePlugins(p => p.OnRun(args));

        await base.OnRun(args);
    }

    /// <summary>
    /// Invoked after the application builder is created, filtering the plugins to include only enabled ones
    /// and invoking their post-creation logic.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected override async ValueTask AfterCreateBuilder(WebApplicationBuilder builder)
    {
        this.Plugins = await this.Plugins
            .ToAsyncEnumerable()
            .WhereAwait(p => p.IsEnabled(builder).UnderlyingTask)
            .ToArrayAsync();

        await this.InvokePlugins(p => p.OnAfterCreateBuilder(builder));
    }

    /// <summary>
    /// Invoked to configure services for the application,
    /// allowing plugins to contribute their own service configurations.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance to configure services for.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected override ValueTask ConfigureServices(WebApplicationBuilder builder)
        => this.InvokePlugins(p => p.OnConfigureServices(builder));

    /// <summary>
    /// Invoked after services have been configured, allowing plugins to execute additional configuration logic.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected override ValueTask AfterConfigureServices(WebApplicationBuilder builder)
        => this.InvokePlugins(p => p.OnAfterConfigureServices(builder));

    /// <summary>
    /// Configures the web application's request pipeline, allowing plugins to add middleware or other configurations.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected override ValueTask Configure(WebApplication app)
        => this.InvokePlugins(p => p.OnConfigure(app));

    /// <summary>
    /// Invoked after the application has been configured, allowing plugins to perform additional configuration steps.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected override ValueTask AfterConfigure(WebApplication app)
        => this.InvokePlugins(p => p.OnAfterConfigure(app));

    /// <summary>
    /// Invoked immediately before the application starts running,
    /// allowing plugins to execute custom logic after the application
    /// has been fully configured and built, but before
    /// <see cref="WebApplication.RunAsync"/> is called.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected override ValueTask BeforeStartup(WebApplication app)
        => this.InvokePlugins(p => p.OnBeforeStartup(app));

    /// <summary>
    /// Handles exceptions that occur during the application's execution by invoking each plugin's exception handler.
    /// </summary>
    /// <param name="exception">The exception that occurred.</param>
    /// <returns>An integer exit code if handled by a plugin, or null if the exception should be re-thrown.</returns>
    protected override async TaskOrValue<int?> OnException(Exception exception)
    {
        int? exitCode = null;

        foreach (var plugin in this.Plugins)
        {
            exitCode ??= await plugin.OnException(exception);
        }

        return exitCode;
    }

    /// <summary>
    /// Hook method called after the application has finished running,
    /// allowing plugins to perform cleanup or finalization.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected override ValueTask OnFinally()
        => this.InvokePlugins(p => p.OnFinally());

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
