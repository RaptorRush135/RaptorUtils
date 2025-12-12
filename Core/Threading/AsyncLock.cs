namespace RaptorUtils.Threading;

/// <summary>
/// Provides an asynchronous lock mechanism that ensures only one task can access a resource at a time.
/// </summary>
public class AsyncLock
{
    private readonly SemaphoreSlim semaphore = new(initialCount: 1, maxCount: 1);

    /// <summary>
    /// Gets a value indicating whether the lock is currently held by any task.
    /// Returns true if the lock is engaged, otherwise false.
    /// </summary>
    public bool Locked => this.semaphore.CurrentCount == 0;

    /// <summary>
    /// Acquires the lock asynchronously.
    /// When the lock is acquired, the caller is guaranteed exclusive access to the shared resource.
    /// The returned <see cref="Scope"/> object should be disposed to release the lock.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result is a <see cref="Scope"/> object that releases the lock upon disposal.
    /// </returns>
    public async Task<Scope> LockAsync()
    {
        await this.semaphore.WaitAsync();
        return new Scope(this.semaphore);
    }

    /// <summary>
    /// Represents a disposable scope that releases the lock when disposed.
    /// </summary>
    /// <param name="semaphore">The <see cref="SemaphoreSlim"/> used to control access to the shared resource.</param>
    public struct Scope(SemaphoreSlim semaphore) : IDisposable
    {
        private SemaphoreSlim? semaphore = semaphore;

        /// <summary>
        /// Releases the lock.
        /// </summary>
        public void Dispose()
        {
            if (this.semaphore == null)
            {
                return;
            }

            this.semaphore.Release();
            this.semaphore = null;
        }
    }
}
