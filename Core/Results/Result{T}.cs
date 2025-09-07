namespace RaptorUtils.Results;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents the result of an operation that can succeed with a value of type <typeparamref name="T"/>
/// or fail with an error message.
/// </summary>
/// <typeparam name="T">The type of the successful result value.</typeparam>
public sealed class Result<T>
{
    private Result()
    {
    }

    /// <summary>
    /// Gets the value of a successful operation.
    /// Will be <see langword="null"/> if <see cref="Success"/> is <see langword="false"/>.
    /// </summary>
    public T? Value { get; private init; }

    /// <summary>
    /// Gets the error message of a failed operation.
    /// Will be <see langword="null"/> if <see cref="Success"/> is <see langword="true"/>.
    /// </summary>
    public string? Error { get; private init; }

    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; private init; }

    public static implicit operator Result<T>(T value) => new()
    {
        Success = true,
        Value = value,
    };

    /// <summary>
    /// Creates a failed <see cref="Result{T}"/> with the specified error message.
    /// </summary>
    /// <param name="error">The error message describing the failure.</param>
    /// <returns>A failed <see cref="Result{T}"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown if <paramref name="error"/> is <see langword="null"/>.</exception>
    public static Result<T> Fail(string error) => new()
    {
        Error = error ?? throw new InvalidOperationException("Error message cannot be null."),
    };
}
