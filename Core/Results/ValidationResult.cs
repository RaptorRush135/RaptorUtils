namespace RaptorUtils.Results;

using RaptorUtils.CodeAnalysis;

/// <summary>
/// Represents the result of a validation operation, including information
/// about whether the operation succeeded and any associated validation errors.
/// </summary>
public sealed class ValidationResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationResult"/> class with the specified errors.
    /// </summary>
    /// <param name="errors">
    /// A collection of <see cref="ValidationFailure"/> instances that represent the validation errors.
    /// Null values are ignored.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="errors"/> parameter is null.</exception>
    public ValidationResult(params IEnumerable<ValidationFailure?> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        this.Errors = [.. errors.OfType<ValidationFailure>()];
    }

    /// <summary>
    /// Gets a pre-defined <see cref="ValidationResult"/> instance that represents a successful validation.
    /// </summary>
    public static ValidationResult Success { get; } = new();

    /// <summary>
    /// Gets a value indicating whether the validation operation was successful.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if there are no validation errors; otherwise, <see langword="false"/>.
    /// </value>
    public bool Succeeded => this.Errors.Count == 0;

    /// <summary>
    /// Gets the collection of validation errors associated with this result.
    /// </summary>
    /// <value>A read-only collection of <see cref="ValidationFailure"/> instances.</value>
    public IReadOnlyCollection<ValidationFailure> Errors { get; }

    /// <summary>
    /// Creates a new <see cref="ValidationResult"/> instance that represents a failed validation result.
    /// </summary>
    /// <param name="errors">
    /// A collection of <see cref="ValidationFailure"/> instances that represent the validation errors.
    /// Null values are ignored.
    /// </param>
    /// <returns>
    /// A <see cref="ValidationResult"/> instance containing the provided validation errors.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the provided errors do not result in a failed state (i.e., no valid errors are provided).
    /// </exception>
    [MustUseReturnValue]
    public static ValidationResult Failed(
        params IEnumerable<ValidationFailure?> errors)
    {
        var result = new ValidationResult(errors);
        if (result.Succeeded)
        {
            throw new InvalidOperationException(
                "ValidationResult creation failed: The provided errors did not result in a failed state.");
        }

        return result;
    }

    /// <summary>
    /// Returns a string representation of the <see cref="ValidationResult"/>,
    /// with error messages separated by a newline character.
    /// </summary>
    /// <returns>A string containing all validation error messages, separated by newlines.</returns>
    public override string ToString()
    {
        return this.ToString(Environment.NewLine);
    }

    /// <summary>
    /// Returns a string representation of the <see cref="ValidationResult"/>,
    /// with error messages separated by the specified string.
    /// </summary>
    /// <param name="separator">
    /// The string to use as a separator between error messages.
    /// </param>
    /// <returns>
    /// A string containing all validation error messages, separated by the specified <paramref name="separator"/>.
    /// </returns>
    public string ToString(string separator)
    {
        return string.Join(
            separator,
            this.Errors.Select(failure => failure.ErrorMessage));
    }

    /// <summary>
    /// Converts the validation errors into a dictionary, where the keys are property names
    /// and the values are arrays of error messages associated with each property.
    /// </summary>
    /// <returns>
    /// A dictionary where the keys are property names, and the values are arrays of error messages.
    /// </returns>
    [MustUseReturnValue]
    public Dictionary<string, string[]> ToDictionary()
    {
        return this.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray());
    }
}
