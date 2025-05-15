using Gateway.Application.Interfaces;
using Gateway.Application.UseCases;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Gateway.Tests.Application
{
    public class StorageServiceTests
    {
        [Fact]
        public async Task GetAllWorksAsync_Tests()
        {
            Mock<IOuterStorageService> outerService = new();
            Mock<HttpContext> context = new();
            StorageService service = new(outerService.Object);
            await service.GetAllWorksAsync(context.Object);
            outerService.Verify(x => x.GetAllWorksAsync(It.IsAny<HttpContext>()), Times.Once);
        }

        [Fact]
        public async Task GetWorkAsync_Tests()
        {
            Mock<IOuterStorageService> outerService = new();
            Mock<HttpContext> context = new();
            StorageService service = new(outerService.Object);
            await service.GetWorkAsync(context.Object, 1);
            outerService.Verify(x => x.GetWorkAsync(It.IsAny<HttpContext>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task UploadWorkAsync_Tests()
        {
            Mock<IOuterStorageService> outerService = new();
            Mock<HttpContext> context = new();
            StorageService service = new(outerService.Object);
            await service.UploadWorkAsync(context.Object);
            outerService.Verify(x => x.UploadWorkAsync(It.IsAny<HttpContext>()), Times.Once);
        }
    }
}
