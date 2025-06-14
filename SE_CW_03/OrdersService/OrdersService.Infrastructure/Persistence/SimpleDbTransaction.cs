using Microsoft.EntityFrameworkCore.Storage;
using OrdersService.Domain.Interfaces;

namespace OrdersService.Infrastructure.Persistence
{
    public class SimpleDbTransaction(OrdersDbContext context) : IDbTransaction, IDisposable
    {
        private readonly OrdersDbContext context = context ?? throw new ArgumentNullException(nameof(context));

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
