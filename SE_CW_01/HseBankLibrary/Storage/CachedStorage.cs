using HseBankLibrary.Models.Domain;
using HseBankLibrary.Storage.AutoIncrement;

namespace HseBankLibrary.Storage
{
    /// <summary>
    /// Репозиторий для кэширования настоящего репозитория.
    /// </summary>
    /// <typeparam name="T_ID">Тип идентификатора сущности.</typeparam>
    /// <typeparam name="T">Тип сущности.</typeparam>
    /// <typeparam name="T_DTO">Тип DTO сущности.</typeparam>
    public class CachedStorage<T_ID, T, T_DTO> : MemoryStorage<T_ID, T, T_DTO>
        where T : IEntity<T_ID, T_DTO> where T_ID : notnull
    {
        /// <summary>
        /// Настоящий репозиторий.
        /// </summary>
        private readonly IStorage<T_ID, T, T_DTO> storage;

        private CachedStorage(IStorage<T_ID, T, T_DTO> storage, IAutoIncrement<T_ID> autoIncrement) : base(autoIncrement)
        {
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public static CachedStorage<T_ID, T, T_DTO> Create(IStorage<T_ID, T, T_DTO> storage, IAutoIncrement<T_ID> autoIncrement)
        {
            CachedStorage<T_ID, T, T_DTO> cachedStorage = new(storage, autoIncrement);
            cachedStorage.Rewrite(storage);
            return cachedStorage;
        }

        public override bool Ping()
        {
            return storage.Ping();
        }

        /// <summary>
        /// Сохранение данных в настоящий репозиторий.
        /// </summary>
        public void SaveData()
        {
            storage.Rewrite(this);
        }
    }
}
