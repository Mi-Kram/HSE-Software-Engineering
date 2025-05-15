using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FileAnalysisService.Application.Services
{
    /// <summary>
    /// Сервис анализа работ.
    /// </summary>
    public class AnalyseService(
        IAnalyseReportRepository analyseReportRepository,
        IWorkStorageService workStorageService,
        IAnalyseTool analyseTool,
        IAnalyseStorageService analyseStorageService,
        IWordsCloudService wordsCloudService,
        ILogger<AnalyseService> logger) : IAnalyseService
    {
        private readonly IAnalyseReportRepository analyseReportRepository = analyseReportRepository ?? throw new ArgumentNullException(nameof(analyseReportRepository));
        private readonly IWorkStorageService workStorageService = workStorageService ?? throw new ArgumentNullException(nameof(workStorageService));
        private readonly IAnalyseTool analyseTool = analyseTool ?? throw new ArgumentNullException(nameof(analyseTool));
        private readonly IAnalyseStorageService analyseStorageService = analyseStorageService ?? throw new ArgumentNullException(nameof(analyseStorageService));
        private readonly IWordsCloudService wordsCloudService = wordsCloudService ?? throw new ArgumentNullException(nameof(wordsCloudService));
        private readonly ILogger<AnalyseService> logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Получить отчёт анализа работы.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <returns>Отчёт анализа работы.</returns>
        public async Task<AnalyzeReport?> GetReportAsync(int workID)
        {
            // Проверка наличия отчёта в базе.
            AnalyzeReport? report = await analyseReportRepository.GetAsync(workID);
            if (report != null) return report;

            // Получение работы.
            using Stream? work = await workStorageService.TryGetWorkAsync(workID);
            if (work == null) return null;

            // Анализ работы.
            report = await analyseTool.AnalyzeAsync(work);
            if (report == null) return null;

            // Сохранение отчёта.
            report.WorkID = workID;
            await analyseReportRepository.AddAsync(report);
            return report;
        }

        /// <summary>
        /// Получить облако слов.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <param name="image">Картинка облака слов.</param>
        /// <returns>Content-Type.</returns>
        public async Task<string> GetWordsCloudAsync(int workID, Stream image, bool regenerate)
        {
            ArgumentNullException.ThrowIfNull(image, nameof(image));

            if (!regenerate)
            {
                try
                {
                    // Проверяем, есть ли уже результат запроса.
                    return await analyseStorageService.GetWordsCloudImageAsync(workID, image);
                }
                catch
                { }
            }

            // Получение работы.
            using Stream work = await workStorageService.GetWorkAsync(workID);

            // Генерация изображения.
            string contentType = await wordsCloudService.GenerateAsync(work, image);

            try
            {
                // Сохранения изображения.
                await analyseStorageService.SaveWordsCloudImageAsync(workID, image, contentType);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);
            }

            image.Seek(0, SeekOrigin.Begin);
            return contentType;
        }
    }
}
