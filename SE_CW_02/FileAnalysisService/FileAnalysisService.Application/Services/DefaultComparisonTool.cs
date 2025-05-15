using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Domain.Models;
using Microsoft.Extensions.Logging;

namespace FileAnalysisService.Application.Services
{
    /// <summary>
    /// Инструмента для сравнения работы.
    /// </summary>
    public class DefaultComparisonTool(ILogger<DefaultComparisonTool> logger) : IComparisonTool
    {
        private readonly ILogger<DefaultComparisonTool> logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Проведение сравнения работы.
        /// </summary>
        /// <param name="work1">Поток данных первой работы.</param>
        /// <param name="work2">Поток данных второй работы.</param>
        /// <returns>Отчёт анализа сравнения работ.</returns>
        private async Task<ComparisonReport?> CompareAsync(Stream work1, Stream work2)
        {
            ArgumentNullException.ThrowIfNull(work1, nameof(work1));
            ArgumentNullException.ThrowIfNull(work2, nameof(work2));

            string content1, content2;

            try
            {
                // Чтение потока данных.
                using (StreamReader sr = new(work1, leaveOpen: true))
                    content1 = await sr.ReadToEndAsync();
                using (StreamReader sr = new(work2, leaveOpen: true))
                    content2 = await sr.ReadToEndAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);
                return null;
            }
            finally
            {
                work1.Seek(0, SeekOrigin.Begin);
                work2.Seek(0, SeekOrigin.Begin);
            }

            // Сравнение работ и формирование отчёта.
            return new ComparisonReport()
            {
                Similarity = content1.Trim() == content2.Trim() ? 1 : 0,
            };
        }

        /// <summary>
        /// Проведение сравнения работы.
        /// </summary>
        /// <param name="works">Поток данных работ {id: работа}.</param>
        /// <returns>Отчёт анализа работы.</returns>
        public async Task<IEnumerable<ComparisonReport>> CompareAsync(Dictionary<int, Stream> works)
        {
            ArgumentNullException.ThrowIfNull(works, nameof(works));

            int[] keys = [.. works.Keys];                      // id работ.
            List<ComparisonReport> result = new(keys.Length);  // Список отчётов.
            
            for (int i = 0; i < keys.Length; i++)
            {
                Stream work1 = works[keys[i]];

                for (int j = i + 1; j < keys.Length; j++)
                {
                    Stream work2 = works[keys[j]];

                    // Сравнение двух работ.
                    ComparisonReport? report = await CompareAsync(work1, work2);

                    // Добавляем отчёт.
                    if (report != null)
                    {
                        report.Work1ID = keys[i];
                        report.Work2ID = keys[j];
                        result.Add(report);
                    }
                }
            }

            return result;
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

            List<ComparisonReport> result = new(comparisons.Count());

            foreach (ComparisonKey key in comparisons)
            {
                if (!works.TryGetValue(key.Work1ID, out Stream? work1) ||
                    !works.TryGetValue(key.Work2ID, out Stream? work2)) continue;

                // Сравнение двух работ.
                ComparisonReport? report = await CompareAsync(work1, work2);

                // Добавляем отчёт.
                if (report != null)
                {
                    report.Work1ID = key.Work1ID;
                    report.Work2ID = key.Work2ID;
                    result.Add(report);
                }
            }

            return result;
        }
    }
}
