namespace RaptorUtils.AspNet.Applications.Plugins;

using System.Diagnostics;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

using RaptorUtils.Threading.Tasks;

/// <summary>
/// A plugin that measures and logs the time it takes for the web application to start.
/// </summary>
/// <inheritdoc />
public sealed class StartupTimerWebAppPlugin(
    Func<WebApplicationBuilder, TaskOrValue<bool>>? isEnabled = null)
    : WebAppPlugin(isEnabled)
{
    private long startTimestamp;

    /// <summary>
    /// Records the starting timestamp when the application begins running.
    /// </summary>
    /// <inheritdoc />
    public override ValueTask OnRun(string[] args)
    {
        this.startTimestamp = Stopwatch.GetTimestamp();

        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Calculates the elapsed time since <see cref="OnRun"/> and logs it.
    /// </summary>
    /// <inheritdoc />
    public override ValueTask OnBeforeStartup(WebApplication app)
    {
        TimeSpan elapsedTime = Stopwatch.GetElapsedTime(this.startTimestamp);

        app.Logger.LogInformation(
            "Host startup completed in {ElapsedTime} ms",
            elapsedTime.Milliseconds);

        return ValueTask.CompletedTask;
    }
}
