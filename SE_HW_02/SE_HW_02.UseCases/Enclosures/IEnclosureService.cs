using SE_HW_02.Entities.Models;

namespace SE_HW_02.UseCases.Enclosures
{
    /// <summary>
    /// Сервис вольеров.
    /// </summary>
    public interface IEnclosureService
    {
        /// <summary>
        /// Получение списка всех вольеров.
        /// </summary>
        /// <returns>Список всех вольеров</returns>
        IEnumerable<Enclosure> GetAll();
  
        /// <summary>
        /// Получение вольера по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор вольера.</param>
        /// <returns>null, если вольер не найдено. Иначе сущность вольера.</returns>
        Enclosure? Get(int id);

        /// <summary>
        /// Добавление вольера.
        /// </summary>
        /// <param name="enclosure">Сущность вольера.</param>
        /// <returns>null, если вольер не удалось добавить. Иначе идентификатор вольера.</returns>
        int? Add(Enclosure enclosure);

        /// <summary>
        /// Удаление вольера.
        /// </summary>
        /// <param name="id">Идентификатор вольера.</param>
        /// <returns>True, если удалось удалить вольер, иначе - False.</returns>
        bool Remove(int id);

        /// <summary>
        /// Обновление вольера.
        /// </summary>
        /// <param name="id">Идентификатор вольера.</param>
        /// <param name="enclosure">Новая сущность вольера.</param>
        /// <returns>True, если удалось обновить вольер, иначе - False.</returns>
        bool Update(int id, Enclosure enclosure);
    }
}
