namespace FileAnalysisService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для хранения объектов анализа.
    /// </summary>
    public interface IAnalyseStorageService
    {
        /// <summary>
        /// Получение картинки облака слов.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <param name="image">Поток изображения.</param>
        /// <returns>Content-Type.</returns>
        Task<string> GetWordsCloudImageAsync(int workID, Stream image);

        /// <summary>
        /// Сохранение картинки облака слов.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <param name="image">Поток изображения.</param>
        /// <param name="contentType">Тип изображения.</param>
        Task SaveWordsCloudImageAsync(int workID, Stream image, string contentType);

        /// <summary>
        /// Удаление картинки облака слов.
        /// </summary>
        /// <param name="workID">id работы.</param>
        Task DeleteWordsCloudImageAsync(int workID);
    }
}
