using HseBankLibrary.Models.Domain;
using HseBankLibrary.Storage;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.Crud.BankAccounts
{
    /// <summary>
    /// Удаление счёта.
    /// </summary>
    public class DeleteController(ServiceProvider provider) : ICommand<CommandArguments>
    {
        private readonly Database db = provider.GetRequiredService<Database>();

        public void Execute(CommandArguments args)
        {
            // Поучение всех счетов.
            IEnumerable<BankAccount> accounts = db.BankAccounts.GetAll();

            if (!accounts.Any())
            {
                Console.WriteLine("Нет записей");
                return;
            }

            // Выбор счета для удаления.
            Console.Write("Введите ID счета (пустая строка - отмена): ");
            string? input = Console.ReadLine();

            args.AskForEnter = false;
            if (string.IsNullOrEmpty(input)) return;

            // Пока данные введены некорректно - повторять ввод.
            ulong id;
            while (!ulong.TryParse(input, out id))
            {
                Console.WriteLine("Неверный ввод!\n");
                Console.Write("Введите ID счета (пустая строка - отмена): ");
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) return;
            }

            args.AskForEnter = true;

            // Проверка возможности удалить объект.
            if (db.Operations.GetAll().Any(x => x.BankAccountID == id))
            {
                Console.WriteLine("Нельзя удалить объект, так как на него есть ссылки.");
                return;
            }

            // Удаление объекта.
            if (db.BankAccounts.Delete(id)) Console.WriteLine("Объект успешно удалён");
            else Console.WriteLine("Не удалось удалить объект");
        }
    }
}
