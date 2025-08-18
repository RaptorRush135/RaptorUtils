namespace RaptorUtils.Aspire.Hosting;

using global::Aspire.Hosting;
using global::Aspire.Hosting.ApplicationModel;

using RaptorUtils.Collections.Extensions;

/// <summary>
/// Provides extension methods for resource builders.
/// </summary>
public static class ResourceBuilderExtensions
{
    /// <summary>
    /// Adds positional arguments to the resource builder's argument context.
    /// Ensures that a placeholder for positional arguments is present in the context.
    /// </summary>
    /// <typeparam name="T"> The resource type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="args">The positional arguments to add to the resource builder.</param>
    /// <returns>
    /// <returns>The <see cref="IResourceBuilder{T}"/>.</returns>
    /// </returns>
    public static IResourceBuilder<T> WithPositionalArgs<T>(
        this IResourceBuilder<T> builder, params string[] args)
        where T : IResourceWithArgs
    {
        return builder.WithArgs(context =>
        {
            if (!context.Args.OfType<string>().Any(arg => arg.AsSpan().Trim() == "--"))
            {
                context.Args.Add("--");
            }

            context.Args.AddRange(args.Cast<object>());
        });
    }

    /// <summary>
    /// References a resource and wait for its readiness.
    /// </summary>
    /// <typeparam name="T">The resource type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="dependency">The resource builder for the dependency to reference.</param>
    /// <param name="connectionName">
    /// An override of the dependency resource's name for the connection string.
    /// The resulting connection string will be "ConnectionStrings__connectionName"
    /// if this is not <see langword="null"/>.
    /// </param>
    /// <returns>The <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<T> WithReferencedDependency<T>(
        this IResourceBuilder<T> builder,
        IResourceBuilder<IResourceWithConnectionString> dependency,
        string? connectionName = null)
        where T : IResourceWithEnvironment, IResourceWithWaitSupport
    {
        return builder
            .WithReference(dependency, connectionName)
            .WaitFor(dependency);
    }
}
