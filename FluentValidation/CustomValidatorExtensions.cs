namespace RaptorUtils.FluentValidation;

using global::FluentValidation;

using RaptorUtils.FluentValidation.Validators;

/// <summary>
/// Extension methods that provide a custom set of validators.
/// </summary>
public static class CustomValidatorExtensions
{
    /// <summary>
    /// Defines the maximum valid length for an email address according to RFC standards.
    /// </summary>
    public const int EmailMaxValidLength = 254;

    /// <summary>
    /// Applies email address validation along with a maximum length check.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <returns>
    /// An IRuleBuilderOptions instance that allows for further configuration of the validation rule.
    /// </returns>
    public static IRuleBuilderOptions<T, string> EmailAddressWithMaxLength<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.EmailAddress()
            .MaximumLength(EmailMaxValidLength);
    }

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
