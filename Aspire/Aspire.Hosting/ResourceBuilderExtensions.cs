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
    /// <typeparam name="T">
    /// The type of the resource being built, which must implement <see cref="IResourceWithArgs"/>.
    /// </typeparam>
    /// <param name="builder">The resource builder to extend.</param>
    /// <param name="args">The positional arguments to add to the resource builder.</param>
    /// <returns>
    /// An updated instance of <see cref="IResourceBuilder{T}"/> with the added positional arguments.
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
}
