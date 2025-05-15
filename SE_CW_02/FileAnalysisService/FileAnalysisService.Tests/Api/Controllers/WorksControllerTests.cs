using FileAnalysisService.Api.Controllers;
using FileAnalysisService.Domain.Interfaces;
using Moq;

namespace FileAnalysisService.Tests.Api.Controllers
{
    public class WorksControllerTests
    {
        [Fact]
        public async Task DeleteAsync_Tests()
        {
            Mock<IWorkService> mockWorkService = new();
            WorksController controller = new(mockWorkService.Object);
            Assert.NotNull(controller.DeleteAsync(1));
            mockWorkService.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
