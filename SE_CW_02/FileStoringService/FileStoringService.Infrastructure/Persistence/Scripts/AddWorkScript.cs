using FileStoringService.Domain.Entities;
using FileStoringService.Domain.Interfaces.Scripts;
using Microsoft.EntityFrameworkCore.Storage;

namespace FileStoringService.Infrastructure.Persistence.Scripts
{
    public class AddWorkScript(WorkDbContext context, CancellationToken cancellationToken = default) : IAddWorkScript
    {
        private readonly WorkDbContext context = context ?? throw new ArgumentNullException(nameof(context));
        private readonly CancellationToken cancellationToken = cancellationToken;
        private IDbContextTransaction? transaction = null;
        private bool confirmed = false;

        /// <summary>
        /// Добавление основы сущности.
        /// </summary>
        /// <param name="work">Сущность.</param>
        /// <returns>ID будущей сущности.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<int> AddBaseAsync(WorkInfo work)
        {
            // Проверка параметров и состяния.
            ArgumentNullException.ThrowIfNull(work, nameof(work));
            if (confirmed) throw new InvalidOperationException($"Транзакция уже завершена");
            if (transaction != null) throw new InvalidOperationException($"Метод {nameof(IAddWorkScript)}.{nameof(AddBaseAsync)} уже был вызван");
            
            // Инициализация транзакции.
            transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            work.Uploaded = work.Uploaded.ToUniversalTime();

            // Контроль длины названия работы.
            int max = WorkConfiguration.MAX_TITLE_LENGTH;
            if (work.Title.Length > max)
            {
                int dotIndex = work.Title.LastIndexOf('.');
                if (dotIndex == -1) work.Title = work.Title[..max];
                else work.Title = $"{work.Title[..(max - (work.Title.Length - dotIndex))]}{work.Title[dotIndex..]}";
            }

            // Добавление сущности.
            await context.Works.AddAsync(work, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            // Возвращение id.
            return work.ID;
        }

        /// <summary>
        /// Подтверждение сохранения.
        /// </summary>
        public Task ConfirmAsync()
        {
            // Проверка параметров и состояния.
            if (confirmed) throw new InvalidOperationException($"Транзакция уже завершена");
            if (transaction == null) throw new InvalidOperationException($"Метод {nameof(IAddWorkScript)}.{nameof(AddBaseAsync)} ещё не был вызван");

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
