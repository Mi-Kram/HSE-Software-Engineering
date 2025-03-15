using HseBankLibrary.Models.Domain;
using HseBankLibrary.Storage;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.Crud.Operations
{
    /// <summary>
    /// Удаление операции.
    /// </summary>
    public class DeleteController(ServiceProvider provider) : ICommand<CommandArguments>
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

            // Выбор операции для удаления..
            Console.Write("Введите ID операции (пустая строка - отмена): ");
            string? input = Console.ReadLine();
            
            if (string.IsNullOrEmpty(input))
            {
                args.AskForEnter = false;
                return;
            }

            // Получение и удаление объекта.
            Operation? operation = operations.FirstOrDefault(x => x.ID == input);
            if (operation == null || !db.Operations.Delete(input))
            {
                Console.WriteLine("Не удалось удалить объект");
                return;
            }
            Console.WriteLine("Объект успешно удалён");

            // Изменение баланса счета.
            BankAccount? account = db.BankAccounts.Get(operation.BankAccountID);
            if (account == null) return;
            account.Balance += operation.IsIncome ? -operation.Amount : operation.Amount;
        }
    }
}
