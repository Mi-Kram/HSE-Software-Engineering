using Main.Models.ParserGenerator;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.ImportExport.Import
{
    /// <summary>
    /// Импорт данных из файла в формате JSON.
    /// </summary>
    public class ImportJsonController(ServiceProvider provider) : ImportFileCotroller(provider)
    {
        protected override IParserGenerator GetParserGenerator()
        {
            return new JsonParserGenerator();
        }
    }
}
