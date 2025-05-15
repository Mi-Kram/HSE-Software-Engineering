using Gateway.Api.Initializing;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Moq;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Gateway.Tests.Api
{
    public class SwaggerAddUploadWorkFormDataTests
    {
        [Fact]
        public void Apply_AddsMultipartFormDataRequestBody_WhenPathIsApiWorksAndMethodIsPost()
        {
            // Arrange
            var operation = new OpenApiOperation();
            var apiDescription = new ApiDescription
            {
                HttpMethod = "POST",
                RelativePath = "api/works"
            };

            var schemaGeneratorMock = new Mock<ISchemaGenerator>();
            var methodInfo = typeof(SwaggerAddUploadWorkFormDataTests).GetMethod(nameof(DummyMethod), BindingFlags.NonPublic | BindingFlags.Static)!;

            var context = new OperationFilterContext(
                apiDescription,
                schemaGeneratorMock.Object,
                new SchemaRepository(),
                methodInfo
            );

            var filter = new SwaggerAddUploadWorkFormData();

            // Act
            filter.Apply(operation, context);

            // Assert
            Assert.NotNull(operation.RequestBody);
            Assert.True(operation.RequestBody.Content.ContainsKey("multipart/form-data"));

            var schema = operation.RequestBody.Content["multipart/form-data"].Schema;
            Assert.Equal("object", schema.Type);
            Assert.True(schema.Properties.ContainsKey("UserID"));
            Assert.True(schema.Properties.ContainsKey("file"));
            Assert.Equal("integer", schema.Properties["UserID"].Type);
            Assert.Equal("int32", schema.Properties["UserID"].Format);
            Assert.Equal("string", schema.Properties["file"].Type);
            Assert.Equal("binary", schema.Properties["file"].Format);
        }

        [Theory]
        [InlineData("GET", "api/works")]
        [InlineData("POST", "api/other")]
        [InlineData("PUT", "api/works")]
        public void Apply_DoesNothing_WhenConditionsNotMet(string method, string path)
        {
            // Arrange
            var operation = new OpenApiOperation();
            var apiDescription = new ApiDescription
            {
                HttpMethod = method,
                RelativePath = path
            };

            var schemaGeneratorMock = new Mock<ISchemaGenerator>();
            var methodInfo = typeof(SwaggerAddUploadWorkFormDataTests).GetMethod(nameof(DummyMethod), BindingFlags.NonPublic | BindingFlags.Static)!;

            var context = new OperationFilterContext(
                apiDescription,
                schemaGeneratorMock.Object,
                new SchemaRepository(),
                methodInfo
            );

            var filter = new SwaggerAddUploadWorkFormData();

            // Act
            filter.Apply(operation, context);

            // Assert
            Assert.Null(operation.RequestBody);
        }

        // Просто заглушка, чтобы был MethodInfo
        private static void DummyMethod() { }
    }
}
