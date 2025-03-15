using HseBankLibrary.Models.Domain;
using HseBankLibrary.Storage.AutoIncrement;

namespace HseBankLibrary.Storage
{
    /// <summary>
    /// Репозиторий для хранения данных в оперативной памяти.
    /// </summary>
    /// <typeparam name="T_ID">Тип идентификатора сущности.</typeparam>
    /// <typeparam name="T">Тип сущности.</typeparam>
    /// <typeparam name="T_DTO">Тип DTO сущности.</typeparam>
    public class MemoryStorage<T_ID, T, T_DTO>(IAutoIncrement<T_ID> autoIncrement) : AutoIncrementStorage<T_ID, T, T_DTO>(autoIncrement)
        where T : IEntity<T_ID, T_DTO> where T_ID : notnull
    {
        /// <summary>
        /// Коллекция для хранения данных.
        /// </summary>
        private Dictionary<T_ID, T> cache = [];

        public override IEnumerable<T> GetAll()
        {
            return cache.Values;
        }

        public override T? Get(T_ID id)
        {
            return cache.GetValueOrDefault(id);
        }

        public override T_ID Add(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            T_ID id = GetNextID();
            item.ID = id;
            cache[id] = item;
            return id;
        }

        public override bool Update(T_ID id, T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (!cache.ContainsKey(id)) return false;
            item.ID = id;
            cache[id] = item;
            return true;
        }

        public override bool Delete(T_ID id)
        {
            return cache.Remove(id);
        }

        public override bool Ping()
        {
            return true;
        }

        public override void Rewrite(IStorage<T_ID, T, T_DTO> storage)
        {
            ArgumentNullException.ThrowIfNull(storage, nameof(storage));
            IEnumerable<T> collection = storage.GetAll() ?? throw new InvalidDataException(nameof(storage));

            if (collection.Any(x => x == null))
            {
                throw new InvalidDataException(nameof(storage));
            }

            if (collection.GroupBy(x => x.ID).Any(x => x.Count() != 1))
            {
                throw new InvalidDataException(nameof(storage));
            }

            cache = collection.ToDictionary(x => x.ID);
        }
    }
}
