using MenuLibrary.Commands;
using MenuLibrary;
using Microsoft.Extensions.DependencyInjection;
using Main.Controllers.Common;
using Main.Models.Timing;

namespace Main.Controllers.ImportExport.Import
{
    /// <summary>
    /// Меню импорта разных форматов.
    /// </summary>
    public class GeneralController(ServiceProvider provider) : ICommand<CommandArguments>
    {
        private readonly ServiceProvider provider = provider;
        private readonly TimingReport reporter = provider.GetRequiredService<TimingReport>();

        public void Execute(CommandArguments args)
        {
            Menu menu = CreateMenu();
            menu.Run(false);
            args.AskForEnter = false;
        }

        private Menu CreateMenu()
        {
            Menu menu = new()
            {
                Title = "Выберите формат",
                ClearConsole = false
            };

            TimingDecorator json = new(new ImportJsonController(provider), "ImportExport.Import.JSON", reporter);
            TimingDecorator csv = new(new ImportCsvController(provider), "ImportExport.Import.CSV", reporter);
            TimingDecorator yaml = new(new ImportYamlController(provider), "ImportExport.Import.YAML", reporter);

            menu.Add(new MenuItem("Json формат", json));
            menu.Add(new MenuItem("Csv формат", csv));
            menu.Add(new MenuItem("Yaml формат", yaml));
            menu.Add(new MenuItem("Назад", new ExitController(menu)));

            return menu;
        }
    }
}
