using HseBankLibrary.Models.Domain;
using HseBankLibrary.Storage.AutoIncrement;

namespace HseBankLibrary.Storage
{
    /// <summary>
    /// Репозиторий, который сам увеличивает идентификатор.
    /// </summary>
    /// <typeparam name="T_ID"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T_DTO"></typeparam>
    public abstract class AutoIncrementStorage<T_ID, T, T_DTO>(IAutoIncrement<T_ID> autoIncrement) : IStorage<T_ID, T, T_DTO> 
        where T : IEntity<T_ID, T_DTO> where T_ID : notnull
    {
        /// <summary>
        /// Объект, который занимается увеличением идентификатора.
        /// </summary>
        private readonly IAutoIncrement<T_ID> autoIncrement = autoIncrement ?? throw new ArgumentNullException(nameof(autoIncrement));

        /// <summary>
        /// Получить уникальный идентификатор.
        /// </summary>
        /// <returns>Уникальный идентификатор.</returns>
        protected T_ID GetNextID()
        {
            T_ID id = autoIncrement.GetNext();
            while (Get(id) != null) id = GetNextID();
            return id;
        }

        public abstract T_ID Add(T item);
        public abstract bool Delete(T_ID id);
        public abstract T? Get(T_ID id);
        public abstract IEnumerable<T> GetAll();
        public abstract bool Ping();
        public abstract bool Update(T_ID id, T item);
        public abstract void Rewrite(IStorage<T_ID, T, T_DTO> storage);
    }
}
