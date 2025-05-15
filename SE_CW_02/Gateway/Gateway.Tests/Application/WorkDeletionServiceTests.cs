using Gateway.Application.Interfaces;
using Gateway.Application.UseCases;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Gateway.Tests.Application
{
    public class WorkDeletionServiceTests
    {
        [Fact]
        public async Task DeleteWorkAsync_Tests()
        {
            Mock<IOuterWorkDeletionService> outerService = new();
            Mock<HttpContext> context = new();
            WorkDeletionService service = new(outerService.Object);
            await service.DeleteWorkAsync(context.Object, 1);
            outerService.Verify(x => x.DeleteWorkAsync(It.IsAny<HttpContext>(), It.IsAny<int>()), Times.Once);
        }
    }
}
