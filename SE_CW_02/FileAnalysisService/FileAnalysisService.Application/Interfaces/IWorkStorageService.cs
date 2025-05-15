namespace FileAnalysisService.Application.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для получения работ.
    /// </summary>
    public interface IWorkStorageService
    {
        /// <summary>
        /// Получение работы.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <returns>Поток данных работы или null.</returns>
        Task<Stream?> TryGetWorkAsync(int workID);

        /// <summary>
        /// Получение работы.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <returns>Поток данных работы.</returns>
        Task<Stream> GetWorkAsync(int workID);
    }
}
