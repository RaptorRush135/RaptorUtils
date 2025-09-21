namespace RaptorUtils.FluentValidation.Validators;

using global::FluentValidation;
using global::FluentValidation.Validators;

using RaptorUtils.Collections.Extensions;

/// <summary>
/// A FluentValidation property validator that ensures all elements in a collection are unique.
/// </summary>
/// <typeparam name="T">The type of the object being validated that contains the collection property.</typeparam>
/// <typeparam name="TElement">The type of elements contained within the collection being validated.</typeparam>
public class UniqueItemsValidator<T, TElement>
    : PropertyValidator<T, IEnumerable<TElement>>
{
    /// <summary>
    /// Gets the name of the validator.
    /// This is used as the default Error Code for the validator.
    /// </summary>
    public override string Name => nameof(UniqueItemsValidator<T, TElement>);

    /// <inheritdoc/>
    public override bool IsValid(ValidationContext<T> context, IEnumerable<TElement> collection)
        => !collection.ContainsDuplicates();

    /// <summary>
    /// Returns the default error message template for this validator.
    /// </summary>
    /// <inheritdoc/>
    protected override string GetDefaultMessageTemplate(string errorCode)
        => "'{PropertyName}' must contain only unique elements.";
}
