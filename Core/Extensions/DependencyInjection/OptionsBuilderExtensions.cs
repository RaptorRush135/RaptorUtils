namespace RaptorUtils.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

/// <summary>
/// Provides extension methods for configuring and validating options.
/// </summary>
public static class OptionsBuilderExtensions
{
    /// <summary>
    /// Registers a validation type for the specified options.
    /// The validation type must implement <see cref="IValidateOptions{TOptions}"/>.
    /// </summary>
    /// <typeparam name="TOptions">
    /// The type of the options being configured.
    /// </typeparam>
    /// <typeparam name="TValidate">
    /// The type that implements <see cref="IValidateOptions{TOptions}"/> and performs the validation.
    /// </typeparam>
    /// <param name="optionsBuilder">
    /// The <see cref="OptionsBuilder{TOptions}"/> used to configure options.
    /// </param>
    /// <returns>
    /// The original <see cref="OptionsBuilder{TOptions}"/>, allowing for method chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="optionsBuilder"/> is null.
    /// </exception>
    public static OptionsBuilder<TOptions> Validate<TOptions, TValidate>(this OptionsBuilder<TOptions> optionsBuilder)
        where TOptions : class
        where TValidate : IValidateOptions<TOptions>, new()
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);

        optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(_ => new TValidate());
        return optionsBuilder;
    }
}
