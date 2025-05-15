using FileAnalysisService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileAnalysisService.Infrastructure.Persistence.Context
{
    /// <summary>
    /// Конфигурация сущности <see cref="AnalyzeReport"/>.
    /// </summary>
    public class AnalyzeReportConfiguration : IEntityTypeConfiguration<AnalyzeReport>
    {
        public void Configure(EntityTypeBuilder<AnalyzeReport> builder)
        {
            builder.ToTable("analyse-reports", x =>
            {
                // Paragraphs, Words, Numbers, Symbols не могут быть отрицательными.
                x.HasCheckConstraint("paragraphs_check", "paragraphs >= 0");
                x.HasCheckConstraint("words_check", "words >= 0");
                x.HasCheckConstraint("numbers_check", "numbers >= 0");
                x.HasCheckConstraint("symbols_check", "symbols >= 0");
            });

            // WorkID
            builder.HasKey(x => x.WorkID);

            builder.Property(w => w.WorkID)
                   .HasColumnName("work_id")
                   .IsRequired();

            // Paragraphs
            builder.Property(w => w.Paragraphs)
                   .HasColumnName("paragraphs")
                   .IsRequired();

            // Words
            builder.Property(w => w.Words)
                   .HasColumnName("words")
                   .IsRequired();

            // Numbers
            builder.Property(w => w.Numbers)
                   .HasColumnName("numbers")
                   .IsRequired();

            // Symbols
            builder.Property(w => w.Symbols)
                   .HasColumnName("symbols")
                   .IsRequired();
        }
    }
}
