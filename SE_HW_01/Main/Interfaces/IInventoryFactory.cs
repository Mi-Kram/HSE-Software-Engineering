namespace Main.Interfaces
{
    /// <summary>
    /// Интерфейс фабричного метода для создания предметов.
    /// </summary>
    /// <typeparam name="Args">Тип параметров для создания объекта.</typeparam>
    public interface IInventoryFactory<Args>
    {
        /// <summary>
        /// Метод для создания предмета.
        /// </summary>
        /// <param name="number">Номер объекта.</param>
        /// <param name="args">Спец. параметры.</param>
        /// <returns></returns>
        IInventory Create(int number, Args args);
    }
}
