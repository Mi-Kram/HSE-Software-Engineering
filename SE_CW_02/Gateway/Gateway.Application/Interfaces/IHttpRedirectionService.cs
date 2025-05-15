using Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Gateway.Application.Interfaces
{
    /// <summary>
    /// Интерфейс для проксирования http запросов.
    /// </summary>
    public interface IHttpRedirectionService : IRedirectionService<
        HttpRequest, HttpRequestMessage, string,
        HttpResponse, HttpResponseMessage, string>;
}
