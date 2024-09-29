namespace RaptorUtils.Aspire.Hosting;

using global::Aspire.Hosting;
using global::Aspire.Hosting.ApplicationModel;

using RaptorUtils.Collections.Extensions;

public static class ResourceBuilderExtensions
{
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
