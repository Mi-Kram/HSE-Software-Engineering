using HseBankLibrary.Models.Domain;
using HseBankLibrary.Storage;
using Main.Printers;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.Crud.Categories
{
    /// <summary>
    /// Вывод информации о категории.
    /// </summary>
    public class PrintController(ServiceProvider provider, IPrinter<Category> printer) : ICommand<CommandArguments>
    {
        private readonly Database db = provider.GetRequiredService<Database>();
        private readonly IPrinter<Category> printer = printer ?? throw new AbandonedMutexException(nameof(printer));

        public void Execute(CommandArguments args)
        {
            // Получение всех категорий.
            IEnumerable<Category> categories = db.Categories.GetAll();

            if (!categories.Any())
            {
                Console.WriteLine("Нет записей");
                return;
            }

            // Вывод категорий.
            foreach (Category category in categories) printer.Print(category);
        }
    }
}
