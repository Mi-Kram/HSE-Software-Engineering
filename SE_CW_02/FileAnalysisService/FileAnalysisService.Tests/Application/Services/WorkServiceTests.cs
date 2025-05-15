using FileAnalysisService.Application.Services;
using FileAnalysisService.Domain.Interfaces;
using Moq;

namespace FileAnalysisService.Tests.Application.Services
{
    public class WorkServiceTests
    {
        private readonly Mock<IWorkRepository> mockWorkRepository = new();
        private readonly Mock<IAnalyseStorageService> mockAnalyseStorageService = new();

        [Fact]
        public async Task DeleteAsync_Tests()
        {
            WorkService workService = new(mockWorkRepository.Object, mockAnalyseStorageService.Object);
            await workService.DeleteAsync(1);
            mockAnalyseStorageService.Verify(x => x.DeleteWordsCloudImageAsync(1), Times.Once);
            mockWorkRepository.Verify(x => x.DeleteAsync(1), Times.Once);
        }
    }
}
