using ImportExportLibrary.Exceptions;

namespace ImportExportLibrary
{
    /// <summary>
    /// Интерфейс импорта/экспорта данных.
    /// </summary>
    public interface IDataParser<T>
    {
        /// <summary>
        /// Экспорт данных.
        /// </summary>
        /// <param name="collection">Данные для экспорта.</param>
        /// <param name="writer">Место, куда надо записать данные.</param>
        /// <exception cref="ExportException"></exception>
        void Export(IEnumerable<T> collection, TextWriter writer);


        /// <summary>
        /// Импорт данных данных.
        /// </summary>
        /// <param name="reader">Место, откуда надо читать данные.</param>
        /// <returns>Прочитанные данные.</returns>
        /// <exception cref="ImportException"></exception>
        IEnumerable<T> Import(TextReader reader);
    }
}
