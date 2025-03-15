using HseBankLibrary.Models.Domain;
using HseBankLibrary.Storage;
using Main.Printers;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.Crud.Operations
{
    /// <summary>
    /// Обновление операции.
    /// </summary>
    /// <param name="provider"></param>
    public class UpdateController(ServiceProvider provider) : ICommand<CommandArguments>
    {
        private readonly Database db = provider.GetRequiredService<Database>();

        public void Execute(CommandArguments args)
        {
            // Поучение всех операций.
            IEnumerable<Operation> operations = db.Operations.GetAll();

            if (!operations.Any())
            {
                Console.WriteLine("Нет записей");
                return;
            }

            // Выбор операции для редактирования.
            Console.Write("Введите ID операции (пустая строка - отмена): ");
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                args.AskForEnter = false;
                return;
            }

            // Получение объекта.
            Operation? operation = operations.FirstOrDefault(x => x.ID == input);
            if (operation == null)
            {
                Console.WriteLine("Не удалось найти объект объект");
                return;
            }

            // Получение всех счетов.
            IEnumerable<BankAccount> accounts = db.BankAccounts.GetAll();

            BankAccount? oldAccount = accounts.FirstOrDefault(x => x.ID == operation.BankAccountID);
            if (oldAccount == null)
            {
                Console.WriteLine("Неверное действие");
                return;
            }

            // Получение всех категорий.
            IEnumerable<Category> categories = db.Categories.GetAll();

            args.AskForEnter = false;

            // Ввод типа операции.
            bool? isIncome = Utils.ReadOperationType("Выберите тип операции (1 - доход, 2 - расход, пустая строка - не изменять): ", 1, 2);
            isIncome ??= operation.IsIncome;

            // Вывод и выбор счета.
            Console.WriteLine("\nСписок счетов:");
            BankAccountPrinter accountPrinter = new();
            foreach (BankAccount item in accounts) accountPrinter.Print(item);

            ulong? accountID = Utils.ReadUlong("Выберите счет (ID, пустая строка - не изменять): ", accounts);
            accountID ??= operation.BankAccountID;

            // Вывод и выбор категории.
            Console.WriteLine("\nСписок категорий:");
            CategoryPrinter categoryPrinter = new();
            foreach (Category item in categories) categoryPrinter.Print(item);

            ulong? categoryID = Utils.ReadUlong("Выберите категорию (ID, пустая строка - не изменять): ", categories);
            categoryID ??= operation.CategoryID;

            // Ввод суммы операции.
            decimal? amount = Utils.ReadAmount("Выберите сумму операции (пустая строка - не изменять): ");
            amount ??= operation.Amount;

            // Ввод комментария.
            Console.Write("Введите комментарий операции: ");
            string description = Console.ReadLine()?.Trim() ?? string.Empty;
            args.AskForEnter = true;

            // Изменение баланса счета.
            oldAccount.Balance += operation.IsIncome ? -operation.Amount : operation.Amount;
            db.BankAccounts.Update(oldAccount.ID, oldAccount);

            // Обновление операции.
            operation.IsIncome = isIncome.Value;
            operation.BankAccountID = accountID.Value;
            operation.CategoryID = categoryID.Value;
            operation.Amount = amount.Value;
            operation.Description = description;
            db.Operations.Update(operation.ID, operation);

            Console.WriteLine("Операция успешно обновлена");

            // Изменение баланса счета.
            BankAccount account = accounts.First(x => x.ID == accountID.Value);
            oldAccount.Balance += operation.IsIncome ? operation.Amount : -operation.Amount;
            db.BankAccounts.Update(account.ID, account);
        }
    }
}
