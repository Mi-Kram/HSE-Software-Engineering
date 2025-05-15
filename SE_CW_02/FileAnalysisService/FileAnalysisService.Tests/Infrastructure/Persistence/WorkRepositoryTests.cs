using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Moq;
using FileAnalysisService.Infrastructure.Persistence.Context;
using FileAnalysisService.Domain.Interfaces;
using FileAnalysisService.Infrastructure.Persistence;

namespace FileAnalysisService.Tests.Infrastructure.Persistence
{
    public class WorkRepositoryTests
    {
        private static ApplicationDbContext GetInstances(string database)
        {
            DbContextOptionsBuilder<ApplicationDbContext> builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: database);

            Mock<IDbContextTransaction> mockTransaction = new();

            ApplicationDbContext applicationDbContext = new(builder.Options);
            var mockDatabase = new Mock<DatabaseFacade>(applicationDbContext);

            mockDatabase.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            mockDatabase.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(mockTransaction.Object));

            Mock<ApplicationDbContext> mockContext = new(builder.Options);
            mockContext.Setup(x => x.AnalyzeReports).Returns(applicationDbContext.AnalyzeReports);
            mockContext.Setup(x => x.ComparisonReports).Returns(applicationDbContext.ComparisonReports);
            mockContext.Setup(x => x.Database).Returns(mockDatabase.Object);

            return mockContext.Object;
        }

        [Fact]
        public async Task DeleteAsync_Tests()
        {
            ApplicationDbContext context = GetInstances(nameof(DeleteAsync_Tests));
            Mock<IAnalyseReportRepository> mockAnalyseReportRepository = new();
            Mock<IComparisonReportRepository> mockComparisonReportRepository = new();

            WorkRepository repository = new(context, mockAnalyseReportRepository.Object, mockComparisonReportRepository.Object);
            await repository.DeleteAsync(1);

            mockAnalyseReportRepository.Verify(x => x.DeleteAsync(1), Times.Once);
            mockComparisonReportRepository.Verify(x => x.DeleteAsync(1), Times.Once);
        }
    }
}
