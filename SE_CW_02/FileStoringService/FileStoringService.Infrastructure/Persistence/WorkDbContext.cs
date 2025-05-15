using FileStoringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileStoringService.Infrastructure.Persistence
{
    /// <summary>
    /// Контекст удалённого репозитория.
    /// </summary>
    public class WorkDbContext(DbContextOptions<WorkDbContext> options) : DbContext(options)
    {
        /// <summary>
        /// Таблица работ.
        /// </summary>
        public virtual DbSet<WorkInfo> Works { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WorkConfiguration());
        }
    }
}
