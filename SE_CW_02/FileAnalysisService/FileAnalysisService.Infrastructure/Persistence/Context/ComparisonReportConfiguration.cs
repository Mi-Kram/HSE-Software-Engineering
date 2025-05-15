using FileAnalysisService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileAnalysisService.Infrastructure.Persistence.Context
{
    /// <summary>
    /// Конфигурация сущности <see cref="ComparisonReport"/>.
    /// </summary>
    public class ComparisonReportConfiguration : IEntityTypeConfiguration<ComparisonReport>
    {
        public void Configure(EntityTypeBuilder<ComparisonReport> builder)
        {
            builder.ToTable("comparison-reports", x =>
            {
                // Work1ID должен быть меньше Work2ID.
                x.HasCheckConstraint("work1_id_work2_id_check", "work1_id < work2_id");

                // Similarity должно быть в пределах от 0 до 1, т.к. является процентом схожести. 
                x.HasCheckConstraint("similarity_check", "similarity BETWEEN 0 AND 1");
            });

            // Keys
            builder.HasKey(x => new { x.Work1ID, x.Work2ID });

            // Work1ID
            builder.Property(w => w.Work1ID)
                   .HasColumnName("work1_id")
                   .IsRequired();

            // Work2ID
            builder.Property(w => w.Work2ID)
                   .HasColumnName("work2_id")
                   .IsRequired();

            // Similarity
            builder.Property(w => w.Similarity)
                   .HasColumnName("similarity")
                   .IsRequired();
        }
    }
}
