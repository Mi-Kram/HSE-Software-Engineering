using Microsoft.Extensions.DependencyInjection;
using SE_HW_02.Entities.Models;
using SE_HW_02.Infrastructure.Repositories;
using SE_HW_02.UseCases.AnimalEnclosure;
using SE_HW_02.UseCases.Animals;
using SE_HW_02.UseCases.Enclosures;
using SE_HW_02.UseCases.Feeding;

namespace SE_HW_02.Tests.UseCases.Animals
{
    public class AnimalServiceTests
    {
        private static IServiceProvider GetServiceProvider()
        {
            ServiceCollection services = new();
            services.AddSingleton<IAnimalRepository, AnimalRepository>();
            services.AddSingleton<IEnclosureRepository, EnclosureRepository>();
            services.AddSingleton<IFeedingScheduleRepository, FeedingScheduleRepository>();
            services.AddSingleton<IAnimalTransferService, AnimalTransferService>();
            return services.BuildServiceProvider();
        }

        [Fact]
        public void Add_ThrowException()
        {
            AnimalService service = new(GetServiceProvider());
            Assert.Throws<ArgumentNullException>(() => service.Add(null!));
        }

        [Fact]
        public void Add_ReturnNull()
        {
            IServiceProvider provider = GetServiceProvider();
            AnimalService service = new(provider);
            IEnclosureRepository enclosureRepository = provider.GetRequiredService<IEnclosureRepository>();

            int? enclosureID = enclosureRepository.Add(new Enclosure() { AnimalsAmount = 5, AnimalsCapacity = 5 });
            Assert.NotNull(enclosureID);

            Animal item = new Animal()
            {
                Name = "name",
                Type = "type",
                Birthday = DateTime.Now,
                FavoriteFood = "food",
                IsMale = true,
                IsHealthy = true,
                EnclosureID = enclosureID.Value
            };

            Assert.Null(service.Add(item));
        }

        [Fact]
        public void Add_ReturnID()
        {
            IServiceProvider provider = GetServiceProvider();
            AnimalService service = new(provider);
            IEnclosureRepository enclosureRepository = provider.GetRequiredService<IEnclosureRepository>();

            int? enclosureID = enclosureRepository.Add(new Enclosure() { AnimalsCapacity = 5 });
            Assert.NotNull(enclosureID);

            Animal item = new Animal()
            {
                Name = "name",
                Type = "type",
                Birthday = DateTime.Now,
                FavoriteFood = "food",
                IsMale = true,
                IsHealthy = true,
                EnclosureID = enclosureID.Value
            };

            Assert.NotNull(service.Add(item));
            Assert.Equal(1, enclosureRepository.Get(enclosureID.Value)?.AnimalsAmount);
        }

        [Fact]
        public void Get_Test()
        {
            IServiceProvider provider = GetServiceProvider();
            AnimalService service = new(provider);
            IEnclosureRepository enclosureRepository = provider.GetRequiredService<IEnclosureRepository>();

            int? enclosureID = enclosureRepository.Add(new Enclosure() { AnimalsCapacity = 5 });
            Assert.NotNull(enclosureID);

            Animal item = new Animal()
            {
                Name = "name",
                Type = "type",
                Birthday = DateTime.Now,
                FavoriteFood = "food",
                IsMale = true,
                IsHealthy = true,
                EnclosureID = enclosureID.Value
            };


            int? id = service.Add(item);
            Assert.NotNull(id);
            Assert.NotNull(service.Get(id.Value));
        }

        [Fact]
        public void GetAll_Test()
        {
            IServiceProvider provider = GetServiceProvider();
            AnimalService service = new(provider);
            IEnclosureRepository enclosureRepository = provider.GetRequiredService<IEnclosureRepository>();

            int? enclosureID = enclosureRepository.Add(new Enclosure() { AnimalsCapacity = 5 });
            Assert.NotNull(enclosureID);

            Animal item = new Animal()
            {
                Name = "name",
                Type = "type",
                Birthday = DateTime.Now,
                FavoriteFood = "food",
                IsMale = true,
                IsHealthy = true,
                EnclosureID = enclosureID.Value
            };

            Assert.NotNull(service.Add(item));
            Assert.NotNull(service.Add(item));
            Assert.NotNull(service.Add(item));
            Assert.NotNull(service.Add(item));

            Assert.Equal(4, service.GetAll().Count());
        }

