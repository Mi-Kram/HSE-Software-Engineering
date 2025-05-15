using FileStoringService.Domain.Entities;
using FileStoringService.Domain.Interfaces.Scripts;

namespace FileStoringService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория работ.
    /// </summary>
    public interface IWorkRepository
    {
        /// <summary>
        /// Получить коллекцию всех работ.
        /// </summary>
        /// <returns>Коллекция всех работ.</returns>
        Task<IEnumerable<WorkInfo>> GetAllAsync();

        /// <summary>
        /// Получить коллекцию всех работ одного владельца.
        /// </summary>
        /// <param name="userID">id владельца.</param>
        /// <returns>Коллекция всех работ одного владельца.</returns>
        Task<IEnumerable<WorkInfo>> GetAllByUserIDAsync(int userID);

        /// <summary>
        /// Получить коллекцию всех работ одного владельца.
        /// </summary>
        /// <param name="hash">хэш работы.</param>
        /// <returns>Коллекция всех работ одного владельца.</returns>
        Task<IEnumerable<WorkInfo>> GetAllByHashAsync(string hash);

        /// <summary>
        /// Получить работу по id.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <returns>Работа с указанным id. Null, если работа не найдена.</returns>
        Task<WorkInfo?> GetAsync(int workID);

        /// <summary>
        /// Получение сценария на добавление работы.
        /// </summary>
        /// <returns>Сценарий на добавление работы.</returns>
        Task<IAddWorkScript> GetAddScriptAsync();

        /// <summary>
        /// Получение сценария на удаление работы.
        /// </summary>
        /// <returns>Сценарий на удаление работы.</returns>
        Task<IRemoveWorkScript> GetRemoveScriptAsync();
    }
}
