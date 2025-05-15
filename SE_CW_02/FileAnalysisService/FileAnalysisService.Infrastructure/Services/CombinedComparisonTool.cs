using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Application.Services;
using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FileAnalysisService.Infrastructure.Services
{
    /// <summary>
    /// Инструмент для комбинорованного сравнения работ
    /// <see cref="JplagComparisonTool"/> и <see cref="DefaultComparisonTool"/>.
    /// </summary>
    public class CombinedComparisonTool(ILogger<DefaultComparisonTool> logger, IConfiguration configuration) : IComparisonTool
    {
        private readonly DefaultComparisonTool defaultComparison = new(logger);
        private readonly JplagComparisonTool jplagComparison = new(configuration);

        /// <summary>
        /// Проведение сравнения работы.
        /// </summary>
        /// <param name="works">Поток данных работ {id: работа}.</param>
        /// <returns>Отчёт анализа работы.</returns>
        public async Task<IEnumerable<ComparisonReport>> CompareAsync(Dictionary<int, Stream> works)
        {
            int[] worksID = [.. works.Keys];
            HashSet<ComparisonKey> unique = new(worksID.Length);

            // Создание пар работ, которые надо сравнить.
            for (int i = 0; i < worksID.Length; i++)
                for (int j = i + 1; j < worksID.Length; j++)
                    unique.Add(new ComparisonKey(worksID[i], worksID[j]));

            return await CompareCouplesAsync(works, unique);
        }

        /// <summary>
        /// Проведение сравнения работы.
        /// </summary>
        /// <param name="works">Поток данных работ {id: работа}.</param>
        /// <param name="comparisons">id работы, которые надо сравнить.</param>
        /// <returns>Отчёт анализа работы.</returns>
        public async Task<IEnumerable<ComparisonReport>> CompareAsync(Dictionary<int, Stream> works, IEnumerable<ComparisonKey> comparisons)
        {
            ArgumentNullException.ThrowIfNull(works, nameof(works));
            ArgumentNullException.ThrowIfNull(comparisons, nameof(comparisons));

            HashSet<ComparisonKey> unique = [.. comparisons];
            return await CompareCouplesAsync(works, unique);
        }

        /// <summary>
        /// Проведение сравнения работы.
        /// </summary>
        /// <param name="works">Поток данных работ {id: работа}.</param>
        /// <param name="comparisons">id работы, которые надо сравнить.</param>
        /// <returns>Отчёт анализа работы.</returns>
        public async Task<IEnumerable<ComparisonReport>> CompareCouplesAsync(Dictionary<int, Stream> works, HashSet<ComparisonKey> unique)
        {
            IEnumerable<ComparisonReport> reports = [];
            try
            {
                // Проверка jplag.
                reports = await jplagComparison.CompareAsync(works, unique);
                foreach (ComparisonReport report in reports)
                {
                    unique.Remove(new ComparisonKey(report.Work1ID, report.Work2ID));
                }
            }
            catch
            { }

            IEnumerable<ComparisonReport> defaultReports = await defaultComparison.CompareAsync(works, unique);
            return [.. reports.Concat(defaultReports)];
        }
    }
}
