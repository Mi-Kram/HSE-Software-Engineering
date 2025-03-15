using ImportExportLibrary;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.ImportExport.Export
{
    /// <summary>
    /// Экспорт данных в файл в формате JSON.
    /// </summary>
    public class ExportJsonController(ServiceProvider provider) : ExportFileCotroller(provider)
    {
        protected override void Parse<T>(IEnumerable<T> collection, TextWriter textWriter)
        {
            JsonParser<T> parser = new();
            parser.Export(collection, textWriter);
        }
    }
}
