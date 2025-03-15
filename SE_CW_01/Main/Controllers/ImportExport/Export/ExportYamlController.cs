using ImportExportLibrary;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.ImportExport.Export
{
    /// <summary>
    /// Экспорт данных в файл в формате YAML.
    /// </summary>
    public class ExportYamlController(ServiceProvider provider) : ExportFileCotroller(provider)
    {
        protected override void Parse<T>(IEnumerable<T> collection, TextWriter textWriter)
        {
            YamlParser<T> parser = new();
            parser.Export(collection, textWriter);
        }
    }
}
