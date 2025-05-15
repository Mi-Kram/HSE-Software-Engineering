using Microsoft.AspNetCore.Http;

namespace Gateway.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для удаления работы.
    /// </summary>
    public interface IWorkDeletionService
    {
        /// <summary>
        /// Удаление работы по id.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        /// <param name="workID">id работы.</param>
        Task DeleteWorkAsync(HttpContext httpContext, int workID);
    }
}
