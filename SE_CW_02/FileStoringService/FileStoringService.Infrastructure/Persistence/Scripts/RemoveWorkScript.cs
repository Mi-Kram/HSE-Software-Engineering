using FileStoringService.Domain.Entities;
using FileStoringService.Domain.Interfaces.Scripts;
using Microsoft.EntityFrameworkCore.Storage;

namespace FileStoringService.Infrastructure.Persistence.Scripts
{
    /// <summary>
    /// Сценарий удаления работы.
    /// </summary>
    public class RemoveWorkScript(WorkDbContext context, CancellationToken cancellationToken = default) : IRemoveWorkScript
    {
        private readonly WorkDbContext context = context ?? throw new ArgumentNullException(nameof(context));
        private readonly CancellationToken cancellationToken = cancellationToken;
        private IDbContextTransaction? transaction = null;
        private bool confirmed = false;


        /// <summary>
        /// Удаление работы.
        /// </summary>
        /// <param name="work">Работа.</param>
        public async Task RemoveAsync(WorkInfo work)
        {
            // Проверка параметров и состояния.
            ArgumentNullException.ThrowIfNull(work, nameof(work));
            if (confirmed) throw new InvalidOperationException($"Транзакция уже завершена");
            if (transaction != null) throw new InvalidOperationException($"Метод {nameof(IRemoveWorkScript)}.{nameof(RemoveAsync)} уже был вызван");

            // Начало транзакции, удаления.
            transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            context.Works.Remove(work);
            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Подтверждение удаления работы.
        /// </summary>
        public Task ConfirmAsync()
        {
            // Проверка параметров и состояния.
            if (confirmed) throw new InvalidOperationException($"Транзакция уже завершена");
            if (transaction == null) throw new InvalidOperationException($"Метод {nameof(IRemoveWorkScript)}.{nameof(RemoveAsync)} ещё не был вызван");
            
            // Зафиксировать подтверждение.
            confirmed = true;
            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            if (transaction == null) return;

            if (confirmed) await transaction.CommitAsync(cancellationToken);
            else await transaction.RollbackAsync(cancellationToken);

            await transaction.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}
