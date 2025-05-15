using FileAnalysisService.Domain.Interfaces;
using FileAnalysisService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace FileAnalysisService.Infrastructure.Persistence
{
    /// <summary>
    /// Репозитория для работы с работами.
    /// </summary>
    public class WorkRepository(
        ApplicationDbContext context,
        IAnalyseReportRepository analyseReportRepository,
        IComparisonReportRepository comparisonReportRepository) : IWorkRepository
    {
        private readonly ApplicationDbContext context = context ?? throw new ArgumentNullException(nameof(context));
        private readonly IAnalyseReportRepository analyseReportRepository = analyseReportRepository ?? throw new ArgumentNullException(nameof(analyseReportRepository));
        private readonly IComparisonReportRepository comparisonReportRepository = comparisonReportRepository ?? throw new ArgumentNullException(nameof(comparisonReportRepository));

        /// <summary>
        /// Удаление данных, связанные с работой.
        /// </summary>
        /// <param name="workID">id работы.</param>
        public async Task DeleteAsync(int workID)
        {
            using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();

            try
            {
                // Удаление сущностей, связанных с этой работой.
                await analyseReportRepository.DeleteAsync(workID);
                await comparisonReportRepository.DeleteAsync(workID);
                
                // Подтвердить транзакцию.
                await transaction.CommitAsync();
            }
            catch
            {
                // Отменить транзакцию.
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
