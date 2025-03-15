using ImportExportLibrary;

namespace Main.Models.ParserGenerator
{
    /// <summary>
    /// Парсер для формата CSV.
    /// </summary>
    public class CsvParserGenerator : IParserGenerator
    {
        public IDataParser<T> Create<T>()
        {
            return new CsvParser<T>();
        }
    }
}
