using Microsoft.Extensions.DependencyInjection;
using SE_HW_02.Entities.Models;
using SE_HW_02.UseCases.Animals;

namespace SE_HW_02.UseCases.Feeding
{
    /// <summary>
    /// Сервис кормления животных.
    /// </summary>
    public class FeedingMasterService : IFeedingMasterService
    {
        private IAnimalService animalService;
        private IFeedingOrganizationService scheduleService;

        public FeedingMasterService(IServiceProvider provider)
        {
            animalService = provider.GetRequiredService<IAnimalService>();
            scheduleService = provider.GetRequiredService<IFeedingOrganizationService>();
            scheduleService.OnFeedingTime += (_, e) => Feed(e.FeedingScheduleID);
        }

        public bool Feed(int feedingScheduleID)
        {
            // Получение информации о кормлении
            FeedingSchedule? schedule = scheduleService.Get(feedingScheduleID);
            if (schedule == null) return false;

            // Кормление животного.
            return Feed(schedule.AnimalID, schedule.Food);
        }

        public bool Feed(int animalID, string food)
        {
            // Получение информации о животном.
            Animal? animal = animalService.Get(animalID);
            if (animal == null) return false;

            // Имитация кормления.
            Console.WriteLine($"Кормление животного {animal.Name} (id={animalID}). Пища: {food}");
            return true;
        }
    }
}
