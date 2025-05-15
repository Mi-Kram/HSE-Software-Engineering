using FileAnalysisService.Api.Controllers;
using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Domain.Interfaces;
using FileAnalysisService.Domain.Models;
using Moq;

namespace FileAnalysisService.Tests.Api.Controllers
{
    public class ComparisonControllerTests
    {
        [Fact]
        public async Task CompareAsync_Tests()
        {
            Mock<IComparisonService> mockcomparisonService = new();
            mockcomparisonService.Setup(x => x.GetReportsAsync(It.IsAny<IEnumerable<ComparisonKey>>()))
                .ReturnsAsync([
                    new ComparisonReport() { Work1ID = 1, Work2ID = 2, Similarity = 0.4f },
                    new ComparisonReport() { Work1ID = 1, Work2ID = 3, Similarity = 0.6f },
                    new ComparisonReport() { Work1ID = 2, Work2ID = 3, Similarity = 0.7f }
                ]);

            ComparisonController controller = new(mockcomparisonService.Object);
            Assert.NotNull(await controller.CompareAsync([1,1,2], [2,3,3]));
        }

        [Fact]
        public async Task CompareAllAsync_Tests()
        {
            Mock<IComparisonService> mockcomparisonService = new();
            mockcomparisonService.Setup(x => x.GetReportsAsync(It.IsAny<IEnumerable<ComparisonKey>>()))
                .ReturnsAsync([
                    new ComparisonReport() { Work1ID = 1, Work2ID = 2, Similarity = 0.4f },
                    new ComparisonReport() { Work1ID = 1, Work2ID = 3, Similarity = 0.6f },
                    new ComparisonReport() { Work1ID = 2, Work2ID = 3, Similarity = 0.7f }
                ]);

            ComparisonController controller = new(mockcomparisonService.Object);
            Assert.NotNull(await controller.CompareAllAsync([1,2,3]));
        }
    }
}
