namespace RaptorUtils.Results;

/// <summary>
/// Represents a validation failure that includes the name of the property
/// and the corresponding error message.
/// </summary>
/// <param name="PropertyName">The name of the property that failed validation.</param>
/// <param name="ErrorMessage">The error message describing the validation failure.</param>
public sealed record ValidationFailure(
    string PropertyName,
    string ErrorMessage)
{
    /// <summary>
    /// Allows implicit conversion from a tuple containing a property name
    /// and an error message to a <see cref="ValidationFailure"/> instance.
    /// </summary>
    /// <param name="tuple">
    /// A tuple containing the property name and the error message.
    /// The first item represents the property name,
    /// and the second item represents the error message.
    /// </param>
    public static implicit operator ValidationFailure(
        (string PropertyName, string ErrorMessage) tuple) => new(tuple.PropertyName, tuple.ErrorMessage);
}
