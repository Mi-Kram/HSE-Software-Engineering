using Gateway.Api.Controllers;
using Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Gateway.Tests.Api.Controllers
{
    public class AnalyseControllerTests
    {
        [Fact]
        public async Task GetAsync_Tests()
        {
            Mock<IAnalyseService> service = new();
            AnalyseController controller = new(service.Object);
            await controller.GetAsync([]);
            service.Verify(x => x.GetAnalyseReportsAsync(It.IsAny<HttpContext>()), Times.Once);
        }

        [Fact]
        public async Task GetWordsCloudAsync_Tests()
        {
            Mock<IAnalyseService> service = new();
            AnalyseController controller = new(service.Object);
            await controller.GetWordsCloudAsync(1, true);
            service.Verify(x => x.GetWordsCloudAsync(It.IsAny<HttpContext>(), It.IsAny<int>()), Times.Once);
        }
    }
}
