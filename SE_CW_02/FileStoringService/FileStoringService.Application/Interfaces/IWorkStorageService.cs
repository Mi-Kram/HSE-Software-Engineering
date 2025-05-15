namespace FileStoringService.Application.Interfaces
{
    /// <summary>
    /// Интерфейс хранилища работ.
    /// </summary>
    public interface IWorkStorageService
    {
        /// <summary>
        /// Сохранение работы.
        /// </summary>
        /// <param name="stream">Поток данных.</param>
        /// <param name="workID">id работы.</param>
        Task SaveAsync(Stream stream, int workID);

        /// <summary>
        /// Получить работу.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <param name="work">Поток данных работы.</param>
        Task GetAsync(int workID, Stream work);

        /// <summary>
        /// Удалить работу.
        /// </summary>
        /// <param name="workID">id работы.</param>
        Task DeleteAsync(int workID);
    }
}
