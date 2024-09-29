namespace RaptorUtils.AspNet.Applications.Plugins.Serilog;

using global::Serilog;
using global::Serilog.Events;
using global::Serilog.Sinks.SystemConsole.Themes;

using Microsoft.AspNetCore.Builder;

using RaptorUtils.AspNet.Applications;

public abstract class SerilogWebAppPlugin(
    Func<WebApplicationBuilder, Task<bool>>? isEnabled)
    : WebAppPlugin(isEnabled)
{
    public virtual ConsoleTheme? ConsoleTheme { get; }

    public override Task OnRun(string[] args)
    {
        Log.Logger = this.CreateBootstrapLogger();

        Log.Information("Starting web application");

        return Task.CompletedTask;
    }

    public override Task OnAfterCreateBuilder(WebApplicationBuilder builder)
    {
        Log.Information("Environment: {Environment}", builder.Environment.EnvironmentName);

        builder.Services.AddSerilog(this.ConfigureLogger);

        return Task.CompletedTask;
    }

    public override Task OnConfigureServices(WebApplicationBuilder builder)
    {
        Log.Information("Configuring services");
        return Task.CompletedTask;
    }

    public override Task OnAfterConfigureServices(WebApplicationBuilder builder)
    {
        Log.Information("Total services: {Count}", builder.Services.Count);
        return Task.CompletedTask;
    }

    public override Task OnConfigure(WebApplication app)
    {
        Log.Information("Configuring app");
        return Task.CompletedTask;
    }

    public override Task OnAfterConfigure(WebApplication app)
    {
        Log.Information("Starting app");
        return Task.CompletedTask;
    }

    public override Task<int?> OnException(Exception exception)
    {
        Log.Fatal(exception, "Application terminated unexpectedly");
        return Task.FromResult<int?>(1);
    }

    public override async Task OnFinally()
    {
        Log.Information("Flushing logs...");
        await Log.CloseAndFlushAsync();
    }

    protected virtual ILogger CreateBootstrapLogger()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(theme: this.ConsoleTheme, applyThemeToRedirectedOutput: true)
            .CreateBootstrapLogger();
    }

    protected abstract void ConfigureLogger(IServiceProvider serviceProvider, LoggerConfiguration options);
}
