using Microsoft.EntityFrameworkCore;
using OrdersService.Infrastructure.Persistence;
using OrdersService.Domain.Entities;
using OrdersService.Domain.DTO;

namespace OrdersService.Tests.OrdersService.Infrastructure.Persistence
{
    public class OrdersRepositoryTests
    {
        private static (OrdersDbContext context, OrdersRepository repository) GetInstances(string database)
        {
            DbContextOptionsBuilder<OrdersDbContext> builder = new DbContextOptionsBuilder<OrdersDbContext>()
                .UseInMemoryDatabase(databaseName: database);

            OrdersDbContext context = new(builder.Options);
            OrdersRepository repository = new(context);

            return (context, repository);
        }

        [Fact]
        public async Task GetAllAsync_Test()
        {
            var (context, repository) = GetInstances(nameof(GetAllAsync_Test));

            await context.Orders.AddRangeAsync(
                new Order() { ID = Guid.NewGuid(), UserID = 1, Bill = 50, Status = OrderStatus.NewOrder, CreatedAt = DateTime.Now },
                new Order() { ID = Guid.NewGuid(), UserID = 2, Bill = 50, Status = OrderStatus.NewOrder, CreatedAt = DateTime.Now },
                new Order() { ID = Guid.NewGuid(), UserID = 2, Bill = 50, Status = OrderStatus.NewOrder, CreatedAt = DateTime.Now });
            await context.SaveChangesAsync();

            Assert.Equal(3, (await repository.GetAllAsync()).Count);
        }

        [Fact]
        public async Task GetAsync_Test()
        {
            var (context, repository) = GetInstances(nameof(GetAsync_Test));

            Guid guid = Guid.NewGuid();
            await context.Orders.AddRangeAsync(
                new Order() { ID = Guid.NewGuid(), UserID = 1, Bill = 50, Status = OrderStatus.NewOrder, CreatedAt = DateTime.Now },
                new Order() { ID = guid, UserID = 2, Bill = 50, Status = OrderStatus.NewOrder, CreatedAt = DateTime.Now },
                new Order() { ID = Guid.NewGuid(), UserID = 2, Bill = 50, Status = OrderStatus.NewOrder, CreatedAt = DateTime.Now });
            await context.SaveChangesAsync();

            Assert.NotNull(await repository.GetAsync(guid));
        }

        [Fact]
        public async Task CreateAccountAsync_Test()
        {
            var (context, repository) = GetInstances(nameof(CreateAccountAsync_Test));

            Order order = await repository.AddOrderAsync(new OrderDTO() { UserID = 1, Bill = 50, CreatedAt = DateTime.Now });
            Assert.Equal(OrderStatus.NewOrder, order.Status);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.AddOrderAsync(null!));
        }
    }
}
