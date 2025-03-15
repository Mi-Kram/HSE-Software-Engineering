using HseBankLibrary.Factories;
using HseBankLibrary.Models.Domain.DTO;
using HseBankLibrary.Storage;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.Crud.Categories
{
    /// <summary>
    /// Создание категории.
    /// </summary>
    public class CreateController(ServiceProvider provider) : ICommand<CommandArguments>
    {
        private readonly Database db = provider.GetRequiredService<Database>();
        private readonly CategoryFactory factory = provider.GetRequiredService<CategoryFactory>();

        public void Execute(CommandArguments args)
        {
            // Ввод названия категории.
            Console.Write("Введите название категории (пустая строка - отмена): ");
            string? name = Console.ReadLine();

            // Создание категории.
            while (string.IsNullOrWhiteSpace(name)) 
            {
                args.AskForEnter = false;
                return;
            }

            CategoryDTO dto = new() { Name = name };
            db.Categories.Add(factory.Create(dto));

            Console.WriteLine("Категория успешно добавлена");
        }
    }
}
