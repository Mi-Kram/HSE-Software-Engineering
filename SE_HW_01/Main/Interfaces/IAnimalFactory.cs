using Main.Models.AnimalArguments;
using Main.Models.Animals;

namespace Main.Interfaces
{
    /// <summary>
    /// Интерфейс фабричного метода для создания зверюшек.
    /// </summary>
    /// <typeparam name="Args">Тип параметров для создания объекта.</typeparam>
    public interface IAnimalFactory<Args> where Args : AnimalArguments
    {
        /// <summary>
        /// Метод для создания зверюшек.
        /// </summary>
        /// <param name="number">Номер объекта.</param>
        /// <param name="args">Спец. параметры.</param>
        /// <returns></returns>
        Animal Create(int number, Args args);
    }
}
