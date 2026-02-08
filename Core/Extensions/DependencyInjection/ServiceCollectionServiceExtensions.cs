namespace RaptorUtils.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using MsExtensions = Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions;

/// <summary>
/// <inheritdoc cref="MsExtensions" />
/// </summary>
public static class ServiceCollectionServiceExtensions
{
    /// <summary>
    /// Adds a service of the type specified in <typeparamref name="TService"/> with an
    /// implementation type specified in <typeparamref name="TImplementation"/> to the
    /// specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the service.</param>
    /// <inheritdoc cref="MsExtensions.AddScoped{TService, TImplementation}(IServiceCollection)" />
    public static IServiceCollection Add<TService, TImplementation>(
        this IServiceCollection services,
        ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(services);

        var descriptor = new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime);
        services.Add(descriptor);

        return services;
    }
}
