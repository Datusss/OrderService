using Domain.Abstractions;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence.EF;

public class UnitOfWork(AppDbContext context, IMediator mediator) : IUnitOfWork
{
        private readonly AppDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private IDbContextTransaction? _currentTransaction;
        private bool _disposed;

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }
            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitAsync(AggregateRoot? aggregateRoot, CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No transaction is in progress.");
            }
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                await _currentTransaction.CommitAsync(cancellationToken);
                await mediator.PublishDomainEventsAsync(aggregateRoot, cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
                throw new Exception("Concurrency conflict detected. Please retry the operation.");
            }
            catch (Exception)
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
                throw;
            }
            finally
            {
                _currentTransaction?.Dispose();
                _currentTransaction = null;
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                return;
            }
            try
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                _currentTransaction?.Dispose();
                _currentTransaction = null;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _currentTransaction?.Dispose();
                _context.Dispose();
                _disposed = true;
            }
        }
    
}