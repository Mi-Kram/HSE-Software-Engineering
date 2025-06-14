using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentsService.Domain.Entities;

namespace PaymentsService.Infrastructure.Persistence.Configurations
{
    public class OrderToPayMessageConfiguration : IEntityTypeConfiguration<OrderToPayMessage>
    {
        public const string TABLE = "order-to-pay-messages";
        public void Configure(EntityTypeBuilder<OrderToPayMessage> builder)
        {
            builder.ToTable(TABLE);

            builder.HasKey(o => o.OrderID);

            builder.Property(o => o.OrderID)
                .HasColumnName("order_id")
                .IsRequired();

            builder.Property(o => o.UserID)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(o => o.Bill)
                .HasColumnName("bill")
                .IsRequired()
                .HasColumnType("numeric(12, 2)");

            builder.Property(o => o.CreatedAt)
                .HasColumnName("created_at")
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                );

            builder.Property(o => o.Reserved)
                .HasColumnName("reserved")
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                );
        }
    }
}
