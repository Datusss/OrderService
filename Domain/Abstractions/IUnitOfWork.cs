namespace Domain.Abstractions;

public interface IUnitOfWork : IDisposable
{
    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitAsync(AggregateRoot? aggregateRoot, CancellationToken cancellationToken);
    Task RollbackAsync(CancellationToken cancellationToken);
}