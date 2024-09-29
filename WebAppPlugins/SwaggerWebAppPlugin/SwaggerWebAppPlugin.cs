namespace RaptorUtils.AspNet.Applications.Plugins.Swagger;

using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SwaggerThemes;

using Swashbuckle.AspNetCore.SwaggerGen;

public class SwaggerWebAppPlugin(
    Func<WebApplicationBuilder, Task<bool>>? isEnabled = null,
    Action<WebApplicationBuilder>? metadataAction = null,
    Func<WebApplicationBuilder, Action<SwaggerGenOptions>>? setupActionProvider = null,
    Func<WebApplication, Theme?>? themeProvider = null)
    : WebAppPlugin(isEnabled)
{
    public override Task OnConfigureServices(WebApplicationBuilder builder)
    {
        metadataAction?.Invoke(builder);

        var setupAction = setupActionProvider?.Invoke(builder);
        builder.Services.AddSwaggerGen(setupAction);

        return Task.CompletedTask;
    }

    public override Task OnConfigure(WebApplication app)
    {
        app.UseSwagger();

        if (themeProvider?.Invoke(app) is { } theme)
        {
            app.UseSwaggerThemes(theme);
        }

        app.UseSwaggerUI();

        app.Logger.LogInformation("Swagger enabled");

        return Task.CompletedTask;
    }
}
