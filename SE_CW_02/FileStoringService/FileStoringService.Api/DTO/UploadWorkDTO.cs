namespace FileStoringService.Api.DTO
{
    /// <summary>
    /// DTO для загрузки работы.
    /// </summary>
    public class UploadWorkDTO
    {
        /// <summary>
        /// id владельца работы.
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Файл работы.
        /// </summary>
        public IFormFile File { get; set; } = null!;
    }
}
