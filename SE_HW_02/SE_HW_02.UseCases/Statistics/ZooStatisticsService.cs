using Microsoft.Extensions.DependencyInjection;
using SE_HW_02.Entities.Models;
using SE_HW_02.Entities.Models.Statistics;
using SE_HW_02.UseCases.Animals;
using SE_HW_02.UseCases.Enclosures;
using SE_HW_02.UseCases.Feeding;

namespace SE_HW_02.UseCases.Statistics
{
    /// <summary>
    /// Сервис статистики.
    /// </summary>
    public class ZooStatisticsService : IZooStatisticsService
    {
        private IAnimalRepository animalRepository;
        private IEnclosureRepository enclosureRepository;
        private IFeedingScheduleRepository feedingRepository;

        public ZooStatisticsService(IServiceProvider provider)
        {
            animalRepository = provider.GetRequiredService<IAnimalRepository>();
            enclosureRepository = provider.GetRequiredService<IEnclosureRepository>();
            feedingRepository = provider.GetRequiredService<IFeedingScheduleRepository>();
        }

        public AnimalsStatistics? GetAnimalsStatistics()
        {
            AnimalsStatistics stat = new();

            // Максимальная вместимость животных.
            stat.TotalCapacity = enclosureRepository.GetAll().Sum(x => x.AnimalsCapacity);

            foreach (Animal animal in animalRepository.GetAll())
            {
                // Подсчёт общего количества животных.
                ++stat.TotalAmount;

                // Подсчёт количества здоровых животных.
                if (animal.IsHealthy) ++stat.HealthyAmount;

                // Подсчёт количества животных мужского пола.
                if (animal.IsMale) ++stat.MaleAmount;

                // Подсчёт количества животных определённого вида.
                if (stat.TypeAmount.ContainsKey(animal.Type)) ++stat.TypeAmount[animal.Type];
                else stat.TypeAmount[animal.Type] = 1;
            }

            return stat;
        }

        public EnclosureStatistics? GetEnclosuresStatistics()
        {
            EnclosureStatistics stat = new();

            foreach (Enclosure enclosure in enclosureRepository.GetAll())
            {
                // Подсчёт общего количества вольеров.
                ++stat.TotalAmount;

                // Подсчёт количества пустых вольеров.
                if (enclosure.AnimalsAmount == 0) ++stat.EmptyAmount;

                // Подсчёт количества вольеров определённого вида.
                if (stat.TypeAmount.ContainsKey(enclosure.Type)) ++stat.TypeAmount[enclosure.Type];
                else stat.TypeAmount[enclosure.Type] = 1;
            }

            return stat;
        }

        public FeedingScheduleStatistics? GetFeedingScheduleStatistics()
        {
            FeedingScheduleStatistics stat = new();

            // Делители четвертей суток.
            TimeOnly quarter2 = new(6, 0, 0);
            TimeOnly quarter3 = new(12, 0, 0);
            TimeOnly quarter4 = new(18, 0, 0);

            // Количество кормлений в каждой четверти.
            int cnt1 = 0, cnt2 = 0, cnt3 = 0, cnt4 = 0;

            // Текущее время.
            TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);

            foreach (FeedingSchedule schedule in feedingRepository.GetAll())
            {
                // Подсчёт общего количества кормлений.
                ++stat.Amount;

                // Подсчёт выполненых кормлений за сегодня.
                if (schedule.Time < now) ++stat.CompletedAmount;

                // Подсчёт кормлений в кадой четверти.
                if (schedule.Time < quarter2) ++cnt1;
                else if (schedule.Time < quarter3) ++cnt2;
                else if (schedule.Time < quarter4) ++cnt3;
                else ++cnt4;
            }

            // Добавление количества кормлений в статистику.
            stat.TimeAmount.Add("00:00-06:00", cnt1);
            stat.TimeAmount.Add("06:00-12:00", cnt2);
            stat.TimeAmount.Add("12:00-18:00", cnt3);
            stat.TimeAmount.Add("18:00-24:00", cnt4);

            return stat;
        }
    }
}
