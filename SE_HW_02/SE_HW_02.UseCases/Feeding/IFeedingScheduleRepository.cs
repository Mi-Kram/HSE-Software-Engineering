using SE_HW_02.Entities.Models;

namespace SE_HW_02.UseCases.Feeding
{
    /// <summary>
    /// Хранилище расписаний кормления.
    /// </summary>
    public interface IFeedingScheduleRepository
    {
        /// <summary>
        /// Получение расписания кормления по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор расписания кормления.</param>
        /// <returns>null, если расписание кормления не найдено. Иначе сущность расписания кормления.</returns>
        FeedingSchedule? Get(int id);

        /// <summary>
        /// Получение списка всех расписаний кормления.
        /// </summary>
        /// <returns>Список всех расписаний кормления</returns>
        IEnumerable<FeedingSchedule> GetAll();

        /// <summary>
        /// Добавление расписания кормления.
        /// </summary>
        /// <param name="schedule">Сущность расписания кормления.</param>
        /// <returns>null, если расписание кормления не удалось добавить. Иначе идентификатор расписания кормления.</returns>
        int? Add(FeedingSchedule schedule);

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
        /// <param name="schedule">Новая сущность расписания кормления.</param>
        /// <returns>True, если удалось обновить расписание кормления, иначе - False.</returns>
        bool Update(int id, FeedingSchedule schedule);
    }
}
