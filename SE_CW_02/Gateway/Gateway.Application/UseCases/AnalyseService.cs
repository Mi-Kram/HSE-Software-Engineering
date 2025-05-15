using Gateway.Application.Interfaces;
using Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Gateway.Application.UseCases
{
    /// <summary>
    /// Сервис для взаимодействия с сервисом для анализа работ.
    /// </summary>
    public class AnalyseService(IOuterAnalyseService outerAnalyseService) : IAnalyseService
    {
        private readonly IOuterAnalyseService outerAnalyseService = outerAnalyseService ?? throw new ArgumentNullException(nameof(outerAnalyseService));

        /// <summary>
        /// Получить отчёты анализа работ.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        public async Task GetAnalyseReportsAsync(HttpContext httpContext)
        {
            await outerAnalyseService.GetAnalyseReportsAsync(httpContext);
        }

        /// <summary>
        /// Получить изображение облака слов.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        /// <param name="workID">id работы.</param>
        public async Task GetWordsCloudAsync(HttpContext httpContext, int workID)
        {
            await outerAnalyseService.GetWordsCloudAsync(httpContext, workID);
        }

        /// <summary>
        /// Получить отчёты сравнения работ.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        public async Task GetCoupleComparisonReportsAsync(HttpContext httpContext)
        {
            await outerAnalyseService.GetCoupleComparisonReportsAsync(httpContext);
        }

        /// <summary>
        /// Получить отчёты сравнения работ.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        public async Task GetAllComparisonReportsAsync(HttpContext httpContext)
        {
            await outerAnalyseService.GetAllComparisonReportsAsync(httpContext);
        }
    }
}
