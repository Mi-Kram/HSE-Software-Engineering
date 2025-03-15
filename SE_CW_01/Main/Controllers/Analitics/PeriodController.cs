using HseBankLibrary.Models.Domain;
using HseBankLibrary.Storage;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace Main.Controllers.Analitics
{
    /// <summary>
    /// Подсчет разницы доходов и расходов за выбранный период.
    /// </summary>
    public class PeriodController(ServiceProvider provider) : ICommand<CommandArguments>
    {
        private readonly Database db = provider.GetRequiredService<Database>();

        public void Execute(CommandArguments args)
        {
            // Получение всех операций.
            IEnumerable<Operation> operations = db.Operations.GetAll();

            if (!operations.Any())
            {
                Console.WriteLine("В базе нет операций");
                return;
            }

            // Получение всех счетов.
            IEnumerable<BankAccount> accounts = db.BankAccounts.GetAll();

            // Даты первой и последней операции.
            DateTime min = operations.Min(x => x.Date);
            DateTime max = operations.Max(x => x.Date);

            Console.WriteLine($"Найдены операции за период {min:dd.MM.yyyy}-{max:dd.MM.yyyy}\n");

            // Запрос ввод первой даты.
            Console.Write("Введите дату, начиная с которой надо начать расчёт (dd.MM.yyyy): ");
            if (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", 
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime from))
            {
                Console.WriteLine("Неверный формат");
                return;
            }

            // Запрос ввод второй даты.
            Console.Write("Введите дату, до которой надо производить расчёт (dd.MM.yyyy): ");
            if (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", 
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime to))
            {
                Console.WriteLine("Неверный формат");
                return;
            }
            to = to.AddDays(1);

            if (to <= from)
            {
                Console.WriteLine("Неверный ввод");
                return;
            }
            Console.WriteLine();

            // Получение операций в диапазоне первой и второй даты.
            operations = operations.Where(x => from <= x.Date && x.Date < to);

            if (!operations.Any())
            {
                Console.WriteLine("За данный период операций не было");
                return;
            }

            // Создание перечисления объектов { Счёт, Доход, Расход, Разница дохода и расхода }.
            var difference = operations.GroupBy(x => x.BankAccountID)
                .ToDictionary(x => x.Key, x =>
                {
                    decimal plus = x.Where(o => o.IsIncome).Sum(x => x.Amount);
                    decimal minus = x.Where(o => !o.IsIncome).Sum(x => x.Amount);
                    return new
                    {
                        BankAccount = accounts.FirstOrDefault(o => o.ID == x.Key)?.Name ?? x.Key.ToString(),
                        Plus = plus,
                        Minus = minus,
                        Difference = plus - minus
                    };
                });

            // Вычисление ширины столбцов.
            int w1 = Math.Max("Счёт".Length, difference.Max(x => x.Value.BankAccount.Length));
            int w2 = Math.Max("Доход".Length, difference.Max(x => x.Value.Plus.ToString().Length));
            int w3 = Math.Max("Расход".Length, difference.Max(x => x.Value.Minus.ToString().Length));
            int w4 = Math.Max("Разница".Length, difference.Max(x => x.Value.Difference.ToString().Length));

            // Формирование строк формата и разделителя.
            string format = $" {{0,-{w1}}} │ {{1,-{w2}}} │ {{2,-{w3}}} │ {{3}}";
            char[] separatorArr = new string('─', w1 + w2 + w3 + w4 + 11).ToArray();
            separatorArr[w1 + 2] = '┼';
            separatorArr[w1 + w2 + 5] = '┼';
            separatorArr[w1 + w2 + w3 + 8] = '┼';
            string separator = new(separatorArr);

            // Вывод информации.
            Console.WriteLine(format, "Счёт", "Доход", "Расход", "Разница");
            foreach (var item in difference)
            {
                Console.WriteLine(separator);
                Console.WriteLine(format, item.Value.BankAccount,
                    item.Value.Plus, item.Value.Minus, item.Value.Difference);
            }
        }
    }
}
