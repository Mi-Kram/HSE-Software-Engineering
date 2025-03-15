using Main.Models.Timing;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.Statistics
{
    /// <summary>
    /// Измерение времени работы отдельных пользовательских сценариев
    /// </summary>
    public class TimingController(ServiceProvider provider) : ICommand<CommandArguments>
    {
        private readonly TimingReport reporter = provider.GetRequiredService<TimingReport>();

        public void Execute(CommandArguments args)
        {
            // Получение данных для вывода.
            List<TimingReportItem> report = reporter.GetResult();

            if (report.Count == 0)
            {
                Console.WriteLine("Нет информации для вывода");
                return;
            }

            // Максимальныя длина метки.
            int labelWidth = report.Max(x => x.Label.Length);
            labelWidth = Math.Max(labelWidth, "Метки".Length);

            // Формирование строк формата и разделителя.
            string foramt = $" {{0,-{labelWidth}}} │ {{1}}";

            char[] separatorArr = new string('─', labelWidth + 21).ToArray();
            if (labelWidth + 2 < separatorArr.Length) separatorArr[labelWidth + 2] = '┼';
            string separator = new(separatorArr);

            // Вывод данных.
            Console.WriteLine(foramt, "Метки", "Время исполнения");
            foreach (TimingReportItem item in report)
            {
                Console.WriteLine(separator);
                Print(item.Label, item.Times, foramt);
            }
        }

        /// <summary>
        /// Вывод данных об одной метке.
        /// </summary>
        /// <param name="label">Метка.</param>
        /// <param name="times">Список времени исполнений.</param>
        /// <param name="foramt">Формат вывода.</param>
        private static void Print(string label, List<TimeSpan> times, string foramt)
        {
            if (times.Count == 0)
            {
                Console.WriteLine(foramt, label, string.Empty);
                return;
            }

            // Вывод данных.
            Console.WriteLine(foramt, label, ConvertTime(times[0]));
            for (int i = 1; i < times.Count; i++)
            {
                Console.WriteLine(foramt, string.Empty, ConvertTime(times[i]));
            }
        }

        /// <summary>
        /// Преобразование TimeSpan в string.
        /// </summary>
        /// <param name="ts">Время исполнения.</param>
        /// <returns>Строка представляющая TimeSpan.</returns>
        private static string ConvertTime(TimeSpan ts)
        {
            if (ts < TimeSpan.FromSeconds(1)) return $"{ts.Milliseconds} мс";
            if (ts < TimeSpan.FromMinutes(1)) return $"{ts.Seconds:00} сек {ts.Milliseconds} мс";
            if (ts < TimeSpan.FromHours(1)) return $"{ts.Minutes:00} мин {ts.Seconds:00} сек {ts.Milliseconds} мс";
            return $"{ts.Hours} ч {ts.Minutes:00} мин {ts.Seconds:00} сек {ts.Milliseconds} мс";
        }
    }
}
