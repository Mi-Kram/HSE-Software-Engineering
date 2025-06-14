using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersService.Domain.Entities;

namespace OrdersService.Infrastructure.Persistence.Configurations
{
    public class PaidOrderMessageConfiguration : IEntityTypeConfiguration<PaidOrderMessage>
    {
        public const string TABLE = "paid-order-messages";

        public void Configure(EntityTypeBuilder<PaidOrderMessage> builder)
        {
            builder.ToTable(TABLE, x =>
            {
                var valid = Enum.GetNames<OrderStatus>().Select(x => x.ToLowerInvariant());
                x.HasCheckConstraint(
                    $"CK_{TABLE}_status_valid",
                    $"LOWER(status) IN ('{string.Join("', '", valid)}')"
                    );
            });

            builder.HasKey(o => o.OrderID);

            builder.Property(o => o.OrderID)
                .HasColumnName("order_id")
                .IsRequired();

            builder.Property(o => o.Status)
                .HasColumnName("status")
                .IsRequired()
                .HasConversion(
                    v => v.ToString().ToLowerInvariant(),
                    v => Enum.Parse<OrderStatus>(v, true))
                .HasMaxLength(64);

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
