using Microsoft.Extensions.DependencyInjection;
using SE_HW_02.Entities.Models;
using SE_HW_02.Infrastructure.Repositories;
using SE_HW_02.UseCases.AnimalEnclosure;
using SE_HW_02.UseCases.Animals;
using SE_HW_02.UseCases.Enclosures;
using SE_HW_02.UseCases.Feeding;

namespace SE_HW_02.Tests.UseCases.Feeding
{
    [Collection("Console")]
    public class FeedingMasterServiceTests
    {
        private static IServiceProvider GetServiceProvider()
        {
            ServiceCollection services = new();
            services.AddSingleton<IAnimalRepository, AnimalRepository>();
            services.AddSingleton<IEnclosureRepository, EnclosureRepository>();
            services.AddSingleton<IFeedingScheduleRepository, FeedingScheduleRepository>();
            services.AddSingleton<IFeedingOrganizationService>(FeedingOrganizationService.Initialize);
            services.AddSingleton<IAnimalService, AnimalService>();
            services.AddSingleton<IAnimalTransferService, AnimalTransferService>();
            return services.BuildServiceProvider();
        }

        [Fact]
        public void Feed_ByScheduleID_ReturnFalse()
        {
            IServiceProvider provider = GetServiceProvider();
            FeedingMasterService service = new FeedingMasterService(provider);

            Assert.False(service.Feed(5));
        }

        [Fact]
        public void Feed_ByScheduleID_ReturnTrue()
        {
            IServiceProvider provider = GetServiceProvider();
            FeedingMasterService service = new FeedingMasterService(provider);
            IAnimalRepository animalRepository = provider.GetRequiredService<IAnimalRepository>();
            IFeedingScheduleRepository scheduleRepository = provider.GetRequiredService<IFeedingScheduleRepository>();

            int? animal1ID = animalRepository.Add(new Animal() { Name = "Muka" });
            Assert.NotNull(animal1ID);

            int? animal2ID = animalRepository.Add(new Animal() { Name = "Ragu" });
            Assert.NotNull(animal2ID);

            int? f1ID = scheduleRepository.Add(new FeedingSchedule() { AnimalID = animal1ID.Value, Food = "Meat" });
            Assert.NotNull(f1ID);

            int? f2ID = scheduleRepository.Add(new FeedingSchedule() { AnimalID = animal2ID.Value, Food = "Fish" });
            Assert.NotNull(f2ID);

            int? f3ID = scheduleRepository.Add(new FeedingSchedule() { AnimalID = animal1ID.Value, Food = "Apple" });
            Assert.NotNull(f3ID);

            int? f4ID = scheduleRepository.Add(new FeedingSchedule() { AnimalID = animal2ID.Value, Food = "Funny Bobs" });
            Assert.NotNull(f4ID);

            Action action = () =>
            {
                Assert.True(service.Feed(f1ID.Value));
                Assert.True(service.Feed(f2ID.Value));
                Assert.True(service.Feed(f3ID.Value));
                Assert.True(service.Feed(f4ID.Value));
            };

            StringWriter sw = new StringWriter();
            sw.WriteLine($"Кормление животного Muka (id={animal1ID}). Пища: Meat");
            sw.WriteLine($"Кормление животного Ragu (id={animal2ID}). Пища: Fish");
            sw.WriteLine($"Кормление животного Muka (id={animal1ID}). Пища: Apple");
            sw.WriteLine($"Кормление животного Ragu (id={animal2ID}). Пища: Funny Bobs");

            Assert.Equal(sw.ToString().Trim(), ProcessFeed(action).Trim());
        }

        [Fact]
        public void Feed_AnimalFood_ReturnFalse()
        {
            IServiceProvider provider = GetServiceProvider();
            FeedingMasterService service = new FeedingMasterService(provider);

            Assert.False(service.Feed(5, "Meat"));
        }

        [Fact]
        public void Feed_AnimalFood_ReturnTrue()
        {
            IServiceProvider provider = GetServiceProvider();
            FeedingMasterService service = new FeedingMasterService(provider);
            IAnimalRepository animalRepository = provider.GetRequiredService<IAnimalRepository>();

            int? animal1ID = animalRepository.Add(new Animal() { Name = "Muka" });
            Assert.NotNull(animal1ID);

            int? animal2ID = animalRepository.Add(new Animal() { Name = "Ragu" });
            Assert.NotNull(animal2ID);

            Action action = () =>
            {
                Assert.True(service.Feed(animal1ID.Value, "Meat"));
                Assert.True(service.Feed(animal2ID.Value, "Fish"));
                Assert.True(service.Feed(animal1ID.Value, "Apple"));
                Assert.True(service.Feed(animal2ID.Value, "Funny Bobs"));
            };

            StringWriter sw = new StringWriter();
            sw.WriteLine($"Кормление животного Muka (id={animal1ID}). Пища: Meat");
            sw.WriteLine($"Кормление животного Ragu (id={animal2ID}). Пища: Fish");
            sw.WriteLine($"Кормление животного Muka (id={animal1ID}). Пища: Apple");
            sw.WriteLine($"Кормление животного Ragu (id={animal2ID}). Пища: Funny Bobs");

            Assert.Equal(sw.ToString().Trim(), ProcessFeed(action).Trim());
        }

        private static string ProcessFeed(Action action)
        {
            TextWriter consoleOut = Console.Out;
            using StringWriter sw = new StringWriter();

            try
            {
                Console.SetOut(sw);
                action?.Invoke();
            }
            finally
            {
                Console.SetOut(consoleOut);
            }

            return sw.ToString();
        }
    }
}
