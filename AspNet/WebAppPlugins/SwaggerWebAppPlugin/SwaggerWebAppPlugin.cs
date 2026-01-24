namespace RaptorUtils.AspNet.Applications.Plugins.Swagger;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using RaptorUtils.Threading.Tasks;

using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// A plugin for integrating Swagger into a web application. This class configures
/// Swagger services and middleware to generate and serve API documentation.
/// </summary>
/// <param name="isEnabled">
/// <inheritdoc cref="WebAppPlugin(Func{WebApplicationBuilder, TaskOrValue{bool}}?)" path="/param[@name='isEnabled']"/>
/// </param>
/// <param name="metadataAction">
/// An optional action to configure additional metadata for the Swagger documentation.
/// </param>
/// <param name="setupActionProvider">
/// An optional function that provides a setup action for SwaggerGen options.
/// </param>
/// <param name="onConfigure">
/// An optional action invoked during application configuration time.
/// Can be used to register additional middleware or perform runtime configuration
/// before `UseSwagger` &amp; `UseSwaggerUI` are applied.
/// </param>
public class SwaggerWebAppPlugin(
    Func<WebApplicationBuilder, TaskOrValue<bool>>? isEnabled = null,
    Action<WebApplicationBuilder>? metadataAction = null,
    Func<WebApplicationBuilder, Action<SwaggerGenOptions>>? setupActionProvider = null,
    Action<WebApplication>? onConfigure = null)
    : WebAppPlugin(isEnabled)
{
    /// <summary>
    /// Configures the services required for Swagger in the web application.
    /// Invokes the optional metadata action and adds Swagger services with the provided setup action.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <inheritdoc/>
    public override ValueTask OnConfigureServices(WebApplicationBuilder builder)
    {
        metadataAction?.Invoke(builder);

        var setupAction = setupActionProvider?.Invoke(builder);
        builder.Services.AddSwaggerGen(setupAction);

        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Configures the middleware for serving Swagger and Swagger UI in the web application.
    /// <para>
    /// If a theme provider is specified, it applies the theme to the Swagger UI.
    /// </para>
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <inheritdoc/>
    public override ValueTask OnConfigure(WebApplication app)
    {
        onConfigure?.Invoke(app);

        app.UseSwagger();
        app.UseSwaggerUI();

        app.Logger.LogWarning("Swagger enabled");

        return ValueTask.CompletedTask;
    }
}
