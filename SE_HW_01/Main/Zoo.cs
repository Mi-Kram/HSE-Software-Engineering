using Main.Interfaces;
using Main.Models.AnimalArguments;
using Main.Models.Animals;
using Main.Models.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Main
{
    public class Zoo : IZoo
    {
        private IVetClinic? vetClicnic;
        private IInventoryStorage inventoryStorage;

        public IVetClinic? VetClinic
        {
            get => vetClicnic;
            set => vetClicnic = value;
        }

        public Zoo(ServiceProvider services)
        {
            inventoryStorage = services.GetRequiredService<IInventoryStorage>();
        }

        public int AddInventory<Args>(IInventoryFactory<Args> factory, Args args)
        {
            return inventoryStorage.Add(factory, args);
        }

        /// <inheritdoc cref="IZoo.AddAnimal{Args}(IAnimalFactory{Args}, Args)"/>
        /// <exception cref="VetClinicNotSetException"></exception>
        /// <exception cref="AnimalDiseaseException"></exception>
        public int AddAnimal<Args>(IAnimalFactory<Args> factory, Args args) where Args : AnimalArguments
        {
            // Проверка существования клиники.
            if (vetClicnic == null) throw new VetClinicNotSetException();

            // Создание животного и проверка его здоровья.
            Animal animal = factory.Create(0, args);
            if (!vetClicnic.IsHealthy(animal)) throw new AnimalDiseaseException();

            // Принимаем на учёт здоровое животное.
            return inventoryStorage.Add(animal);
        }

        public IEnumerable<Animal> GetContactAnimals()
        {
            return inventoryStorage.GetAllAnimals().Where(x => x.IsContacting);

        }

        public void ReportInventories(TextWriter output)
        {
            // Вывод информации о животных.
            var animals = inventoryStorage.GetAllAnimals();
            output.WriteLine($"Животные ({animals.Count()}):");
            foreach (Animal animal in animals)
            {
                output.WriteLine($" - #{animal.Number}: {animal.ToString()}");
            }

            // Вывод информации о предметах.
            var inventories = inventoryStorage.GetAll().Except(animals);
            output.WriteLine();
            output.WriteLine($"Вещи ({inventories.Count()}):");
            foreach (IInventory inventory in inventories)
            {
                output.WriteLine($" - #{inventory.Number}: {inventory.ToString()}");
            }
        }

        public float TotalFood()
        {
            return inventoryStorage.GetAllAnimals().Sum(x => x.Food);
        }

    }
}
