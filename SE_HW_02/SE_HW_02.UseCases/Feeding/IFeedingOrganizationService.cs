using SE_HW_02.Entities.Events;
using SE_HW_02.Entities.Models;

namespace SE_HW_02.UseCases.Feeding
{
    /// <summary>
    /// Сервис расписаний кормления.
    /// </summary>
    public interface IFeedingOrganizationService
    {
        /// <summary>
        /// Событие о достижении времени кормления животного.
        /// </summary>
        event EventHandler<FeedingTimeEvent>? OnFeedingTime;

        /// <summary>
        /// Получение списка всех расписаний кормления.
        /// </summary>
        /// <returns>Список всех расписаний кормления</returns>
        IEnumerable<FeedingSchedule> GetAll();

        /// <summary>
        /// Получение расписания кормления по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор расписания кормления.</param>
        /// <returns>null, если расписание кормления не найдено. Иначе сущность расписания кормления.</returns>
        FeedingSchedule? Get(int id);

        /// <summary>
        /// Добавление расписания кормления.
        /// </summary>
        /// <param name="feedingSchedule">Сущность расписания кормления.</param>
        /// <returns>null, если расписание кормления не удалось добавить. Иначе идентификатор расписания кормления.</returns>
        int? Add(FeedingSchedule feedingSchedule);

        /// <summary>
        /// Удаление расписания кормления.
        /// </summary>
        /// <param name="id">Идентификатор расписания кормления.</param>
        /// <returns>True, если удалось удалить расписание кормления, иначе - False.</returns>
        bool Remove(int id);

        /// <summary>
        /// Обновление расписания кормления.
        /// </summary>
        /// <param name="id">Идентификатор расписания кормления.</param>
        /// <param name="feedingSchedule">Новая сущность расписания кормления.</param>
        /// <returns>True, если удалось обновить расписание кормления, иначе - False.</returns>
        bool Update(int id, FeedingSchedule feedingSchedule);
    }
}
