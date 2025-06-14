using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using PaymentsService.Infrastructure.Persistence;
using PaymentsService.Infrastructure.Persistence.Configurations;
using PaymentsService.Domain.Entities;

namespace PaymentsService.Tests.PaymentsService.Infrastructure.Persistence
{
    public class ConfigurationsTests
    {
        [Fact]
        public void AppDbContextFactory_Test()
        {
            AppDbContextFactory factory = new();
            AppDbContext context = factory.CreateDbContext([]);
            Assert.NotNull(context);
        }

        [Fact]
        public void AccountConfiguration_Test()
        {
            DbContextOptionsBuilder<AppDbContext> builder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase");
            AppDbContext context = new(builder.Options);
            IEntityType? entityType = context.Model.FindEntityType(typeof(Account));

            Assert.NotNull(entityType);

            Assert.Equal("accounts", entityType.GetTableName());

            Assert.Contains(entityType.FindPrimaryKey()?.Properties ?? [], p => p.Name == nameof(Account.UserID));

            IProperty? userIdProperty = entityType.FindProperty(nameof(Account.UserID));
            Assert.NotNull(userIdProperty);
            Assert.Equal("user_id", userIdProperty.GetColumnName());

            IProperty? captionProperty = entityType.FindProperty(nameof(Account.Caption));
            Assert.NotNull(captionProperty);
            Assert.Equal("caption", captionProperty.GetColumnName());
            Assert.Equal(128, captionProperty.GetMaxLength());

            IProperty? balanceProperty = entityType.FindProperty(nameof(Account.Balance));
            Assert.NotNull(balanceProperty);
            Assert.Equal("balance", balanceProperty.GetColumnName());
        }

        [Fact]
        public void OrderToPayMessageConfiguration_Test()
        {
            DbContextOptionsBuilder<AppDbContext> builder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase");
            AppDbContext context = new(builder.Options);
            IEntityType? entityType = context.Model.FindEntityType(typeof(OrderToPayMessage));

            Assert.NotNull(entityType);

            Assert.Equal(OrderToPayMessageConfiguration.TABLE, entityType.GetTableName());

            Assert.Contains(entityType.FindPrimaryKey()?.Properties ?? [], p => p.Name == nameof(OrderToPayMessage.OrderID));

            IProperty? orderIdProperty = entityType.FindProperty(nameof(OrderToPayMessage.OrderID));
            Assert.NotNull(orderIdProperty);
            Assert.Equal("order_id", orderIdProperty.GetColumnName());

            IProperty? userIdProperty = entityType.FindProperty(nameof(OrderToPayMessage.UserID));
            Assert.NotNull(userIdProperty);
            Assert.Equal("user_id", userIdProperty.GetColumnName());

            IProperty? billProperty = entityType.FindProperty(nameof(OrderToPayMessage.Bill));
            Assert.NotNull(billProperty);
            Assert.Equal("bill", billProperty.GetColumnName());

            IProperty? createdAtProperty = entityType.FindProperty(nameof(OrderToPayMessage.CreatedAt));
            Assert.NotNull(createdAtProperty);
            Assert.Equal("created_at", createdAtProperty.GetColumnName());

            IProperty? reservedProperty = entityType.FindProperty(nameof(OrderToPayMessage.Reserved));
            Assert.NotNull(reservedProperty);
            Assert.Equal("reserved", reservedProperty.GetColumnName());
        }

        [Fact]
        public void PaidOrderMessageConfiguration_Test()
        {
            DbContextOptionsBuilder<AppDbContext> builder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase");
            AppDbContext context = new(builder.Options);
            IEntityType? entityType = context.Model.FindEntityType(typeof(PaidOrderMessage));

            Assert.NotNull(entityType);

            Assert.Equal(PaidOrderMessageConfiguration.TABLE, entityType.GetTableName());

            Assert.Contains(entityType.FindPrimaryKey()?.Properties ?? [], p => p.Name == nameof(PaidOrderMessage.OrderID));

            IProperty? orderIdProperty = entityType.FindProperty(nameof(PaidOrderMessage.OrderID));
            Assert.NotNull(orderIdProperty);
            Assert.Equal("order_id", orderIdProperty.GetColumnName());

            IProperty? payloadProperty = entityType.FindProperty(nameof(PaidOrderMessage.Payload));
            Assert.NotNull(payloadProperty);
            Assert.Equal("payload", payloadProperty.GetColumnName());
            Assert.Equal(128, payloadProperty.GetMaxLength());

            IProperty? reservedProperty = entityType.FindProperty(nameof(PaidOrderMessage.Reserved));
            Assert.NotNull(reservedProperty);
            Assert.Equal("reserved", reservedProperty.GetColumnName());
        }
    }
}
