using SE_HW_02.Entities.Models;
using SE_HW_02.UseCases.Feeding;

namespace SE_HW_02.Infrastructure.Repositories
{
    /// <summary>
    /// Хранилице расписаний кормления в памяти.
    /// </summary>
    public class FeedingScheduleRepository : IFeedingScheduleRepository
    {
        /// <summary>
        /// Словарь расписаний кормления по идентификатору.
        /// </summary>
        private readonly Dictionary<int, FeedingSchedule> data = new();

        /// <summary>
        /// Счётчик идентификатора.
        /// </summary>
        private readonly IdCounter idCounter = new();

        public int? Add(FeedingSchedule schedule)
        {
            ArgumentNullException.ThrowIfNull(schedule, nameof(schedule));

            // Получение уникального id.
            int id = idCounter.Next();
            while (data.ContainsKey(id)) id = idCounter.Next();

            // Сохранение объекта.
            schedule.ID = id;
            data[id] = schedule.Clone();

            return id;
        }

        public FeedingSchedule? Get(int id)
        {
            return data.TryGetValue(id, out FeedingSchedule? f) ? f.Clone() : null;
        }

        public IEnumerable<FeedingSchedule> GetAll()
        {
            return data.Select(x => x.Value.Clone());
        }

        public bool Remove(int id)
        {
            if (!data.ContainsKey(id)) return false;
            return data.Remove(id);
        }

        public bool Update(int id, FeedingSchedule schedule)
        {
            ArgumentNullException.ThrowIfNull(schedule, nameof(schedule));

            if (!data.TryGetValue(id, out FeedingSchedule? item)) return false;

            // Обновление объекта.
            item.AnimalID = schedule.AnimalID;
            item.Time = schedule.Time;
            item.Food = schedule.Food;

            return true;
        }
    }
}
