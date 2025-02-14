using Main.Interfaces;
using Main.Models.Animals;

namespace Main.Services
{
    public class InventoryStorage : IInventoryStorage
    {
        /// <summary>
        /// Хранилище всех объектов.
        /// </summary>
        protected Dictionary<int, IInventory> storage = new Dictionary<int, IInventory>();

        /// <summary>
        /// Неиспользованный номер в хранилище.
        /// </summary>
        private int nextID = 1;

        public int Add<Args>(IInventoryFactory<Args> factory, Args args)
        {
            IInventory inventory = factory.Create(nextID++, args);
            storage[inventory.Number] = inventory;
            return inventory.Number;
        }

        public int Add(IInventory inventory)
        {
            inventory.Number = nextID++;
            storage[inventory.Number] = inventory;
            return inventory.Number;
        }

        public IEnumerable<IInventory> GetAll()
        {
            return storage.Values;
        }

        public IEnumerable<Animal> GetAllAnimals()
        {
            return storage.Values
                .Where(x => x is Animal)
                .Cast<Animal>();
        }

        public bool Contains(int id)
        {
            return storage.ContainsKey(id);
        }

        public IInventory? Get(int id)
        {
            return storage.TryGetValue(id, out IInventory? item) ? item : null;
        }

        public bool Remove(int id)
        {
            return storage.Remove(id);
        }

        public int GetCount()
        {
            return storage.Count;
        }

    }
}
