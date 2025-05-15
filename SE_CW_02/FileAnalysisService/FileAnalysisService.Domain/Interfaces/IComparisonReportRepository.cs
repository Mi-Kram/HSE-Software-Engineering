using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Domain.Models;

namespace FileAnalysisService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория отчётов сравнения работ.
    /// </summary>
    public interface IComparisonReportRepository
    {
        /// <summary>
        /// Получить отчёт сравнения работ.
        /// </summary>
        /// <param name="key">id работ.</param>
        /// <returns>Отчёт сравнения работы.</returns>
        Task<ComparisonReport?> GetAsync(ComparisonKey key);

        /// <summary>
        /// Получить отчёты сравнения работ.
        /// </summary>
        /// <param name="worksID">id работ.</param>
        /// <returns>Отчёты сравнения работ.</returns>
        Task<IEnumerable<ComparisonReport>> GetAsync(params ComparisonKey[] worksID);

        /// <summary>
        /// Получить отчёт сравнения работы со всеми остальными.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <returns>Отчёт сравнения работ.</returns>
        Task<IEnumerable<ComparisonReport>> GetAsync(int workID);

        /// <summary>
        /// Добавить отчёты сравнения работ.
        /// </summary>
        /// <param name="reports">Отчёты сравнения работ.</param>
        Task AddAsync(params ComparisonReport[] reports);

        /// <summary>
        /// Удалить отчёты сравнения работ, связанные с определённой работой.
        /// </summary>
        /// <param name="workID">id работы.</param>
        Task DeleteAsync(int workID);
    }
}
