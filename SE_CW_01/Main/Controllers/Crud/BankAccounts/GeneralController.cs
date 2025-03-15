using Main.Models.Timing;
using Main.Controllers.Common;
using Main.Printers;
using MenuLibrary;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.Crud.BankAccounts
{
    /// <summary>
    /// Меню действий со счетами.
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

            TimingDecorator print = new(new PrintController(provider, new BankAccountPrinter()), "CRUD.Accounts.Print", reporter);
            TimingDecorator create = new(new CreateController(provider), "CRUD.Accounts.Create", reporter);
            TimingDecorator delete = new(new DeleteController(provider), "CRUD.Accounts.Delete", reporter);
            TimingDecorator update = new(new UpdateController(provider), "CRUD.Accounts.Update", reporter);

            menu.Add(new MenuItem("Просмотр счетов", print));
            menu.Add(new MenuItem("Создать счет", create));
            menu.Add(new MenuItem("Удалить счет", delete));
            menu.Add(new MenuItem("Редактировать счет", update));
            menu.Add(new MenuItem("Назад", new ExitController(menu)));

            return menu;
        }

    }
}
