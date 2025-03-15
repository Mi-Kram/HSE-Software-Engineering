using MenuLibrary.Commands;
using MenuLibrary;
using Microsoft.Extensions.DependencyInjection;
using Main.Controllers.Common;
using Main.Models.Timing;

namespace Main.Controllers.Crud
{
    /// <summary>
    /// Меню таблиц сущностей.
    /// </summary>
    public class GeneralController(ServiceProvider provider) : ICommand<CommandArguments>
    {
        private readonly ServiceProvider provider = provider;
        private readonly TimingReport reporter = provider.GetRequiredService<TimingReport>();

        public void Execute(CommandArguments args)
        {
            Menu menu = CreateTablesMenu();
            menu.Run();
            args.AskForEnter = false;
        }

        private Menu CreateTablesMenu()
        {
            Menu menu = new()
            {
                Title = "Выберите таблицу",
                ClearConsole = false
            };

            TimingDecorator accounts = new(new BankAccounts.GeneralController(provider), "CRUD.Accounts", reporter);
            TimingDecorator categories = new(new Categories.GeneralController(provider), "CRUD.Categories", reporter);
            TimingDecorator operations = new(new Operations.GeneralController(provider), "CRUD.Operations", reporter);

            menu.Add(new MenuItem("Счета", accounts));
            menu.Add(new MenuItem("Категории", categories));
            menu.Add(new MenuItem("Операции", operations));
            menu.Add(new MenuItem("Назад", new ExitController(menu)));

            return menu;
        }
    }
}
