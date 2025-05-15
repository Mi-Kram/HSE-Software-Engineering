using FileStoringService.Domain.Models;
using FileStoringService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.DataModel.Response;
using Moq;
using System.Net;
using System.Text;

namespace FileStoringService.Tests.Infrastructure.Services
{
    public class WorkStorageServiceTests
    {
        [Fact]
        public async Task SaveAsync_SavesObjectSuccessfully()
        {
            Mock<IMinioClient> minioMock = new Mock<IMinioClient>();

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    [ApplicationVariables.SRORAGE_BUCKET] = "TestBucket"
                }).Build();

            WorkStorageService storage = new WorkStorageService(minioMock.Object, config);

            var stream = new MemoryStream(Encoding.UTF8.GetBytes("some work"));

            minioMock.Setup(c => c.BucketExistsAsync(It.IsAny<BucketExistsArgs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            minioMock.Setup(c => c.PutObjectAsync(It.IsAny<PutObjectArgs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PutObjectResponse(HttpStatusCode.OK, "some work", new Dictionary<string, string>(), stream.Length, "123"));

            minioMock.Setup(c => c.StatObjectAsync(It.IsAny<StatObjectArgs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ObjectStat.FromResponseHeaders("123", new Dictionary<string, string>()
                {
                    ["Content-Length"] = stream.Length.ToString()
                }));

            await storage.SaveAsync(stream, 123);

            minioMock.VerifyAll();
        }

        [Fact]
        public async Task DeleteAsync_RemovesObjectSuccessfully()
        {
            Mock<IMinioClient> minioMock = new Mock<IMinioClient>();

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    [ApplicationVariables.SRORAGE_BUCKET] = "TestBucket"
                }).Build();

            WorkStorageService storage = new WorkStorageService(minioMock.Object, config);

            minioMock.Setup(c => c.RemoveObjectAsync(It.IsAny<RemoveObjectArgs>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            minioMock.Setup(c => c.StatObjectAsync(It.IsAny<StatObjectArgs>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Not found"));

            await storage.DeleteAsync(7);
            minioMock.Verify(m => m.RemoveObjectAsync(It.IsAny<RemoveObjectArgs>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
