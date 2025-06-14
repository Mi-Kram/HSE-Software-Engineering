using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentsService.Domain.Entities;

namespace PaymentsService.Infrastructure.Persistence.Configurations
{
    public class PaidOrderMessageConfiguration : IEntityTypeConfiguration<PaidOrderMessage>
    {
        public const string TABLE = "paid-order-messages";

        public void Configure(EntityTypeBuilder<PaidOrderMessage> builder)
        {
            builder.ToTable(TABLE);

            builder.HasKey(o => o.OrderID);

            builder.Property(o => o.OrderID)
                .HasColumnName("order_id")
                .IsRequired();

            builder.Property(o => o.Payload)
                .HasColumnName("payload")
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(o => o.Reserved)
                .HasColumnName("reserved")
                .IsRequired()
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                );
        }
    }
}
