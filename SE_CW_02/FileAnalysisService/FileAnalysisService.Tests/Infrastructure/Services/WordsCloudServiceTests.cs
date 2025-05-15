using FileAnalysisService.Infrastructure.Services;

namespace FileAnalysisService.Tests.Infrastructure.Services
{
    public class WordsCloudServiceTests
    {
        [Fact]
        public async Task GenerateAsync_Tests()
        {
            WordsCloudService service = new();

            using MemoryStream ms = new();
            using MemoryStream res = new();
            using (StreamWriter sw = new StreamWriter(ms, leaveOpen: true)) await sw.WriteAsync("Hello World, Hello");
            ms.Seek(0, SeekOrigin.Begin);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.GenerateAsync(null!, null!));
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.GenerateAsync(ms, null!));

            string contentType = await service.GenerateAsync(ms, res);
            Assert.NotEqual(0, res.Length);
            Assert.Equal("image/png", contentType.ToLowerInvariant());
        }
    }
}
