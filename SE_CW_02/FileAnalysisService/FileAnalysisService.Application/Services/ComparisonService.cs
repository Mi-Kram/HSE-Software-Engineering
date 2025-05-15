using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Domain.Interfaces;
using FileAnalysisService.Domain.Models;

namespace FileAnalysisService.Application.Services
{
    /// <summary>
    /// Интерфейс сравнения работ.
    /// </summary>
    public class ComparisonService(
        IComparisonReportRepository comparisonReportRepository,
        IWorkStorageService workStorageService,
        IComparisonTool comparisonTool) : IComparisonService
    {
        private readonly IComparisonReportRepository comparisonReportRepository = comparisonReportRepository ?? throw new ArgumentNullException(nameof(comparisonReportRepository));
        private readonly IWorkStorageService workStorageService = workStorageService ?? throw new ArgumentNullException(nameof(workStorageService));
        private readonly IComparisonTool comparisonTool = comparisonTool ?? throw new ArgumentNullException(nameof(comparisonTool));

        /// <summary>
        /// Получить работы по id.
        /// </summary>
        /// <param name="worksID">id работ.</param>
        /// <returns>Словарь: {id: работа.}</returns>
        private async Task<Dictionary<int, Stream>> GetWorksAsync(IEnumerable<int> worksID)
        {
            Dictionary<int, Stream> result = new(worksID.Count());

            // Для каждого id получаем работу.
            foreach (int workID in worksID)
            {
                // Если работа была уже получена.
                if (result.ContainsKey(workID)) continue;

                // Получаем новую работу и сохраняем.
                Stream? work = await workStorageService.TryGetWorkAsync(workID);
                if (work != null) result[workID] = work;
            }

            return result;
        }

        /// <summary>
        /// Проверка отчётов в ранее проведённых сравнениях работ.
        /// </summary>
        /// <param name="keys">Пары работ для сравнений.</param>
        /// <returns>Отчёты найденных сравнений.</returns>
        private async Task<IEnumerable<ComparisonReport>> GetStoredReportsAsync(IEnumerable<ComparisonKey> keys)
        {
            // Результат сравнений.
            List<ComparisonReport> result = new(keys.Count());

            // Смотрим, что уже проверено.
            foreach (ComparisonKey key in keys)
            {
                ComparisonReport? report = await comparisonReportRepository.GetAsync(key);
                if (report != null) result.Add(report);
            }

            return result;
        }

        /// <summary>
        /// Сравнение пар работ.
        /// </summary>
        /// <param name="keys">Пары работ для сравнения.</param>
        /// <returns>Отчёты сравнений.</returns>
        private async Task<IEnumerable<ComparisonReport>> CompareCouplesAsync(IEnumerable<ComparisonKey> keys)
        {
            // Список всех id для сравнений.
            IEnumerable<int> worksID = keys.Select(x => x.Work1ID).Concat(keys.Select(x => x.Work2ID));

            // Получить работы по id.
            Dictionary<int, Stream> works = await GetWorksAsync(worksID);

            // Сравниваем работы.
            IEnumerable<ComparisonReport> comparisonReports = await comparisonTool.CompareAsync(works, keys);

            // Освобождаем ресурсы.
            foreach (KeyValuePair<int, Stream> work in works)
            {
                work.Value.Dispose();
            }

            return comparisonReports;
        }

        /// <summary>
        /// Получить отчёт сравнения работ.
        /// </summary>
        /// <param name="keys">id работ для сравнения.</param>
        /// <returns>Коллекция отчётов сравнений.</returns>
        public async Task<IEnumerable<ComparisonReport>> GetReportsAsync(IEnumerable<ComparisonKey> keys)
        {
            // Уникальные пары работ.
            HashSet<ComparisonKey> unique = [.. keys.Where(x => x.Work1ID != x.Work2ID)];

            // Получаем результаты прошлых сравнений.
            IEnumerable<ComparisonReport> result = await GetStoredReportsAsync(unique);

            // Исключаем проверенные работы.
            foreach (ComparisonReport report in result)
            {
                unique.Remove(new ComparisonKey(report.Work1ID, report.Work2ID));
            }

            // Сравниваем работы.
            ComparisonReport[] comparisonReports = [.. await CompareCouplesAsync(unique)];

            // Сохраняем результат.
            await comparisonReportRepository.AddAsync(comparisonReports);

            return result.Concat(comparisonReports);
        }

        /// <summary>
        /// Получить отчёт сравнения всех работ друг с другом.
        /// </summary>
        /// <param name="worksID">id работ для сравнения.</param>
        /// <returns>Коллекция отчётов сравнений.</returns>
        public async Task<IEnumerable<ComparisonReport>> GetReportsAsync(IEnumerable<int> worksID)
        {
            HashSet<ComparisonKey> unique = new(worksID.Count());

            // Получаем список пар работ все ко всем.
            foreach (int work1ID in worksID)
            {
                foreach (int work2ID in worksID)
                {
                    if (work1ID == work2ID) continue;
                    unique.Add(new ComparisonKey(work1ID, work2ID));
                }
            }

            return await GetReportsAsync(unique);
        }
    }
}
