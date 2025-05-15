using FileStoringService.Domain.Entities;

namespace FileStoringService.Domain.Interfaces.Scripts
{
    /// <summary>
    /// Сценарий удаления работы.
    /// </summary>
    public interface IRemoveWorkScript : IAsyncDisposable
    {
        /// <summary>
        /// Удаление работы.
        /// </summary>
        /// <param name="work">Работа.</param>
        Task RemoveAsync(WorkInfo work);

        /// <summary>
        /// Подтверждение удаления работы.
        /// </summary>
        Task ConfirmAsync();
    }
}
