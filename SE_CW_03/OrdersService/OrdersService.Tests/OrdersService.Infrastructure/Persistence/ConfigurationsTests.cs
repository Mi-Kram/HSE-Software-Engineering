using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using OrdersService.Domain.Entities;
using OrdersService.Infrastructure.Persistence.Configurations;
using OrdersService.Infrastructure.Persistence;

namespace OrdersService.Tests.OrdersService.Infrastructure.Persistence
{
    public class ConfigurationsTests
    {
        [Fact]
        public void AppDbContextFactory_Test()
        {
            OrdersDbContextFactory factory = new();
            OrdersDbContext context = factory.CreateDbContext([]);
            Assert.NotNull(context);
        }

        [Fact]
        public void OrderConfiguration_Test()
        {
            DbContextOptionsBuilder<OrdersDbContext> builder = new DbContextOptionsBuilder<OrdersDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase");
            OrdersDbContext context = new(builder.Options);
            IEntityType? entityType = context.Model.FindEntityType(typeof(Order));
            Assert.NotNull(entityType);

            Assert.Equal(OrdersConfiguration.TABLE, entityType.GetTableName());

            Assert.Contains(entityType.FindPrimaryKey()?.Properties ?? [], p => p.Name == nameof(Order.ID));

            IProperty? orderIdProperty = entityType.FindProperty(nameof(Order.ID));
            Assert.NotNull(orderIdProperty);
            Assert.Equal("id", orderIdProperty.GetColumnName());

            IProperty? userIdProperty = entityType.FindProperty(nameof(Order.UserID));
            Assert.NotNull(userIdProperty);
            Assert.Equal("user_id", userIdProperty.GetColumnName());

            IProperty? billProperty = entityType.FindProperty(nameof(Order.Bill));
            Assert.NotNull(billProperty);
            Assert.Equal("bill", billProperty.GetColumnName());

            IProperty? createdAtProperty = entityType.FindProperty(nameof(Order.CreatedAt));
            Assert.NotNull(createdAtProperty);
            Assert.Equal("created_at", createdAtProperty.GetColumnName());

            IProperty? statusProperty = entityType.FindProperty(nameof(Order.Status));
            Assert.NotNull(statusProperty);
            Assert.Equal("status", statusProperty.GetColumnName());
        }

        [Fact]
        public void NewOrderMessageConfiguration_Test()
        {
            DbContextOptionsBuilder<OrdersDbContext> builder = new DbContextOptionsBuilder<OrdersDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase");
            OrdersDbContext context = new(builder.Options);
            IEntityType? entityType = context.Model.FindEntityType(typeof(NewOrderMessage));

            Assert.NotNull(entityType);

            Assert.Equal(NewOrderMessageConfiguration.TABLE, entityType.GetTableName());

            Assert.Contains(entityType.FindPrimaryKey()?.Properties ?? [], p => p.Name == nameof(NewOrderMessage.OrderID));

            IProperty? orderIdProperty = entityType.FindProperty(nameof(NewOrderMessage.OrderID));
            Assert.NotNull(orderIdProperty);
            Assert.Equal("order_id", orderIdProperty.GetColumnName());

            IProperty? userIdProperty = entityType.FindProperty(nameof(NewOrderMessage.UserID));
            Assert.NotNull(userIdProperty);
            Assert.Equal("user_id", userIdProperty.GetColumnName());

            IProperty? payloadProperty = entityType.FindProperty(nameof(NewOrderMessage.Payload));
            Assert.NotNull(payloadProperty);
            Assert.Equal("payload", payloadProperty.GetColumnName());
            Assert.Equal(128, payloadProperty.GetMaxLength());

            IProperty? createdAtProperty = entityType.FindProperty(nameof(NewOrderMessage.CreatedAt));
            Assert.NotNull(createdAtProperty);
            Assert.Equal("created_at", createdAtProperty.GetColumnName());

            IProperty? reservedProperty = entityType.FindProperty(nameof(NewOrderMessage.Reserved));
            Assert.NotNull(reservedProperty);
            Assert.Equal("reserved", reservedProperty.GetColumnName());
        }

        [Fact]
        public void PaidOrderMessageConfiguration_Test()
        {
            DbContextOptionsBuilder<OrdersDbContext> builder = new DbContextOptionsBuilder<OrdersDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase");
            OrdersDbContext context = new(builder.Options);
            IEntityType? entityType = context.Model.FindEntityType(typeof(PaidOrderMessage));

            Assert.NotNull(entityType);

            Assert.Equal(PaidOrderMessageConfiguration.TABLE, entityType.GetTableName());

            Assert.Contains(entityType.FindPrimaryKey()?.Properties ?? [], p => p.Name == nameof(PaidOrderMessage.OrderID));

            IProperty? orderIdProperty = entityType.FindProperty(nameof(PaidOrderMessage.OrderID));
            Assert.NotNull(orderIdProperty);
            Assert.Equal("order_id", orderIdProperty.GetColumnName());

            IProperty? payloadProperty = entityType.FindProperty(nameof(PaidOrderMessage.Status));
            Assert.NotNull(payloadProperty);
            Assert.Equal("status", payloadProperty.GetColumnName());
            Assert.Equal(64, payloadProperty.GetMaxLength());

            IProperty? reservedProperty = entityType.FindProperty(nameof(PaidOrderMessage.Reserved));
            Assert.NotNull(reservedProperty);
            Assert.Equal("reserved", reservedProperty.GetColumnName());
        }
    }
}
