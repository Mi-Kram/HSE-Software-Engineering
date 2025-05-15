using Gateway.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Text;

namespace Gateway.Tests.Infrastructure
{
    public class HttpRedirectionServiceTests
    {
        private readonly HttpRedirectionService _service = new();

        [Fact]
        public async Task RedirectRequestAsync_CopiesMethodAndHeaders()
        {
            var context = new DefaultHttpContext();
            var request = context.Request;
            request.Method = HttpMethod.Post.Method;
            request.Headers["Custom-Header"] = "value";
            request.Body = new MemoryStream(Encoding.UTF8.GetBytes("test content"));
            request.ContentType = "text/plain";

            var redirected = await _service.RedirectRequestAsync(request, "https://example.com/api");

            Assert.Equal(HttpMethod.Post, redirected.Method);
            Assert.Equal("https://example.com/api", redirected.RequestUri!.ToString());
            Assert.True(redirected.Headers.TryGetValues("Custom-Header", out var values));
            Assert.Equal("value", values!.First());

            var body = await redirected.Content!.ReadAsStringAsync();
            Assert.Equal("test content", body);
        }

        [Fact]
        public async Task RedirectRequestAsync_HandlesMultipartFormData()
        {
            var context = new DefaultHttpContext();
            var request = context.Request;

            // Подготовка формы с полями
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                { "field1", "value1" },
                { "field2", "value2" }
            });

            request.ContentType = "multipart/form-data";
            request.Form = form;
            request.Method = HttpMethod.Post.Method;

            var redirected = await _service.RedirectRequestAsync(request, "https://localhost");

            var multipart = Assert.IsType<MultipartFormDataContent>(redirected.Content);

            // Проверка, что multipart содержит нужные поля
            var stringParts = multipart
                .OfType<StringContent>()
                .ToDictionary(part => part.Headers.ContentDisposition!.Name!.Trim('"'), async part => await part.ReadAsStringAsync());

            Assert.True(stringParts.ContainsKey("field1"));
            Assert.Equal("value1", await stringParts["field1"]);

            Assert.True(stringParts.ContainsKey("field2"));
            Assert.Equal("value2", await stringParts["field2"]);
        }

        [Fact]
        public async Task RedirectResponseAsync_CopiesStatusCodeAndHeadersAndBody()
        {
            var context = new DefaultHttpContext();
            var response = context.Response;
            var bodyStream = new MemoryStream();
            response.Body = bodyStream;

            var message = new HttpResponseMessage(HttpStatusCode.Accepted);
            message.Headers.Add("X-Test", "ok");
            message.Content = new StringContent("response content", Encoding.UTF8, "text/plain");
            message.Content.Headers.Add("X-Content-Test", "yes");

            await _service.RedirectResponseAsync(response, message);

            Assert.Equal((int)HttpStatusCode.Accepted, response.StatusCode);
            Assert.Equal("ok", response.Headers["X-Test"]);
            Assert.Equal("yes", response.Headers["X-Content-Test"]);

            response.Body.Seek(0, SeekOrigin.Begin);
            var result = await new StreamReader(response.Body).ReadToEndAsync();
            Assert.Equal("response content", result);
        }

        [Fact]
        public async Task RedirectRequestAsync_EmptyBody()
        {
            var context = new DefaultHttpContext();
            var request = context.Request;
            request.Method = "GET";
            request.ContentLength = 0;

            var message = await _service.RedirectRequestAsync(request, "http://localhost");
            var content = await message.Content!.ReadAsStringAsync();

            Assert.Equal(string.Empty, content);
        }
    }
}
