using HseBankLibrary.Exceptions;
using HseBankLibrary.Models.Domain;
using HseBankLibrary.Models.Domain.DTO;

namespace HseBankLibrary.Storage
{
	/// <summary>
	/// Сборник необходимых репозиториев для работы со счетами.
	/// </summary>
    public class Database
    {
        /// <summary>
        /// Репозиторий счетов.
        /// </summary>
        public CachedStorage<ulong, BankAccount, BankAccountDTO> BankAccounts
		{
			get;
			private set;
        }

        /// <summary>
        /// Репозиторий категорий.
        /// </summary>
        public CachedStorage<ulong, Category, CategoryDTO> Categories
		{
			get;
			private set;
		}

        /// <summary>
        /// Репозиторий операций.
        /// </summary>
        public CachedStorage<string, Operation, OperationDTO> Operations
        {
			get;
			private set;
		}

        // Вызов конструктора происходит в Database.Builder, а он в свою очередь сам следит за корректностью данных.
#pragma warning disable CS8618
        private Database() { }
#pragma warning restore CS8618

		/// <summary>
		/// Создание строителя Database.
		/// </summary>
		/// <returns></returns>
        public static Builder CreateBuilder()
		{
			return new Builder();
		}

		/// <summary>
		/// Строитель Database.
		/// </summary>
		public class Builder
		{
			private readonly Database db = new();

            /// <summary>
            /// Добавление репозитория счетов.
            /// </summary>
            /// <param name="storage">Новый репозиторий.</param>
            /// <returns>Возвращает ссылку на текущий объект.</returns>
            /// <exception cref="ArgumentNullException"></exception>
            public Builder SetBankAccountsStorage(CachedStorage<ulong, BankAccount, BankAccountDTO> storage)
			{
				db.BankAccounts = storage ?? throw new ArgumentNullException(nameof(storage));
				return this;
			}

            /// <summary>
            /// Добавление репозитория категорий.
            /// </summary>
            /// <param name="storage">Новый репозиторий.</param>
            /// <returns>Возвращает ссылку на текущий объект.</returns>
            /// <exception cref="ArgumentNullException"></exception>
            public Builder SetCategoriesStorage(CachedStorage<ulong, Category, CategoryDTO> storage)
			{
				db.Categories = storage ?? throw new ArgumentNullException(nameof(storage));
				return this;
			}

            /// <summary>
            /// Добавление репозитория операций.
            /// </summary>
            /// <param name="storage">Новый репозиторий.</param>
            /// <returns>Возвращает ссылку на текущий объект.</returns>
            /// <exception cref="ArgumentNullException"></exception>
            public Builder SetOperationsStorage(CachedStorage<string, Operation, OperationDTO> storage)
			{
				db.Operations = storage ?? throw new ArgumentNullException(nameof(storage));
				return this;
			}

			/// <summary>
			/// Создание Database.
			/// </summary>
			/// <returns>Объект типа Database.</returns>
			/// <exception cref="InvalidDataException"></exception>
			/// <exception cref="DatabaseEntitiesMismatchException"></exception>
			public Database Build()
			{
				// Проверка наличия репозиториев.
				if (db.BankAccounts == null ||
					db.Categories == null ||
					db.Operations == null) throw new InvalidDataException();

                // Проверка корректности данных.
                return IsValid() ? db : throw new DatabaseEntitiesMismatchException();
            }

            private bool IsValid()
			{
				// Счета.
                IEnumerable<BankAccount> bankAccounts = db.BankAccounts.GetAll();
                HashSet<ulong> uniqueBankAccountsID = [.. bankAccounts.Select(x => x.ID)];
				if (bankAccounts.Count() != uniqueBankAccountsID.Count) return false;

				// Категории.
                IEnumerable<Category> categories = db.Categories.GetAll();
                HashSet<ulong> uniqueCategoriesID = [.. categories.Select(x => x.ID)];
                if (categories.Count() != uniqueCategoriesID.Count) return false;

				// Операции.
                IEnumerable<Operation> operations = db.Operations.GetAll();
                HashSet<string> uniqueOperationsID = [.. operations.Select(x => x.ID)];
                if (operations.Count() != uniqueOperationsID.Count) return false;

				foreach (Operation operation in operations)
				{
					// Проверка существования нужных идентификаторов.
					if (!uniqueBankAccountsID.Contains(operation.BankAccountID)) return false;
					if (!uniqueCategoriesID.Contains(operation.CategoryID)) return false;

					// Проверка корректности значений.
					if (operation.Amount <= 0) return false;
				}

				return true;
            }
        }
	
		/// <summary>
		/// Сохранение репозиториев.
		/// </summary>
		public void SaveData()
		{
			OnSaveDatabaseException onSaveException = new();

			// Сохранить данные всех репозиториев. При неудачи записать название репозитория.
			if (!SaveStorage(BankAccounts)) onSaveException.NameOfStorages.Add(nameof(BankAccounts));
            if (!SaveStorage(Categories)) onSaveException.NameOfStorages.Add(nameof(Categories));
            if (!SaveStorage(Operations)) onSaveException.NameOfStorages.Add(nameof(Operations));

			if (onSaveException.NameOfStorages.Count != 0) throw onSaveException;
        }
	
		/// <summary>
		/// Сохранение конкретного репозитория.
		/// </summary>
		/// <param name="storage">Репозиторий для сохранения.</param>
		/// <returns>True, если удалось сохранить данные, иначе - False.</returns>
		private static bool SaveStorage<T_ID, T, T_DTO>(CachedStorage<T_ID, T, T_DTO> storage)
			where T : IEntity<T_ID, T_DTO> where T_ID : notnull
        {
			try
			{
				storage.SaveData();
				return true;
			}
			catch { }
			return false;
		}
	}
}
