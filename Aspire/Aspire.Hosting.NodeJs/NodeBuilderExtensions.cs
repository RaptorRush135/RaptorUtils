namespace RaptorUtils.Aspire.Hosting.NodeJs;

using global::Aspire.Hosting;
using global::Aspire.Hosting.ApplicationModel;

using RaptorUtils.Net;

public static class NodeBuilderExtensions
{
    public static IResourceBuilder<NodeAppResource> WithRandomPort(
        this IResourceBuilder<NodeAppResource> builder)
    {
        int port = PortFinder.GetAvailablePort();

        return builder.WithPort(port);
    }

    public static IResourceBuilder<NodeAppResource> WithPort(
        this IResourceBuilder<NodeAppResource> builder,
        int port)
    {
        return builder
            .WithPositionalArgs($"--port={port}")
            .WithHttpEndpoint(targetPort: port);
    }
}
