namespace FileAnalysisService.Application.Interfaces
{
    /// <summary>
    /// Интерфейс для генерации облака слов.
    /// </summary>
    public interface IWordsCloudService
    {
        /// <summary>
        /// Генерация облака слов.
        /// </summary>
        /// <param name="data">Данные для обработки.</param>
        /// <param name="image">Картинка облака слов.</param>
        /// <returns>Content-Type.</returns>
        Task<string> GenerateAsync(Stream data, Stream image);
    }
}
