using SE_HW_02.Entities.Models;
using SE_HW_02.UseCases.Animals;

namespace SE_HW_02.Infrastructure.Repositories
{
    /// <summary>
    /// Хранилице животных в памяти.
    /// </summary>
    public class AnimalRepository : IAnimalRepository
    {
        /// <summary>
        /// Словарь животных по идентификатору.
        /// </summary>
        private readonly Dictionary<int, Animal> data = new();

        /// <summary>
        /// Счётчик идентификатора.
        /// </summary>
        private readonly IdCounter idCounter = new();

        public int? Add(Animal animal)
        {
            ArgumentNullException.ThrowIfNull(animal, nameof(animal));

            // Получение уникального id.
            int id = idCounter.Next();
            while (data.ContainsKey(id)) id = idCounter.Next();

            // Сохранение объекта.
            animal.ID = id;
            data[id] = animal.Clone();

            return id;
        }

        public Animal? Get(int id)
        {
            return data.TryGetValue(id, out Animal? a) ? a.Clone() : null;
        }

        public IEnumerable<Animal> GetAll()
        {
            return data.Select(x => x.Value.Clone());
        }

        public bool Remove(int id)
        {
            if (!data.ContainsKey(id)) return false;
            return data.Remove(id);
        }

        public bool Update(int id, Animal animal)
        {
            ArgumentNullException.ThrowIfNull(animal, nameof(animal));

            if (!data.TryGetValue(id, out Animal? item)) return false;

            // Обновление объекта.
            item.Name = animal.Name;
            item.Type = animal.Type;
            item.FavoriteFood = animal.FavoriteFood;
            item.IsMale = animal.IsMale;
            item.IsHealthy = animal.IsHealthy;
            item.Birthday = animal.Birthday;
            item.EnclosureID = animal.EnclosureID;

            return true;
        }
    }
}
