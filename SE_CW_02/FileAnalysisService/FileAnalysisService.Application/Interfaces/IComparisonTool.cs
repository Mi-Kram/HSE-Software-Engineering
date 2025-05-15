using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Domain.Models;

namespace FileAnalysisService.Application.Interfaces
{
    /// <summary>
    /// Интерфейс инструмента для сравнения работы.
    /// </summary>
    public interface IComparisonTool
    {
        /// <summary>
        /// Проведение сравнения работы.
        /// </summary>
        /// <param name="works">Поток данных работ {id: работа}.</param>
        /// <returns>Отчёт анализа работы.</returns>
        Task<IEnumerable<ComparisonReport>> CompareAsync(Dictionary<int, Stream> works);

        /// <summary>
        /// Проведение сравнения работы.
        /// </summary>
        /// <param name="works">Поток данных работ {id: работа}.</param>
        /// <param name="comparisons">id работы, которые надо сравнить.</param>
        /// <returns>Отчёт анализа работы.</returns>
        Task<IEnumerable<ComparisonReport>> CompareAsync(Dictionary<int, Stream> works, IEnumerable<ComparisonKey> comparisons);
    }
}
