using FileAnalysisService.Domain.Entities;

namespace FileAnalysisService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс анализа работ.
    /// </summary>
    public interface IAnalyseService
    {
        /// <summary>
        /// Получить отчёт анализа работы.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <returns>Отчёт анализа работы.</returns>
        Task<AnalyzeReport?> GetReportAsync(int workID);

        /// <summary>
        /// Получить облако слов.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <param name="cloud">Картинка облака слов.</param>
        /// <returns>Content-Type.</returns>
        Task<string> GetWordsCloudAsync(int workID, Stream cloud, bool regenerate);
    }
}
