using Main.Interfaces;
using Main.Models;
using Main.Models.AnimalArguments;
using Main.Models.Animals;
using Main.Models.Exceptions;
using Main.Models.ThingArguments;
using Main.Services;
using Main.Services.AnimalFactories;
using Main.Services.ThingFactories;
using Microsoft.Extensions.DependencyInjection;

namespace Main
{
    public class Program
    {
        public static void Main()
        {
            // Создание необходимых сервисов.
            ServiceCollection services = new();
            services.AddSingleton<IInventoryStorage, InventoryStorage>();
            using ServiceProvider provider = services.BuildServiceProvider();

            // Создание ключевых объектов - ветеринарная клиника и зоопарк.
            IVetClinic vetClinic = new RandomVetClinic(0.8f);
            Zoo zoo = new Zoo(provider)
            {
                VetClinic = vetClinic
            };

            // Добавление оъектов.
            AddAnimals(zoo);
            AddThings(zoo);

            // Начало демонстрации работы приложения.

            // Подсчёт потребляемой еды в сутки.
            Console.WriteLine($"\nНа всех животных надо {zoo.TotalFood():0.0#} кг еды в сутки");

            // Список контактных животных.
            Console.WriteLine("\nСписок контактных животных:");
            var contactAnimals = zoo.GetContactAnimals();
            foreach (Animal animal in contactAnimals)
            {
                Console.WriteLine($" - #{animal.Number}: {animal}");
            }

            // Вывод отчёта зоопарка.
            Console.WriteLine("\nОтчёт зоопарка:");
            zoo.ReportInventories(Console.Out);
        }

        private static void AddAnimals(IZoo zoo)
        {
            Console.WriteLine("Добавление животных...");

            MonkeyFactory monkeyFactory = new();
            RabbitFactory rabbitFactory = new();
            TigerFactory tigerFactory = new();
            WolfFactory wolfFactory = new();

            // Добавление зверюшек и обработка ответа.
            // Т.к. используется рандомная клиника, некоторые зверюшки не будут добавлены.
            AddAnimal(zoo, monkeyFactory, new MonkeyArguments() { Food = 1.5f, KindnessLevel = 6 });
            AddAnimal(zoo, rabbitFactory, new RabbitArguments() { Food = 0.3f, KindnessLevel = 8 });
            AddAnimal(zoo, tigerFactory, new TigerArguments() { Food = 5.2f });
            AddAnimal(zoo, wolfFactory, new WolfArguments() { Food = 3.7f });

            AddAnimal(zoo, monkeyFactory, new MonkeyArguments() { Food = 2.6f, KindnessLevel = 3 });
            AddAnimal(zoo, rabbitFactory, new RabbitArguments() { Food = 0.35f, KindnessLevel = 7 });
            AddAnimal(zoo, tigerFactory, new TigerArguments() { Food = 4.7f });
            AddAnimal(zoo, wolfFactory, new WolfArguments() { Food = 3.1f });

            AddAnimal(zoo, monkeyFactory, new MonkeyArguments() { Food = 2.0f, KindnessLevel = 2 });
            AddAnimal(zoo, monkeyFactory, new MonkeyArguments() { Food = 1.9f, KindnessLevel = 9 });
        }

        private static void AddAnimal<Args>(IZoo zoo, IAnimalFactory<Args> factory, Args args) where Args : AnimalArguments
        {
            // Добавление животного и обработка резульатата.
            try
            {
                int number = zoo.AddAnimal(factory, args);
                Console.WriteLine($" - Животное принято в зоопарк с номером {number}");
            }
            catch (AnimalDiseaseException)
            {
                Console.WriteLine(" - Животное не принято в зоопарк из-за состояния здоровья");
            }
            catch (VetClinicNotSetException)
            {
                Console.WriteLine(" - Животное не принято в зоопарк из-за отсутствия ветеринарной клиники");
            }
        }

        private static void AddThings(Zoo zoo)
        {
            // Добавление предметов и обработка ответа.

            Console.WriteLine("\nДобавление вещей...");

            TableFactory tableFactory = new();
            ComputerFactory computerFactory = new();

            AddThing(zoo, tableFactory, new TableArguments());
            AddThing(zoo, tableFactory, new TableArguments());
            AddThing(zoo, tableFactory, new TableArguments());
            AddThing(zoo, tableFactory, new TableArguments());
            AddThing(zoo, tableFactory, new TableArguments());

            AddThing(zoo, computerFactory, new ComputerArguments());
            AddThing(zoo, computerFactory, new ComputerArguments());
        }

        private static void AddThing<Args>(IZoo zoo, IInventoryFactory<Args> factory, Args args)
        {
            int number = zoo.AddInventory(factory, args);
            Console.WriteLine($" - Вещь принята в зоопарк с номером {number}");
        }

    }
}
