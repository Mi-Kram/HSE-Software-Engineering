using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Domain.Exceptions;
using FileAnalysisService.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.IO.Compression;
using System.Text.Json;

namespace FileAnalysisService.Infrastructure.Services
{
    /// <summary>
    /// Инструмента для сравнения работы, использующий JPlag.
    /// </summary>
    public class JplagComparisonTool(IConfiguration configuration) : IComparisonTool
    {
        private readonly string jplagPath = configuration?.GetSection(ApplicationVariables.COMPARISON_FILE)?.Value ?? throw new EnvVariableException(ApplicationVariables.COMPARISON_FILE);
        private const string resultFile = "result.zip";

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

            IEnumerable<ComparisonReport> allReports = await CompareAsync(works);

            HashSet<ComparisonKey> unique = comparisons is HashSet<ComparisonKey> set ? set : [.. comparisons];
            return [.. allReports.Where(x => unique.Contains(new ComparisonKey(x.Work1ID, x.Work2ID)))];
        }

        /// <summary>
        /// Проведение сравнения работы.
        /// </summary>
        /// <param name="works">Поток данных работ {id: работа}.</param>
        /// <returns>Отчёт анализа работы.</returns>
        public async Task<IEnumerable<ComparisonReport>> CompareAsync(Dictionary<int, Stream> works)
        {
            ArgumentNullException.ThrowIfNull(works, nameof(works));
            if (works.Count <= 1) return [];

            string tempDir = GetTempDirectory();

            try
            {
                // Сохранение работ.
                SaveWorks(works, tempDir);

                // Запуск процесса сравнения.
                await RunProcess(tempDir, jplagPath);

                // Парсинг и возвращение результата.
                return await ParseResult(Path.Join(tempDir, resultFile));
            }
            catch
            {
                throw;
            }
            finally
            {
                // Удаляем временный каталог.
                Directory.Delete(tempDir, true);
            }
        }


        /// <summary>
        /// Создание временного каталога.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static string GetTempDirectory()
        {
            // Поиск не занятого каталога.
            string path = $"/tmp/{Guid.NewGuid()}";
            while (Directory.Exists(path)) path = $"/tmp/{Guid.NewGuid()}";

            // Создание каталога.
            if (!Directory.CreateDirectory(path).Exists) throw new Exception($"JPlagTool: Не удалось создать каталог {path}");
            return path;
        }

        /// <summary>
        /// Сохранение работ в каталог.
        /// </summary>
        /// <param name="works">Работы для сохранения.</param>
        /// <param name="dir">Место сохранения.</param>
        private static void SaveWorks(Dictionary<int, Stream> works, string dir)
        {
            foreach (var work in works)
            {
                using (FileStream fs = new(Path.Combine(dir, $"{work.Key}.txt"), 
                    FileMode.Create, FileAccess.Write)) work.Value.CopyTo(fs);
            }
        }

        /// <summary>
        /// Запуск процесса сравнения работ.
        /// </summary>
        /// <param name="dir">Каталог с работами.</param>
        /// <param name="processFile">Файл проверки.</param>
        private static async Task RunProcess(string dir, string processFile)
        {
            // Параметры запуска процесса сравнения работ.
            ProcessStartInfo options = new("java", ["-jar", processFile,
                dir, "-l", "text", "-r", Path.Join(dir, resultFile)])
            {
                CreateNoWindow = true,
                ErrorDialog = false,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            
            // Запуск процесса сравнения работ.
            Process? process = Process.Start(options);


            if (process == null) throw new Exception("JPlagTool: Не удалось сравнить файлы");
            
            // Ожидание завершения процесса и проверка успешности сравнения.
            await process.WaitForExitAsync();

            if (!File.Exists(Path.Join(dir, resultFile)))
            {
                throw new Exception($"JPlagTool: Файл с результатом сравнения не найден. Обработка: {await process.StandardOutput.ReadToEndAsync()}");
            }
        }

        /// <summary>
        /// Парсинг результата, получение процента схожести.
        /// </summary>
        /// <param name="path">Путь к результату проверки.</param>
        /// <returns>Процент схожести [0; 1].</returns>
        private static async Task<IEnumerable<ComparisonReport>> ParseResult(string path)
        {
            // Открываем архив.
            using ZipArchive zip = ZipFile.Open(path, ZipArchiveMode.Read);

            List<ComparisonReport> reports = [];

            // Перебираем каждый элемент архива.
            foreach (ZipArchiveEntry entry in zip.Entries)
            {
                ComparisonReport? report = await ParseItemAsync(entry);
                if (report != null) reports.Add(report);
            }

            return reports;
        }

        /// <summary>
        /// Парсинг элемента архива.
        /// </summary>
        /// <param name="entry">Элемент архива.</param>
        /// <returns>Отчёт сравнения или null.</returns>
        private static async Task<ComparisonReport?> ParseItemAsync(ZipArchiveEntry entry)
        {
            // Пропускаем не интересующие нас файлы.
            if (entry.Name == "overview.json" || entry.Name == "submissionFileIndex.json" ||
                entry.Name == "README.txt" || entry.Name == "options.json" ||
                entry.FullName.Contains('/') || entry.FullName.Contains('\\')) return null;
            
            try
            {
                // Открываем поток для чтения информации о проверке.
                using Stream stream = entry.Open();
                using JsonDocument jsonDocument = await JsonDocument.ParseAsync(stream);

                // Получаем результат.
                return new ComparisonReport
                {
                    Work1ID = int.Parse(jsonDocument.RootElement.GetProperty("id1").GetString()!.Split('.')[0]),
                    Work2ID = int.Parse(jsonDocument.RootElement.GetProperty("id2").GetString()!.Split('.')[0]),
                    Similarity = jsonDocument.RootElement.GetProperty("similarities").GetProperty("MAX").GetSingle()
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
