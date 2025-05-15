using FileStoringService.Api.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace FileStoringService.Tests.Api
{
    public class AuthorizationMiddlewareTests
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

        [Fact]
        public async Task Handle_ShouldCallNext_WhenTokenIsValid()
        {
            var validToken = "valid-token";
            var middleware = new AuthorizationMiddleware(new[] { validToken });

            var context = new DefaultHttpContext();
            context.Request.Headers["token"] = validToken;

            var wasCalled = false;
            Task Next() { wasCalled = true; return Task.CompletedTask; }

            await middleware.Handle(context, Next);

            Assert.True(wasCalled);
            Assert.Equal(200, context.Response.StatusCode);
        }

        [Fact]
        public async Task Handle_ShouldReturn401_WhenTokenHeaderMissing()
        {
            var middleware = new AuthorizationMiddleware(new[] { "any-token" });

            var context = new DefaultHttpContext();
            var responseBody = new MemoryStream();
            context.Response.Body = responseBody;
            context.RequestServices = new ServiceCollection()
                .AddSingleton<IActionResultExecutor<JsonResult>, JsonResultExecutorStub>()
                .BuildServiceProvider();
            Task Next() => Task.FromException(new Exception("Should not be called"));

            await middleware.Handle(context, Next);

            Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);

            responseBody.Seek(0, SeekOrigin.Begin);
            var json = await JsonDocument.ParseAsync(responseBody);
            Assert.Equal("Unauthorized", json.RootElement.GetProperty("error").GetString());
        }

        [Fact]
        public async Task Handle_ShouldReturn401_WhenTokenIsInvalid()
        {
            var middleware = new AuthorizationMiddleware(new[] { "valid-token" });

            var context = new DefaultHttpContext();
            context.Request.Headers["token"] = "invalid-token";

            var responseBody = new MemoryStream();
            context.Response.Body = responseBody;
            context.RequestServices = new ServiceCollection()
                .AddSingleton<IActionResultExecutor<JsonResult>, JsonResultExecutorStub>()
                .BuildServiceProvider();

            Task Next() => Task.FromException(new Exception("Should not be called"));

            await middleware.Handle(context, Next);

            Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);

            responseBody.Seek(0, SeekOrigin.Begin);
            var json = await JsonDocument.ParseAsync(responseBody);
            Assert.Equal("Unauthorized", json.RootElement.GetProperty("error").GetString());
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenTokensIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new AuthorizationMiddleware(null!));
            Assert.Equal("tokens", exception.ParamName);
        }
    }
}
