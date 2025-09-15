namespace RaptorUtils.Common;

/// <summary>
/// Represents an object that has a unique identifier of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">
/// The type of the identifier.
/// </typeparam>
public interface IIdentifiable<out T>
{
    /// <summary>
    /// Gets the unique identifier of this object.
    /// </summary>
    T Id { get; }
}
