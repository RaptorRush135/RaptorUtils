namespace RaptorUtils.AspNet.Applications.Plugins.Serilog;

using global::Serilog;
using global::Serilog.Events;
using global::Serilog.Sinks.SystemConsole.Themes;

using Microsoft.AspNetCore.Builder;

using RaptorUtils.AspNet.Applications;

/// <summary>
/// An abstract base class for implementing a Serilog-based plugin for web applications.
/// This class provides lifecycle methods for configuring logging and application services.
/// </summary>
/// <inheritdoc/>
public abstract class SerilogWebAppPlugin(
    Func<WebApplicationBuilder, Task<bool>>? isEnabled)
    : WebAppPlugin(isEnabled)
{
    /// <summary>
    /// Gets the console theme used for logging output. Can be overridden by derived classes.
    /// </summary>
    public virtual ConsoleTheme? ConsoleTheme { get; }

    /// <summary>
    /// Executes when the web application is run. Initializes the logger and logs  the start of the application.
    /// </summary>
    /// <inheritdoc/>
    public override Task OnRun(string[] args)
    {
        Log.Logger = this.CreateBootstrapLogger();

        Log.Information("Starting web application");

        return Task.CompletedTask;
    }

    /// <summary>
    /// Executes after the web application builder is created. Logs the current environment
    /// and configures Serilog logging.
    /// </summary>
    /// <inheritdoc/>
    public override Task OnAfterCreateBuilder(WebApplicationBuilder builder)
    {
        Log.Information("Environment: {Environment}", builder.Environment.EnvironmentName);

        builder.Services.AddSerilog(this.ConfigureLogger);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Executes to configure application services. Logs the configuration process.
    /// </summary>
    /// <inheritdoc/>
    public override Task OnConfigureServices(WebApplicationBuilder builder)
    {
        Log.Information("Configuring services");
        return Task.CompletedTask;
    }

    /// <summary>
    /// Executes after services are configured. Logs the total number of services registered.
    /// </summary>
    /// <inheritdoc/>
    public override Task OnAfterConfigureServices(WebApplicationBuilder builder)
    {
        Log.Information("Total services: {Count}", builder.Services.Count);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Executes to configure the web application. Logs the configuration process.
    /// </summary>
    /// <inheritdoc/>
    public override Task OnConfigure(WebApplication app)
    {
        Log.Information("Configuring app");
        return Task.CompletedTask;
    }

    /// <summary>
    /// Executes after the application is configured. Logs the start of the application.
    /// </summary>
    /// <inheritdoc/>
    public override Task OnAfterConfigure(WebApplication app)
    {
        Log.Information("Starting app");
        return Task.CompletedTask;
    }

    /// <summary>
    /// Executes when an exception occurs during the application lifecycle.
    /// Logs the exception and returns an exit code of 1.
    /// </summary>
    /// <inheritdoc/>
    public override Task<int?> OnException(Exception exception)
    {
        Log.Fatal(exception, "Application terminated unexpectedly");
        return Task.FromResult<int?>(1);
    }

    /// <summary>
    /// Executes during the finalization of the application lifecycle.
    /// Flushes the logs and ensures all log entries are written.
    /// </summary>
    /// <returns>A task representing the asynchronous operation of finalizing logging.</returns>
    public override async Task OnFinally()
    {
        Log.Information("Flushing logs...");
        await Log.CloseAndFlushAsync();
    }

    /// <summary>
    /// Creates the initial bootstrap logger configuration. Can be overridden by derived classes
    /// to customize the logger settings.
    /// </summary>
    /// <returns>An instance of <see cref="ILogger"/> configured for the application.</returns>
    protected virtual ILogger CreateBootstrapLogger()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(theme: this.ConsoleTheme, applyThemeToRedirectedOutput: true)
            .CreateBootstrapLogger();
    }

    /// <summary>
    /// Configures the logger for the application. Must be implemented by derived classes
    /// to provide specific logging configurations.
    /// </summary>
    /// <param name="serviceProvider">The service provider to resolve dependencies for logging configuration.</param>
    /// <param name="options">The logger configuration to modify.</param>
    protected abstract void ConfigureLogger(IServiceProvider serviceProvider, LoggerConfiguration options);
}
