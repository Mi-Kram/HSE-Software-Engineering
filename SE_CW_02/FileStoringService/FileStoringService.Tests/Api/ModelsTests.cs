using FileStoringService.Api.DTO;
using FileStoringService.Api.Initializing;
using Microsoft.OpenApi.Models;

namespace FileStoringService.Tests.Api
{
    public class ModelsTests
    {
        [Fact]
        public void UploadWorkDTO_Tests()
        {
            UploadWorkDTO dto = new();
            Assert.Equal(default, dto.File);
            Assert.Equal(default, dto.UserID);
        }

        [Fact]
        public void SwaggerAddTokenHeader_Tests()
        {
            SwaggerAddTokenHeader model = new();
            model.Apply(new OpenApiOperation(), null!);
        }
    }
}
