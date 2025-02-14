using Main.Interfaces;
using Main.Models.AnimalArguments;
using Main.Models.Animals;
using Main.Models.Exceptions;
using Main.Models.ThingArguments;
using Main.Services;
using Main.Services.AnimalFactories;
using Main.Services.ThingFactories;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Text;

namespace Main.Tests
{
    public class ZooTests
    {
        [Fact]
        public void AddInventory_ReturnInt()
        {
            using ServiceProvider sp = GetServiceProvider();
            Zoo zoo = new Zoo(sp);

            TableFactory factory = new();
            TableArguments args = new();

            for (int i = 1; i <= 5; i++)
            {
                int number = zoo.AddInventory(factory, args);
                Assert.Equal(i, number);
            }
        }

        [Fact]
        public void AddAnimal_ReturnInt()
        {
            using ServiceProvider sp = GetServiceProvider();
            Zoo zoo = new Zoo(sp)
            {
                VetClinic = GetVetClinic(true)
            };

            TigerFactory factory = new();
            TigerArguments args = new();

            for (int i = 1; i <= 5; i++)
            {
                int number = zoo.AddAnimal(factory, args);
                Assert.Equal(i, number);
            }
        }

        [Fact]
        public void AddAnimal_ThrowAnimalDiseaseException()
        {
            using ServiceProvider sp = GetServiceProvider();
            Zoo zoo = new Zoo(sp)
            {
                VetClinic = GetVetClinic(false)
            };

            TigerFactory factory = new();
            TigerArguments args = new();

            Assert.Throws<AnimalDiseaseException>(() => zoo.AddAnimal(factory, args));
        }

        [Fact]
        public void AddAnimal_ThrowVetClinicNotSetException()
        {
            using ServiceProvider sp = GetServiceProvider();
            Zoo zoo = new Zoo(sp);

            TigerFactory factory = new();
            TigerArguments args = new();

            Assert.Throws<VetClinicNotSetException>(() => zoo.AddAnimal(factory, args));
        }

        [Fact]
        public void GetContactAnimals_ReturnIEnumerable()
        {
            using ServiceProvider sp = GetServiceProvider();
            Zoo zoo = new Zoo(sp)
            {
                VetClinic = GetVetClinic(true)
            };

            MonkeyFactory monkeyFactory = new();
            MonkeyArguments monkeyArgs = new() { KindnessLevel = 8 };

            zoo.AddAnimal(monkeyFactory, monkeyArgs);
            zoo.AddAnimal(monkeyFactory, monkeyArgs);
            zoo.AddAnimal(monkeyFactory, monkeyArgs);
            Assert.Equal(3, zoo.GetContactAnimals().Count());

            monkeyArgs.KindnessLevel = 3;
            zoo.AddAnimal(monkeyFactory, monkeyArgs);
            Assert.Equal(3, zoo.GetContactAnimals().Count());

            TigerFactory tigerFactory = new();
            TigerArguments tigerArgs = new() { };

            zoo.AddAnimal(tigerFactory, tigerArgs);
            Assert.Equal(3, zoo.GetContactAnimals().Count());
        }

        [Fact]
        public void TotalFood_ReturnFloat()
        {
            using ServiceProvider sp = GetServiceProvider();
            Zoo zoo = new Zoo(sp)
            {
                VetClinic = GetVetClinic(true)
            };

            MonkeyFactory monkeyFactory = new();
            float[] food = { 1, 10, 7.5f, 2.3f, 4.8f };

            foreach (float f in food)
            {
                zoo.AddAnimal(monkeyFactory, new MonkeyArguments() { Food = f });
            }


            float expected = food.Sum();
            float result = zoo.TotalFood();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReportInventories_ReturnString()
        {
            using ServiceProvider sp = GetServiceProvider();
            Zoo zoo = new Zoo(sp)
            {
                VetClinic = GetVetClinic(true)
            };

            MonkeyFactory monkeyFactory = new();
            RabbitFactory rabbitFactory = new();
            TigerFactory tigerFactory = new();
            WolfFactory wolfFactory = new();
            TableFactory tableFactory = new();
            ComputerFactory computerFactory = new();

            zoo.AddAnimal(monkeyFactory, new MonkeyArguments());
            zoo.AddAnimal(rabbitFactory, new RabbitArguments());
            zoo.AddAnimal(tigerFactory, new TigerArguments());
            zoo.AddAnimal(wolfFactory, new WolfArguments());

            zoo.AddInventory(tableFactory, new TableArguments());
            zoo.AddInventory(computerFactory, new ComputerArguments());

            StringBuilder sb = new();
            using StringWriter sw = new(sb);
            zoo.ReportInventories(sw);

            string report = sb.ToString();

            sb.Clear();
            sb.AppendLine("Животные (4):");
            sb.AppendLine(" - #1: Обезьяна");
            sb.AppendLine(" - #2: Кролик");
            sb.AppendLine(" - #3: Тигр");
            sb.AppendLine(" - #4: Волк");
            sb.AppendLine();
            sb.AppendLine("Вещи (2):");
            sb.AppendLine(" - #5: Стол");
            sb.AppendLine(" - #6: Компьютер");
            string expected = sb.ToString();

            Assert.Equal(expected, report);
        }

        private static IVetClinic GetVetClinic(bool result)
        {
            var vetClinicMoq = new Mock<IVetClinic>();
            vetClinicMoq.Setup(x => x.IsHealthy(It.IsAny<Animal>())).Returns(result);
            return vetClinicMoq.Object;
        }

        private static ServiceProvider GetServiceProvider()
        {
            ServiceCollection sc = new ServiceCollection();
            sc.AddSingleton<IInventoryStorage, InventoryStorage>();
            return sc.BuildServiceProvider();
        }
    }
}
