namespace RaptorUtils.Threading.Tasks;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

/// <summary>
/// Represents a builder for asynchronous methods that returns a <see cref="TaskOrValue{TResult}"/>.
/// </summary>
/// <typeparam name="TResult">The type of the result.</typeparam>
[StructLayout(LayoutKind.Auto)]
public struct TaskOrValueMethodBuilder<TResult>
{
    private AsyncValueTaskMethodBuilder<TResult> builder;

    /// <summary>
    /// Gets the value task for this builder.
    /// </summary>
    public TaskOrValue<TResult> Task => new(this.builder.Task);

    /// <summary>
    /// Creates an instance of the <see cref="TaskOrValueMethodBuilder{TResult}"/> struct.
    /// </summary>
    /// <returns>The initialized instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TaskOrValueMethodBuilder<TResult> Create() => default;

    /// <summary>Begins running the builder with the associated state machine.</summary>
    /// <typeparam name="TStateMachine">The type of the state machine.</typeparam>
    /// <param name="stateMachine">The state machine instance, passed by reference.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Start<TStateMachine>(ref TStateMachine stateMachine)
        where TStateMachine : IAsyncStateMachine =>
        this.builder.Start(ref stateMachine);

    /// <summary>Associates the builder with the specified state machine.</summary>
    /// <param name="stateMachine">The state machine instance to associate with the builder.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetStateMachine(IAsyncStateMachine stateMachine)
        => this.builder.SetStateMachine(stateMachine);

    /// <summary>Marks the value task as successfully completed.</summary>
    /// <param name="result">The result to use to complete the value task.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetResult(TResult result)
        => this.builder.SetResult(result);

    /// <summary>
    /// Marks the value task as failed and binds the specified exception to the value task.
    /// </summary>
    /// <param name="exception">The exception to bind to the value task.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetException(Exception exception)
        => this.builder.SetException(exception);

    /// <summary>
    /// Schedules the state machine to proceed to the next action when the specified awaiter completes.
    /// </summary>
    /// <typeparam name="TAwaiter">The type of the awaiter.</typeparam>
    /// <typeparam name="TStateMachine">The type of the state machine.</typeparam>
    /// <param name="awaiter">the awaiter.</param>
    /// <param name="stateMachine">The state machine.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AwaitOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter,
        ref TStateMachine stateMachine)
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine
        => this.builder.AwaitOnCompleted(ref awaiter, ref stateMachine);

    /// <summary>
    /// Schedules the state machine to proceed to the next action when the specified awaiter completes.
    /// </summary>
    /// <typeparam name="TAwaiter">The type of the awaiter.</typeparam>
    /// <typeparam name="TStateMachine">The type of the state machine.</typeparam>
    /// <param name="awaiter">the awaiter.</param>
    /// <param name="stateMachine">The state machine.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter,
        ref TStateMachine stateMachine)
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine
        => this.builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
}
