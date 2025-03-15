using MenuLibrary.Commands;
using MenuLibrary;
using Microsoft.Extensions.DependencyInjection;
using Main.Controllers.Common;
using Main.Models.Timing;

namespace Main.Controllers.Analitics
{
    /// <summary>
    /// Меню аналитики.
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

            TimingDecorator period = new(new PeriodController(provider), "Analitics.Period", reporter);
            TimingDecorator groupByCategory = new(new GroupByCategoryController(provider), "Analitics.GroupByCategory", reporter);

            menu.Add(new MenuItem("Подсчет разницы доходов и расходов за выбранный период", period));
            menu.Add(new MenuItem("Группировка доходов и расходов по категориям", groupByCategory));
            menu.Add(new MenuItem("Назад", new ExitController(menu)));

            return menu;
        }
    }
}
