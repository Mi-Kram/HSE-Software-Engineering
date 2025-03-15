using ImportExportLibrary;

namespace Main.Models.ParserGenerator
{
    /// <summary>
    /// Парсер для формата YAML.
    /// </summary>
    public class YamlParserGenerator : IParserGenerator
    {
        public IDataParser<T> Create<T>()
        {
            return new YamlParser<T>();
        }
    }
}
