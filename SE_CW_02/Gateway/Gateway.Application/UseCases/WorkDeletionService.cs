using Gateway.Application.Interfaces;
using Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Gateway.Application.UseCases
{
    /// <summary>
    /// Cервис для удаления работы.
    /// </summary>
    public class WorkDeletionService(IOuterWorkDeletionService workDeletionService) : IWorkDeletionService
    {
        private readonly IOuterWorkDeletionService workDeletionService = workDeletionService ?? throw new ArgumentNullException(nameof(workDeletionService));

        /// <summary>
        /// Удаление работы по id.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        /// <param name="workID">id работы.</param>
        public async Task DeleteWorkAsync(HttpContext httpContext, int workID)
        {
            await workDeletionService.DeleteWorkAsync(httpContext, workID);
        }
    }
}
