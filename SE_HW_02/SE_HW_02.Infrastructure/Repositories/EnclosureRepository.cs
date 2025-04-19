using SE_HW_02.Entities.Models;
using SE_HW_02.UseCases.Enclosures;

namespace SE_HW_02.Infrastructure.Repositories
{
    /// <summary>
    /// Хранилице вольеров в памяти.
    /// </summary>
    public class EnclosureRepository : IEnclosureRepository
    {
        /// <summary>
        /// Словарь вольеров по идентификатору.
        /// </summary>
        private readonly Dictionary<int, Enclosure> data = new();

        /// <summary>
        /// Счётчик идентификатора.
        /// </summary>
        private readonly IdCounter idCounter = new();

        public int Add(Enclosure enclosure)
        {
            ArgumentNullException.ThrowIfNull(enclosure, nameof(enclosure));

            // Получение уникального id.
            int id = idCounter.Next();
            while (data.ContainsKey(id)) id = idCounter.Next();

            // Сохранение объекта.
            enclosure.ID = id;
            data[id] = enclosure.Clone();

            return id;
        }

        public Enclosure? Get(int id)
        {
            return data.TryGetValue(id, out Enclosure? e) ? e.Clone() : null;
        }

        public IEnumerable<Enclosure> GetAll()
        {
            return data.Select(x => x.Value.Clone());
        }

        public bool Remove(int id)
        {
            if (!data.ContainsKey(id)) return false;
            return data.Remove(id);
        }

        public bool Update(int id, Enclosure enclosure)
        {
            ArgumentNullException.ThrowIfNull(enclosure, nameof(enclosure));

            if (!data.TryGetValue(id, out Enclosure? item)) return false;

            // Обновление объекта.
            item.Type = enclosure.Type;
            item.Size = enclosure.Size;
            item.AnimalsAmount = enclosure.AnimalsAmount;
            item.AnimalsCapacity = enclosure.AnimalsCapacity;

            return true;
        }
    }
}
