namespace ImportExportLibrary.Exceptions
{
    /// <summary>
    /// Исключение при экспорте данных.
    /// </summary>
    public class ExportException(Exception baseException) 
        : Exception(baseException?.Message, baseException)
    {
    }
}
