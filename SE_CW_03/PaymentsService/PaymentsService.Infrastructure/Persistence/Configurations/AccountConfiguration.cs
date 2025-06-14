using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentsService.Domain.Entities;

namespace PaymentsService.Infrastructure.Persistence.Configurations
{
    class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("accounts");

            builder.HasKey(o => o.UserID);

            builder.Property(o => o.UserID)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(o => o.Caption)
                .HasColumnName("caption")
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(o => o.Balance)
                .HasColumnName("balance")
                .IsRequired()
                .HasColumnType("numeric(12, 2)");
        }
    }
}
