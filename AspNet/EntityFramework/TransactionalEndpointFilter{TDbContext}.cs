namespace RaptorUtils.AspNet.EntityFramework;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

/// <summary>
/// An endpoint filter that wraps the execution of an endpoint in a database transaction.
/// Supports retrying operations using the DbContext's execution strategy and
/// resets the DbContext state on retries.
/// </summary>
/// <typeparam name="TDbContext">
/// The type of <see cref="DbContext"/> used.
/// </typeparam>
/// <param name="dbContext">
/// The <typeparamref name="TDbContext"/> instance used to manage transactions and database operations.
/// </param>
public abstract class TransactionalEndpointFilter<TDbContext>(
    TDbContext dbContext)
    : IEndpointFilter
    where TDbContext : DbContext, IDbContextPoolable
{
    /// <summary>
    /// Invokes the endpoint filter, executing the provided <paramref name="next"/> delegate
    /// inside a database transaction. If retries occur due to the execution strategy,
    /// the DbContext state is reset before retrying.
    /// </summary>
    /// <inheritdoc />
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        int attempts = 0;

        return await strategy.ExecuteAsync(async () =>
        {
            if (attempts++ > 0)
            {
                await dbContext.ResetStateAsync();
            }

            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            var result = await next(context);

            if (this.ShouldCommit())
            {
                await transaction.CommitAsync();
            }

            return result;
        });
    }

    /// <summary>
    /// Determines whether the current transaction should be committed.
    /// Derived classes can implement custom logic to decide whether to commit or roll back.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the transaction should be committed; otherwise, <see langword="false"/>.
    /// </returns>
    protected abstract bool ShouldCommit();
}
