namespace FileAnalysisService.Domain.Models
{
    /// <summary>
    /// Переменные среды для приложения.
    /// </summary>
    public static class ApplicationVariables
    {
        /// <summary>
        /// Токен доступа приложения.
        /// </summary>
        public static string TOKEN { get; } = "FAnalysis.TOKEN";

        /// <summary>
        /// Адрес главного сервера.
        /// </summary>
        public static string MAIN_SERVER { get; } = "FAnalysis.MAIN_SERVER";

        /// <summary>
        /// Строка подключения к базе данных.
        /// </summary>
        public static string DB_CONNECTION { get; } = "FAnalysis.DB_CONNECTION";

        /// <summary>
        /// библиотека для сравнения файлов.
        /// </summary>
        public static string COMPARISON_FILE { get; } = "FAnalysis.COMPARISON_FILE";

        /// <summary>
        /// Адрес хранилища изображений облака слов.
        /// </summary>
        public static string WORDS_CLOUD_SRORAGE_ADDRESS { get; } = "FAnalysis.WordsCloud.SRORAGE_ADDRESS";

        /// <summary>
        /// Логин хранилища изображений облака слов.
        /// </summary>
        public static string WORDS_CLOUD_SRORAGE_LOGIN { get; } = "FAnalysis.WordsCloud.SRORAGE_LOGIN";

        /// <summary>
        /// Пароль хранилища изображений облака слов.
        /// </summary>
        public static string WORDS_CLOUD_SRORAGE_PASSWORD { get; } = "FAnalysis.WordsCloud.SRORAGE_PASSWORD";

        /// <summary>
        /// Бакет хранилища изображений облака слов.
        /// </summary>
        public static string WORDS_CLOUD_SRORAGE_BUCKET { get; } = "FAnalysis.WordsCloud.SRORAGE_BUCKET";

        /// <summary>
        /// Поддержка ssl хранилища изображений облака слов.
        /// </summary>
        public static string WORDS_CLOUD_SRORAGE_SSL { get; } = "FAnalysis.WordsCloud.SRORAGE_SSL";
    }
}
