namespace FileStoringService.Domain.Models
{
    /// <summary>
    /// Информация о работе при загрузке.
    /// </summary>
    public class UploadWorkData
    {
        private int userID;
        private string title = string.Empty;

        /// <summary>
        /// id владельца работы.
        /// </summary>
        public int UserID
        {
            get => userID;
            set => userID = value;
        }

        /// <summary>
        /// Название работы.
        /// </summary>
        public string Title
        {
            get => title;
            set => title = value ?? throw new ArgumentNullException(nameof(Title));
        }
    }
}
