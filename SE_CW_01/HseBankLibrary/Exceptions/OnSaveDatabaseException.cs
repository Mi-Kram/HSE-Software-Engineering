namespace HseBankLibrary.Exceptions
{
    /// <summary>
    /// Исключение сохранения данных.
    /// </summary>
    public class OnSaveDatabaseException : Exception
    {
        /// <summary>
        /// Список таблиц, данные которых не получилось сохранить.
        /// </summary>
        public List<string> NameOfStorages { get; } = [];
    }
}
