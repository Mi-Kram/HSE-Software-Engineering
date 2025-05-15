using FileStoringService.Application.Interfaces;
using FileStoringService.Domain.Entities;
using FileStoringService.Domain.Exceptions;
using FileStoringService.Domain.Interfaces;
using FileStoringService.Domain.Interfaces.Scripts;
using FileStoringService.Domain.Models;

namespace FileStoringService.Application.UseCases
{
    /// <summary>
    /// Сервис для работы с работами.
    /// </summary>
    public class WorkService(
        IWorkRepository workRepository, 
        IStreamHasherService streamHasher,
        IWorkStorageService workStorage) : IWorkService
    {
        private readonly IWorkRepository workRepository = workRepository ?? throw new ArgumentNullException(nameof(workRepository));
        private readonly IStreamHasherService streamHasher = streamHasher ?? throw new ArgumentNullException(nameof(streamHasher));
        private readonly IWorkStorageService workStorage = workStorage ?? throw new ArgumentNullException(nameof(streamHasher));

        /// <summary>
        /// Получить все работы.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<WorkInfo>> GetAllWorksAsync()
        {
            return await workRepository.GetAllAsync();
        }

        /// <summary>
        /// Получить работу по id.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <returns>Работа по id или null.</returns>
        public async Task<WorkInfo?> GetWorkAsync(int workID)
        {
            return await workRepository.GetAsync(workID);
        }

        /// <summary>
        /// Скачивание работы.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <param name="work">Поток данных работы.</param>
        /// <returns>Информация о работе.</returns>
        public async Task<WorkInfo> DownloadWorkAsync(int workID, Stream work)
        {
            ArgumentNullException.ThrowIfNull(work, nameof(work));

            // Проверка существования работы по id.
            WorkInfo? workInfo = await workRepository.GetAsync(workID);
            if (workInfo == null) throw new ArgumentException("Работа не найдена");

            // Возвращение потока данных работы.
            await workStorage.GetAsync(workID, work);
            return workInfo;
        }

        /// <summary>
        /// Проверка на уникальность среди ранее загруженных работ у владельца.
        /// </summary>
        /// <param name="stream">Поток данных.</param>
        /// <param name="userID">id владельца работы.</param>
        /// <param name="hash">Хэш работы.</param>
        /// <returns>id работы, если работа была уже загружена владельцом, иначе null.</returns>
        private async Task<int?> HasAlreadyUploadedAsync(Stream stream, int userID, string hash)
        {
            // Предыдущие работы с одинаковым хэшем.
            IEnumerable<WorkInfo> oldWorks = (await workRepository.GetAllByHashAsync(hash))
                .Where(x => x.UserID == userID);

            if (!oldWorks.Any()) return null;

            using StreamReader reader = new(stream, leaveOpen: true);
            string newWork = await reader.ReadToEndAsync(), oldWork = string.Empty;
            stream.Seek(0, SeekOrigin.Begin);

            foreach (WorkInfo work in oldWorks)
            {
                // Чтение предыдущей работы и сравнение содержимого.
                using MemoryStream ms = new();
                await workStorage.GetAsync(work.ID, ms);
                using StreamReader sr = new(ms);
                oldWork = await sr.ReadToEndAsync();

                // Сравнение работ.
                if (newWork == oldWork) return work.ID;
            }

            return null;
        }

        /// <summary>
        /// Загрузка работы на сервер.
        /// </summary>
        /// <param name="stream">Поток данных для загрузки.</param>
        /// <param name="data">Информация о работе.</param>
        /// <returns>id созданной работы.</returns>
        public async Task<int> UploadWorkAsync(Stream stream, UploadWorkData data)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));
            if (stream.Length == 0) throw new ArgumentException("Пустой поток данных");

            // Хэширование файла.
            string? hash = await streamHasher.HashAsync(stream) ?? throw new Exception("Не удалось обработать работу");
            stream.Seek(0, SeekOrigin.Begin);

            // Проверка на уникальность среди ранее загруженных работ.
            int? workID = await HasAlreadyUploadedAsync(stream, data.UserID, hash);
            if (workID != null) throw new WorkAlreadyUploadedException(workID.Value);

            // Инициализация сущности.
            WorkInfo work = new()
            {
                UserID = data.UserID,
                Title = data.Title,
                Hash = hash,
                Uploaded = DateTime.Now
            };

            // Сценарий добавления работы.
            await using IAddWorkScript script = await workRepository.GetAddScriptAsync();
            
            // Добавление информации о работе в репозиторий.
            work.ID = await script.AddBaseAsync(work);

            // Сохранение работы.
            await workStorage.SaveAsync(stream, work.ID);

            await script.ConfirmAsync();
            return work.ID;
        }

        /// <summary>
        /// Удаление работы.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <returns>True, если работы удалена, иначе False.</returns>
        public async Task DeleteWorkAsync(int workID)
        {
            WorkInfo? work = await workRepository.GetAsync(workID) ?? throw new ArgumentException("Работа не найдена");

            // Сценарий удаления работы.
            await using IRemoveWorkScript script = await workRepository.GetRemoveScriptAsync();

            await script.RemoveAsync(work);
            await workStorage.DeleteAsync(work.ID);
            await script.ConfirmAsync();
        }
    }
}
