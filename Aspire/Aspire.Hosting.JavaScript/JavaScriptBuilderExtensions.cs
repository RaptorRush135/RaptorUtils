namespace RaptorUtils.Aspire.Hosting.JavaScript;

using global::Aspire.Hosting;
using global::Aspire.Hosting.ApplicationModel;
using global::Aspire.Hosting.JavaScript;

using RaptorUtils.Net;

/// <summary>
/// Provides extension methods for building <see cref="JavaScriptAppResource"/> resources.
/// </summary>
public static class JavaScriptBuilderExtensions
{
    /// <summary>
    /// Configures the resource builder to use a randomly available port.
    /// This method finds an available port and assigns it to the resource.
    /// </summary>
    /// <param name="builder">The resource builder to extend.</param>
    /// <param name="positional">Indicates whether the port should be passed as a positional argument.</param>
    /// <returns>
    /// An updated instance of <see cref="IResourceBuilder{JavaScriptAppResource}"/>
    /// configured with a random port.</returns>
    public static IResourceBuilder<JavaScriptAppResource> WithRandomPort(
        this IResourceBuilder<JavaScriptAppResource> builder,
        bool positional = true)
    {
        int port = PortFinder.GetAvailablePort();

        return builder.WithPort(port, positional);
    }

    /// <summary>
    /// Configures the resource builder to use a specific port.
    /// This method sets the port for the resource and configures the HTTP endpoint accordingly.
    /// </summary>
    /// <param name="builder">The resource builder to extend.</param>
    /// <param name="port">The port number to assign to the resource.</param>
    /// <param name="positional">Indicates whether the port should be passed as a positional argument.</param>
    /// <returns>
    /// An updated instance of <see cref="IResourceBuilder{JavaScriptAppResource}"/> configured with the specified port.
    /// </returns>
    public static IResourceBuilder<JavaScriptAppResource> WithPort(
        this IResourceBuilder<JavaScriptAppResource> builder,
        int port,
        bool positional = true)
    {
        string portArg = $"--port={port}";

        if (positional)
        {
            builder.WithPositionalArgs(portArg);
        }
        else
        {
            builder.WithArgs(portArg);
        }

        return builder.WithHttpEndpoint(targetPort: port);
    }
}
