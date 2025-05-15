using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Infrastructure.Models;
using System.Net.Http.Json;

namespace FileAnalysisService.Infrastructure.Services
{
    /// <summary>
    /// Сервис для генерации облака слов.
    /// </summary>
    public class WordsCloudService : IWordsCloudService
    {
        private readonly HttpClient httpClient = new()
        {
            BaseAddress = new Uri("https://quickchart.io")
        };

        /// <summary>
        /// Генерация облака слов.
        /// </summary>
        /// <param name="data">Данные для обработки.</param>
        /// <param name="image">Картинка облака слов.</param>
        /// <returns>Content-Type.</returns>
        public async Task<string> GenerateAsync(Stream data, Stream image)
        {
            ArgumentNullException.ThrowIfNull(data, nameof(data));
            ArgumentNullException.ThrowIfNull(image, nameof(image));

            // Текст для обработки.
            using StreamReader sr = new(data, leaveOpen: true);
            string text = await sr.ReadToEndAsync();

            // Формируем параметры запроса.
            WordsCloudOptions options = GetDefaultOptions();
            var opts = new
            {
                text,
                options.LoadGoogleFonts,
                options.FontFamily,
                options.FontScale,
                options.Width,
                options.Height,
                options.Format,
            };

            // Отправляем запрос на генерацию облака изображения.
            using HttpResponseMessage response = await httpClient.PostAsJsonAsync("wordcloud", opts);
            response.EnsureSuccessStatusCode();

            // Читаем результат.
            await response.Content.CopyToAsync(image);
            image.Seek(0, SeekOrigin.Begin);
            return "image/png";
        }

        /// <summary>
        /// Формирование параметров генерации облака слов.
        /// </summary>
        /// <returns>Параметры генерации облака слов.</returns>
        private static WordsCloudOptions GetDefaultOptions()
        {
            return new WordsCloudOptions()
            {
                BackgroundColor = "transparent",
                Case = "lower",
                CleanWords = true,
                LoadGoogleFonts = "Roboto",
                FontFamily = "Roboto",
                FontScale = 25,
                FontWeight = "normal",
                Format = "png",
                Width = 1000,
                Height = 1000,
                MaxNumWords = 200,
                MinWordLength = 1,
                Padding = 1,
                RemoveStopwords = false,
                Rotation = 20,
                Scale = "linear",
                UseWordList = false
            };
        }
    }
}
