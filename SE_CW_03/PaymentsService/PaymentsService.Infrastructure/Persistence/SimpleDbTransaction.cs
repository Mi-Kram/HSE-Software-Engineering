using Microsoft.EntityFrameworkCore.Storage;
using PaymentsService.Domain.Interfaces;

namespace PaymentsService.Infrastructure.Persistence
{
    public class SimpleDbTransaction(AppDbContext context) : IDbTransaction, IDisposable
    {
        private readonly AppDbContext context = context ?? throw new ArgumentNullException(nameof(context));

        private IDbContextTransaction? transaction = null;

        public async Task BeginTransactionAsync()
        {
            if (transaction != null) throw new InvalidOperationException("Транзакция уже запущена");
            transaction = await context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (transaction == null) throw new InvalidOperationException("Транзакция не запущена");

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
            await transaction.DisposeAsync();
            transaction = null;
        }

        public async Task RollbackTransactionAsync()
        {
            if (transaction == null) throw new InvalidOperationException("Транзакция не запущена");

            await transaction.RollbackAsync();
            await transaction.DisposeAsync();
            transaction = null;
        }

        public void Dispose()
        {
            transaction?.Dispose();
        }
    }
}
