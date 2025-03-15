using HseBankLibrary.Factories;
using HseBankLibrary.Models.Domain;
using HseBankLibrary.Models.Domain.DTO;
using HseBankLibrary.Storage;
using Main.Printers;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.Crud.Operations
{
    /// <summary>
    /// Создание операции.
    /// </summary>
    public class CreateController(ServiceProvider provider) : ICommand<CommandArguments>
    {
        private readonly Database db = provider.GetRequiredService<Database>();
        private readonly OperationFactory factory = provider.GetRequiredService<OperationFactory>();

        public void Execute(CommandArguments args)
        {
            // Получение всех счетов.
            IEnumerable<BankAccount> accounts = db.BankAccounts.GetAll();
            if (!accounts.Any())
            {
                Console.WriteLine("Невозможно создать операцию: таблица счетов пустая!");
                return;
            }

            // Получение всех категорий.
            IEnumerable<Category> categories = db.Categories.GetAll();
            if (!categories.Any())
            {
                Console.WriteLine("Невозможно создать операцию: таблица категорий пустая!");
                return;
            }

            args.AskForEnter = false;

            // Ввод типа операции.
            bool? isIncome = Utils.ReadOperationType("Выберите тип операции (1 - доход, 2 - расход, пустая строка - отмена): ", 1, 2);
            if (isIncome == null) return;

            // Вывод и выбор счета.
            Console.WriteLine("\nСписок счетов:");
            BankAccountPrinter accountPrinter = new();
            foreach (BankAccount item in accounts) accountPrinter.Print(item);

            ulong? accountID = Utils.ReadUlong("Выберите счет (ID, пустая строка - отмена): ", accounts);
            if (accountID == null) return;

            // Вывод и выбор категории.
            Console.WriteLine("\nСписок категорий:");
            CategoryPrinter categoryPrinter = new();
            foreach (Category item in categories) categoryPrinter.Print(item);

            ulong? categoryID = Utils.ReadUlong("Выберите категорию (ID, пустая строка - отмена): ", categories);
            if (categoryID == null) return;

            // Ввод суммы операции.
            decimal? amount = Utils.ReadAmount("Выберите сумму операции (пустая строка - отмена): ");
            if (amount == null) return;

            // Ввод комментария.
            Console.Write("Введите комментарий операции: ");
            string description = Console.ReadLine()?.Trim() ?? string.Empty;
            args.AskForEnter = true;

            // Создание операции.
            OperationDTO dto = new()
            {
                IsIncome = isIncome.Value,
                BankAccountID = accountID.Value,
                CategoryID = categoryID.Value,
                Amount = amount.Value,
                Date = DateTime.Now,
                Description = description
            };
            db.Operations.Add(factory.Create(dto));

            Console.WriteLine("Операция успешно добавлена");

            // Изменение баланса счета.
            BankAccount account = accounts.First(x => x.ID == accountID.Value);
            account.Balance += isIncome.Value ? amount.Value : -amount.Value;
            db.BankAccounts.Update(account.ID, account);
        }
    }
}
