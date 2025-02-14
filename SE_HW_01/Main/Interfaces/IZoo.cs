using Main.Models;
using Main.Models.AnimalArguments;
using Main.Models.Animals;

namespace Main.Interfaces
{
    /// <summary>
    /// Интерфея зоопарка.
    /// </summary>
    public interface IZoo
    {
        /// <summary>
        /// Добавление животного в зоопарк.
        /// </summary>
        /// <typeparam name="Args">Тип параметров для создания объекта.</typeparam>
        /// <param name="factory">Фабрика создания объекта.</param>
        /// <param name="args">Параметры для создания объекта.</param>
        /// <returns>Номер объекта.</returns>
        int AddAnimal<Args>(IAnimalFactory<Args> factory, Args args) where Args : AnimalArguments;

        /// <summary>
        /// Добавление предмета в зоопарк.
        /// </summary>
        /// <typeparam name="Args">Тип параметров для создания объекта.</typeparam>
        /// <param name="factory">Фабрика создания объекта.</param>
        /// <param name="args">Параметры для создания объекта.</param>
        /// <returns>Номер объекта.</returns>
        int AddInventory<Args>(IInventoryFactory<Args> factory, Args args);

        /// <summary>
        /// Подсчёт общей массы еды в кг, потребляемой всеми животными в сутки.
        /// </summary>
        /// <returns></returns>
        float TotalFood();

        /// <summary>
        /// Получение списка контактных животных.
        /// </summary>
        /// <returns>Списк контактных животных.</returns>
        IEnumerable<Animal> GetContactAnimals();

        /// <summary>
        /// Формирование отчёта зоопарка.
        /// </summary>
        /// <param name="output">Объект, куда писать отчёт.</param>
        void ReportInventories(TextWriter output);
    }
}
