using FileStoringService.Domain.Entities;
using FileStoringService.Infrastructure.Persistence.Scripts;
using FileStoringService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Moq;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace FileStoringService.Tests.Infrastructure.Persistence
{
    public class AddWorkScriptTests
    {
        private static WorkDbContext GetInstances(string database)
        {
            DbContextOptionsBuilder<WorkDbContext> builder = new DbContextOptionsBuilder<WorkDbContext>()
                .UseInMemoryDatabase(databaseName: database);

            Mock<IDbContextTransaction> mockTransaction = new();

            WorkDbContext workDbContext = new(builder.Options);
            var mockDatabase = new Mock<DatabaseFacade>(workDbContext);

            mockDatabase.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            mockDatabase.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(mockTransaction.Object));

            Mock<WorkDbContext> mockContext = new(builder.Options);
            mockContext.Setup(x => x.Works).Returns(workDbContext.Works);
            mockContext.Setup(x => x.Database).Returns(mockDatabase.Object);

            return mockContext.Object;
        }


        [Fact]
        public async Task AddBaseAsync_ValidWork_BeginsTransactionAddsEntitySavesChangesReturnsId()
        {
            WorkDbContext context = GetInstances(nameof(AddBaseAsync_ValidWork_BeginsTransactionAddsEntitySavesChangesReturnsId));
            var workToAdd = new WorkInfo { UserID = 1, Hash = "test_hash", Uploaded = DateTime.UtcNow };
            var script = new AddWorkScript(context);

            var resultId = await script.AddBaseAsync(workToAdd);

            Assert.NotNull(script.GetType().GetField("transaction", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(script));
        }

        [Fact]
        public async Task AddBaseAsync_NullWork_ThrowsArgumentNullException()
        {
            WorkDbContext context = GetInstances(nameof(AddBaseAsync_NullWork_ThrowsArgumentNullException));
            var script = new AddWorkScript(context);

            await Assert.ThrowsAsync<ArgumentNullException>(() => script.AddBaseAsync(null!));
        }

        [Fact]
        public async Task AddBaseAsync_CalledTwice_ThrowsInvalidOperationException()
        {
            WorkDbContext context = GetInstances(nameof(AddBaseAsync_CalledTwice_ThrowsInvalidOperationException));
            var script = new AddWorkScript(context);
            var workToAdd = new WorkInfo { UserID = 1, Hash = "test_hash", Uploaded = DateTime.UtcNow };
            await script.AddBaseAsync(workToAdd);

            await Assert.ThrowsAsync<InvalidOperationException>(() => script.AddBaseAsync(workToAdd));
        }

        [Fact]
        public async Task ConfirmAsync_TransactionNotStarted_ThrowsInvalidOperationException()
        {
            WorkDbContext context = GetInstances(nameof(ConfirmAsync_TransactionNotStarted_ThrowsInvalidOperationException));
            var script = new AddWorkScript(context);

            await Assert.ThrowsAsync<InvalidOperationException>(script.ConfirmAsync);
        }

        [Fact]
        public async Task ConfirmAsync_TransactionStarted_SetsConfirmedToTrue()
        {
            WorkDbContext context = GetInstances(nameof(ConfirmAsync_TransactionStarted_SetsConfirmedToTrue));
            var script = new AddWorkScript(context);
            var workToAdd = new WorkInfo { UserID = 1, Hash = "test_hash", Uploaded = DateTime.UtcNow };
            await script.AddBaseAsync(workToAdd);

            await script.ConfirmAsync();
            Assert.True((bool)script.GetType().GetField("confirmed", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(script)!);
        }

        [Fact]
        public async Task ConfirmAsync_AlreadyConfirmed_ThrowsInvalidOperationException()
        {
            WorkDbContext context = GetInstances(nameof(ConfirmAsync_AlreadyConfirmed_ThrowsInvalidOperationException));
            var script = new AddWorkScript(context);
            var workToAdd = new WorkInfo { UserID = 1, Hash = "test_hash", Uploaded = DateTime.UtcNow };

            await script.AddBaseAsync(workToAdd);
            await script.ConfirmAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(script.ConfirmAsync);
        }

        [Fact]
        public async Task DisposeAsync_TransactionIsNull_DoesNothing()
        {
            WorkDbContext context = GetInstances(nameof(DisposeAsync_TransactionIsNull_DoesNothing));
            var script = new AddWorkScript(context);

            await script.DisposeAsync();
        }

        [Fact]
        public async Task DisposeAsync_Confirmed_CommitsTransactionDisposesTransaction()
        {
            WorkDbContext context = GetInstances(nameof(DisposeAsync_Confirmed_CommitsTransactionDisposesTransaction));
            var script = new AddWorkScript(context);
            var workToAdd = new WorkInfo { UserID = 1, Hash = "test_hash", Uploaded = DateTime.UtcNow };
            await script.AddBaseAsync(workToAdd);
            await script.ConfirmAsync();

            await script.DisposeAsync();
        }

        [Fact]
        public async Task DisposeAsync_NotConfirmed_RollsBackTransactionDisposesTransaction()
        {
            WorkDbContext context = GetInstances(nameof(DisposeAsync_NotConfirmed_RollsBackTransactionDisposesTransaction));
            var script = new AddWorkScript(context);
            var workToAdd = new WorkInfo { UserID = 1, Hash = "test_hash", Uploaded = DateTime.UtcNow };
            await script.AddBaseAsync(workToAdd);

            await script.DisposeAsync();
        }
    }
}
