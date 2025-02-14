namespace Main.Interfaces
{
    /// <summary>
    /// Для определения принадлежности наших типов к категории «инвентаризационная вещь».
    /// </summary>
    public interface IInventory
    {
        /// <summary>
        /// Номер объекта.
        /// </summary>
        int Number { get; set; }
    }
}
