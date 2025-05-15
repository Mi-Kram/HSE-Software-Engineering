namespace FileAnalysisService.Infrastructure.Models
{
    /// <summary>
    /// Параметры для генерации облака слов.
    /// </summary>
    public class WordsCloudOptions
    {
        /// <summary>
        /// Текст, из которого будет сгенерировано облако слов. Обязательный параметр.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Формат выходного изображения: "svg" или "png".
        /// </summary>
        public string Format { get; set; } = string.Empty;

        /// <summary>
        /// Ширина изображения в пикселях.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Высота изображения в пикселях.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Цвет фона изображения. Может быть задан в формате RGB, HSL, HEX или названием цвета.
        /// </summary>
        public string BackgroundColor { get; set; } = string.Empty;

        /// <summary>
        /// Шрифт, используемый для слов.
        /// </summary>
        public string FontFamily { get; set; } = string.Empty;

        /// <summary>
        /// Толщина шрифта: "normal", "bold" и т.д.
        /// </summary>
        public string FontWeight { get; set; } = string.Empty;

        /// <summary>
        /// Название шрифта Google Fonts для загрузки. Используется только для формата SVG.
        /// </summary>
        public string LoadGoogleFonts { get; set; } = string.Empty;

        /// <summary>
        /// Размер самого большого слова (примерно).
        /// </summary>
        public int FontScale { get; set; }

        /// <summary>
        /// Метод масштабирования частоты: "linear", "sqrt" или "log". По умолчанию: "linear".
        /// </summary>
        public string Scale { get; set; } = string.Empty;

        /// <summary>
        /// Отступ между словами в пикселях.
        /// </summary>
        public int Padding { get; set; }

        /// <summary>
        /// Максимальный угол поворота слов в градусах.
        /// </summary>
        public int Rotation { get; set; }

        /// <summary>
        /// Максимальное количество слов для отображения.
        /// </summary>
        public int MaxNumWords { get; set; }

        /// <summary>
        /// Минимальная длина слова для включения в облако.
        /// </summary>
        public int MinWordLength { get; set; }

        /// <summary>
        /// Приведение регистра слов: "upper", "lower" или "none".
        /// </summary>
        public string Case { get; set; } = string.Empty;

        /// <summary>
        /// Список цветов для слов в формате JSON. Пример: ["red", "#00ff00", "rgba(0,0,255,1.0)"].
        /// </summary>
        public List<string> Colors { get; } = [];

        /// <summary>
        /// Удалять ли часто встречающиеся слова (стоп-слова). По умолчанию: false.
        /// </summary>
        public bool RemoveStopwords { get; set; } = false;

        /// <summary>
        /// Удалять ли символы и лишние знаки из слов. По умолчанию: true.
        /// </summary>
        public bool CleanWords { get; set; } = true;

        /// <summary>
        /// Двухбуквенный код языка для удаления стоп-слов.
        /// </summary>
        public string Language { get; set; } = string.Empty;

        /// <summary>
        /// Интерпретировать ли текст как список слов или фраз, разделённых запятыми.
        /// </summary>
        public bool UseWordList { get; set; }
    }
}
