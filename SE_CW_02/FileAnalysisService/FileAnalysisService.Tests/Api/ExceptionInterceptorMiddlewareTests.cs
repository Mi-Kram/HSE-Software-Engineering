using FileAnalysisService.Api.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace FileAnalysisService.Tests.Api
{
    public class ExceptionInterceptorMiddlewareTests
    {
        private class JsonResultExecutorStub : IActionResultExecutor<JsonResult>
        {
            public Task ExecuteAsync(ActionContext context, JsonResult result)
            {
                context.HttpContext.Response.StatusCode = result.StatusCode ?? StatusCodes.Status200OK;
                context.HttpContext.Response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(result.Value);
                return context.HttpContext.Response.WriteAsync(json);
            }
        }

        private static DefaultHttpContext CreateContextWithServices()
        {
            var context = new DefaultHttpContext();
            context.RequestServices = new ServiceCollection()
                .AddSingleton<IActionResultExecutor<JsonResult>, JsonResultExecutorStub>()
                .BuildServiceProvider();

            context.Response.Body = new MemoryStream();
            return context;
        }

        [Fact]
        public async Task Handle_ShouldCallNext_WhenNoException()
        {
            var loggerMock = new Mock<ILogger<ExceptionInterceptorMiddleware>>();
            var middleware = new ExceptionInterceptorMiddleware(loggerMock.Object);

            var context = CreateContextWithServices();

            var nextCalled = false;
            Task Next() { nextCalled = true; return Task.CompletedTask; }

            await middleware.Handle(context, Next);

            Assert.True(nextCalled);
            Assert.Equal(200, context.Response.StatusCode);
        }

        [Fact]
        public async Task Handle_ShouldReturn400_WhenArgumentExceptionThrown()
        {
            var loggerMock = new Mock<ILogger<ExceptionInterceptorMiddleware>>();
            var middleware = new ExceptionInterceptorMiddleware(loggerMock.Object);

            var context = CreateContextWithServices();

            Task Next() => throw new ArgumentException("Некорректный аргумент");

            await middleware.Handle(context, Next);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var json = await JsonDocument.ParseAsync(context.Response.Body);

            Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
            Assert.Equal("Некорректный аргумент", json.RootElement.GetProperty("error").GetString());
        }

        [Fact]
        public async Task Handle_ShouldReturn500AndLog_WhenUnhandledExceptionThrown()
        {
            var loggerMock = new Mock<ILogger<ExceptionInterceptorMiddleware>>();
            var middleware = new ExceptionInterceptorMiddleware(loggerMock.Object);

            var context = CreateContextWithServices();

            var exception = new InvalidOperationException("что-то пошло не так");
            Task Next() => throw exception;

            await middleware.Handle(context, Next);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var json = await JsonDocument.ParseAsync(context.Response.Body);

            Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
            Assert.Equal("Ошибка сервера", json.RootElement.GetProperty("error").GetString());

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("что-то пошло не так")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenLoggerIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new ExceptionInterceptorMiddleware(null!));
            Assert.Equal("logger", ex.ParamName);
        }
    }
}
