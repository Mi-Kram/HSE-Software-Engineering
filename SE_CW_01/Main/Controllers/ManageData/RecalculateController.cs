using HseBankLibrary.Models.Domain;
using HseBankLibrary.Storage;
using Main.Printers;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.ManageData
{
    /// <summary>
    /// Обработчик пересчета баланса.
    /// </summary>
    public class RecalculateController(ServiceProvider provider) : ICommand<CommandArguments>
    {
        private readonly Database db = provider.GetRequiredService<Database>();

        public void Execute(CommandArguments args)
        {
            // Получение вссех счетов.
            IEnumerable<BankAccount> accounts = db.BankAccounts.GetAll();

            if (!accounts.Any())
            {
                Console.WriteLine("Нет счетов");
                return;
            }

            // Вывод и выбор счета.
            BankAccountPrinter printer = new();
            BankAccount? account = GetAccount(accounts, printer);
            if (account == null)
            {
                args.AskForEnter = false;
                return;
            }

            // Вывод выбранного счета.
            Console.WriteLine("\nВыбранный счет:");
            printer.Print(account);
            Console.WriteLine();

            // Получение всех операций выбранного счета.
            IEnumerable<Operation> operations = db.Operations.GetAll()
                .Where(x => x.BankAccountID == account.ID).OrderBy(x => x.Date);

            if (!operations.Any())
            {
                Console.WriteLine("Со счётом не было никаких операций");
                return;
            }

            // Вывод всех операций выбранного счета и подсчет настоящего баланса.
            Console.WriteLine("История операций:");
            decimal balance = 0;
            foreach (Operation op in operations)
            {
                string sign = op.IsIncome ? "+" : "-";
                balance += op.IsIncome ? op.Amount : -op.Amount;
                Console.WriteLine($"{op.Date:dd.MM.yyyy HH:mm:ss}: {sign}{op.Amount}  -> Баланс: {balance}");
            }

            Console.WriteLine($"Итого: {balance}");

            // Если балансы совпадают - прекращение работы.
            if (balance == account.Balance) return;

            // Предложение обновить баланс.
            Console.WriteLine("Баланс на счете не совпадает с операциями.");
            while (true)
            {
                Console.Write("Обновить баланс? (1 - да, 2 - нет): ");
                string? input = Console.ReadLine()?.Trim();

                if (input == "1") // Обновить счет.
                {
                    account.Balance = balance;
                    Console.WriteLine("\nБаланс обновлён");
                    break;
                }
                if (input == "2") // Не обновлять счет.
                {
                    Console.WriteLine("\nБаланс прежним");
                    break;
                }
            }
        }

        /// <summary>
        /// Выбор счета.
        /// </summary>
        /// <param name="accounts">Все счета.</param>
        /// <param name="printer">Принтер счета.</param>
        /// <returns></returns>
        private static BankAccount? GetAccount(IEnumerable<BankAccount> accounts, BankAccountPrinter printer)
        {
            // Вывод всех счетов.
            foreach (BankAccount account in accounts) printer.Print(account);

            // Выбор счета.
            Console.Write("Выберите счет (id, пустая строка - отмена): ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) return null;

            // Повторять ввод, пока данные некорректны.
            ulong id;
            while (!ulong.TryParse(input, out id) || !accounts.Any(x => x.ID == id))
            {
                Console.WriteLine("Неверный ввод!\n");
                Console.Write("Выберите счет (id, пустая строка - отмена): ");
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) return null;
            }

            return accounts.FirstOrDefault(x => x.ID == id);
        }
    }
}
