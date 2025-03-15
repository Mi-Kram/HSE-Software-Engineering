using HseBankLibrary.Models.Domain;
using HseBankLibrary.Storage;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.Crud.BankAccounts
{
    /// <summary>
    /// Обновление счёта.
    /// </summary>
    public class UpdateController(ServiceProvider provider) : ICommand<CommandArguments>
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

            // Ввод названия счета.
            Console.Write("Введите новое название счета (пустая строка - отмена): ");
            string? name = Console.ReadLine()?.Trim();

            while (string.IsNullOrWhiteSpace(name))
            {
                args.AskForEnter = false;
                return;
            }

            // Обновление счета.
            BankAccount? account = accounts.FirstOrDefault(x => x.ID == id);
            if (account == null) return;
            account.Name = name;
            db.BankAccounts.Update(id, account);

            Console.WriteLine("Счет успешно обновлён");
        }
    }
}
