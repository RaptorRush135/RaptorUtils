namespace RaptorUtils.AspNet.Logging;

using Microsoft.Extensions.Logging;

#pragma warning disable CA2254

public readonly record struct ConditionalLogContext(
    ILogger Logger,
    LogLevel LogLevel)
{
    public void Log(string messageTemplate, params object[] args)
    {
        this.Logger.Log(this.LogLevel, messageTemplate, args);
    }

    public void Log(EventId eventId, string messageTemplate, params object[] args)
    {
        this.Logger.Log(this.LogLevel, eventId, messageTemplate, args);
    }

    public void Log(Exception exception, string messageTemplate, params object[] args)
    {
        this.Logger.Log(this.LogLevel, exception, messageTemplate, args);
    }

    public void Log(Exception exception, EventId eventId, string messageTemplate, params object[] args)
    {
        this.Logger.Log(this.LogLevel, eventId, exception, messageTemplate, args);
    }
}
