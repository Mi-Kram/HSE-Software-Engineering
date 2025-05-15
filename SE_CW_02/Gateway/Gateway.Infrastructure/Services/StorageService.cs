using Gateway.Application.Interfaces;
using Gateway.Domain.Exceptions;
using Gateway.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Gateway.Infrastructure.Services
{
    /// <summary>
    /// Сервис для взаимодействия с сервисом для хранения файлов.
    /// </summary>
    public class StorageService : IOuterStorageService, IDisposable
    {
        private readonly HttpClient httpClient;
        private readonly IHttpRedirectionService httpRedirectionService;

        public StorageService(IHttpRedirectionService httpRedirectionService, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

            // Адрес и токен сервиса.
            string baseAddress = configuration?.GetSection(ApplicationVariables.STORAGE_SERVER)?.Value ?? throw new EnvVariableException(ApplicationVariables.STORAGE_SERVER);
            string token = configuration?.GetSection(ApplicationVariables.STORAGE_TOKEN)?.Value ?? throw new EnvVariableException(ApplicationVariables.STORAGE_TOKEN);

            this.httpRedirectionService = httpRedirectionService ?? throw new ArgumentNullException(nameof(httpRedirectionService));

            // Создание http клиента с базовым адресом.
            httpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress),
            };

            // Добавление токена авторизации по умолчанию.
            httpClient.DefaultRequestHeaders.Add("token", token);
        }

        /// <summary>
        /// Проксирование запроса.
        /// </summary>
        /// <param name="context">Контекст запроса.</param>
        /// <param name="address">Адрес проксирования.</param>
        private async Task RedirectAsync(HttpContext context, string address)
        {
            using HttpRequestMessage request = await httpRedirectionService.RedirectRequestAsync(context.Request, address);
            using HttpResponseMessage response = await httpClient.SendAsync(request);
            await httpRedirectionService.RedirectResponseAsync(context.Response, response);
        }

        /// <summary>
        /// Получить все работы.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        public async Task GetAllWorksAsync(HttpContext httpContext)
        {
            await RedirectAsync(httpContext, "api/works");
        }

        /// <summary>
        /// Получить работу по id.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        /// <param name="workID">id работы.</param>
        public async Task GetWorkAsync(HttpContext httpContext, int workID)
        {
            await RedirectAsync(httpContext, $"api/works/{workID}");
        }

        /// <summary>
        /// Загрузка работы на сервер.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        public async Task UploadWorkAsync(HttpContext httpContext)
        {
            await RedirectAsync(httpContext, "api/works");
        }

        /// <summary>
        /// Освобождение ресурсов.
        /// </summary>
        public void Dispose()
        {
            httpClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
