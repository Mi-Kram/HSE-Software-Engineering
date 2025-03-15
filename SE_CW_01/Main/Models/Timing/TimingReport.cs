namespace Main.Models.Timing
{
    /// <summary>
    /// Хранилище времени исполнения обработчиков.
    /// </summary>
    public class TimingReport
    {
        /// <summary>
        /// Объект для хранения времени исполнения.
        /// </summary>
        private readonly Dictionary<string, List<TimeSpan>> items = [];

        /// <summary>
        /// Лимит на количество записей для каждой метки. 0 - безлимит.
        /// </summary>
        private uint recordLimit = 100;

        /// <inheritdoc cref="recordLimit"/>
        public uint RecordLimit
        {
            get => recordLimit;
            set 
            {

                recordLimit = value;
                if (recordLimit == 0) return;

                // Если лимит изменился - удалить записи, которые нарушаю этот лимит.
                foreach (var item in items)
                {
                    if (item.Value.Count <= recordLimit) continue;
                    item.Value.RemoveRange(0, item.Value.Count - (int)recordLimit);
                }
            }
        }

        /// <summary>
        /// Добавление новой записи.
        /// </summary>
        /// <param name="label">Метка.</param>
        /// <param name="time">Время исполнения.</param>
        /// <exception cref="InvalidDataException"></exception>
        public void Add(string label, TimeSpan time)
        {
            // Проверка корректности входных данных.
            ArgumentNullException.ThrowIfNull(label, nameof(label));
            if (time.TotalMilliseconds < 0) throw new InvalidDataException();

            // Получение или создание списка времени исполнения.
            if (!items.TryGetValue(label, out List<TimeSpan>? lst))
            {
                lst = [];
                items[label] = lst;
            }

            // Добавление нового времени.
            lst.Add(time);

            // Проверка лимита.
            if (recordLimit == 0 || lst.Count <= recordLimit) return;
            lst.RemoveRange(0, lst.Count - (int)recordLimit);
        }

        /// <summary>
        /// Формирование отчета.
        /// </summary>
        /// <returns>Отчёт времени исполнения.</returns>
        public List<TimingReportItem> GetResult()
        {
            return [.. items
                .Select(x => new TimingReportItem(x.Key, [.. x.Value]))
                .OrderBy(x => x.Label)];
        }
    }
}
