using HseBankLibrary.Models.Domain;
using HseBankLibrary.Storage;
using Main.Printers;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.Crud.BankAccounts
{
    /// <summary>
    /// Вывод информации о счете.
    /// </summary>
    public class PrintController(ServiceProvider provider, IPrinter<BankAccount> printer) : ICommand<CommandArguments>
    {
        private readonly Database db = provider.GetRequiredService<Database>();
        private readonly IPrinter<BankAccount> printer = printer ?? throw new AbandonedMutexException(nameof(printer));

        public void Execute(CommandArguments args)
        {
            // Получение всех счетов.
            IEnumerable<BankAccount> accounts = db.BankAccounts.GetAll();

            if (!accounts.Any())
            {
                Console.WriteLine("Нет записей");
                return;
            }

            // Вывод счетов.
            foreach (BankAccount account in accounts) printer.Print(account);
        }
    }
}
