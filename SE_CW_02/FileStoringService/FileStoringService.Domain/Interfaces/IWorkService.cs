using FileStoringService.Domain.Entities;
using FileStoringService.Domain.Models;

namespace FileStoringService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с работами.
    /// </summary>
    public interface IWorkService
    {
        /// <summary>
        /// Получить все работы.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<WorkInfo>> GetAllWorksAsync();

        /// <summary>
        /// Получить работу по id.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <returns>Работа по id или null.</returns>
        Task<WorkInfo?> GetWorkAsync(int workID);

        /// <summary>
        /// Загрузка работы на сервер.
        /// </summary>
        /// <param name="stream">Поток данных для загрузки.</param>
        /// <param name="data">Информация о работе.</param>
        /// <returns>id созданной работы.</returns>
        Task<int> UploadWorkAsync(Stream stream, UploadWorkData data);

        /// <summary>
        /// Скачивание работы.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <param name="work">Поток данных работы.</param>
        /// <returns>Информация о работе.</returns>
        Task<WorkInfo> DownloadWorkAsync(int workID, Stream work);

        /// <summary>
        /// Удаление работы.
        /// </summary>
        /// <param name="workID">id работы.</param>
        Task DeleteWorkAsync(int workID);
    }
}
