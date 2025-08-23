namespace RaptorUtils.AspNet.Applications.Plugins.Swagger;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SwaggerThemes;

using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// A plugin for integrating Swagger into a web application. This class configures
/// Swagger services and middleware to generate and serve API documentation.
/// </summary>
/// <inheritdoc/>
/// <param name="isEnabled">
/// <inheritdoc cref="WebAppPlugin(Func{WebApplicationBuilder, Task{bool}}?)" path="/param[@name='isEnabled']"/>
/// </param>
/// <param name="metadataAction">
/// An optional action to configure additional metadata for the Swagger documentation.
/// </param>
/// <param name="setupActionProvider">
/// An optional function that provides a setup action for SwaggerGen options.
/// </param>
/// <param name="themeProvider">
/// An optional function that provides a theme for the Swagger UI.
/// </param>
public class SwaggerWebAppPlugin(
    Func<WebApplicationBuilder, Task<bool>>? isEnabled = null,
    Action<WebApplicationBuilder>? metadataAction = null,
    Func<WebApplicationBuilder, Action<SwaggerGenOptions>>? setupActionProvider = null,
    Func<WebApplication, Theme?>? themeProvider = null)
    : WebAppPlugin(isEnabled)
{
    /// <summary>
    /// Configures the services required for Swagger in the web application.
    /// Invokes the optional metadata action and adds Swagger services with the provided setup action.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <inheritdoc/>
    public override Task OnConfigureServices(WebApplicationBuilder builder)
    {
        metadataAction?.Invoke(builder);

        var setupAction = setupActionProvider?.Invoke(builder);
        builder.Services.AddSwaggerGen(setupAction);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Configures the middleware for serving Swagger and Swagger UI in the web application.
    /// <para>
    /// If a theme provider is specified, it applies the theme to the Swagger UI.
    /// </para>
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <inheritdoc/>
    public override Task OnConfigure(WebApplication app)
    {
        app.UseSwagger();

        if (themeProvider?.Invoke(app) is { } theme)
        {
            app.UseSwaggerUI(theme);
        }

        app.UseSwaggerUI();

        app.Logger.LogInformation("Swagger enabled");

        return Task.CompletedTask;
    }
}
