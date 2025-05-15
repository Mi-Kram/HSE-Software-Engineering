using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Domain.Models;

namespace FileAnalysisService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс сравнения работ.
    /// </summary>
    public interface IComparisonService
    {
        /// <summary>
        /// Получить отчёт сравнения работ.
        /// </summary>
        /// <param name="keys">id работ для сравнения.</param>
        /// <returns>Коллекция отчётов сравнений.</returns>
        Task<IEnumerable<ComparisonReport>> GetReportsAsync(IEnumerable<ComparisonKey> keys);

        /// <summary>
        /// Получить отчёт сравнения всех работ друг с другом.
        /// </summary>
        /// <param name="worksID">id работ для сравнения.</param>
        /// <returns>Коллекция отчётов сравнений.</returns>
        Task<IEnumerable<ComparisonReport>> GetReportsAsync(IEnumerable<int> worksID);
    }
}
