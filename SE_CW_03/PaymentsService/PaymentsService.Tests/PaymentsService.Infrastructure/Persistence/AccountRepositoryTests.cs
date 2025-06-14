using Microsoft.EntityFrameworkCore;
using Moq;
using PaymentsService.Domain.Entities;
using PaymentsService.Infrastructure.Persistence;

namespace PaymentsService.Tests.PaymentsService.Infrastructure.Persistence
{
    public class AccountRepositoryTests
    {
        private static (AppDbContext context, AccountRepository repository) GetInstances(string database)
        {
            DbContextOptionsBuilder<AppDbContext> builder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: database);

            AppDbContext context = new(builder.Options);
            AccountRepository repository = new(context);

            return (context, repository);
        }

        [Fact]
        public async Task GetAllAsync_Test()
        {
            var (context, repository) = GetInstances(nameof(GetAllAsync_Test));

            await context.Accounts.AddRangeAsync(
                new Account() { Balance = 10, Caption = "Cap1" },
                new Account() { Balance = 20, Caption = "Cap2" },
                new Account() { Balance = 30, Caption = "Cap3" });
            await context.SaveChangesAsync();

            Assert.Equal(3, (await repository.GetAllAsync()).Count);
        }

        [Fact]
        public async Task GetAsync_Test()
        {
            var (context, repository) = GetInstances(nameof(GetAsync_Test));

            await context.Accounts.AddRangeAsync(
                new Account() { Balance = 10, Caption = "Cap1" },
                new Account() { Balance = 20, Caption = "Cap2" },
                new Account() { Balance = 30, Caption = "Cap3" });
            await context.SaveChangesAsync();

            Assert.NotNull(await repository.GetAsync(2));
        }

        [Fact]
        public async Task CreateAccountAsync_Test()
        {
            var (context, repository) = GetInstances(nameof(CreateAccountAsync_Test));

            int id = await repository.CreateAccountAsync(new Account() { Balance = 10, Caption = "Cap" });
            Assert.Equal(1, id);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.CreateAccountAsync(null!));
        }
    }
}
