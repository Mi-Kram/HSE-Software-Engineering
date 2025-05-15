using Gateway.Api.Controllers;
using Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Gateway.Tests.Api.Controllers
{
    public class ComparisonControllerTests
    {
        [Fact]
        public async Task GetAsync_Tests()
        {
            Mock<IAnalyseService> service = new();
            ComparisonController controller = new(service.Object);
            await controller.GetAsync([], []);
            service.Verify(x => x.GetCoupleComparisonReportsAsync(It.IsAny<HttpContext>()), Times.Once);
        }

        [Fact]
        public async Task CompareAllAsync_Tests()
        {
            Mock<IAnalyseService> service = new();
            ComparisonController controller = new(service.Object);
            await controller.CompareAllAsync([]);
            service.Verify(x => x.GetAllComparisonReportsAsync(It.IsAny<HttpContext>()), Times.Once);
        }

    }
}
