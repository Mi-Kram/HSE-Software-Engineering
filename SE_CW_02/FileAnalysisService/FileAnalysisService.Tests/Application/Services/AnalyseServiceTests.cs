using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Application.Services;
using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace FileAnalysisService.Tests.Application.Services
{
    public class AnalyseServiceTests
    {
        private readonly Mock<IAnalyseReportRepository> reportRepoMock = new();
        private readonly Mock<IWorkStorageService> workStorageMock = new();
        private readonly Mock<IAnalyseTool> analyseToolMock = new();
        private readonly Mock<IAnalyseStorageService> analyseStorageMock = new();
        private readonly Mock<IWordsCloudService> wordsCloudMock = new();
        private readonly Mock<ILogger<AnalyseService>> loggerMock = new();

        private AnalyseService CreateService() =>
            new(reportRepoMock.Object, workStorageMock.Object, analyseToolMock.Object, analyseStorageMock.Object, wordsCloudMock.Object, loggerMock.Object);

        [Fact]
        public async Task GetReportAsync_ReturnsFromRepository_IfExists()
        {
            // Arrange
            var expectedReport = new AnalyzeReport { WorkID = 1 };
            reportRepoMock.Setup(r => r.GetAsync(1)).ReturnsAsync(expectedReport);
            var service = CreateService();

            // Act
            var result = await service.GetReportAsync(1);

            // Assert
            Assert.Equal(expectedReport, result);
            workStorageMock.Verify(w => w.TryGetWorkAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetReportAsync_AnalyzesAndSaves_IfNotExists()
        {
            // Arrange
            var stream = new MemoryStream();
            var generatedReport = new AnalyzeReport();

            reportRepoMock.Setup(r => r.GetAsync(1)).ReturnsAsync((AnalyzeReport?)null);
            workStorageMock.Setup(w => w.TryGetWorkAsync(1)).ReturnsAsync(stream);
            analyseToolMock.Setup(a => a.AnalyzeAsync(stream)).ReturnsAsync(generatedReport);
            var service = CreateService();

            // Act
            var result = await service.GetReportAsync(1);

            // Assert
            Assert.Equal(generatedReport, result);
            reportRepoMock.Verify(r => r.AddAsync(It.Is<AnalyzeReport>(r => r.WorkID == 1)), Times.Once);
        }

        [Fact]
        public async Task GetReportAsync_ReturnsNull_IfWorkIsMissing()
        {
            reportRepoMock.Setup(r => r.GetAsync(1)).ReturnsAsync((AnalyzeReport?)null);
            workStorageMock.Setup(w => w.TryGetWorkAsync(1)).ReturnsAsync((Stream?)null);
            var service = CreateService();

            var result = await service.GetReportAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetWordsCloudAsync_ReturnsFromStorage_IfExists_AndNotRegenerate()
        {
            var stream = new MemoryStream();
            analyseStorageMock.Setup(a => a.GetWordsCloudImageAsync(1, stream)).ReturnsAsync("image/png");

            var service = CreateService();

            var result = await service.GetWordsCloudAsync(1, stream, regenerate: false);

            Assert.Equal("image/png", result);
            workStorageMock.Verify(w => w.GetWorkAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetWordsCloudAsync_Regenerates_IfNotExists()
        {
            var imageStream = new MemoryStream();
            var workStream = new MemoryStream();

            analyseStorageMock.Setup(a => a.GetWordsCloudImageAsync(1, imageStream)).ThrowsAsync(new Exception("Not found"));
            workStorageMock.Setup(w => w.GetWorkAsync(1)).ReturnsAsync(workStream);
            wordsCloudMock.Setup(w => w.GenerateAsync(workStream, imageStream)).ReturnsAsync("image/svg+xml");

            var service = CreateService();

            var result = await service.GetWordsCloudAsync(1, imageStream, regenerate: false);

            Assert.Equal("image/svg+xml", result);
            analyseStorageMock.Verify(a => a.SaveWordsCloudImageAsync(1, imageStream, "image/svg+xml"), Times.Once);
        }

        [Fact]
        public async Task GetWordsCloudAsync_LogsError_OnSaveFailure()
        {
            var imageStream = new MemoryStream();
            var workStream = new MemoryStream();

            analyseStorageMock.Setup(a => a.GetWordsCloudImageAsync(1, imageStream)).ThrowsAsync(new Exception("not found"));
            workStorageMock.Setup(w => w.GetWorkAsync(1)).ReturnsAsync(workStream);
            wordsCloudMock.Setup(w => w.GenerateAsync(workStream, imageStream)).ReturnsAsync("image/png");
            analyseStorageMock.Setup(a => a.SaveWordsCloudImageAsync(1, imageStream, "image/png")).ThrowsAsync(new Exception("save failed"));

            var service = CreateService();

            var result = await service.GetWordsCloudAsync(1, imageStream, regenerate: false);

            Assert.Equal("image/png", result);
        }
    }
}
