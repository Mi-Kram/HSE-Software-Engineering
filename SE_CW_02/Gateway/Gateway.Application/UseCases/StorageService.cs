using Gateway.Application.Interfaces;
using Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Gateway.Application.UseCases
{
    /// <summary>
    /// Сервис для взаимодействия с сервисом для хранения работ.
    /// </summary>
    public class StorageService(IOuterStorageService outerStorageService) : IStorageService
    {
        private readonly IOuterStorageService outerStorageService = outerStorageService ?? throw new ArgumentNullException(nameof(outerStorageService));

        /// <summary>
        /// Получить все работы.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        public async Task GetAllWorksAsync(HttpContext httpContext)
        {
            await outerStorageService.GetAllWorksAsync(httpContext);
        }

        /// <summary>
        /// Получить работу по id.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        /// <param name="workID">id работы.</param>
        public async Task GetWorkAsync(HttpContext httpContext, int workID)
        {
            await outerStorageService.GetWorkAsync(httpContext, workID);
        }

        /// <summary>
        /// Загрузка работы на сервер.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        public async Task UploadWorkAsync(HttpContext httpContext)
        {
            await outerStorageService.UploadWorkAsync(httpContext);
        }
    }
}
