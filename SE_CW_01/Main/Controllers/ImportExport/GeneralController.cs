using Main.Models.Timing;
using Main.Controllers.Common;
using MenuLibrary;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.ImportExport
{
    /// <summary>
    /// Меню импорта экспорта.
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

            TimingDecorator export = new(new Export.GeneralController(provider), "ImportExport.Export", reporter);
            TimingDecorator import = new(new Import.GeneralController(provider), "ImportExport.Import", reporter);

            menu.Add(new MenuItem("Экспорт", export));
            menu.Add(new MenuItem("Импорт", import));
            menu.Add(new MenuItem("Назад", new ExitController(menu)));

            return menu;
        }
    }
}
