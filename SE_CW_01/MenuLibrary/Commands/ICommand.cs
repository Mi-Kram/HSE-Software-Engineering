namespace MenuLibrary.Commands
{
    /// <summary>
    /// Команда для запуска при выборе пункта меню.
    /// </summary>
    /// <typeparam name="Args"></typeparam>
    public interface ICommand<Args>
    {
        /// <summary>
        /// Запускаемый метод при выборе пункта меню.
        /// </summary>
        /// <param name="args">Некий аргумент.</param>
        void Execute(Args args);
    }
}
