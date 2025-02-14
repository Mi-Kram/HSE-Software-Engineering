namespace Main.Interfaces
{
    /// <summary>
    /// Для определения принадлежности наших типов к категории «живых».
    /// </summary>
    public interface IAlive
    {
        /// <summary>
        /// Для учета потребляемого количества еды в кг в сутки.
        /// </summary>
        float Food { get; set; }
    }
}
