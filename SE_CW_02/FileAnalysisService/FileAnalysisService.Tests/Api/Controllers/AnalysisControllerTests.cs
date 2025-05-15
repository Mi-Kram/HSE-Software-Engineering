using FileAnalysisService.Api.Controllers;
using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Domain.Interfaces;
using Moq;

namespace FileAnalysisService.Tests.Api.Controllers
{
    public class AnalysisControllerTests
    {
        [Fact]
        public async Task GetAsync_Tests()
        {
            Mock<IAnalyseService> mockService = new();
            AnalyzeReport[] reports = [
                new() { WorkID = 1, Paragraphs = 1, Words = 2, Numbers = 3, Symbols = 4 },
                new() { WorkID = 2, Paragraphs = 2, Words = 4, Numbers = 6, Symbols = 8 },
                new() { WorkID = 3, Paragraphs = 3, Words = 6, Numbers = 9, Symbols = 16 },
                ];

            mockService.Setup(x => x.GetReportAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => reports.FirstOrDefault(x => x.WorkID == id));

            AnalysisController worksController = new(mockService.Object);
            Assert.NotNull(await worksController.GetAsync([1, 2, 4]));
        }

        [Fact]
        public async Task GetWordsCloudAsync_Tests()
        {
            Mock<IAnalyseService> mockService = new();
            mockService.Setup(x => x.GetWordsCloudAsync(It.IsAny<int>(), It.IsAny<Stream>(), It.IsAny<bool>()))
                .Callback<int, Stream, bool>((id, image, r) =>
                {
                    using (StreamWriter sw = new(image, leaveOpen: true)) sw.Write("words cloud");
                    image.Seek(0, SeekOrigin.Begin);
                })
                .ReturnsAsync("image/png");

            AnalysisController worksController = new(mockService.Object);
            Assert.NotNull(await worksController.GetWordsCloudAsync(1, true));
        }
    }
}
