using Gateway.Application.Interfaces;
using Gateway.Domain.Exceptions;
using Gateway.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Gateway.Infrastructure.Services
{
    /// <summary>
    /// Сервис для взаимодействия с сервисом для анализа работ.
    /// </summary>
    public class AnalyseService : IOuterAnalyseService, IDisposable
    {
        private readonly HttpClient httpClient;
        private readonly IHttpRedirectionService httpRedirectionService;

        public AnalyseService(IHttpRedirectionService httpRedirectionService, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

            // Адрес и токен сервиса.
            string baseAddress = configuration?.GetSection(ApplicationVariables.ANALYSE_SERVER)?.Value ?? throw new EnvVariableException(ApplicationVariables.ANALYSE_SERVER);
            string token = configuration?.GetSection(ApplicationVariables.ANALYSE_TOKEN)?.Value ?? throw new EnvVariableException(ApplicationVariables.ANALYSE_TOKEN);

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
        /// Получить отчёты анализа работ.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        public async Task GetAnalyseReportsAsync(HttpContext httpContext)
        {
            await RedirectAsync(httpContext, "api/analyse");
        }

        /// <summary>
        /// Получить изображение облака слов.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        /// <param name="workID">id работы.</param>
        public async Task GetWordsCloudAsync(HttpContext httpContext, int workID)
        {
            await RedirectAsync(httpContext, $"api/analyse/wordcloud/{workID}");
        }

        /// <summary>
        /// Получить отчёты сравнения работ.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        public async Task GetCoupleComparisonReportsAsync(HttpContext httpContext)
        {
            await RedirectAsync(httpContext, "api/compare");
        }

        /// <summary>
        /// Получить отчёты сравнения работ.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        public async Task GetAllComparisonReportsAsync(HttpContext httpContext)
        {
            await RedirectAsync(httpContext, "api/compare/all");
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
