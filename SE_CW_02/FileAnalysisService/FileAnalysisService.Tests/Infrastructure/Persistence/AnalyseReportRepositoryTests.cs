using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Infrastructure.Persistence.Context;
using FileAnalysisService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FileAnalysisService.Tests.Infrastructure.Persistence
{
    public class AnalyseReportRepositoryTests
    {
        private static ApplicationDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetAsync_ById_ReturnsExpectedReport()
        {
            using var context = CreateContext(nameof(GetAsync_ById_ReturnsExpectedReport));
            var report = new AnalyzeReport { WorkID = 1, Paragraphs = 10, Words = 100, Numbers = 5, Symbols = 200 };
            context.AnalyzeReports.Add(report);
            await context.SaveChangesAsync();

            var repo = new AnalyseReportRepository(context);
            var result = await repo.GetAsync(1);

            Assert.NotNull(result);
            Assert.Equal(report.WorkID, result!.WorkID);
        }

        [Fact]
        public async Task GetAsync_ByIds_ReturnsExpectedReports()
        {
            using var context = CreateContext(nameof(GetAsync_ByIds_ReturnsExpectedReports));
            context.AnalyzeReports.AddRange(
                new AnalyzeReport { WorkID = 1, Paragraphs = 1, Words = 10, Numbers = 0, Symbols = 50 },
                new AnalyzeReport { WorkID = 2, Paragraphs = 2, Words = 20, Numbers = 1, Symbols = 60 }
            );
            await context.SaveChangesAsync();

            var repo = new AnalyseReportRepository(context);
            var result = await repo.GetAsync(1, 2);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAsync_ByEmptyArray_ReturnsEmpty()
        {
            using var context = CreateContext(nameof(GetAsync_ByEmptyArray_ReturnsEmpty));
            var repo = new AnalyseReportRepository(context);
            var result = await repo.GetAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task AddAsync_AddsReports()
        {
            using var context = CreateContext(nameof(AddAsync_AddsReports));
            var repo = new AnalyseReportRepository(context);

            var reports = new[]
            {
                new AnalyzeReport { WorkID = 1, Paragraphs = 1, Words = 10, Numbers = 0, Symbols = 50 },
                new AnalyzeReport { WorkID = 2, Paragraphs = 2, Words = 20, Numbers = 1, Symbols = 60 }
            };

            await repo.AddAsync(reports);

            Assert.Equal(2, context.AnalyzeReports.Count());
        }

        [Fact]
        public async Task AddAsync_IgnoresNulls()
        {
            using var context = CreateContext(nameof(AddAsync_IgnoresNulls));
            var repo = new AnalyseReportRepository(context);

            var reports = new AnalyzeReport?[]
            {
                new AnalyzeReport { WorkID = 1, Paragraphs = 1, Words = 10, Numbers = 0, Symbols = 50 },
                null
            };

            await repo.AddAsync(reports!);

            Assert.Single(context.AnalyzeReports);
        }

        [Fact]
        public async Task DeleteAsync_RemovesReportIfExists()
        {
            using var context = CreateContext(nameof(DeleteAsync_RemovesReportIfExists));
            var report = new AnalyzeReport { WorkID = 1, Paragraphs = 1, Words = 10, Numbers = 0, Symbols = 50 };
            context.AnalyzeReports.Add(report);
            await context.SaveChangesAsync();

            var repo = new AnalyseReportRepository(context);
            await repo.DeleteAsync(1);

            Assert.Empty(context.AnalyzeReports);
        }

        [Fact]
        public async Task DeleteAsync_DoesNothingIfReportNotFound()
        {
            using var context = CreateContext(nameof(DeleteAsync_DoesNothingIfReportNotFound));
            var repo = new AnalyseReportRepository(context);

            await repo.DeleteAsync(99);

            Assert.Empty(context.AnalyzeReports);
        }
    }
}
