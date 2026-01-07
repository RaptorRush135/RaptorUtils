namespace RaptorUtils.AspNet.Applications.Plugins.Serilog;

using global::Serilog;
using global::Serilog.Events;
using global::Serilog.Sinks.SystemConsole.Themes;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

using RaptorUtils.AspNet.Applications;
using RaptorUtils.Threading.Tasks;

using ILogger = Microsoft.Extensions.Logging.ILogger;
using ISerilogLogger = global::Serilog.ILogger;

/// <summary>
/// An abstract base class for implementing a Serilog-based plugin for web applications.
/// This class provides lifecycle methods for configuring logging and application services.
/// </summary>
/// <inheritdoc/>
public abstract class SerilogWebAppPlugin(
    Func<WebApplicationBuilder, TaskOrValue<bool>>? isEnabled)
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
    public override ValueTask OnRun(string[] args)
    {
        Log.Logger = this.CreateBootstrapLogger();

        Log.Information("Starting web application");

        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Executes after the web application builder is created. Logs the current environment
    /// and configures Serilog logging.
    /// </summary>
    /// <inheritdoc/>
    public override ValueTask OnAfterCreateBuilder(WebApplicationBuilder builder)
    {
        Log.Information("Environment: {Environment}", builder.Environment.EnvironmentName);

        builder.Services.AddSerilog(this.ConfigureLogger, preserveStaticLogger: true);

        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Executes to configure application services. Logs the configuration process.
    /// </summary>
    /// <inheritdoc/>
    public override ValueTask OnConfigureServices(WebApplicationBuilder builder)
    {
        Log.Information("Configuring services");
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Executes after services are configured. Logs the total number of services registered.
    /// </summary>
    /// <inheritdoc/>
    public override ValueTask OnAfterConfigureServices(WebApplicationBuilder builder)
    {
        Log.Information("Total services: {Count}", builder.Services.Count);
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Executes to configure the web application. Logs the configuration process.
    /// </summary>
    /// <inheritdoc/>
    public override ValueTask OnConfigure(WebApplication app)
    {
        app.Logger.LogInformation("Configuring app");
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Executes after the application is configured. Logs the start of the application.
    /// </summary>
    /// <inheritdoc/>
    public override ValueTask OnAfterConfigure(WebApplication app)
    {
        app.Logger.LogInformation("Starting app");
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Invoked when an unhandled exception occurs during the application lifecycle.
    /// Executes registered exception handlers in order, and returns
    /// the first non-<see langword="null"/> exit code produced by a handler.
    /// If no handler handles the exception, logs a critical error and returns an exit code of 1.
    /// </summary>
    /// <inheritdoc/>
    public override TaskOrValue<int?> OnException(Exception exception, WebApplication? app)
    {
        var logger = GetLogger(app);

        foreach (var handler in this.GetExceptionHandlers())
        {
            int? result = handler.Invoke(exception, app, logger);
            if (result is not null)
            {
                return result;
            }
        }

        logger.LogCritical(exception, "Application terminated unexpectedly");

        return 1;
    }

    /// <summary>
    /// Executes during the finalization of the application lifecycle.
    /// Flushes the logs and ensures all log entries are written.
    /// </summary>
    /// <inheritdoc/>
    public override ValueTask OnFinally(WebApplication? app)
    {
        var logger = GetLogger(app);
        logger.LogInformation("Flushing logs...");
        return Log.CloseAndFlushAsync();
    }

    /// <summary>
    /// Creates the initial bootstrap logger configuration. Can be overridden by derived classes
    /// to customize the logger settings.
    /// </summary>
    /// <returns>An instance of <see cref="ISerilogLogger"/> configured for the application.</returns>
    protected virtual ISerilogLogger CreateBootstrapLogger()
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

    /// <summary>
    /// Gets the exception handlers to invoke when an exception occurs.
    /// Handlers receive the <see cref="Exception"/>, optional <see cref="WebApplication"/>,
    /// and an <see cref="ILogger"/>, and return a nullable exit code.
    /// The first non-<see langword="null"/> value is used as the application exit code.
    /// </summary>
    /// <returns>
    /// A collection of exception handler delegates.
    /// </returns>
    protected virtual ICollection<Func<Exception, WebApplication?, ILogger, int?>> GetExceptionHandlers() => [];

    private static ILogger GetLogger(WebApplication? app)
    {
        return app?.Logger ?? ToMicrosoftLogger(Log.Logger);
    }

    private static ILogger ToMicrosoftLogger(ISerilogLogger serilogLogger)
    {
        var factory = LoggerFactory.Create(
            builder => builder.AddSerilog(serilogLogger, dispose: false));

        return factory.CreateLogger(string.Empty);
    }
}
