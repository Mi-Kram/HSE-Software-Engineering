using FileStoringService.Domain.Entities;
using FileStoringService.Infrastructure.Persistence.Scripts;
using FileStoringService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Reflection;

namespace FileStoringService.Tests.Infrastructure.Persistence
{
    public class RemoveWorkScriptTests
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
        public async Task RemoveAsync_ValidWork_BeginsTransactionRemovesEntitySavesChanges()
        {
            WorkDbContext context = GetInstances(nameof(RemoveAsync_ValidWork_BeginsTransactionRemovesEntitySavesChanges));
            RemoveWorkScript script = new(context);
            var workToRemove = new WorkInfo { ID = 1, UserID = 1, Hash = "test_hash", Uploaded = DateTime.UtcNow };

            await script.RemoveAsync(workToRemove);
            Assert.NotNull(script.GetType().GetField("transaction", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(script));
        }

        [Fact]
        public async Task RemoveAsync_NullWork_ThrowsArgumentNullException()
        {
            WorkDbContext context = GetInstances(nameof(RemoveAsync_NullWork_ThrowsArgumentNullException));
            RemoveWorkScript script = new(context);

            await Assert.ThrowsAsync<ArgumentNullException>(() => script.RemoveAsync(null!));
        }

        [Fact]
        public async Task RemoveAsync_CalledTwice_ThrowsInvalidOperationException()
        {
            WorkDbContext context = GetInstances(nameof(RemoveAsync_CalledTwice_ThrowsInvalidOperationException));
            RemoveWorkScript script = new(context);
            var workToRemove = new WorkInfo { ID = 1, UserID = 1, Hash = "test_hash", Uploaded = DateTime.UtcNow };
            await script.RemoveAsync(workToRemove);

            await Assert.ThrowsAsync<InvalidOperationException>(() => script.RemoveAsync(workToRemove));
        }

        [Fact]
        public async Task ConfirmAsync_TransactionNotStarted_ThrowsInvalidOperationException()
        {
            WorkDbContext context = GetInstances(nameof(ConfirmAsync_TransactionNotStarted_ThrowsInvalidOperationException));
            RemoveWorkScript script = new(context);

            await Assert.ThrowsAsync<InvalidOperationException>(script.ConfirmAsync);
        }

        [Fact]
        public async Task ConfirmAsync_TransactionStarted_SetsConfirmedToTrue()
        {
            WorkDbContext context = GetInstances(nameof(ConfirmAsync_TransactionStarted_SetsConfirmedToTrue));
            RemoveWorkScript script = new(context);
            var workToRemove = new WorkInfo { ID = 1, UserID = 1, Hash = "test_hash", Uploaded = DateTime.UtcNow };
            await script.RemoveAsync(workToRemove);

            await script.ConfirmAsync();
            Assert.True((bool)script.GetType().GetField("confirmed", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(script)!);
        }

        [Fact]
        public async Task ConfirmAsync_AlreadyConfirmed_ThrowsInvalidOperationException()
        {
            WorkDbContext context = GetInstances(nameof(ConfirmAsync_AlreadyConfirmed_ThrowsInvalidOperationException));
            RemoveWorkScript script = new(context);
            var workToRemove = new WorkInfo { ID = 1, UserID = 1, Hash = "test_hash", Uploaded = DateTime.UtcNow };
            
            await script.RemoveAsync(workToRemove);
            await script.ConfirmAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(() => script.ConfirmAsync());
        }

        [Fact]
        public async Task DisposeAsync_TransactionIsNull_DoesNothing()
        {
            WorkDbContext context = GetInstances(nameof(DisposeAsync_TransactionIsNull_DoesNothing));
            RemoveWorkScript script = new(context);

            await script.DisposeAsync();
        }

        [Fact]
        public async Task DisposeAsync_Confirmed_CommitsTransactionDisposesTransaction()
        {
            WorkDbContext context = GetInstances(nameof(DisposeAsync_Confirmed_CommitsTransactionDisposesTransaction));
            RemoveWorkScript script = new(context);
            var workToRemove = new WorkInfo { ID = 1, UserID = 1, Hash = "test_hash", Uploaded = DateTime.UtcNow };
            
            await script.RemoveAsync(workToRemove);
            await script.ConfirmAsync();

            await script.DisposeAsync();
        }

        [Fact]
        public async Task DisposeAsync_NotConfirmed_RollsBackTransactionDisposesTransaction()
        {
            WorkDbContext context = GetInstances(nameof(DisposeAsync_NotConfirmed_RollsBackTransactionDisposesTransaction));
            RemoveWorkScript script = new(context);
            var workToRemove = new WorkInfo { ID = 1, UserID = 1, Hash = "test_hash", Uploaded = DateTime.UtcNow };
            await script.RemoveAsync(workToRemove);

            await script.DisposeAsync();
        }
    }
}
