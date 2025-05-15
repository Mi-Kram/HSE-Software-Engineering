using FileAnalysisService.Application.Services;
using FileAnalysisService.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace FileAnalysisService.Tests.Application.Services
{
    public class DefaultAnalyseToolTests
    {
        private readonly Mock<ILogger<DefaultAnalyseTool>> loggerMock = new();

        [Fact]
        public async Task AnalyzeAsync_Tests()
        {
            DefaultAnalyseTool tool = new(loggerMock.Object);
            
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await tool.AnalyzeAsync(null!));

            using MemoryStream ms = new();
            using (StreamWriter sw = new(ms, leaveOpen: true)) 
                await sw.WriteAsync("Hello World!\n1 2 3\nTest  \n\n");
            ms.Seek(0, SeekOrigin.Begin);

            AnalyzeReport? report = await tool.AnalyzeAsync(ms);
            Assert.NotNull(report);

            Assert.Equal(3, report.Paragraphs);
            Assert.Equal(6, report.Words);
            Assert.Equal(3, report.Numbers);
            Assert.Equal(18, report.Symbols);

            Mock<Stream> mockStream = new();
            mockStream.Setup(x => x.Length).Returns(ms.Length);
            mockStream.Setup(x => x.Read(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>())).Throws<Exception>();

            Assert.Null(await tool.AnalyzeAsync(mockStream.Object));
        }
    }
}
