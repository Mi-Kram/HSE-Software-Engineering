using Microsoft.Extensions.DependencyInjection;
using SE_HW_02.Entities.Models;
using SE_HW_02.Infrastructure.Repositories;
using SE_HW_02.UseCases.Animals;
using SE_HW_02.UseCases.Feeding;

namespace SE_HW_02.Tests.UseCases.Feeding
{
    public class FeedingOrganizationServiceTests
    {
        private static IServiceProvider GetServiceProvider()
        {
            ServiceCollection services = new();
            services.AddSingleton<IAnimalRepository, AnimalRepository>();
            services.AddSingleton<IFeedingScheduleRepository, FeedingScheduleRepository>();
            return services.BuildServiceProvider();
        }

        [Fact]
        public void Add_ThrowException()
        {
            FeedingOrganizationService service = FeedingOrganizationService.Initialize(GetServiceProvider());
            Assert.Throws<ArgumentNullException>(() => service.Add(null!));
        }

        [Fact]
        public void Add_ReturnNull()
        {
            IServiceProvider provider = GetServiceProvider();
            FeedingOrganizationService service = FeedingOrganizationService.Initialize(provider);

            Assert.Null(service.Add(new FeedingSchedule() { AnimalID = 5 }));
        }

        [Fact]
        public void Add_ReturnID()
        {
            IServiceProvider provider = GetServiceProvider();
            FeedingOrganizationService service = FeedingOrganizationService.Initialize(provider);
            IAnimalRepository animalRepository = provider.GetRequiredService<IAnimalRepository>();
            IFeedingScheduleRepository scheduleRepository = provider.GetRequiredService<IFeedingScheduleRepository>();

            int? animalID = animalRepository.Add(new Animal());
            Assert.NotNull(animalID);

            int? scheduleID = service.Add(new FeedingSchedule() { AnimalID = animalID.Value });
            Assert.NotNull(scheduleID);
            Assert.NotNull(scheduleRepository.Get(scheduleID.Value));
            Assert.Single(scheduleRepository.GetAll());
        }

        [Fact]
        public void Get_Test()
        {
            IServiceProvider provider = GetServiceProvider();
            FeedingOrganizationService service = FeedingOrganizationService.Initialize(provider);
            IAnimalRepository animalRepository = provider.GetRequiredService<IAnimalRepository>();
            IFeedingScheduleRepository scheduleRepository = provider.GetRequiredService<IFeedingScheduleRepository>();

            int? animalID = animalRepository.Add(new Animal());
            Assert.NotNull(animalID);

            int? scheduleID = service.Add(new FeedingSchedule() { AnimalID = animalID.Value });
            Assert.NotNull(scheduleID);

            Assert.NotNull(service.Get(scheduleID.Value));
        }

        [Fact]
        public void GetAll_Test()
        {
            IServiceProvider provider = GetServiceProvider();
            FeedingOrganizationService service = FeedingOrganizationService.Initialize(provider);
            IAnimalRepository animalRepository = provider.GetRequiredService<IAnimalRepository>();
            IFeedingScheduleRepository scheduleRepository = provider.GetRequiredService<IFeedingScheduleRepository>();

            int? animalID = animalRepository.Add(new Animal());
            Assert.NotNull(animalID);

            Assert.NotNull(service.Add(new FeedingSchedule() { AnimalID = animalID.Value }));
            Assert.NotNull(service.Add(new FeedingSchedule() { AnimalID = animalID.Value }));
            Assert.NotNull(service.Add(new FeedingSchedule() { AnimalID = animalID.Value }));
            Assert.NotNull(service.Add(new FeedingSchedule() { AnimalID = animalID.Value }));

            Assert.Equal(4, service.GetAll().Count());
        }

        [Fact]
        public void Remove_True()
        {
            IServiceProvider provider = GetServiceProvider();
            FeedingOrganizationService service = FeedingOrganizationService.Initialize(provider);
            IAnimalRepository animalRepository = provider.GetRequiredService<IAnimalRepository>();

            Assert.False(service.Remove(5));

            int? animalID = animalRepository.Add(new Animal());
            Assert.NotNull(animalID);

            int? scheduleID = service.Add(new FeedingSchedule() { AnimalID = animalID.Value });
            Assert.NotNull(scheduleID);


            Assert.True(service.Remove(scheduleID.Value));
            Assert.Null(service.Get(scheduleID.Value));
            Assert.Empty(service.GetAll());
        }


        [Fact]
        public void Update_ThrowException()
        {
            FeedingOrganizationService service = FeedingOrganizationService.Initialize(GetServiceProvider());
            Assert.Throws<ArgumentNullException>(() => service.Update(1, null!));
        }

        [Fact]
        public void Update_ReturnFalse()
        {
            IServiceProvider provider = GetServiceProvider();
            FeedingOrganizationService service = FeedingOrganizationService.Initialize(provider);
            IAnimalRepository animalRepository = provider.GetRequiredService<IAnimalRepository>();
            IFeedingScheduleRepository scheduleRepository = provider.GetRequiredService<IFeedingScheduleRepository>();

            Assert.False(service.Update(5, new FeedingSchedule()));

            int? animalID = animalRepository.Add(new Animal());
            Assert.NotNull(animalID);

            FeedingSchedule item = new FeedingSchedule() { AnimalID = animalID.Value };
            int? scheduleID = service.Add(item);
            Assert.NotNull(scheduleID);

            item.AnimalID = animalID.Value + 1;
            Assert.False(service.Update(scheduleID.Value, item));
        }

        [Fact]
        public void Update_ReturnTrue()
        {
            IServiceProvider provider = GetServiceProvider();
            FeedingOrganizationService service = FeedingOrganizationService.Initialize(provider);
            IAnimalRepository animalRepository = provider.GetRequiredService<IAnimalRepository>();
            IFeedingScheduleRepository scheduleRepository = provider.GetRequiredService<IFeedingScheduleRepository>();

            int? animalID = animalRepository.Add(new Animal());
            Assert.NotNull(animalID);

            FeedingSchedule item = new FeedingSchedule() { AnimalID = animalID.Value };
            int? scheduleID = service.Add(item);
            Assert.NotNull(scheduleID);

            item.Food = "Meat";
            Assert.True(service.Update(scheduleID.Value, item));
        }

        [Fact]
        public async Task OnFeedingTimeEvent_Test()
        {
            IServiceProvider provider = GetServiceProvider();
            FeedingOrganizationService service = FeedingOrganizationService.Initialize(provider);
            IAnimalRepository animalRepository = provider.GetRequiredService<IAnimalRepository>();
            IFeedingScheduleRepository scheduleRepository = provider.GetRequiredService<IFeedingScheduleRepository>();

            int? animalID = animalRepository.Add(new Animal() { Name = "Anim" });
            Assert.NotNull(animalID);

            FeedingSchedule item = new FeedingSchedule()
            {
                AnimalID = animalID.Value,
                Time = TimeOnly.FromDateTime(DateTime.Now).Add(TimeSpan.FromSeconds(1))
            };
            Console.WriteLine(item.Time);

            HashSet<int> ids = new();

            int? id = service.Add(item);
            Assert.NotNull(id);
            ids.Add(id.Value);

            id = service.Add(item);
            Assert.NotNull(id);
            ids.Add(id.Value);

            id = service.Add(item);
            Assert.NotNull(id);
            ids.Add(id.Value);

            int cnt = 0;
            service.OnFeedingTime += (_, e) =>
            {
                ++cnt;
                Assert.Contains(e.FeedingScheduleID, ids);
            };

            await Task.Delay(3000);
            Assert.Equal(3, cnt);
        }
    }
}
