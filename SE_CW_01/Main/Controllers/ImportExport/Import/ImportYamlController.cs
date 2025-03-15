using Main.Models.ParserGenerator;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.ImportExport.Import
{
    /// <summary>
    /// Импорт данных из файла в формате YAML.
    /// </summary>
    public class ImportYamlController(ServiceProvider provider) : ImportFileCotroller(provider)
    {
        protected override IParserGenerator GetParserGenerator()
        {
            return new YamlParserGenerator();
        }
    }
}
