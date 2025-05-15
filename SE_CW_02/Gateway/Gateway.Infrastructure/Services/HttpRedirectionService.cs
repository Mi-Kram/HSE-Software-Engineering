using Gateway.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace Gateway.Infrastructure.Services
{
    /// <summary>
    /// Сервис проксирования http запросов.
    /// </summary>
    public class HttpRedirectionService : IHttpRedirectionService
    {
        private readonly HashSet<string> forbiddenHeadersResponse = new(StringComparer.InvariantCultureIgnoreCase)
        {
            "connection",
            "transfer-encoding",
            "keep-alive",
            "upgrade",
            "proxy-connection"
        };

        /// <summary>
        /// проксировать запрос.
        /// </summary>
        /// <param name="originalRequest">Запрос, который надо проксировать.</param>
        /// <param name="address">Адрес проксирования запроса.</param>
        /// <returns>проксированный запрос.</returns>
        public Task<HttpRequestMessage> RedirectRequestAsync(HttpRequest originalRequest, string? address)
        {
            ArgumentNullException.ThrowIfNull(originalRequest, nameof(originalRequest));
            address ??= string.Empty;

            string addressPath = $"{address}{originalRequest.QueryString}";
            HttpRequestMessage requestMessage = new(new HttpMethod(originalRequest.Method), addressPath);

            // Копируем заголовки
            foreach (var header in originalRequest.Headers)
            {
                _ = requestMessage.Headers.TryAddWithoutValidation(header.Key, [.. header.Value]);
            }

            // Обработка контента
            requestMessage.Content = CreateHttpContent(originalRequest);

            return Task.FromResult(requestMessage);
        }

        /// <summary>
        /// Создание содержимого для перенаправленного запроса.
        /// </summary>
        /// <param name="originalRequest">Исходный запрос.</param>
        /// <returns>Содержимое для нового запроса.</returns>
        private static HttpContent CreateHttpContent(HttpRequest originalRequest)
        {
            // multipart/form-data
            if (originalRequest.HasFormContentType)
            {
                IFormCollection form = originalRequest.Form;
                MultipartFormDataContent content = [];

                foreach (var field in form)
                {
                    content.Add(new StringContent(field.Value!), field.Key);
                }

                foreach (IFormFile file in form.Files)
                {
                    StreamContent fileContent = new(file.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    content.Add(fileContent, file.Name, file.FileName);
                }

                return content;
            }

            // Пустое тело
            if (originalRequest.ContentLength == 0) return new StringContent(string.Empty);

            StreamContent streamContent = new(originalRequest.Body);

            if (originalRequest.ContentType != null)
            {
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(originalRequest.ContentType);
            }

            return streamContent;
        }

        /// <summary>
        /// проксировать ответ.
        /// </summary>
        /// <param name="originalResponse">Ответ, куда надо проксировать другой ответ.</param>
        /// <param name="responseMessage">Ответ для проксирования.</param>
        public async Task RedirectResponseAsync(HttpResponse originalResponse, HttpResponseMessage responseMessage, string? _ = null)
        {
            ArgumentNullException.ThrowIfNull(originalResponse, nameof(originalResponse));
            ArgumentNullException.ThrowIfNull(responseMessage, nameof(responseMessage));

            originalResponse.StatusCode = (int)responseMessage.StatusCode;

            // Копирование зоголовков ответа (response.Headers).
            foreach (var header in responseMessage.Headers)
            {
                if (!forbiddenHeadersResponse.Contains(header.Key.ToLowerInvariant()))
                    originalResponse.Headers[header.Key] = header.Value.ToArray();
            }

            if (responseMessage.Content.Headers.ContentLength == 0) return;

            // Копирование зоголовков тела ответа (response.Content.Headers).
            foreach (var header in responseMessage.Content.Headers)
            {
                if (!forbiddenHeadersResponse.Contains(header.Key.ToLowerInvariant()))
                    originalResponse.Headers[header.Key] = header.Value.ToArray();
            }

            // Копируем тело ответа.
            await responseMessage.Content.CopyToAsync(originalResponse.Body);
        }
    }
}
