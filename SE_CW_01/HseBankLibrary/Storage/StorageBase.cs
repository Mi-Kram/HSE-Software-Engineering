using HseBankLibrary.Exceptions;
using HseBankLibrary.Models.Domain;
using HseBankLibrary.Storage.AutoIncrement;
using ImportExportLibrary;
using HseBankLibrary.Factories;

namespace HseBankLibrary.Storage
{
    /// <summary>
    /// Базовая реализация репозитория.
    /// </summary>
    /// <typeparam name="T_ID">Тип идентификатора сущности.</typeparam>
    /// <typeparam name="T">Тип сущности.</typeparam>
    /// <typeparam name="T_DTO">Тип DTO сущности.</typeparam>
    public abstract class StorageBase<T_ID, T, T_DTO>(
        IDataParser<T_DTO> parser, 
        IAutoIncrement<T_ID> autoIncrement,
        IDomainFactory<T, T_DTO> factory
        ) : AutoIncrementStorage<T_ID, T, T_DTO>(autoIncrement) 
        where T : IEntity<T_ID, T_DTO> where T_ID : notnull
    {
        private readonly IDataParser<T_DTO> parser = parser ?? throw new ArgumentNullException(nameof(parser));
        private readonly IDomainFactory<T, T_DTO> factory = factory ?? throw new ArgumentNullException(nameof(factory));

        /// <summary>
        /// Чтение данных из репозитория.
        /// </summary>
        /// <param name="parser">Парсер для обработки прочитанных данных.</param>
        /// <param name="factory">Фабрика для создания новых сущностей.</param>
        /// <returns>Прочитанные данные.</returns>
        /// <exception cref="ReadDataException"></exception>
        protected abstract IEnumerable<T> ReadData(IDataParser<T_DTO> parser, IDomainFactory<T, T_DTO> factory);

        /// <summary>
        /// Запись данных в файл.
        /// </summary>
        /// <param name="collection">Данные для записи.</param>
        /// <param name="parser">Парсер для обработки данных для записи.</param>
        /// <exception cref="WriteDataException"></exception>
        protected abstract void WriteData(IEnumerable<T> collection, IDataParser<T_DTO> parser);

        public override T_ID Add(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            IEnumerable<T> collection = ReadData(parser, factory);

            T_ID id = GetNextID();
            item.ID = id;
            collection = collection.Append(item);

            WriteData(collection, parser);
            return id;
        }

        public override IEnumerable<T> GetAll()
        {
            return ReadData(parser, factory);
        }

        public override bool Delete(T_ID id)
        {
            IEnumerable<T> collection = ReadData(parser, factory);
            bool result = false;

            collection = collection.Where(x =>
            {
                if (!EqualityComparer<T_ID>.Default.Equals(x.ID, id)) return true;
                result = true;
                return false;
            });

            if (!result) return false;
            WriteData(collection, parser);
            return true;
        }

        public override T? Get(T_ID id)
        {
            IEnumerable<T> collection = ReadData(parser, factory);
            return collection.FirstOrDefault(x => EqualityComparer<T_ID>.Default.Equals(x.ID, id));
        }

        public override bool Update(T_ID id, T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            IEnumerable<T> collection = ReadData(parser, factory);
            bool result = false;

            collection = collection.Select(x =>
            {
                if (!EqualityComparer<T_ID>.Default.Equals(x.ID, id)) return x;
                result = true;
                item.ID = id;
                return item;
            });

            if (!result) return false;
            WriteData(collection, parser);
            return true;
        }

        public override void Rewrite(IStorage<T_ID, T, T_DTO> storage)
        {
            ArgumentNullException.ThrowIfNull(storage, nameof(storage));
            IEnumerable<T> collection = storage.GetAll();

            if (collection.Any(x => x == null))
            {
                throw new InvalidDataException(nameof(storage));
            }

            if (collection.GroupBy(x => x.ID).Any(x => x.Count() != 1))
            {
                throw new InvalidDataException(nameof(storage));
            }

            WriteData(collection, parser);
        }
    }
}
