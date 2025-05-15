using Gateway.Api.Controllers;
using Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Gateway.Tests.Api.Controllers
{
    public class WorksControllerTests
    {
        [Fact]
        public async Task GetAllAsync_Tests()
        {
            Mock<IStorageService> storage = new();
            Mock<IWorkDeletionService> deletion = new();
            WorksController controller = new(storage.Object, deletion.Object);

            await controller.GetAsync();
            storage.Verify(x => x.GetAllWorksAsync(It.IsAny<HttpContext>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_Tests()
        {
            Mock<IStorageService> storage = new();
            Mock<IWorkDeletionService> deletion = new();
            WorksController controller = new(storage.Object, deletion.Object);

            await controller.GetAsync(1);
            storage.Verify(x => x.GetWorkAsync(It.IsAny<HttpContext>(), It.IsAny<int>()), Times.Once);
        }


        [Fact]
        public async Task PostAsync_Tests()
        {
            Mock<IStorageService> storage = new();
            Mock<IWorkDeletionService> deletion = new();
            WorksController controller = new(storage.Object, deletion.Object);

            await controller.PostAsync();
            storage.Verify(x => x.UploadWorkAsync(It.IsAny<HttpContext>()), Times.Once);
        }


        [Fact]
        public async Task DeleteAsync_Tests()
        {
            Mock<IStorageService> storage = new();
            Mock<IWorkDeletionService> deletion = new();
            WorksController controller = new(storage.Object, deletion.Object);

            await controller.DeleteAsync(1);
            deletion.Verify(x => x.DeleteWorkAsync(It.IsAny<HttpContext>(), It.IsAny<int>()), Times.Once);
        }
    }
}
