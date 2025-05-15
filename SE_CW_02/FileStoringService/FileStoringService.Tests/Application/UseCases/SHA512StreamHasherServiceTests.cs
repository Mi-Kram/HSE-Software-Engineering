using FileStoringService.Application.UseCases;

namespace FileStoringService.Tests.Application.UseCases
{
    public class SHA512StreamHasherServiceTests
    {
        [Theory]
        [InlineData("string to hash")]
        public async Task HashAsync_Tests(string data)
        {
            SHA512StreamHasherService hasher = new();

            using MemoryStream ms = new();
            using StreamWriter sw = new(ms);
            await sw.WriteLineAsync(data);
            ms.Seek(0, SeekOrigin.Begin);

            string? hash = await hasher.HashAsync(ms);
            if (hash != null) Assert.True(hash.Length <= 64);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await hasher.HashAsync(null!));
        }
    }
}
