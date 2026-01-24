namespace RaptorUtils.AspNet.Http.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using MsExtensions = Microsoft.AspNetCore.Http.EndpointFilterExtensions;

/// <summary>
/// <inheritdoc cref="MsExtensions" />
/// </summary>
public static class EndpointFilterExtensions
{
    /// <inheritdoc cref="MsExtensions.AddEndpointFilter{TFilterType}(RouteGroupBuilder)" />
    /// <param name="builder">The <see cref="RouteHandlerBuilder"/>.</param>
    /// <param name="condition">
    /// A value indicating whether the endpoint filter should be added.
    /// If <see langword="true"/>, the filter is added;
    /// otherwise, the builder is returned unchanged.
    /// </param>
    public static RouteGroupBuilder AddEndpointFilterIf<TFilterType>(
        this RouteGroupBuilder builder,
        bool condition)
        where TFilterType : IEndpointFilter
    {
        return condition
            ? builder.AddEndpointFilter<TFilterType>()
            : builder;
    }
}
