namespace RaptorUtils.AspNet.Logging;

using Microsoft.Extensions.Logging;

#pragma warning disable CA2254

/// <summary>
/// Represents a logging context that conditionally logs messages
/// based on the specified log level. This struct encapsulates a logger
/// and the level at which messages should be logged.
/// </summary>
/// <param name="Logger">The logger instance used for logging messages.</param>
/// <param name="LogLevel">The minimum log level required for messages to be logged.</param>
public readonly record struct ConditionalLogContext(
    ILogger Logger,
    LogLevel LogLevel)
{
    /// <summary>
    /// Logs a message at the specified log level using the provided message template and arguments.
    /// </summary>
    /// <param name="messageTemplate">The message template that defines the format of the logged message.</param>
    /// <param name="args">An array of objects to format into the message template.</param>
    public void Log(string messageTemplate, params object[] args)
    {
        this.Logger.Log(this.LogLevel, messageTemplate, args);
    }

    /// <summary>
    /// Logs a message with an event ID at the specified log level using the provided message template and arguments.
    /// </summary>
    /// <param name="eventId">The event ID associated with the logged message.</param>
    /// <param name="messageTemplate">The message template that defines the format of the logged message.</param>
    /// <param name="args">An array of objects to format into the message template.</param>
    public void Log(EventId eventId, string messageTemplate, params object[] args)
    {
        this.Logger.Log(this.LogLevel, eventId, messageTemplate, args);
    }

    /// <summary>
    /// Logs an exception with a message template and arguments at the specified log level.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="messageTemplate">The message template that defines the format of the logged message.</param>
    /// <param name="args">An array of objects to format into the message template.</param>
    public void Log(Exception exception, string messageTemplate, params object[] args)
    {
        this.Logger.Log(this.LogLevel, exception, messageTemplate, args);
    }

    /// <summary>
    /// Logs an exception with an event ID, message template, and arguments at the specified log level.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="eventId">The event ID associated with the logged message.</param>
    /// <param name="messageTemplate">The message template that defines the format of the logged message.</param>
    /// <param name="args">An array of objects to format into the message template.</param>
    public void Log(Exception exception, EventId eventId, string messageTemplate, params object[] args)
    {
        this.Logger.Log(this.LogLevel, eventId, exception, messageTemplate, args);
    }
}
