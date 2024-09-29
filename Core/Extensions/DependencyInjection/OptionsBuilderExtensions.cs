namespace RaptorUtils.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public static class OptionsBuilderExtensions
{
    public static OptionsBuilder<TOptions> Validate<TOptions, TValidate>(this OptionsBuilder<TOptions> optionsBuilder)
        where TOptions : class
        where TValidate : IValidateOptions<TOptions>, new()
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);

        optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(_ => new TValidate());
        return optionsBuilder;
    }
}
