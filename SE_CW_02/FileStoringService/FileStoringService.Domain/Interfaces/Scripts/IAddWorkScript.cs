using FileStoringService.Domain.Entities;

namespace FileStoringService.Domain.Interfaces.Scripts
{
    /// <summary>
    /// Сценарий добавления работы.
    /// </summary>
    public interface IAddWorkScript : IAsyncDisposable
    {
        /// <summary>
        /// Добавление основы сущности.
        /// </summary>
        /// <param name="work">Сущность.</param>
        /// <returns>ID будущей сущности.</returns>
        Task<int> AddBaseAsync(WorkInfo work);

        /// <summary>
        /// Подтверждение сохранения.
        /// </summary>
        Task ConfirmAsync();
    }
}
