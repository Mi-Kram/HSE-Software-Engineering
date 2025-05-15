using FileStoringService.Domain.Entities;
using FileStoringService.Infrastructure.Persistence.Scripts;
using FileStoringService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FileStoringService.Tests.Infrastructure.Persistence
{
    public class WorkRepositoryTests
    {
        private static (WorkDbContext context, WorkRepository repository) GetInstances(string database)
        {
            DbContextOptionsBuilder<WorkDbContext> builder = new DbContextOptionsBuilder<WorkDbContext>()
                .UseInMemoryDatabase(databaseName: database);

            WorkDbContext context = new(builder.Options);
            WorkRepository repository = new(context);

            return (context, repository);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllWorks()
        {
            var (context, repository) = GetInstances(nameof(GetAllAsync_ReturnsAllWorks));

            context.Works.AddRange(
                new WorkInfo { ID = 1, UserID = 10, Hash = "hash1", Uploaded = DateTime.UtcNow.AddDays(-1) },
                new WorkInfo { ID = 2, UserID = 20, Hash = "hash2", Uploaded = DateTime.UtcNow.AddDays(-2) },
                new WorkInfo { ID = 3, UserID = 10, Hash = "hash1", Uploaded = DateTime.UtcNow.AddDays(-3) }
            );
            context.SaveChanges();

            var result = await repository.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAllByHashAsync_ReturnsWorksWithGivenHash()
        {
            var (context, repository) = GetInstances(nameof(GetAllByHashAsync_ReturnsWorksWithGivenHash));

            context.Works.AddRange(
                new WorkInfo { ID = 1, UserID = 10, Hash = "hash1", Uploaded = DateTime.UtcNow.AddDays(-1) },
                new WorkInfo { ID = 2, UserID = 20, Hash = "hash2", Uploaded = DateTime.UtcNow.AddDays(-2) },
                new WorkInfo { ID = 3, UserID = 10, Hash = "hash1", Uploaded = DateTime.UtcNow.AddDays(-3) }
            );
            context.SaveChanges();

            var result = await repository.GetAllByHashAsync("hash1");

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllByUserIDAsync_ReturnsWorksWithGivenUserID()
        {
            var (context, repository) = GetInstances(nameof(GetAllByUserIDAsync_ReturnsWorksWithGivenUserID));

            context.Works.AddRange(
                new WorkInfo { ID = 1, UserID = 10, Hash = "hash1", Uploaded = DateTime.UtcNow.AddDays(-1) },
                new WorkInfo { ID = 2, UserID = 20, Hash = "hash2", Uploaded = DateTime.UtcNow.AddDays(-2) },
                new WorkInfo { ID = 3, UserID = 10, Hash = "hash1", Uploaded = DateTime.UtcNow.AddDays(-3) }
            );
            context.SaveChanges();

            var result = await repository.GetAllByUserIDAsync(10);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(10, result.First().UserID);
        }

        [Fact]
        public async Task GetAsync_ReturnsWorkWithGivenId_IfExists()
        {
            var (context, repository) = GetInstances(nameof(GetAsync_ReturnsWorkWithGivenId_IfExists));

            context.Works.AddRange(
                new WorkInfo { ID = 1, UserID = 10, Hash = "hash1", Uploaded = DateTime.UtcNow.AddDays(-1) },
                new WorkInfo { ID = 2, UserID = 20, Hash = "hash2", Uploaded = DateTime.UtcNow.AddDays(-2) },
                new WorkInfo { ID = 3, UserID = 10, Hash = "hash1", Uploaded = DateTime.UtcNow.AddDays(-3) }
            );
            context.SaveChanges();

            var result = await repository.GetAsync(2);

            Assert.NotNull(result);
            Assert.Equal(2, result.ID);
        }

        [Fact]
        public async Task GetAsync_ReturnsNull_IfWorkNotFound()
        {
            var (_, repository) = GetInstances(nameof(GetAsync_ReturnsNull_IfWorkNotFound));

            var result = await repository.GetAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAddScriptAsync_ReturnsAddWorkScriptInstance()
        {
            var (_, repository) = GetInstances(nameof(GetAddScriptAsync_ReturnsAddWorkScriptInstance));

            var result = await repository.GetAddScriptAsync();

            Assert.NotNull(result);
            Assert.IsType<AddWorkScript>(result);
        }

        [Fact]
        public async Task GetRemoveScriptAsync_ReturnsRemoveWorkScriptInstance()
        {
            var (_, repository) = GetInstances(nameof(GetRemoveScriptAsync_ReturnsRemoveWorkScriptInstance));

            var result = await repository.GetRemoveScriptAsync();

            Assert.NotNull(result);
            Assert.IsType<RemoveWorkScript>(result);
        }
    }
}
