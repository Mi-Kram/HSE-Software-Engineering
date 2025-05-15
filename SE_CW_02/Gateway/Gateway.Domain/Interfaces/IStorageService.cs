using Microsoft.AspNetCore.Http;

namespace Gateway.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс для взаимодействия с сервисом для хранения работ.
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Получить все работы.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        Task GetAllWorksAsync(HttpContext httpContext);

        /// <summary>
        /// Получить работу по id.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        /// <param name="workID">id работы.</param>
        Task GetWorkAsync(HttpContext httpContext, int workID);

        /// <summary>
        /// Загрузка работы на сервер.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        Task UploadWorkAsync(HttpContext httpContext);
    }
}