        [Fact]
        public void Remove_ReturnFalse()
        {
            IServiceProvider provider = GetServiceProvider();
            AnimalService service = new(provider);
            IEnclosureRepository enclosureRepository = provider.GetRequiredService<IEnclosureRepository>();
            IFeedingScheduleRepository feedingRepository = provider.GetRequiredService<IFeedingScheduleRepository>();

            Assert.False(service.Remove(5));

            int? enclosureID = enclosureRepository.Add(new Enclosure() { AnimalsCapacity = 5 });
            Assert.NotNull(enclosureID);

            Animal item = new Animal()
            {
                Name = "name",
                Type = "type",
                Birthday = DateTime.Now,
                FavoriteFood = "food",
                IsMale = true,
                IsHealthy = true,
                EnclosureID = enclosureID.Value
            };

            int? animalID = service.Add(item);
            Assert.NotNull(animalID);

            int? scheduleID = feedingRepository.Add(new FeedingSchedule() { AnimalID = animalID.Value });
            Assert.NotNull(scheduleID);

            Assert.False(service.Remove(animalID.Value));
        }

        [Fact]
        public void Remove_ReturnTrue()
        {
            IServiceProvider provider = GetServiceProvider();
            AnimalService service = new(provider);
            IEnclosureRepository enclosureRepository = provider.GetRequiredService<IEnclosureRepository>();

            int? enclosureID = enclosureRepository.Add(new Enclosure() { AnimalsCapacity = 5 });
            Assert.NotNull(enclosureID);

            Animal item = new Animal()
            {
                Name = "name",
                Type = "type",
                Birthday = DateTime.Now,
                FavoriteFood = "food",
                IsMale = true,
                IsHealthy = true,
                EnclosureID = enclosureID.Value
            };

            int? animalID = service.Add(item);
            Assert.NotNull(animalID);

            Assert.True(service.Remove(animalID.Value));
            Assert.Equal(0, enclosureRepository.Get(enclosureID.Value)?.AnimalsAmount);
        }


        [Fact]
        public void Update_ThrowException()
        {
            AnimalService service = new(GetServiceProvider());
            Assert.Throws<ArgumentNullException>(() => service.Update(1, null!));
        }

        [Fact]
        public void Update_ReturnFalse()
        {
            IServiceProvider provider = GetServiceProvider();
            AnimalService service = new(provider);
            IEnclosureRepository enclosureRepository = provider.GetRequiredService<IEnclosureRepository>();

            Assert.False(service.Update(1, new Animal()));

            int? enclosureID = enclosureRepository.Add(new Enclosure() { AnimalsCapacity = 5 });
            Assert.NotNull(enclosureID);

            Animal item = new Animal()
            {
                Name = "name",
                Type = "type",
                Birthday = DateTime.Now,
                FavoriteFood = "food",
                IsMale = true,
                IsHealthy = true,
                EnclosureID = enclosureID.Value
            };

            int? id = service.Add(item);
            Assert.NotNull(id);

            item.EnclosureID = enclosureID.Value + 1;
            Assert.False(service.Update(id.Value, item));
        }

        [Fact]
        public void Update_ReturnTrue()
        {
            IServiceProvider provider = GetServiceProvider();
            AnimalService service = new(provider);
            IEnclosureRepository enclosureRepository = provider.GetRequiredService<IEnclosureRepository>();

            int? enclosureID = enclosureRepository.Add(new Enclosure() { AnimalsCapacity = 5 });
            Assert.NotNull(enclosureID);

            Animal item = new Animal()
            {
                Name = "name",
                Type = "type",
                Birthday = DateTime.Now,
                FavoriteFood = "food",
                IsMale = true,
                IsHealthy = true,
                EnclosureID = enclosureID.Value
            };

            int? id = service.Add(item);
            Assert.NotNull(id);

            item.IsHealthy = false;
            Assert.True(service.Update(id.Value, item));
        }
    }
}
