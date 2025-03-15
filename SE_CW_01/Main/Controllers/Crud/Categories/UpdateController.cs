using HseBankLibrary.Models.Domain;
using HseBankLibrary.Storage;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.Crud.Categories
{
    /// <summary>
    /// Обновление категории.
    /// </summary>
    public class UpdateController(ServiceProvider provider) : ICommand<CommandArguments>
    {
        private readonly Database db = provider.GetRequiredService<Database>();

        public void Execute(CommandArguments args)
        {
            // Поучение всех категорий.
            IEnumerable<Category> categories = db.Categories.GetAll();

            if (!categories.Any())
            {
                Console.WriteLine("Нет записей");
                return;
            }

            // Выбор категории для удаления..
            Console.Write("Введите ID категории (пустая строка - отмена): ");
            string? input = Console.ReadLine();

            args.AskForEnter = false;
            if (string.IsNullOrEmpty(input)) return;

            // Пока данные введены некорректно - повторять ввод.
            ulong id;
            while (!ulong.TryParse(input, out id))
            {
                Console.WriteLine("Неверный ввод!\n");
                Console.Write("Введите ID категории (пустая строка - отмена): ");
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) return;
            }

            args.AskForEnter = true;

            // Ввод названия категории.
            Console.Write("Введите новое название категории (пустая строка - отмена): ");
            string? name = Console.ReadLine();

            // Создание категории.
            while (string.IsNullOrWhiteSpace(name))
            {
                args.AskForEnter = false;
                return;
            }

            // Обновление категории.
            Category? category = categories.FirstOrDefault(x => x.ID == id);
            if (category == null) return;
            category.Name = name;
            db.Categories.Update(id, category);

            Console.WriteLine("Категория успешно обновлена");
        }
    }
}
