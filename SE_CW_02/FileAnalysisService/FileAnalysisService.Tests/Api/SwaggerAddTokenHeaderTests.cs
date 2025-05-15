using FileAnalysisService.Api.Initializing;
using Microsoft.OpenApi.Models;

namespace FileAnalysisService.Tests.Api
{
    public class SwaggerAddTokenHeaderTests
    {
        [Fact]
        public void Apply_ShouldAddTokenParameter_WhenParametersIsNull()
        {
            // Arrange
            var filter = new SwaggerAddTokenHeader();
            var operation = new OpenApiOperation
            {
                Parameters = null
            };

            // Act
            filter.Apply(operation, null!);

            // Assert
            Assert.NotNull(operation.Parameters);
            Assert.Single(operation.Parameters);

            var parameter = operation.Parameters[0];
            Assert.Equal("token", parameter.Name);
            Assert.Equal(ParameterLocation.Header, parameter.In);
            Assert.Equal("access token", parameter.Description);
            Assert.False(parameter.Required);
            Assert.NotNull(parameter.Schema);
            Assert.Equal("string", parameter.Schema.Type);
        }
    }
}
