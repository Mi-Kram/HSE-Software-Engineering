using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Domain.Interfaces;
using FileAnalysisService.Domain.Models;
using FileAnalysisService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FileAnalysisService.Infrastructure.Persistence
{
    /// <summary>
    /// Репозитория отчётов сравнения работ.
    /// </summary>
    public class ComparisonReportRepository(ApplicationDbContext context) : IComparisonReportRepository
    {
        private readonly ApplicationDbContext context = context ?? throw new ArgumentNullException(nameof(context));

        /// <summary>
        /// Получить отчёт сравнения работ.
        /// </summary>
        /// <param name="key">id работ.</param>
        /// <returns>Отчёт сравнения работы.</returns>
        public async Task<ComparisonReport?> GetAsync(ComparisonKey key)
        {
            ArgumentNullException.ThrowIfNull(key, nameof(key));
            return await context.ComparisonReports.FindAsync(key.Work1ID, key.Work2ID);
        }

        /// <summary>
        /// Получить отчёты сравнения работ.
        /// </summary>
        /// <param name="worksID">id работ.</param>
        /// <returns>Отчёты сравнения работ.</returns>
        public async Task<IEnumerable<ComparisonReport>> GetAsync(params ComparisonKey[] worksID)
        {
            ArgumentNullException.ThrowIfNull(worksID, nameof(worksID));

            var keys = worksID.Select(k => new { k.Work1ID, k.Work2ID }).ToList();

            return await context.ComparisonReports
                .Where(x => keys.Contains(new { x.Work1ID, x.Work2ID }))
                .ToListAsync();
        }

        /// <summary>
        /// Получить отчёт сравнения работы со всеми остальными.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <returns>Отчёт сравнения работ.</returns>
        public async Task<IEnumerable<ComparisonReport>> GetAsync(int workID)
        {
            return await context.ComparisonReports.Where(x => x.Work1ID == workID || x.Work2ID == workID).ToListAsync();
        }

        /// <summary>
        /// Добавить отчёты сравнения работ.
        /// </summary>
        /// <param name="reports">Отчёты сравнения работ.</param>
        public async Task AddAsync(params ComparisonReport[] reports)
        {
            ArgumentNullException.ThrowIfNull(reports, nameof(reports));

            IEnumerable<ComparisonReport> entities = reports
                .Where(x => x.Work1ID != x.Work2ID)
                .Select(x =>
                {
                    if (x.Work2ID < x.Work1ID) (x.Work1ID, x.Work2ID) = (x.Work2ID, x.Work1ID);
                    return x;
                });

            if (!entities.Any()) return;

            // Добавление отчётов.
            await context.ComparisonReports.AddRangeAsync(entities);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить отчёты сравнения работ, связанные с определённой работой.
        /// </summary>
        /// <param name="workID">id работы.</param>
        public async Task DeleteAsync(int workID)
        {
            await context.ComparisonReports
                .Where(x => x.Work1ID == workID || x.Work2ID == workID)
                .ExecuteDeleteAsync();

            await context.SaveChangesAsync();
        }
    }
}
