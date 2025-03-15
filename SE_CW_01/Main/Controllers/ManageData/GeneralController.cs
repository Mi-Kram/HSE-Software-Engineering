using Main.Models.Timing;
using Main.Controllers.Common;
using MenuLibrary;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.ManageData
{
    /// <summary>
    /// Меню управления данными.
    /// </summary>
    public class GeneralController(ServiceProvider provider) : ICommand<CommandArguments>
    {
        private readonly ServiceProvider provider = provider;
        private readonly TimingReport reporter = provider.GetRequiredService<TimingReport>();

        public void Execute(CommandArguments args)
        {
            Menu menu = CreateMenu();
            menu.Run();
            args.AskForEnter = false;
        }

        private Menu CreateMenu()
        {
            Menu menu = new()
            {
                Title = "Выберите действие",
                ClearConsole = false
            };

            TimingDecorator recalculate = new(new RecalculateController(provider), "ManageData.Recalculate", reporter);

            menu.Add(new MenuItem("Пересчета баланса", recalculate));
            menu.Add(new MenuItem("Назад", new ExitController(menu)));

            return menu;
        }
    }
}
