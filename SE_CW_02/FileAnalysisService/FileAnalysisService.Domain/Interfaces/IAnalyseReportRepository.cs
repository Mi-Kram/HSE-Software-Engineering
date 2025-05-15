using FileAnalysisService.Domain.Entities;

namespace FileAnalysisService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория отчётов анализа работ.
    /// </summary>
    public interface IAnalyseReportRepository
    {
        /// <summary>
        /// Получить отчёт анализа работы.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <returns>Отчёт анализа работы.</returns>
        Task<AnalyzeReport?> GetAsync(int workID);

        /// <summary>
        /// Получить отчёты анализа работ.
        /// </summary>
        /// <param name="worksID">id работы.</param>
        /// <returns>Отчёты анализа работ.</returns>
        Task<IEnumerable<AnalyzeReport>> GetAsync(params int[] worksID);

        /// <summary>
        /// Добавить отчёты анализа работ.
        /// </summary>
        /// <param name="reports">Отчёты анализа работ.</param>
        Task AddAsync(params AnalyzeReport[] reports);

        /// <summary>
        /// Удалить отчёт анализа работы.
        /// </summary>
        /// <param name="workID">id работы.</param>
        Task DeleteAsync(int workID);
    }
}
