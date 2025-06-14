using Microsoft.EntityFrameworkCore;
using PaymentsService.Domain.Entities;
using PaymentsService.Infrastructure.Persistence;
using PaymentsService.Infrastructure.Persistence.MessagesRepository;

namespace PaymentsService.Tests.PaymentsService.Infrastructure.Persistence
{
    public class OrderToPayMessagesRepositoryTests
    {
        private static (AppDbContext context, OrderToPayMessagesRepository repository) GetInstances(string database)
        {
            DbContextOptionsBuilder<AppDbContext> builder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: database);

            AppDbContext context = new(builder.Options);
            OrderToPayMessagesRepository repository = new(context);

            return (context, repository);
        }

        [Fact]
        public async Task GetMessageAsync_Test()
        {
            var (context, repository) = GetInstances(nameof(GetMessageAsync_Test));

            Guid guid = Guid.NewGuid();

            context.OrderToPayMessages.AddRange(
                new OrderToPayMessage { OrderID = Guid.NewGuid(), UserID = 1, Bill = 10, CreatedAt = DateTime.UtcNow.AddDays(-1), Reserved = DateTime.UtcNow.AddMinutes(-1) },
                new OrderToPayMessage { OrderID = guid, UserID = 1, Bill = 20, CreatedAt = DateTime.UtcNow.AddDays(-3), Reserved = DateTime.UtcNow.AddMinutes(-1) },
                new OrderToPayMessage { OrderID = Guid.NewGuid(), UserID = 1, Bill = 30, CreatedAt = DateTime.UtcNow.AddDays(-2), Reserved = DateTime.UtcNow.AddMinutes(-1) }
            );
            context.SaveChanges();

            var result = await repository.GetMessageAsync(guid);

            Assert.NotNull(result);
            Assert.Equal(20, result.Bill);

            Assert.Null(await repository.GetMessageAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task AddMessageAsync_Test()
        {
            var (context, repository) = GetInstances(nameof(AddMessageAsync_Test));

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.AddMessageAsync(null!));

            Guid guid = Guid.NewGuid();
            OrderToPayMessage message = new OrderToPayMessage { OrderID = guid, UserID = 1, Bill = 10, CreatedAt = DateTime.UtcNow.AddDays(-1), Reserved = DateTime.UtcNow.AddMinutes(-1) };

            await repository.AddMessageAsync(message);
            Assert.NotNull(await context.OrderToPayMessages.FindAsync(guid));
        }
    }
}
