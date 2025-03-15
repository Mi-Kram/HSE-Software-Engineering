namespace HseBankLibrary.Exceptions
{
    /// <summary>
    /// Исключение при записи данных.
    /// </summary>
    public class WriteDataException(Exception ex) : Exception(ex.Message, ex)
    {
    }
}
