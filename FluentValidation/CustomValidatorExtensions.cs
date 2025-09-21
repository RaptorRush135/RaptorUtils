namespace RaptorUtils.FluentValidation;

using global::FluentValidation;

using RaptorUtils.FluentValidation.Validators;

/// <summary>
/// Provides extension methods for FluentValidation rule builders.
/// </summary>
public static class CustomValidatorExtensions
{
    /// <summary>
    /// This extension applies the UniqueItemsValidator to ensure no duplicate items exist in the collection.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated that contains the collection property.</typeparam>
    /// <typeparam name="TElement">The type of elements contained within the collection being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <returns>
    /// An IRuleBuilderOptions instance that allows for further configuration of the validation rule.
    /// </returns>
    public static IRuleBuilderOptions<T, IEnumerable<TElement>> MustHaveUniqueItems<T, TElement>(
        this IRuleBuilder<T, IEnumerable<TElement>> ruleBuilder)
    {
        return ruleBuilder.SetValidator(new UniqueItemsValidator<T, TElement>());
    }
}
