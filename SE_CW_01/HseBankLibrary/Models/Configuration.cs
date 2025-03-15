namespace HseBankLibrary.Models
{
    /// <summary>
    /// Конфигурация приложения..
    /// </summary>
    public class Configuration
    {
		private string storagePath = ".";
		private string bankAccountsFileName = "bankAccounts";
		private string categoriesFileName = "categories";
		private string operationsFileName = "operations";

        /// <summary>
        /// Путь репозитория программы.
        /// </summary>
        public string StoragePath
        {
			get => storagePath;
			init => storagePath = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException(string.Empty, nameof(StoragePath));
		}

        /// <summary>
        /// Название  файла для хранения счетов.
        /// </summary>
        public string BankAccountsFileName
        {
            get => bankAccountsFileName;
            init => bankAccountsFileName = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException(string.Empty, nameof(BankAccountsFileName));
        }

        /// <summary>
        /// Название  файла для хранения категорий.
        /// </summary>
        public string CategoriesFileName
        {
            get => categoriesFileName;
            init => categoriesFileName = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException(string.Empty, nameof(CategoriesFileName));
        }

        /// <summary>
        /// Название  файла для хранения операций.
        /// </summary>
        public string OperationsFileName
        {
            get => operationsFileName;
            init => operationsFileName = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException(string.Empty, nameof(OperationsFileName));
        }
    }
}
