namespace FileStoringService.Application.Interfaces
{
    /// <summary>
    /// Интерфейс хэширования файла.
    /// </summary>
    public interface IStreamHasherService
    {
        /// <summary>
        /// Хэширует поток данных. 
        /// </summary>
        /// <param name="stream">Поток данных.</param>
        /// <returns>Значение хэша. Null при ошибке.</returns>
        Task<string?> HashAsync(Stream stream);
    }
}
