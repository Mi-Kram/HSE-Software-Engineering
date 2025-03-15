namespace HseBankLibrary.Factories
{
    /// <summary>
    /// Интерфес фабрики для создания доменных моделей.
    /// </summary>
    /// <typeparam name="T">Тип доменной модели.</typeparam>
    /// <typeparam name="Args">Тип аргументов для создания доменной модели.</typeparam>
    public interface IDomainFactory<T, Args>
    {
        /// <summary>
        /// Создание доменной модели.
        /// </summary>
        /// <param name="args">Аргументы для создания доменной модели.</param>
        /// <returns>Доменную модель.</returns>
        T Create(Args args);
    }
}
