using HseBankLibrary.Models.Domain;
using HseBankLibrary.Storage;
using Main.Printers;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.Crud.Operations
{
    /// <summary>
    /// Вывод информации об операциях.
    /// </summary>
    public class PrintController(ServiceProvider provider, IPrinter<Operation> printer) : ICommand<CommandArguments>
    {
        private readonly Database db = provider.GetRequiredService<Database>();
        private readonly IPrinter<Operation> printer = printer ?? throw new AbandonedMutexException(nameof(printer));

        public void Execute(CommandArguments args)
        {
            // Получение всех операций.
            IEnumerable<Operation> operations = db.Operations.GetAll();

            if (!operations.Any())
            {
                Console.WriteLine("Нет записей");
                return;
            }

            // Вывод операций.
            foreach (Operation operation in operations) printer.Print(operation);
        }
    }
}
