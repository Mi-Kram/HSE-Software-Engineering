using ImportExportLibrary;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.ImportExport.Export
{
    /// <summary>
    /// Экспорт данных в файл в формате CSV.
    /// </summary>
    public class ExportCsvController(ServiceProvider provider) : ExportFileCotroller(provider)
    {
        protected override void Parse<T>(IEnumerable<T> collection, TextWriter textWriter)
        {
            CsvParser<T> parser = new();
            parser.Export(collection, textWriter);
        }
    }
}
