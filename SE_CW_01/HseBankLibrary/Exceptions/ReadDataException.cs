namespace HseBankLibrary.Exceptions
{
    /// <summary>
    /// Исключение при чтении данных.
    /// </summary>
    public class ReadDataException(Exception ex) : Exception(ex.Message, ex)
    {
    }
}
