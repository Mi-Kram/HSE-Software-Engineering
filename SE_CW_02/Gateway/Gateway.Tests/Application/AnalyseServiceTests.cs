using Gateway.Application.Interfaces;
using Gateway.Application.UseCases;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Gateway.Tests.Application
{
    public class AnalyseServiceTests
    {
        [Fact]
        public async Task GetAnalyseReportsAsync_Tests()
        {
            Mock<IOuterAnalyseService> outerService = new();
            Mock<HttpContext> context = new();
            AnalyseService service = new(outerService.Object);
            await service.GetAnalyseReportsAsync(context.Object);
            outerService.Verify(x => x.GetAnalyseReportsAsync(It.IsAny<HttpContext>()), Times.Once);
        }

        [Fact]
        public async Task GetWordsCloudAsync_Tests()
        {
            Mock<IOuterAnalyseService> outerService = new();
            Mock<HttpContext> context = new();
            AnalyseService service = new(outerService.Object);
            await service.GetWordsCloudAsync(context.Object, 1);
            outerService.Verify(x => x.GetWordsCloudAsync(It.IsAny<HttpContext>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetCoupleComparisonReportsAsync_Tests()
        {
            Mock<IOuterAnalyseService> outerService = new();
            Mock<HttpContext> context = new();
            AnalyseService service = new(outerService.Object);
            await service.GetCoupleComparisonReportsAsync(context.Object);
            outerService.Verify(x => x.GetCoupleComparisonReportsAsync(It.IsAny<HttpContext>()), Times.Once);
        }

        [Fact]
        public async Task GetAllComparisonReportsAsync_Tests()
        {
            Mock<IOuterAnalyseService> outerService = new();
            Mock<HttpContext> context = new();
            AnalyseService service = new(outerService.Object);
            await service.GetAllComparisonReportsAsync(context.Object);
            outerService.Verify(x => x.GetAllComparisonReportsAsync(It.IsAny<HttpContext>()), Times.Once);
        }
    }
}
