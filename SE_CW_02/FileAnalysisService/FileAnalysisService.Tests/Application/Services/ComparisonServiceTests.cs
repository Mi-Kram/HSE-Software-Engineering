using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Application.Services;
using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Domain.Interfaces;
using FileAnalysisService.Domain.Models;
using Moq;

namespace FileAnalysisService.Tests.Application.Services
{
    public class ComparisonServiceTests
    {
        private readonly Mock<IComparisonReportRepository> repoMock = new();
        private readonly Mock<IWorkStorageService> storageMock = new();
        private readonly Mock<IComparisonTool> toolMock = new();

        private ComparisonService CreateService() =>
            new(repoMock.Object, storageMock.Object, toolMock.Object);

        [Fact]
        public async Task GetReportsAsync_SkipsAlreadyComparedPairs()
        {
            var keys = new[]
            {
                new ComparisonKey(1, 2),
                new ComparisonKey(2, 3),
            };

            var existingReport = new ComparisonReport { Work1ID = 1, Work2ID = 2 };
            repoMock.Setup(r => r.GetAsync(It.Is<ComparisonKey>(k => k.Work1ID == 1 && k.Work2ID == 2)))
                    .ReturnsAsync(existingReport);
            repoMock.Setup(r => r.GetAsync(It.Is<ComparisonKey>(k => k.Work1ID == 2 && k.Work2ID == 3)))
                    .ReturnsAsync((ComparisonReport?)null);

            var stream = new MemoryStream();
            storageMock.Setup(s => s.TryGetWorkAsync(It.IsAny<int>())).ReturnsAsync(stream);

            var generatedReport = new ComparisonReport { Work1ID = 2, Work2ID = 3 };
            toolMock.Setup(t => t.CompareAsync(It.IsAny<Dictionary<int, Stream>>(), It.IsAny<IEnumerable<ComparisonKey>>()))
                    .ReturnsAsync([generatedReport]);

            var service = CreateService();

            var result = (await service.GetReportsAsync(keys)).ToList();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.Work1ID == 1 && r.Work2ID == 2);
            Assert.Contains(result, r => r.Work1ID == 2 && r.Work2ID == 3);
        }

        [Fact]
        public async Task GetReportsAsync_AllVsAll_GeneratesCorrectPairs()
        {
            var ids = new[] { 1, 2 };
            var expectedKey = new ComparisonKey(1, 2);

            storageMock.Setup(s => s.TryGetWorkAsync(It.IsAny<int>()))
                       .ReturnsAsync(new MemoryStream());

            toolMock.Setup(t => t.CompareAsync(It.IsAny<Dictionary<int, Stream>>(), It.Is<IEnumerable<ComparisonKey>>(ks =>
                ks.Any(k => k.Work1ID == 1 && k.Work2ID == 2))))
                .ReturnsAsync(new[] { new ComparisonReport { Work1ID = 1, Work2ID = 2 } });

            var service = CreateService();

            var result = await service.GetReportsAsync(ids);

            Assert.Single(result);
            Assert.Contains(result, r => r.Work1ID == 1 && r.Work2ID == 2);
        }

        [Fact]
        public async Task GetReportsAsync_DisposesStreams_AfterComparison()
        {
            var keys = new[] { new ComparisonKey(1, 2) };

            var mockStream1 = new Mock<MemoryStream>();
            var mockStream2 = new Mock<MemoryStream>();
            storageMock.Setup(s => s.TryGetWorkAsync(1)).ReturnsAsync(mockStream1.Object);
            storageMock.Setup(s => s.TryGetWorkAsync(2)).ReturnsAsync(mockStream2.Object);

            toolMock.Setup(t => t.CompareAsync(It.IsAny<Dictionary<int, Stream>>(), It.IsAny<IEnumerable<ComparisonKey>>()))
                    .ReturnsAsync(new[] { new ComparisonReport { Work1ID = 1, Work2ID = 2 } });

            var service = CreateService();
            await service.GetReportsAsync(keys);
        }

        [Fact]
        public async Task GetReportsAsync_IgnoresSelfComparison()
        {
            var keys = new[]
            {
                new ComparisonKey(1, 1),
                new ComparisonKey(1, 2)
            };

            var stream = new MemoryStream();
            storageMock.Setup(s => s.TryGetWorkAsync(It.IsAny<int>())).ReturnsAsync(stream);

            var generatedReport = new ComparisonReport { Work1ID = 1, Work2ID = 2 };
            toolMock.Setup(t => t.CompareAsync(It.IsAny<Dictionary<int, Stream>>(), It.IsAny<IEnumerable<ComparisonKey>>()))
                    .ReturnsAsync([generatedReport]);

            var service = CreateService();
            var result = (await service.GetReportsAsync(keys)).ToList();

            Assert.Single(result);
            Assert.Equal(1, result[0].Work1ID);
            Assert.Equal(2, result[0].Work2ID);
        }
    }
}
