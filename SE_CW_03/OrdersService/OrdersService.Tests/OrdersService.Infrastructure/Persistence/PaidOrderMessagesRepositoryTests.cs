using Microsoft.EntityFrameworkCore;
using OrdersService.Domain.Entities;
using OrdersService.Infrastructure.Persistence.MessagesRepository;
using OrdersService.Infrastructure.Persistence;

namespace OrdersService.Tests.OrdersService.Infrastructure.Persistence
{
    public class PaidOrderMessagesRepositoryTests
    {
        private static (OrdersDbContext context, PaidOrderMessagesRepository repository) GetInstances(string database)
        {
            DbContextOptionsBuilder<OrdersDbContext> builder = new DbContextOptionsBuilder<OrdersDbContext>()
            .UseInMemoryDatabase(databaseName: database);

            OrdersDbContext context = new(builder.Options);
            PaidOrderMessagesRepository repository = new(context);

            return (context, repository);
        }

        [Fact]
        public async Task GetMessageAsync_Test()
        {
            var (context, repository) = GetInstances(nameof(GetMessageAsync_Test));

            Guid guid = Guid.NewGuid();

            context.PaidOrderMessages.AddRange(
                new PaidOrderMessage { OrderID = Guid.NewGuid(), Status = OrderStatus.CanceledNoUserFound, Reserved = DateTime.UtcNow.AddMinutes(-1) },
                new PaidOrderMessage { OrderID = guid, Status = OrderStatus.Completed, Reserved = DateTime.UtcNow.AddMinutes(-1) },
                new PaidOrderMessage { OrderID = Guid.NewGuid(), Status = OrderStatus.CanceledNoFunds, Reserved = DateTime.UtcNow.AddMinutes(-1) }
            );
            context.SaveChanges();

            var result = await repository.GetMessageAsync(guid);

            Assert.NotNull(result);
            Assert.Equal(OrderStatus.Completed, result.Status);

            Assert.Null(await repository.GetMessageAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task AddMessageAsync_Test()
        {
            var (context, repository) = GetInstances(nameof(AddMessageAsync_Test));

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.AddMessageAsync(null!));

            Guid guid = Guid.NewGuid();
            PaidOrderMessage message = new() { OrderID = guid, Status = OrderStatus.AwaitForPayment, Reserved = DateTime.UtcNow.AddMinutes(-1) };

            await repository.AddMessageAsync(message);
            Assert.NotNull(await context.PaidOrderMessages.FindAsync(guid));
        }
    }
}
