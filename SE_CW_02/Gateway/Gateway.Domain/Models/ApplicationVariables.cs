namespace Gateway.Domain.Models
{
    /// <summary>
    /// Переменные среды для приложения.
    /// </summary>
    public static class ApplicationVariables
    {
        /// <summary>
        /// Токен доступа к сервису хранения работ.
        /// </summary>
        public static string STORAGE_TOKEN { get; } = "FGateway.STORAGE_TOKEN";

        /// <summary>
        /// Адрес сервиса хранения работ.
        /// </summary>
        public static string STORAGE_SERVER { get; } = "FGateway.STORAGE_SERVER";

        /// <summary>
        /// Токен доступа к сервису анализа работ.
        /// </summary>
        public static string ANALYSE_TOKEN { get; } = "FGateway.ANALYSE_TOKEN";

        /// <summary>
        /// Адрес сервиса анализа работ.
        /// </summary>
        public static string ANALYSE_SERVER { get; } = "FGateway.ANALYSE_SERVER";
    }
}
