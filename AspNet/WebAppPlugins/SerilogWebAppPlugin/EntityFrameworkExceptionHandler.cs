namespace RaptorUtils.AspNet.Applications.Plugins.Serilog;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

/// <summary>
/// Provides helper methods for handling exceptions related to Entity Framework Core.
/// </summary>
public static class EntityFrameworkExceptionHandler
{
    /// <summary>
    /// Handles the special case where the host is aborted by Entity Framework Core design-time tools.
    /// </summary>
    /// <remarks>This method is intended to detect and handle host aborts triggered by Entity Framework Core
    /// design-time operations, such as migrations or scaffolding. In such cases, it logs an informational message and
    /// returns 0 to indicate a controlled shutdown.</remarks>
    /// <param name="exception">The exception to evaluate for a design-time host abort.</param>
    /// <param name="app">The current web application instance. This parameter is not used.</param>
    /// <param name="logger">The logger used to record information about the abort event.</param>
    /// <returns>
    /// 0 if the exception indicates a host abort by Entity Framework Core design-time tools;
    /// otherwise, <see langword="null"/>.
    /// </returns>
    public static int? HandleEfDesignHostAbort(Exception exception, WebApplication? app, ILogger logger)
    {
        _ = app;
        if (exception is HostAbortedException
            && exception.Source == "Microsoft.EntityFrameworkCore.Design")
        {
            logger.LogInformation("Host aborted");
            return 0;
        }

        return null;
    }
}
