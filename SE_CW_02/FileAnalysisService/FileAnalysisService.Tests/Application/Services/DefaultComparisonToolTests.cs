using FileAnalysisService.Application.Services;
using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace FileAnalysisService.Tests.Application.Services
{
    public class DefaultComparisonToolTests
    {
        private readonly Mock<ILogger<DefaultComparisonTool>> loggerMock = new();

        [Fact]
        public async Task CompareAsyncAll_Tests()
        {
            DefaultComparisonTool tool = new(loggerMock.Object);

            using MemoryStream ms1 = new();
            using MemoryStream ms2 = new();
            using MemoryStream ms3 = new();
            
            using (StreamWriter sw = new(ms1, leaveOpen: true)) await sw.WriteAsync("work data");
            using (StreamWriter sw = new(ms3, leaveOpen: true)) await sw.WriteAsync("another work data");
            ms1.Seek(0, SeekOrigin.Begin);
            ms3.Seek(0, SeekOrigin.Begin);

            await ms1.CopyToAsync(ms2);
            ms1.Seek(0, SeekOrigin.Begin);
            ms2.Seek(0, SeekOrigin.Begin);

            Dictionary<int, Stream> dic = new() { [1] = ms1 };

            Assert.Empty(await tool.CompareAsync(dic));

            Mock<Stream> mockStream = new();
            mockStream.Setup(x => x.Length).Returns(ms1.Length);
            mockStream.Setup(x => x.Read(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>())).Throws<Exception>();

            dic[2] = mockStream.Object;
            Assert.Empty(await tool.CompareAsync(dic));

            dic[2] = ms2;

            var result = await tool.CompareAsync(dic);
            Assert.Single(result);
            ComparisonReport report = result.First();
            Assert.NotNull(report);

            Assert.NotEqual(report.Work1ID, report.Work2ID);
            Assert.True(report.Work1ID == 1 || report.Work1ID == 2);
            Assert.True(report.Work2ID == 1 || report.Work2ID == 2);
            Assert.Equal(1, report.Similarity);

            dic[3] = ms3;

            result = await tool.CompareAsync(dic);
            Assert.Equal(3, result.Count());
            Assert.Single(result.Where(x => (x.Work1ID == 1 && x.Work2ID == 2) || (x.Work1ID == 2 && x.Work2ID == 1)));
            Assert.Single(result.Where(x => (x.Work1ID == 1 && x.Work2ID == 3) || (x.Work1ID == 3 && x.Work2ID == 1)));
            Assert.Single(result.Where(x => (x.Work1ID == 2 && x.Work2ID == 3) || (x.Work1ID == 3 && x.Work2ID == 2)));

            Assert.Equal(1, result.Where(x => (x.Work1ID == 1 && x.Work2ID == 2) || (x.Work1ID == 2 && x.Work2ID == 1)).First().Similarity);
            Assert.Equal(0, result.Where(x => (x.Work1ID == 1 && x.Work2ID == 3) || (x.Work1ID == 3 && x.Work2ID == 1)).First().Similarity);
            Assert.Equal(0, result.Where(x => (x.Work1ID == 2 && x.Work2ID == 3) || (x.Work1ID == 3 && x.Work2ID == 2)).First().Similarity);
        }

        [Fact]
        public async Task CompareAsyncCouples_Tests()
        {
            DefaultComparisonTool tool = new(loggerMock.Object);

            using MemoryStream ms1 = new();
            using MemoryStream ms2 = new();
            using MemoryStream ms3 = new();

            using (StreamWriter sw = new(ms1, leaveOpen: true)) await sw.WriteAsync("work data");
            using (StreamWriter sw = new(ms3, leaveOpen: true)) await sw.WriteAsync("another work data");
            ms1.Seek(0, SeekOrigin.Begin);
            ms3.Seek(0, SeekOrigin.Begin);

            await ms1.CopyToAsync(ms2);
            ms1.Seek(0, SeekOrigin.Begin);
            ms2.Seek(0, SeekOrigin.Begin);

            Dictionary<int, Stream> dic = new() { [1] = ms1 };
            List<ComparisonKey> keys = [new(1, 2), new(1, 3), new(2, 3)];

            Assert.Empty(await tool.CompareAsync(dic, keys));

            Mock<Stream> mockStream = new();
            mockStream.Setup(x => x.Length).Returns(ms1.Length);
            mockStream.Setup(x => x.Read(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>())).Throws<Exception>();

            dic[2] = mockStream.Object;
            Assert.Empty(await tool.CompareAsync(dic, keys));

            dic[2] = ms2;
            dic[3] = ms3;

            List<ComparisonReport> result = [.. await tool.CompareAsync(dic, keys)];
            Assert.Equal(3, result.Count());
            Assert.Single(result.Where(x => (x.Work1ID == 1 && x.Work2ID == 2) || (x.Work1ID == 2 && x.Work2ID == 1)));
            Assert.Single(result.Where(x => (x.Work1ID == 1 && x.Work2ID == 3) || (x.Work1ID == 3 && x.Work2ID == 1)));
            Assert.Single(result.Where(x => (x.Work1ID == 2 && x.Work2ID == 3) || (x.Work1ID == 3 && x.Work2ID == 2)));

            Assert.Equal(1, result.Where(x => (x.Work1ID == 1 && x.Work2ID == 2) || (x.Work1ID == 2 && x.Work2ID == 1)).First().Similarity);
            Assert.Equal(0, result.Where(x => (x.Work1ID == 1 && x.Work2ID == 3) || (x.Work1ID == 3 && x.Work2ID == 1)).First().Similarity);
            Assert.Equal(0, result.Where(x => (x.Work1ID == 2 && x.Work2ID == 3) || (x.Work1ID == 3 && x.Work2ID == 2)).First().Similarity);
        }
    }
}
