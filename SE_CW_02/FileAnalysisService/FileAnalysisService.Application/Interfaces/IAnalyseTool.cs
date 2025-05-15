using FileAnalysisService.Domain.Entities;

namespace FileAnalysisService.Application.Interfaces
{
    /// <summary>
    /// Интерфейс инструмента для анализа работы.
    /// </summary>
    public interface IAnalyseTool
    {
        /// <summary>
        /// Проведение анализа работы.
        /// </summary>
        /// <param name="work">Поток данных работы.</param>
        /// <returns>Отчёт анализа работы.</returns>
        Task<AnalyzeReport?> AnalyzeAsync(Stream work);
    }
}
