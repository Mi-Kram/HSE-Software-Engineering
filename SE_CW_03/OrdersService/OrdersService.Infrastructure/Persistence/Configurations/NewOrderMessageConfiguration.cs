using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersService.Domain.Entities;

namespace OrdersService.Infrastructure.Persistence.Configurations
{
    public class NewOrderMessageConfiguration : IEntityTypeConfiguration<NewOrderMessage>
    {
        public const string TABLE = "new-order-messages";
        public void Configure(EntityTypeBuilder<NewOrderMessage> builder)
        {
            builder.ToTable(TABLE);

            builder.HasKey(o => o.OrderID);

            builder.Property(o => o.OrderID)
                .HasColumnName("order_id")
                .IsRequired();

            builder.Property(o => o.UserID)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(o => o.Payload)
                .HasColumnName("payload")
                .IsRequired()
                .HasMaxLength(128);

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
