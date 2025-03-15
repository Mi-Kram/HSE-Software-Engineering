using HseBankLibrary.Factories;
using HseBankLibrary.Models.Domain.DTO;
using HseBankLibrary.Storage;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.Crud.BankAccounts
{
    /// <summary>
    /// Создание счета.
    /// </summary>
    public class CreateController(ServiceProvider provider) : ICommand<CommandArguments>
    {
        private readonly Database db = provider.GetRequiredService<Database>();
        private readonly BankAccountFactory factory = provider.GetRequiredService<BankAccountFactory>();

        public void Execute(CommandArguments args)
        {
            // Ввод названия счета.
            Console.Write("Введите название счета (пустая строка - отмена): ");
            string? name = Console.ReadLine()?.Trim();

            while (string.IsNullOrWhiteSpace(name)) 
            {
                args.AskForEnter = false;
                return;
            }

            // Создание счета.
            BankAccountDTO dto = new()
            {
                Name = name,
                Balance = 0
            };
            db.BankAccounts.Add(factory.Create(dto));

            Console.WriteLine("Счет успешно добавлен");
        }
    }
}
