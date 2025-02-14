using Main.Models.Animals;

namespace Main.Interfaces
{
    /// <summary>
    /// Интерфейс хранения предметов.
    /// </summary>
    public interface IInventoryStorage
    {
        /// <summary>
        /// Получить объект по номеру.
        /// </summary>
        /// <param name="id">Номер объекта.</param>
        /// <returns>Объект с указанным номером или null.</returns>
        IInventory? Get(int id);

        /// <summary>
        /// Получить все объекты.
        /// </summary>
        /// <returns>Коллекция всех объектов.</returns>
        IEnumerable<IInventory> GetAll();

        /// <summary>
        /// Получить всех животных.
        /// </summary>
        /// <returns>Коллекция всех животных.</returns>
        IEnumerable<Animal> GetAllAnimals();

        /// <summary>
        /// Создать и добавить объект.
        /// </summary>
        /// <typeparam name="Args">Тип параметров для создания объектов.</typeparam>
        /// <param name="factory">Фабрика для создания объектов.</param>
        /// <param name="args">Параметры для создания объектов.</param>
        /// <returns>Номер нового объекта.</returns>
        int Add<Args>(IInventoryFactory<Args> factory, Args args);

        /// <summary>
        /// Добавить объект.
        /// </summary>
        /// <param name="inventory">Объект для добавления.</param>
        /// <returns>Номер добавленного объекта.</returns>
        int Add(IInventory inventory);

        /// <summary>
        /// Удалить объект по номеру.
        /// </summary>
        /// <param name="id">Номер объекта.</param>
        /// <returns>True, если удалось удалить объект, иначе - False.</returns>
        bool Remove(int id);

        /// <summary>
        /// Проверка существования объекта.
        /// </summary>
        /// <param name="id">Номер объекта.</param>
        /// <returns>True, если объект существует, иначе - False.</returns>
        bool Contains(int id);

        /// <summary>
        /// Получить количество объектов.
        /// </summary>
        /// <returns>Количество объектов.</returns>
        int GetCount();
    }
}
