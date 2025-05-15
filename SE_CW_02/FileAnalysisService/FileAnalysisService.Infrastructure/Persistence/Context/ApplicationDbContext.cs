using FileAnalysisService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileAnalysisService.Infrastructure.Persistence.Context
{
    /// <summary>
    /// Контекст удалённого репозитория.
    /// </summary>
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        /// <summary>
        /// Таблица отчётов анализа работ.
        /// </summary>
        public virtual DbSet<AnalyzeReport> AnalyzeReports { get; set; }

        /// <summary>
        /// Таблица отчётов сравнения работ.
        /// </summary>
        public virtual DbSet<ComparisonReport> ComparisonReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AnalyzeReportConfiguration());
            modelBuilder.ApplyConfiguration(new ComparisonReportConfiguration());
        }
    }
}
