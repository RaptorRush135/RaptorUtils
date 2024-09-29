namespace RaptorUtils.AspNet.Applications;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;

public abstract class PluginEnabledWebAppDefinition : WebAppDefinition
{
    private ICollection<WebAppPlugin> Plugins { get; set; } = null!;

    protected abstract ICollection<WebAppPlugin>? GetPlugins();

    protected override async Task OnRun(string[] args)
    {
        this.Plugins = this.GetPlugins() ?? [];

        await this.InvokePlugins(p => p.OnRun(args));

        await base.OnRun(args);
    }

    protected override async Task AfterCreateBuilder(WebApplicationBuilder builder)
    {
        this.Plugins = await this.Plugins
            .ToAsyncEnumerable()
            .WhereAwait(async p => await p.IsEnabled(builder))
            .ToArrayAsync();

        await this.InvokePlugins(p => p.OnAfterCreateBuilder(builder));
    }

    protected override async Task ConfigureServices(WebApplicationBuilder builder)
    {
        await this.InvokePlugins(p => p.OnConfigureServices(builder));
    }

    protected override async Task AfterConfigureServices(WebApplicationBuilder builder)
    {
        await this.InvokePlugins(p => p.OnAfterConfigureServices(builder));
    }

    protected override async Task Configure(WebApplication app)
    {
        await this.InvokePlugins(p => p.OnConfigure(app));
    }

    protected override async Task AfterConfigure(WebApplication app)
    {
        await this.InvokePlugins(p => p.OnAfterConfigure(app));
    }

    protected override async Task<int?> OnException(Exception exception)
    {
        int? exitCode = null;

        foreach (var plugin in this.Plugins)
        {
            exitCode ??= await plugin.OnException(exception);
        }

        return exitCode;
    }

    protected override async Task OnFinally()
    {
        await this.InvokePlugins(p => p.OnFinally());
    }

    protected async Task InvokePlugins(Func<WebAppPlugin, Task> pluginFunc)
    {
        foreach (var plugin in this.Plugins)
        {
            await pluginFunc.Invoke(plugin);
        }
    }
}
