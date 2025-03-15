using ImportExportLibrary;

namespace Main.Models.ParserGenerator
{
    /// <summary>
    /// Интерфейс создания парсеров.
    /// </summary>
    public interface IParserGenerator
    {
        /// <summary>
        /// Создать парсер.
        /// </summary>
        /// <typeparam name="T">Тип сущностей парсера.</typeparam>
        /// <returns>Парсер.</returns>
        IDataParser<T> Create<T>();
    }
}
