using SE_HW_02.Entities.Models;

namespace SE_HW_02.UseCases.Animals
{
    /// <summary>
    /// Сервис животных.
    /// </summary>
    public interface IAnimalService
    {
        /// <summary>
        /// Получение списка всех животных.
        /// </summary>
        /// <returns>Список всех животных</returns>
        IEnumerable<Animal> GetAll();

        /// <summary>
        /// Получение животного по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор животного.</param>
        /// <returns>null, если животное не найдено. Иначе сущность животного.</returns>
        Animal? Get(int id);

        /// <summary>
        /// Добавление животного.
        /// </summary>
        /// <param name="animal">Сущность животного.</param>
        /// <returns>null, если животное не удалось добавить. Иначе идентификатор животного.</returns>
        int? Add(Animal animal);

        /// <summary>
        /// Удаление животного.
        /// </summary>
        /// <param name="id">Идентификатор животного.</param>
        /// <returns>True, если удалось удалить животное, иначе - False.</returns>
        bool Remove(int id);

        /// <summary>
        /// Обновление животного.
        /// </summary>
        /// <param name="id">Идентификатор животного.</param>
        /// <param name="animal">Новая сущность животного.</param>
        /// <returns>True, если удалось обновить животное, иначе - False.</returns>
        bool Update(int id, Animal animal);
    }
}
