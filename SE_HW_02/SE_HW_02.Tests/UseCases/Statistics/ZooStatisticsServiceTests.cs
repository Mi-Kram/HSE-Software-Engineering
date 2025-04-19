using Microsoft.Extensions.DependencyInjection;
using SE_HW_02.Entities.Models;
using SE_HW_02.Entities.Models.Statistics;
using SE_HW_02.Infrastructure.Repositories;
using SE_HW_02.UseCases.Animals;
using SE_HW_02.UseCases.Enclosures;
using SE_HW_02.UseCases.Feeding;
using SE_HW_02.UseCases.Statistics;

namespace SE_HW_02.Tests.UseCases.Statistics
{
    public class ZooStatisticsServiceTests
    {
        private static IServiceProvider GetServiceProvider()
        {
            ServiceCollection services = new();
            services.AddSingleton<IAnimalRepository, AnimalRepository>();
            services.AddSingleton<IEnclosureRepository, EnclosureRepository>();
            services.AddSingleton<IFeedingScheduleRepository, FeedingScheduleRepository>();
            return services.BuildServiceProvider();
        }

        [Fact]
        public void GetAnimalsStatistics_Test()
        {
            IServiceProvider provider = GetServiceProvider();
            ZooStatisticsService service = new ZooStatisticsService(provider);
            IAnimalRepository animalRepository = provider.GetRequiredService<IAnimalRepository>();
            IEnclosureRepository enclosureRepository = provider.GetRequiredService<IEnclosureRepository>();

            int? eclosure1ID = enclosureRepository.Add(new Enclosure() { AnimalsAmount = 4, AnimalsCapacity = 10, Type = "A" });
            int? eclosure2ID = enclosureRepository.Add(new Enclosure() { AnimalsAmount = 4, AnimalsCapacity = 5, Type = "A" });
            int? eclosure3ID = enclosureRepository.Add(new Enclosure() { AnimalsAmount = 4, AnimalsCapacity = 12, Type = "B" });

            Assert.NotNull(eclosure1ID);
            Assert.NotNull(eclosure2ID);
            Assert.NotNull(eclosure3ID);

            Assert.NotNull(animalRepository.Add(new Animal() { IsMale = true,  IsHealthy = true,  EnclosureID = eclosure1ID.Value, Type = "A" }));
            Assert.NotNull(animalRepository.Add(new Animal() { IsMale = true,  IsHealthy = false, EnclosureID = eclosure1ID.Value, Type = "A" }));
            Assert.NotNull(animalRepository.Add(new Animal() { IsMale = false, IsHealthy = false, EnclosureID = eclosure1ID.Value, Type = "A" }));
            Assert.NotNull(animalRepository.Add(new Animal() { IsMale = false, IsHealthy = true,  EnclosureID = eclosure1ID.Value, Type = "A" }));
            
            Assert.NotNull(animalRepository.Add(new Animal() { IsMale = true,  IsHealthy = true,  EnclosureID = eclosure2ID.Value, Type = "B" }));
            Assert.NotNull(animalRepository.Add(new Animal() { IsMale = true,  IsHealthy = false, EnclosureID = eclosure2ID.Value, Type = "B" }));
            Assert.NotNull(animalRepository.Add(new Animal() { IsMale = false, IsHealthy = false, EnclosureID = eclosure2ID.Value, Type = "B" }));
            Assert.NotNull(animalRepository.Add(new Animal() { IsMale = false, IsHealthy = true,  EnclosureID = eclosure2ID.Value, Type = "B" }));
            
            Assert.NotNull(animalRepository.Add(new Animal() { IsMale = false, IsHealthy = true, EnclosureID = eclosure3ID.Value, Type = "A" }));
            Assert.NotNull(animalRepository.Add(new Animal() { IsMale = true,  IsHealthy = true, EnclosureID = eclosure3ID.Value, Type = "A" }));
            Assert.NotNull(animalRepository.Add(new Animal() { IsMale = false, IsHealthy = true, EnclosureID = eclosure3ID.Value, Type = "B" }));
            Assert.NotNull(animalRepository.Add(new Animal() { IsMale = true,  IsHealthy = true, EnclosureID = eclosure3ID.Value, Type = "B" }));

            AnimalsStatistics? stat = service.GetAnimalsStatistics();
            Assert.NotNull(stat);

            Assert.Equal(12, stat.TotalAmount);
            Assert.Equal(27, stat.TotalCapacity);
            Assert.Equal(6, stat.MaleAmount);
            Assert.Equal(8, stat.HealthyAmount);
            Assert.Equal(2, stat.TypeAmount.Count);
            Assert.True(stat.TypeAmount.ContainsKey("A"));
            Assert.True(stat.TypeAmount.ContainsKey("B"));
            Assert.Equal(6, stat.TypeAmount["A"]);
            Assert.Equal(6, stat.TypeAmount["B"]);
        }

