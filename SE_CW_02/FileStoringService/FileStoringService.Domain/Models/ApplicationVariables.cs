namespace FileStoringService.Domain.Models
{
    /// <summary>
    /// Переменные среды для приложения.
    /// </summary>
    public static class ApplicationVariables
    {
        /// <summary>
        /// Токен доступа приложения.
        /// </summary>
        public static string TOKEN { get; } = "FStoring.TOKEN";

        /// <summary>
        /// Строка подключения к базе данных.
        /// </summary>
        public static string DB_CONNECTION { get; } = "FStoring.DB_CONNECTION";

        /// <summary>
        /// Адрес хранилища работ.
        /// </summary>
        public static string SRORAGE_ADDRESS { get; } = "FStoring.SRORAGE_ADDRESS";

        /// <summary>
        /// Логин хранилища работ.
        /// </summary>
        public static string SRORAGE_LOGIN { get; } = "FStoring.SRORAGE_LOGIN";

        /// <summary>
        /// Пароль хранилища работ.
        /// </summary>
        public static string SRORAGE_PASSWORD { get; } = "FStoring.SRORAGE_PASSWORD";

        /// <summary>
        /// Бакет хранилища работ.
        /// </summary>
        public static string SRORAGE_BUCKET { get; } = "FStoring.SRORAGE_BUCKET";

        /// <summary>
        /// Поддержка ssl хранилища работ.
        /// </summary>
        public static string SRORAGE_SSL { get; } = "FStoring.SRORAGE_SSL";
    }
}
