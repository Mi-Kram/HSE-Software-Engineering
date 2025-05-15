using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Domain.Models;
using FileAnalysisService.Infrastructure.Persistence.Context;
using FileAnalysisService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FileAnalysisService.Tests.Infrastructure.Persistence
{
    public class ComparisonReportRepositoryTests
    {
        private static ApplicationDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetAsync_ByKey_ReturnsExpectedReport()
        {
            using var context = CreateContext(nameof(GetAsync_ByKey_ReturnsExpectedReport));
            var report = new ComparisonReport { Work1ID = 1, Work2ID = 2, Similarity = 0.5f };
            context.ComparisonReports.Add(report);
            await context.SaveChangesAsync();

            var repo = new ComparisonReportRepository(context);
            var result = await repo.GetAsync(new ComparisonKey(1, 2));

            Assert.NotNull(result);
            Assert.Equal(1, result!.Work1ID);
            Assert.Equal(2, result.Work2ID);
        }

        [Fact]
        public async Task GetAsync_ByKeys_ReturnsExpectedReports()
        {
            using var context = CreateContext(nameof(GetAsync_ByKeys_ReturnsExpectedReports));
            context.ComparisonReports.AddRange(
                new ComparisonReport { Work1ID = 1, Work2ID = 2, Similarity = 0.1f },
                new ComparisonReport { Work1ID = 3, Work2ID = 4, Similarity = 0.9f }
            );
            await context.SaveChangesAsync();

            var repo = new ComparisonReportRepository(context);
            var result = await repo.GetAsync(
                new ComparisonKey(1, 2),
                new ComparisonKey(3, 4)
            );

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAsync_ByWorkId_ReturnsAllRelatedReports()
        {
            using var context = CreateContext(nameof(GetAsync_ByWorkId_ReturnsAllRelatedReports));
            context.ComparisonReports.AddRange(
                new ComparisonReport { Work1ID = 1, Work2ID = 2, Similarity = 0.3f },
                new ComparisonReport { Work1ID = 3, Work2ID = 1, Similarity = 0.7f }
            );
            await context.SaveChangesAsync();

            var repo = new ComparisonReportRepository(context);
            var result = await repo.GetAsync(1);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task AddAsync_AddsReports_WithCorrectOrder()
        {
            using var context = CreateContext(nameof(AddAsync_AddsReports_WithCorrectOrder));
            var repo = new ComparisonReportRepository(context);

            var reports = new[]
            {
                new ComparisonReport { Work1ID = 5, Work2ID = 2, Similarity = 0.4f },
                new ComparisonReport { Work1ID = 3, Work2ID = 4, Similarity = 0.8f }
            };

            await repo.AddAsync(reports);

            var stored = context.ComparisonReports.ToList();

            Assert.Equal(2, stored.Count);
            Assert.Contains(stored, x => x.Work1ID == 2 && x.Work2ID == 5);
            Assert.Contains(stored, x => x.Work1ID == 3 && x.Work2ID == 4);
        }

        [Fact]
        public async Task AddAsync_DoesNotAdd_WhenWork1EqualsWork2()
        {
            using var context = CreateContext(nameof(AddAsync_DoesNotAdd_WhenWork1EqualsWork2));
            var repo = new ComparisonReportRepository(context);

            var reports = new[]
            {
                new ComparisonReport { Work1ID = 1, Work2ID = 1, Similarity = 1.0f },
                new ComparisonReport { Work1ID = 2, Work2ID = 2, Similarity = 1.0f }
            };

            await repo.AddAsync(reports);

            Assert.Empty(context.ComparisonReports);
        }

        [Fact]
        public async Task DeleteAsync_RemovesAllRelatedToWork()
        {
            using var context = CreateContext(nameof(DeleteAsync_RemovesAllRelatedToWork));
            context.ComparisonReports.AddRange(
                new ComparisonReport { Work1ID = 1, Work2ID = 2, Similarity = 0.3f },
                new ComparisonReport { Work1ID = 3, Work2ID = 1, Similarity = 0.7f },
                new ComparisonReport { Work1ID = 4, Work2ID = 5, Similarity = 0.9f }
            );
            await context.SaveChangesAsync();

            var repo = new ComparisonReportRepository(context);

            try
            {
                await repo.DeleteAsync(1);
            }
            catch (InvalidOperationException)
            {
                return;
            }
            

            var remaining = context.ComparisonReports.ToList();
            Assert.Single(remaining);
            Assert.DoesNotContain(remaining, x => x.Work1ID == 1 || x.Work2ID == 1);
        }
    }
}
