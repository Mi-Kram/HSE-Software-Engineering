using Microsoft.EntityFrameworkCore;
using OrdersService.Domain.Entities;
using OrdersService.Infrastructure.Persistence;
using OrdersService.Infrastructure.Persistence.MessagesRepository;
using System;

namespace OrdersService.Tests.OrdersService.Infrastructure.Persistence
{
    public class NewOrderMessagesRepositoryTests
    {
        private static (OrdersDbContext context, NewOrderMessagesRepository repository) GetInstances(string database)
        {
            DbContextOptionsBuilder<OrdersDbContext> builder = new DbContextOptionsBuilder<OrdersDbContext>()
            .UseInMemoryDatabase(databaseName: database);

            OrdersDbContext context = new(builder.Options);
            NewOrderMessagesRepository repository = new(context);

            return (context, repository);
        }

        [Fact]
        public async Task GetMessageAsync_Test()
        {
            var (context, repository) = GetInstances(nameof(GetMessageAsync_Test));

            Guid guid = Guid.NewGuid();

            context.NewOrderMessages.AddRange(
                new NewOrderMessage { OrderID = Guid.NewGuid(), UserID = 1, Payload = "Payload1", CreatedAt = DateTime.UtcNow.AddDays(-1), Reserved = DateTime.UtcNow.AddMinutes(-1) },
                new NewOrderMessage { OrderID = guid, UserID = 1, Payload = "Payload2", CreatedAt = DateTime.UtcNow.AddDays(-3), Reserved = DateTime.UtcNow.AddMinutes(-1) },
                new NewOrderMessage { OrderID = Guid.NewGuid(), UserID = 1, Payload = "Payload3", CreatedAt = DateTime.UtcNow.AddDays(-2), Reserved = DateTime.UtcNow.AddMinutes(-1) }
            );
            context.SaveChanges();

            var result = await repository.GetMessageAsync(guid);

            Assert.NotNull(result);
            Assert.Equal("Payload2", result.Payload);

            Assert.Null(await repository.GetMessageAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task AddMessageAsync_Test()
        {
            var (context, repository) = GetInstances(nameof(AddMessageAsync_Test));

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.AddMessageAsync(null!));

            Guid guid = Guid.NewGuid();
            NewOrderMessage message = new() { OrderID = guid, UserID = 1, Payload = "payload", CreatedAt = DateTime.UtcNow.AddDays(-1), Reserved = DateTime.UtcNow.AddMinutes(-1) };

            await repository.AddMessageAsync(message);
            Assert.NotNull(await context.NewOrderMessages.FindAsync(guid));
        }
    }
}
