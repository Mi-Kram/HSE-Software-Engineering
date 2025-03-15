namespace ImportExportLibrary.Exceptions
{
    /// <summary>
    /// Исключение при импорте данных.
    /// </summary>
    public class ImportException(Exception baseException) 
        : Exception(baseException?.Message, baseException)
    {
    }
}
