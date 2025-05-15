using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Domain.Interfaces;
using FileAnalysisService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FileAnalysisService.Infrastructure.Persistence
{
    /// <summary>
    /// Репозитория отчётов анализа работ.
    /// </summary>
    public class AnalyseReportRepository(ApplicationDbContext context) : IAnalyseReportRepository
    {
        private readonly ApplicationDbContext context = context ?? throw new ArgumentNullException(nameof(context));

        /// <summary>
        /// Получить отчёт анализа работы.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <returns>Отчёт анализа работы.</returns>
        public async Task<AnalyzeReport?> GetAsync(int workID)
        {
            return await context.AnalyzeReports.FindAsync(workID);
        }

        /// <summary>
        /// Получить отчёты анализа работ.
        /// </summary>
        /// <param name="worksID">id работы.</param>
        /// <returns>Отчёты анализа работ.</returns>
        public async Task<IEnumerable<AnalyzeReport>> GetAsync(params int[] worksID)
        {
            ArgumentNullException.ThrowIfNull(worksID, nameof(worksID));
            if (worksID.Length == 0) return [];

            return await context.AnalyzeReports.Where(x => worksID.Contains(x.WorkID)).ToListAsync();
        }

        /// <summary>
        /// Добавить отчёты анализа работ.
        /// </summary>
        /// <param name="reports">Отчёты анализа работ.</param>
        public async Task AddAsync(params AnalyzeReport[] reports)
        {
            ArgumentNullException.ThrowIfNull(reports, nameof(reports));

            // Коллекция отчётов для добавления.
            IEnumerable<AnalyzeReport> entities = reports.Where(x => x != null);

            if (!entities.Any()) return;

            // Добавление отчётов.
            await context.AnalyzeReports.AddRangeAsync(entities);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить отчёт анализа работы.
        /// </summary>
        /// <param name="workID">id работы.</param>
        public async Task DeleteAsync(int workID)
        {
            AnalyzeReport? report = await context.AnalyzeReports.FindAsync(workID);
            if (report == null) return;
            context.AnalyzeReports.Remove(report);
            await context.SaveChangesAsync();
        }
    }
}
