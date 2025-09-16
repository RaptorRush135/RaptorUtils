namespace RaptorUtils.Threading.Tasks;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

/// <summary>
/// Represents a value that may be available synchronously or asynchronously as a <see cref="ValueTask{TResult}"/>,
/// and can be implicitly created from a <typeparamref name="TResult"/> value or a <see cref="ValueTask{TResult}"/>.
/// </summary>
/// <typeparam name="TResult">The type of the result.</typeparam>
[AsyncMethodBuilder(typeof(TaskOrValueMethodBuilder<>))]
[method: MethodImpl(MethodImplOptions.AggressiveInlining)]
public readonly struct TaskOrValue<TResult>(
    ValueTask<TResult> task)
    : IEquatable<TaskOrValue<TResult>>
{
    /// <summary>
    /// Gets the underlying <see cref="ValueTask{TResult}"/>.
    /// </summary>
    public ValueTask<TResult> UnderlyingTask { get; } = task;

    public static implicit operator TaskOrValue<TResult>(TResult value)
        => new(ValueTask.FromResult(value));

    public static implicit operator TaskOrValue<TResult>(ValueTask<TResult> task)
        => new(task);

    public static bool operator ==(TaskOrValue<TResult> left, TaskOrValue<TResult> right)
        => left.UnderlyingTask == right.UnderlyingTask;

    public static bool operator !=(TaskOrValue<TResult> left, TaskOrValue<TResult> right)
        => left.UnderlyingTask != right.UnderlyingTask;

    /// <summary>
    /// Gets an awaiter for this <see cref="TaskOrValue{TResult}"/>.
    /// </summary>
    /// <returns>The awaiter.</returns>
    public ValueTaskAwaiter<TResult> GetAwaiter() => this.UnderlyingTask.GetAwaiter();

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>The hash code.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => this.UnderlyingTask.GetHashCode();

    /// <summary>
    /// Determines whether this instance and a specified object are equal.
    /// </summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="obj"/> is a <see cref="TaskOrValue{TResult}"/> and equal;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals([NotNullWhen(true)] object? obj) => this.UnderlyingTask.Equals(obj);

    /// <summary>
    /// Determines whether this instance and a specified <see cref="TaskOrValue{TResult}"/> are equal.
    /// </summary>
    /// <param name="other">The other <see cref="TaskOrValue{TResult}"/> to compare.</param>
    /// <returns><see langword="true"/> if the two are equal; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(TaskOrValue<TResult> other) => this.UnderlyingTask.Equals(other.UnderlyingTask);

    /// <summary>
    /// Gets a <see cref="Task{TResult}"/> object to represent this ValueTask.
    /// </summary>
    /// <returns>
    /// It will either return the wrapped task object if one exists, or it'll
    /// manufacture a new task object to represent the result.
    /// </returns>
    public Task<TResult> AsTask() => this.UnderlyingTask.AsTask();

    /// <summary>
    /// Gets a <see cref="ValueTask{TResult}"/> that may be used at any point in the future.
    /// </summary>
    /// <returns>The <see cref="ValueTask{TResult}"/>.</returns>
    public ValueTask<TResult> Preserve() => this.UnderlyingTask.Preserve();
}
