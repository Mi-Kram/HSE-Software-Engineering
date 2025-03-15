using ImportExportLibrary;

namespace Main.Models.ParserGenerator
{
    /// <summary>
    /// Парсер для формата JSON.
    /// </summary>
    public class JsonParserGenerator : IParserGenerator
    {
        public IDataParser<T> Create<T>()
        {
            return new JsonParser<T>();
        }
    }
}
