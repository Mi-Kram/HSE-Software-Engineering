using SE_HW_02.Entities.Models.Statistics;

namespace SE_HW_02.UseCases.Statistics
{
    /// <summary>
    /// Сервис статистики.
    /// </summary>
    public interface IZooStatisticsService
    {
        /// <summary>
        /// Получение статистики о животных.
        /// </summary>
        /// <returns>Статистика животных.</returns>
        AnimalsStatistics? GetAnimalsStatistics();

        /// <summary>
        /// Получение статистики о вольерах.
        /// </summary>
        /// <returns>Статистика животных.</returns>
        EnclosureStatistics? GetEnclosuresStatistics();

        /// <summary>
        /// Получение статистики о расписаний кормления.
        /// </summary>
        /// <returns>Статистика животных.</returns>
        FeedingScheduleStatistics? GetFeedingScheduleStatistics();
    }
}