        [Fact]
        public void GetFeedingScheduleStatistics_Test()
        {
            IServiceProvider provider = GetServiceProvider();
            ZooStatisticsService service = new ZooStatisticsService(provider);
            IFeedingScheduleRepository scheduleRepository = provider.GetRequiredService<IFeedingScheduleRepository>();

            for (int i = 0; i < 10; i++)
            {
                Assert.NotNull(scheduleRepository.Add(new FeedingSchedule() { Time = TimeOnly.MinValue }));
            }

            for (int i = 0; i < 7; i++)
            {
                Assert.NotNull(scheduleRepository.Add(new FeedingSchedule() { Time = TimeOnly.MaxValue }));
            }

            FeedingScheduleStatistics? stat = service.GetFeedingScheduleStatistics();
            Assert.NotNull(stat);

            Assert.Equal(17, stat.Amount);
            Assert.Equal(10, stat.CompletedAmount);
            Assert.Equal(4, stat.TimeAmount.Count);

            Assert.True(stat.TimeAmount.ContainsKey("00:00-06:00"));
            Assert.True(stat.TimeAmount.ContainsKey("06:00-12:00"));
            Assert.True(stat.TimeAmount.ContainsKey("12:00-18:00"));
            Assert.True(stat.TimeAmount.ContainsKey("18:00-24:00"));

            Assert.Equal(10, stat.TimeAmount["00:00-06:00"]);
            Assert.Equal(0,  stat.TimeAmount["06:00-12:00"]);
            Assert.Equal(0,  stat.TimeAmount["12:00-18:00"]);
            Assert.Equal(7,  stat.TimeAmount["18:00-24:00"]);
        }

        [Fact]
        public void GetEnclosuresStatistics_Test()
        {
            IServiceProvider provider = GetServiceProvider();
            ZooStatisticsService service = new ZooStatisticsService(provider);
            IEnclosureRepository enclosureRepository = provider.GetRequiredService<IEnclosureRepository>();

            int? eclosure1ID = enclosureRepository.Add(new Enclosure() { AnimalsAmount = 3, AnimalsCapacity = 10, Type = "A" });
            int? eclosure2ID = enclosureRepository.Add(new Enclosure() { AnimalsAmount = 2, AnimalsCapacity = 5,  Type = "A" });
            int? eclosure3ID = enclosureRepository.Add(new Enclosure() { AnimalsAmount = 13, AnimalsCapacity = 17, Type = "B" });
            int? eclosure4ID = enclosureRepository.Add(new Enclosure() { AnimalsAmount = 9, AnimalsCapacity = 12, Type = "C" });
            int? eclosure5ID = enclosureRepository.Add(new Enclosure() { AnimalsAmount = 0, AnimalsCapacity = 3,  Type = "B" });
            int? eclosure6ID = enclosureRepository.Add(new Enclosure() { AnimalsAmount = 1, AnimalsCapacity = 9,  Type = "B" });
            int? eclosure7ID = enclosureRepository.Add(new Enclosure() { AnimalsAmount = 0, AnimalsCapacity = 6,  Type = "C" });

            Assert.NotNull(eclosure1ID);
            Assert.NotNull(eclosure2ID);
            Assert.NotNull(eclosure3ID);
            Assert.NotNull(eclosure4ID);
            Assert.NotNull(eclosure5ID);
            Assert.NotNull(eclosure6ID);
            Assert.NotNull(eclosure7ID);

            EnclosureStatistics? stat = service.GetEnclosuresStatistics();
            Assert.NotNull(stat);

            Assert.Equal(7, stat.TotalAmount);
            Assert.Equal(2, stat.EmptyAmount);
            Assert.Equal(3, stat.TypeAmount.Count);
            Assert.True(stat.TypeAmount.ContainsKey("A"));
            Assert.True(stat.TypeAmount.ContainsKey("B"));
            Assert.True(stat.TypeAmount.ContainsKey("C"));
            Assert.Equal(2, stat.TypeAmount["A"]);
            Assert.Equal(3, stat.TypeAmount["B"]);
            Assert.Equal(2, stat.TypeAmount["C"]);
        }
    }
}
