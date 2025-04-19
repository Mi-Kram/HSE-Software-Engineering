namespace SE_HW_02.Entities.Models.Statistics
{
    /// <summary>
    /// Информация статистики расписаний кормления.
    /// </summary>
    public class FeedingScheduleStatistics
    {
        /// <summary>
        /// Общее количество.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Количество выполненых кормлений за сегодня.
        /// </summary>
        public int CompletedAmount { get; set; }

        /// <summary>
        /// Словарь [интервал времени : количество кормлений в этом интервале].
        /// </summary>
        public Dictionary<string, int> TimeAmount { get; } = new();
    }
}
