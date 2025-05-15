using Microsoft.AspNetCore.Http;

namespace Gateway.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс для взаимодействия с сервисом для анализа работ.
    /// </summary>
    public interface IAnalyseService
    {
        /// <summary>
        /// Получить отчёты анализа работ.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        Task GetAnalyseReportsAsync(HttpContext httpContext);

        /// <summary>
        /// Получить отчёты сравнения работ.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        Task GetCoupleComparisonReportsAsync(HttpContext httpContext);

        /// <summary>
        /// Получить отчёты сравнения работ.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        Task GetAllComparisonReportsAsync(HttpContext httpContext);

        /// <summary>
        /// Получить изображение облака слов.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        /// <param name="workID">id работы.</param>
        Task GetWordsCloudAsync(HttpContext httpContext, int workID);
    }
}
