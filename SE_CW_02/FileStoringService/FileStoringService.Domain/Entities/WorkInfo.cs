namespace FileStoringService.Domain.Entities
{
    /// <summary>
    /// Сущность для хранения информации о работе.
    /// </summary>
    public class WorkInfo
    {
		private int workID;
		private int userID;
		private string title = string.Empty;
		private string hash = string.Empty;
		private DateTime uploaded = DateTime.Now;

        /// <summary>
        /// Идентификатор работы.
        /// </summary>
        public int ID
        {
            get => workID;
            set => workID = value;
        }

        /// <summary>
        /// Название работы.
        /// </summary>
        public string Title
        {
            get => title;
            set => title = value ?? throw new ArgumentNullException(nameof(Title));
        }

        /// <summary>
        /// Идентификатор владельца работы.
        /// </summary>
        public int UserID
        {
            get => userID;
            set => userID = value;
        }

        /// <summary>
        /// Хэш файла.
        /// </summary>
        public string Hash
        {
            get => hash;
            set => hash = value ?? throw new ArgumentNullException(nameof(Hash));
        }

        /// <summary>
        /// Дата и время загрузки работы.
        /// </summary>
        public DateTime Uploaded
        {
            get => uploaded;
            set => uploaded = value;
        }
    }
}
