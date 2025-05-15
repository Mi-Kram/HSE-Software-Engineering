using FileAnalysisService.Domain.Interfaces;

namespace FileAnalysisService.Application.Services
{
    /// <summary>
    /// Интерфейс для работы с работами.
    /// </summary>
    public class WorkService(IWorkRepository workRepository, IAnalyseStorageService analyseStorageService) : IWorkService
    {
        private readonly IWorkRepository workRepository = workRepository ?? throw new ArgumentNullException(nameof(workRepository));
        private readonly IAnalyseStorageService analyseStorageService = analyseStorageService ?? throw new ArgumentNullException(nameof(analyseStorageService));

        /// <summary>
        /// Удаление данных, связанные с работой.
        /// </summary>
        /// <param name="workID">id работы.</param>
        public async Task DeleteAsync(int workID)
        {
            await analyseStorageService.DeleteWordsCloudImageAsync(workID);
            await workRepository.DeleteAsync(workID);
        }
    }
}
