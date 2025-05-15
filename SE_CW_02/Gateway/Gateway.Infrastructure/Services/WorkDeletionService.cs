using Gateway.Application.Interfaces;
using Gateway.Domain.Exceptions;
using Gateway.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Gateway.Infrastructure.Services
{
    /// <summary>
    /// Cервис для удаления работы.
    /// </summary>
    public class WorkDeletionService : IOuterWorkDeletionService, IDisposable
    {
        private readonly HttpClient storageHttpClient;
        private readonly HttpClient analyseHttpClient;
        private readonly IHttpRedirectionService httpRedirectionService;

        public WorkDeletionService(IHttpRedirectionService httpRedirectionService, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

            // Адрес и токен сервиса.
            string storageBaseAddress = configuration?.GetSection(ApplicationVariables.STORAGE_SERVER)?.Value ?? throw new EnvVariableException(ApplicationVariables.STORAGE_SERVER);
            string analyseBaseAddress = configuration?.GetSection(ApplicationVariables.ANALYSE_SERVER)?.Value ?? throw new EnvVariableException(ApplicationVariables.ANALYSE_SERVER);
            string storageToken = configuration?.GetSection(ApplicationVariables.STORAGE_TOKEN)?.Value ?? throw new EnvVariableException(ApplicationVariables.STORAGE_TOKEN);
            string analyseToken = configuration?.GetSection(ApplicationVariables.ANALYSE_TOKEN)?.Value ?? throw new EnvVariableException(ApplicationVariables.ANALYSE_TOKEN);

            this.httpRedirectionService = httpRedirectionService ?? throw new ArgumentNullException(nameof(httpRedirectionService));

            // Создание http клиента сервиса хранения работ с базовым адресом.
            storageHttpClient = new HttpClient()
            {
                BaseAddress = new Uri(storageBaseAddress),
            };

            // Создание http клиента сервиса анализа работ с базовым адресом.
            analyseHttpClient = new HttpClient()
            {
                BaseAddress = new Uri(analyseBaseAddress),
            };

            // Добавление токена авторизации по умолчанию.
            storageHttpClient.DefaultRequestHeaders.Add("token", storageToken);
            analyseHttpClient.DefaultRequestHeaders.Add("token", analyseToken);
        }

        /// <summary>
        /// Удаление работы по id.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        /// <param name="workID">id работы.</param>
        public async Task DeleteWorkAsync(HttpContext httpContext, int workID)
        {
            // Удаление информации у сервиса для анализа.
            using HttpResponseMessage analyseResponse = await analyseHttpClient.DeleteAsync($"api/works/{workID}");
            if (!analyseResponse.IsSuccessStatusCode)
            {
                await httpRedirectionService.RedirectResponseAsync(httpContext.Response, analyseResponse);
                return;
            }

            // Конечное удаление работы у сервиса для хранения.
            using HttpResponseMessage storageResponse = await storageHttpClient.DeleteAsync($"api/works/{workID}");
            await httpRedirectionService.RedirectResponseAsync(httpContext.Response, storageResponse);
        }

        /// <summary>
        /// Освобождение ресурсов.
        /// </summary>
        public void Dispose()
        {
            storageHttpClient.Dispose();
            analyseHttpClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
