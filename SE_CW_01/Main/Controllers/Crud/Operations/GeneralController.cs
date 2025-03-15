using Main.Models.Timing;
using Main.Controllers.Common;
using Main.Printers;
using MenuLibrary;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.Crud.Operations
{
    /// <summary>
    /// Меню действий с операциями.
    /// </summary>
    public class GeneralController(ServiceProvider provider) : ICommand<CommandArguments>
    {
        private readonly ServiceProvider provider = provider;
        private readonly TimingReport reporter = provider.GetRequiredService<TimingReport>();

        public void Execute(CommandArguments args)
        {
            Menu menu = CreateCrudMenu();
            menu.Run();
            args.AskForEnter = false;
        }

        private Menu CreateCrudMenu()
        {
            Menu menu = new()
            {
                Title = "Выберите действие",
                ClearConsole = false
            };

            TimingDecorator print = new(new PrintController(provider, new OperationPrinter()), "CRUD.Operations.Print", reporter);
            TimingDecorator create = new(new CreateController(provider), "CRUD.Operations.Create", reporter);
            TimingDecorator delete = new(new DeleteController(provider), "CRUD.Operations.Delete", reporter);
            TimingDecorator update = new(new UpdateController(provider), "CRUD.Operations.Update", reporter);

            menu.Add(new MenuItem("Просмотр операций", print));
            menu.Add(new MenuItem("Создать операцию", create));
            menu.Add(new MenuItem("Удалить операцию", delete));
            menu.Add(new MenuItem("Редактировать операцию", update));
            menu.Add(new MenuItem("Назад", new ExitController(menu)));

            return menu;
        }

    }
}
