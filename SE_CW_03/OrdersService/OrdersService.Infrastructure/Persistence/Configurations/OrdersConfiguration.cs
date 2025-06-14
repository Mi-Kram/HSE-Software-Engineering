using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersService.Domain.Entities;

namespace OrdersService.Infrastructure.Persistence.Configurations
{
    public class OrdersConfiguration : IEntityTypeConfiguration<Order>
    {
        public const string TABLE = "orders";
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable(TABLE, x =>
            {
                var valid = Enum.GetNames<OrderStatus>().Select(x => x.ToLowerInvariant());
                x.HasCheckConstraint(
                    $"CK_{TABLE}_status_valid",
                    $"LOWER(status) IN ('{string.Join("', '", valid)}')"
                    );
            });

            builder.HasKey(o => o.ID);

            builder.Property(o => o.ID)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(o => o.UserID)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(o => o.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                );

            builder.Property(o => o.Bill)
                .HasColumnName("bill")
                .IsRequired()
                .HasColumnType("numeric(12, 2)");

            builder.Property(o => o.Status)
                .HasColumnName("status")
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<OrderStatus>(v, true))
                .HasMaxLength(64);

            builder.HasIndex(o => o.UserID);
        }
    }
}
