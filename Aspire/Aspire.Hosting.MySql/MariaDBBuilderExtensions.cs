namespace RaptorUtils.Aspire.Hosting.MySql;

using global::Aspire.Hosting;
using global::Aspire.Hosting.ApplicationModel;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for adding MariaDB resources to an <see cref="IDistributedApplicationBuilder"/>.
/// </summary>
public static class MariaDBBuilderExtensions
{
    /// <summary>
    /// Adds a MariaDB server resource to the application model. For local development a container is used.
    /// </summary>
    /// <remarks>
    /// This version of the package defaults to the <inheritdoc cref="MariaDBContainerImageTags.Tag"/> tag
    /// of the <inheritdoc cref="MariaDBContainerImageTags.Image"/> container image.
    /// </remarks>
    /// <param name="builder">The <see cref="IDistributedApplicationBuilder"/>.</param>
    /// <param name="name">
    /// The name of the resource. This name will be used as the connection string name when referenced in a dependency.
    /// </param>
    /// <param name="password">
    /// The parameter used to provide the root password for the MySQL resource.
    /// If <see langword="null"/> a random password will be generated.
    /// </param>
    /// <param name="port">The host port for MySQL.</param>
    /// <returns>A reference to the <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<MySqlServerResource> AddMariaDB(
        this IDistributedApplicationBuilder builder,
        string name,
        IResourceBuilder<ParameterResource>? password = null,
        int? port = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(name);

        var passwordParameter = password?.Resource
            ?? ParameterResourceBuilderExtensions.CreateDefaultPasswordParameter(builder, $"{name}-password");

        var resource = new MySqlServerResource(name, passwordParameter);

        string? connectionString = null;

        builder.Eventing.Subscribe<ConnectionStringAvailableEvent>(resource, async (_, ct) =>
        {
            connectionString = await resource.ConnectionStringExpression.GetValueAsync(ct).ConfigureAwait(false);

            if (connectionString == null)
            {
                throw new DistributedApplicationException(
                    "ConnectionStringAvailableEvent was published for " +
                    $"the '{resource.Name}' resource but the connection string was null.");
            }
        });

        string healthCheckKey = $"{name}_check";

        builder.Services.AddHealthChecks().AddMySql(
            _ => connectionString ?? throw new InvalidOperationException("Connection string is unavailable"),
            name: healthCheckKey);

        return builder
            .AddResource(resource)
            .WithEndpoint(port: port, targetPort: 3306, name: "tcp")
            .WithImage(MariaDBContainerImageTags.Image, MariaDBContainerImageTags.Tag)
            .WithImageRegistry(MariaDBContainerImageTags.Registry)
            .WithEnvironment(context => context.EnvironmentVariables["MYSQL_ROOT_PASSWORD"] = resource.PasswordParameter)
            .WithHealthCheck(healthCheckKey);
    }
}
