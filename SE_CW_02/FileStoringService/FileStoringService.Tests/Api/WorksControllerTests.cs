using FileStoringService.Api.Controllers;
using FileStoringService.Api.DTO;
using FileStoringService.Domain.Entities;
using FileStoringService.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;

namespace FileStoringService.Tests.Api
{
    public class WorksControllerTests
    {
        [Fact]
        public async Task GetAllAsync_Tests()
        {
            Mock<IWorkService> mockService = new();
            WorkInfo[] works = [
                new() { ID = 1, UserID = 1, Hash = "Hash1", Uploaded = DateTime.UtcNow },
                new() { ID = 2, UserID = 2, Hash = "Hash2", Uploaded = DateTime.UtcNow },
                new() { ID = 3, UserID = 3, Hash = "Hash3", Uploaded = DateTime.UtcNow },
                ];

            mockService.Setup(x => x.GetAllWorksAsync())
                .Returns(Task.FromResult<IEnumerable<WorkInfo>>(works));

            WorksController worksController = new(mockService.Object);
            Assert.NotNull(await worksController.GetAsync());
        }

        [Theory]
        [InlineData("work data")]
        public async Task GetAsync_Tests(string data)
        {
            Mock<IWorkService> mockService = new();

            mockService.Setup(x => x.DownloadWorkAsync(It.IsAny<int>(), It.IsAny<Stream>()))
                //.Returns((int arg) => Task.FromResult(works.FirstOrDefault(x => x.ID == arg)));
                .Returns(async (int arg1, Stream arg2) =>
                {
                    using (StreamWriter sw = new(arg2, leaveOpen: true))
                        await sw.WriteAsync(data);
                    arg2.Seek(0, SeekOrigin.Begin);
                    return Task.CompletedTask;
                });

            WorksController worksController = new(mockService.Object);
            Assert.NotNull(await worksController.GetAsync(5));
        }

        [Fact]
        public async Task PostAsync_Tests()
        {
            Mock<IWorkService> mockService = new();
            WorksController worksController = new(mockService.Object);

            Assert.NotNull(await worksController.PostAsync(null!));

            await Assert.ThrowsAsync<ArgumentException>(async () => 
                await worksController.PostAsync(new UploadWorkDTO() { File = null! }));

            Mock<IFormFile> mockFile = new();
            mockFile.Setup(x => x.ContentType).Returns("");

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await worksController.PostAsync(new UploadWorkDTO() { File = mockFile.Object }));

            mockFile.Setup(x => x.ContentType).Returns("text/plain");
            Assert.NotNull(await worksController.PostAsync(new UploadWorkDTO() { File = mockFile.Object }));
        }

        [Fact]
        public async Task dd_Tests()
        {
            Mock<IWorkService> mockService = new();
            WorksController worksController = new(mockService.Object);
            await worksController.DeleteAsync(5);
        }
    }
}
