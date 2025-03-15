using MenuLibrary.Commands;
using MenuLibrary;
using Microsoft.Extensions.DependencyInjection;
using Main.Controllers.Common;
using Main.Models.Timing;

namespace Main.Controllers.ImportExport.Export
{
    /// <summary>
    /// Меню экспорта разных форматов.
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

            TimingDecorator json = new(new ExportJsonController(provider), "ImportExport.Export.JSON", reporter);
            TimingDecorator csv = new(new ExportCsvController(provider), "ImportExport.Export.CSV", reporter);
            TimingDecorator yaml = new(new ExportYamlController(provider), "ImportExport.Export.YAML", reporter);

            menu.Add(new MenuItem("Json формат", json));
            menu.Add(new MenuItem("Csv формат", csv));
            menu.Add(new MenuItem("Yaml формат", yaml));
            menu.Add(new MenuItem("Назад", new ExitController(menu)));

            return menu;
        }
    }
}
