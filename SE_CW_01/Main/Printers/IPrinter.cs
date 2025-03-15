namespace Main.Printers
{
    /// <summary>
    /// Интерфес вывода объекта в консоль..
    /// </summary>
    public interface IPrinter<T>
    {
        /// <summary>
        /// Вывод объекта в консоль.
        /// </summary>
        /// <param name="item">Объект для вывода.</param>
        void Print(T item);
    }
}
