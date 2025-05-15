using FileAnalysisService.Domain.Models;
using FileAnalysisService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Minio.DataModel.Response;
using Minio.DataModel;
using Minio;
using Moq;
using System.Net;
using System.Text;
using Minio.DataModel.Args;

namespace FileAnalysisService.Tests.Infrastructure.Services
{
    public class AnalyseStorageServiceTests
    {
        [Fact]
        public async Task SaveAsync_SavesObjectSuccessfully()
        {
            Mock<IMinioClient> minioMock = new Mock<IMinioClient>();

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    [ApplicationVariables.WORDS_CLOUD_SRORAGE_BUCKET] = "TestBucket"
                }).Build();

            AnalyseStorageService storage = new AnalyseStorageService(minioMock.Object, config);

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

            await storage.SaveWordsCloudImageAsync(123, stream, "image/png");
            minioMock.VerifyAll();
        }

        [Fact]
        public async Task DeleteAsync_RemovesObjectSuccessfully()
        {
            Mock<IMinioClient> minioMock = new Mock<IMinioClient>();

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    [ApplicationVariables.WORDS_CLOUD_SRORAGE_BUCKET] = "TestBucket"
                }).Build();

            AnalyseStorageService storage = new AnalyseStorageService(minioMock.Object, config);

            minioMock.Setup(c => c.RemoveObjectAsync(It.IsAny<RemoveObjectArgs>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            minioMock.Setup(c => c.StatObjectAsync(It.IsAny<StatObjectArgs>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Not found"));

            await storage.DeleteWordsCloudImageAsync(7);
            minioMock.Verify(m => m.RemoveObjectAsync(It.IsAny<RemoveObjectArgs>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
