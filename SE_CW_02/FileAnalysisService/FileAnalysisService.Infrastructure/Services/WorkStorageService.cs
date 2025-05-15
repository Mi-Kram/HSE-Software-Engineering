using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Domain.Exceptions;
using FileAnalysisService.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FileAnalysisService.Infrastructure.Services
{
    /// <summary>
    /// Сервис для получения работ.
    /// </summary>
    public class WorkStorageService(ILogger<WorkStorageService> logger, IConfiguration configuration) : IWorkStorageService
    {
        private readonly ILogger<WorkStorageService> logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        private readonly string address = configuration
            .GetSection(ApplicationVariables.MAIN_SERVER).Value?.TrimEnd('/')
            ?? throw new EnvVariableException(ApplicationVariables.MAIN_SERVER);

        /// <summary>
        /// Получение работы.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <returns>Поток данных работы.</returns>
        public async Task<Stream?> TryGetWorkAsync(int workID)
        {
            try
            {
                using HttpClient client = new()
                {
                    BaseAddress = new Uri(address)
                };

                using Stream work = await client.GetStreamAsync($"api/works/{workID}");

                MemoryStream ms = new();
                await work.CopyToAsync(ms);

                ms.Seek(0, SeekOrigin.Begin);
                return ms;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Получение работы.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <returns>Поток данных работы.</returns>
        public async Task<Stream> GetWorkAsync(int workID)
        {
            return await TryGetWorkAsync(workID) ?? throw new ArgumentException("Работа не найдена");
        }
    }
}
