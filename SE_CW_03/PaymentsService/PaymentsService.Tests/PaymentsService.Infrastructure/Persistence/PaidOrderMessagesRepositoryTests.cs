using Microsoft.EntityFrameworkCore;
using PaymentsService.Domain.Entities;
using PaymentsService.Infrastructure.Persistence.MessagesRepository;
using PaymentsService.Infrastructure.Persistence;

namespace PaymentsService.Tests.PaymentsService.Infrastructure.Persistence
{
    public class PaidOrderMessagesRepositoryTests
    {
        private static (AppDbContext context, PaidOrderMessagesRepository repository) GetInstances(string database)
        {
            DbContextOptionsBuilder<AppDbContext> builder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: database);

            AppDbContext context = new(builder.Options);
            PaidOrderMessagesRepository repository = new(context);

            return (context, repository);
        }

        [Fact]
        public async Task GetMessageAsync_Test()
        {
            var (context, repository) = GetInstances(nameof(GetMessageAsync_Test));

            Guid guid = Guid.NewGuid();

            context.PaidOrderMessages.AddRange(
                new PaidOrderMessage { OrderID = Guid.NewGuid(), Payload = "1", Reserved = DateTime.UtcNow.AddMinutes(-1) },
                new PaidOrderMessage { OrderID = guid, Payload = "2", Reserved = DateTime.UtcNow.AddMinutes(-1) },
                new PaidOrderMessage { OrderID = Guid.NewGuid(), Payload = "3", Reserved = DateTime.UtcNow.AddMinutes(-1) }
            );
            context.SaveChanges();

            var result = await repository.GetMessageAsync(guid);

            Assert.NotNull(result);
            Assert.Equal("2", result.Payload);

            Assert.Null(await repository.GetMessageAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task AddMessageAsync_Test()
        {
            var (context, repository) = GetInstances(nameof(AddMessageAsync_Test));

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.AddMessageAsync(null!));

            Guid guid = Guid.NewGuid();
            PaidOrderMessage message = new PaidOrderMessage { OrderID = guid, Payload = "1", Reserved = DateTime.UtcNow.AddMinutes(-1) };

            await repository.AddMessageAsync(message);
            Assert.NotNull(await context.PaidOrderMessages.FindAsync(guid));
        }
    }
}
