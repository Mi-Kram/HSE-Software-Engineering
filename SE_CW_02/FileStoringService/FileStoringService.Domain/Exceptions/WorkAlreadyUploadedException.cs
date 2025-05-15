namespace FileStoringService.Domain.Exceptions
{
    /// <summary>
    /// Исключение, что ранее загруженном объекте.
    /// </summary>
    /// <param name="id"></param>
    public class WorkAlreadyUploadedException(int id) : Exception("Работа уже загружена")
    {
        /// <summary>
        /// id ранее загруженного объекта.
        /// </summary>
        public int ID { get; set; } = id;
    }
}
