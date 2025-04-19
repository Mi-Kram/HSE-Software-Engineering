using Microsoft.Extensions.DependencyInjection;
using SE_HW_02.Entities.Events;
using SE_HW_02.Entities.Models;
using SE_HW_02.UseCases.Animals;

namespace SE_HW_02.UseCases.Feeding
{
    /// <summary>
    /// Сервис расписаний кормления.
    /// </summary>
    public class FeedingOrganizationService : IFeedingOrganizationService
    {
        public event EventHandler<FeedingTimeEvent>? OnFeedingTime;

        /// <summary>
        /// Словарь расписания кормления животных <время: [идентификаторы животных]>.
        /// </summary>
        private Dictionary<TimeOnly, List<int>> schedule;

        private IFeedingScheduleRepository feedingRepository;
        private IAnimalRepository animalRepository;

        /// <summary>
        /// Время на которое срабатывает таймер.
        /// </summary>
        private TimeOnly targetTime;

        /// <summary>
        /// Таймер проверки кормления животных.
        /// </summary>
        private readonly Timer timer;

        private FeedingOrganizationService(
            IDictionary<TimeOnly, List<int>> dict,
            IFeedingScheduleRepository feedingRepository,
            IAnimalRepository animalRepository)
        {
            schedule = new(dict);
            this.feedingRepository = feedingRepository;
            this.animalRepository = animalRepository;

            // Запуск таймера проверки кормления животных.
            TimeOnly initialValue = TimeOnly.FromDateTime(DateTime.Now).Add(-TimeSpan.FromSeconds(1));
            targetTime = new TimeOnly(initialValue.Hour, initialValue.Minute, initialValue.Second);
            timer = new Timer(OnTimerElapsed, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        /// <summary>
        /// Создание и инициализация сервиса.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static FeedingOrganizationService Initialize(IServiceProvider provider)
        {
            IFeedingScheduleRepository repository = provider.GetRequiredService<IFeedingScheduleRepository>();

            return new FeedingOrganizationService(
                repository.GetAll().GroupBy(x => x.Time).ToDictionary(x => GetKey(x.Key), x => x.Select(t => t.ID).ToList()),
                repository,
                provider.GetRequiredService<IAnimalRepository>());
        }

        public IEnumerable<FeedingSchedule> GetAll()
        {
            return feedingRepository.GetAll();
        }

        public FeedingSchedule? Get(int id)
        {
            return feedingRepository.Get(id);
        }

        public int? Add(FeedingSchedule item)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));

            // Проверка корректности входных данных.
            if (animalRepository.Get(item.AnimalID) == null) return null;

            // Создание расписание кормления.
            int? id = feedingRepository.Add(item);
            if (id == null) return null;

            // Добавление расписания в словарь.
            TimeOnly key = GetKey(item.Time);
            if (schedule.ContainsKey(key)) schedule[key].Add(item.ID);
            else schedule[key] = new List<int>() { item.ID };

            return id;
        }

        public bool Remove(int id)
        {
            // Проверка существования расписания кормления.
            FeedingSchedule? item = feedingRepository.Get(id);
            if (item == null) return false;

            // Удаление расписания кормления.
            if (!feedingRepository.Remove(id)) return false;

            // Удаление расписания кормления из словаря.
            TimeOnly key = GetKey(item.Time);
            if (!schedule.TryGetValue(key, out List<int>? lst)) return false;
            lst.Remove(id);
            if (lst.Count == 0) schedule.Remove(key);

            return true;
        }

        public bool Update(int id, FeedingSchedule item)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));

            // Проверка существования расписания кормления.
            FeedingSchedule? old = feedingRepository.Get(id);
            if (old == null) return false;

            // Если идентификаторы животных не совпадают, проверить существования животного.
            if (old.AnimalID != item.AnimalID)
            {
                if (animalRepository.Get(item.AnimalID) == null) return false;
            }

            // Старый ключ словаря расписания кормления.
            TimeOnly oldKey = GetKey(old.Time);
            if (!schedule.ContainsKey(oldKey)) return false;
            if (!feedingRepository.Update(id, item)) return false;

            // Новый ключ словаря расписания кормления.
            TimeOnly key = GetKey(item.Time);

            // Если они совпадают, то обновление прошло успешно.
            if (oldKey == key) return true;

            // Удаляем из словаря старое расписание.
            schedule[oldKey].Remove(id);
            if (schedule[oldKey].Count == 0) schedule.Remove(oldKey);

            // Добавляем в словарь новое расписание.
            if (schedule.ContainsKey(key)) schedule[key].Add(item.ID);
            else schedule[key] = new List<int>() { item.ID };

            return true;
        }

        /// <summary>
        /// Обработчик проверки расписания кормления.
        /// </summary>
        private void OnTimerElapsed(object? _)
        {
            // Получить текущее время и проверить наличия кормления.
            targetTime = targetTime.Add(TimeSpan.FromSeconds(1));
            if (!schedule.TryGetValue(targetTime, out List<int>? lst)) return;

            // Для всех кормлений вызвать событие.
            foreach (int id in new List<int>(lst))
            {
                OnFeedingTime?.Invoke(this, new FeedingTimeEvent(id));
            }
        }

        /// <summary>
        /// Получение ключа словаря по времени.
        /// </summary>
        /// <param name="time">Время ключа.</param>
        /// <returns>Ключ словаря.</returns>
        private static TimeOnly GetKey(TimeOnly time)
        {
            return new TimeOnly(time.Hour, time.Minute, time.Second);
        }
    }
}
