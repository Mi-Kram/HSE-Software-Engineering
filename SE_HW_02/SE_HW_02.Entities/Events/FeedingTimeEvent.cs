namespace SE_HW_02.Entities.Events
{
    /// <summary>
    /// Информация о событии кормления животного по расписанию.
    /// </summary>
    /// <param name="FeedingScheduleID">id расписания кормления.</param>
    public record FeedingTimeEvent(int FeedingScheduleID);
}
