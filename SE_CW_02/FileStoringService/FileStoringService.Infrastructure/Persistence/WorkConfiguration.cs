using FileStoringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileStoringService.Infrastructure.Persistence
{
    /// <summary>
    /// Конфигурация таблицы работ.
    /// </summary>
    public class WorkConfiguration : IEntityTypeConfiguration<WorkInfo>
    {
        public const int MAX_TITLE_LENGTH = 128;
        public const int MAX_HASH_LENGTH = 64;

        /// <summary>
        /// Конфигурация таблицы работ.
        /// </summary>
        /// <param name="builder">Параметры конфигурации.</param>
        public void Configure(EntityTypeBuilder<WorkInfo> builder)
        {
            builder.ToTable("works");

            // ID
            builder.HasKey(x => x.ID);

            builder.Property(w => w.ID)
                   .HasColumnName("id")
                   .IsRequired();

            // UserID
            builder.Property(w => w.UserID)
                   .HasColumnName("user_id")
                   .IsRequired();

            // Title
            builder.Property(w => w.Title)
                   .HasColumnName("title")
                   .IsRequired()
                   .HasMaxLength(MAX_TITLE_LENGTH);

            // Hash
            builder.Property(w => w.Hash)
                   .HasColumnName("hash")
                   .IsRequired()
                   .HasMaxLength(MAX_HASH_LENGTH);

            builder.HasIndex(w => w.Hash);


            // Uploaded
            builder.Property(w => w.Uploaded)
                   .HasColumnName("uploaded")
                   .IsRequired();
        }
    }
}
