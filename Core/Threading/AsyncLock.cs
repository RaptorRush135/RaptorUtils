namespace RaptorUtils.Threading;

public class AsyncLock
{
    private readonly SemaphoreSlim semaphore = new(initialCount: 1, maxCount: 1);

    public bool Locked => this.semaphore.CurrentCount == 0;

    public async Task<Scope> LockAsync()
    {
        await this.semaphore.WaitAsync();
        return new Scope(this.semaphore);
    }

    public struct Scope(SemaphoreSlim semaphore) : IDisposable
    {
        private SemaphoreSlim? semaphore = semaphore;

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
