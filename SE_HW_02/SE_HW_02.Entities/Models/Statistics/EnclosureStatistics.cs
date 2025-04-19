namespace SE_HW_02.Entities.Models.Statistics
{
    /// <summary>
    /// Информация статистики вольеров.
    /// </summary>
    public class EnclosureStatistics
    {
        /// <summary>
        /// Общее количество.
        /// </summary>
        public int TotalAmount { get; set; }

        /// <summary>
        /// Количество пустых вольеров..
        /// </summary>
        public int EmptyAmount { get; set; }

        /// <summary>
        /// Словарь [тип вольера : количество вольеров такого типа].
        /// </summary>
        public Dictionary<string, int> TypeAmount { get; } = new();
    }
}
