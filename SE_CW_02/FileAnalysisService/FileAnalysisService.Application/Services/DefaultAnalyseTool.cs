using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FileAnalysisService.Application.Services
{
    /// <summary>
    /// Инструмента для анализа работы.
    /// </summary>
    public class DefaultAnalyseTool(ILogger<DefaultAnalyseTool> logger) : IAnalyseTool
    {
        private readonly ILogger<DefaultAnalyseTool> logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Проведение анализа работы.
        /// </summary>
        /// <param name="work">Поток данных работы.</param>
        /// <returns>Отчёт анализа работы.</returns>
        public async Task<AnalyzeReport?> AnalyzeAsync(Stream work)
        {
            ArgumentNullException.ThrowIfNull(work, nameof(work));

            string content;

            try
            {
                // Чтение потока данных.
                using StreamReader sr = new(work, leaveOpen: true);
                content = await sr.ReadToEndAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);
                return null;
            }

            // Анализ работы и формирование отчёта.
            AnalyzeReport report = new()
            {
                Paragraphs = CountParagraphs(content),
                Words = CountWords(content),
                Numbers = CountNumbers(content),
                Symbols = CountSymbols(content)
            };

            return report;
        }

        /// <summary>
        /// Пропуск пробельных символов.
        /// </summary>
        /// <param name="content">Текст для пропуска пробельных символов.</param>
        /// <param name="pos">Текущая позиция в тексте.</param>
        private static void SkipWhitespaces(string content, ref int pos)
        {
            if (pos < 0) return;

            // Перемещать позицию в тексте, пока символ пробельный.
            while (pos < content.Length && char.IsWhiteSpace(content[pos])) ++pos;
        }

        /// <summary>
        /// Подсчт количества абзацев.
        /// </summary>
        /// <param name="content">Текст для анализа.</param>
        /// <returns>Количество абзацев.</returns>
        private static int CountParagraphs(string content)
        {
            int pos = 0, paragraphs = 0;
            SkipWhitespaces(content, ref pos);

            while (0 <= pos && pos < content.Length)
            {
                ++paragraphs;
                pos = content.IndexOf('\n', pos); // переход к следующей строке.
                SkipWhitespaces(content, ref pos);
            }

            return paragraphs;
        }

        /// <summary>
        /// Подсчт количества слов.
        /// </summary>
        /// <param name="content">Текст для анализа.</param>
        /// <returns>Количество слов.</returns>
        private static int CountWords(string content)
        {
            int pos = 0, words = 0;

            // Переход следующему символу, который является символом буквы или цифры.
            while (pos < content.Length && !char.IsLetterOrDigit(content[pos])) ++pos;

            while (pos < content.Length)
            {
                ++words;

                // Переход к следующему символу, который не является символом буквы или цифры.
                while (pos < content.Length && char.IsLetterOrDigit(content[pos])) ++pos;

                // Переход следующему символу, который является символом буквы или цифры.
                while (pos < content.Length && !char.IsLetterOrDigit(content[pos])) ++pos;
            }

            return words;
        }

        /// <summary>
        /// Подсчт количества чисел.
        /// </summary>
        /// <param name="content">Текст для анализа.</param>
        /// <returns>Количество чисел.</returns>
        private static int CountNumbers(string content)
        {
            int pos = 0, numbers = 0;

            // Переход следующему символу, который является символом цифры.
            while (pos < content.Length && !char.IsDigit(content[pos])) ++pos;

            while (pos < content.Length)
            {
                ++numbers;

                // Переход к следующему символу, который не является символом цифры.
                while (pos < content.Length && char.IsDigit(content[pos])) ++pos;

                // Переход следующему символу, который является символом цифры.
                while (pos < content.Length && !char.IsDigit(content[pos])) ++pos;
            }

            return numbers;
        }

        /// <summary>
        /// Подсчт количества символов.
        /// </summary>
        /// <param name="content">Текст для анализа.</param>
        /// <returns>Количество символов.</returns>
        private static int CountSymbols(string content)
        {
            int pos = 0, symbols = 0;
            SkipWhitespaces(content, ref pos);

            while (pos < content.Length)
            {
                while (pos < content.Length && !char.IsWhiteSpace(content[pos++])) ++symbols;
                SkipWhitespaces(content, ref pos);
            }

            return symbols;
        }
    }
}
