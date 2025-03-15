using ImportExportLibrary.Exceptions;
using YamlDotNet.Serialization;

namespace ImportExportLibrary
{
    /// <summary>
    /// Парсер формата YAML.
    /// </summary>
    public class YamlParser<T> : IDataParser<T>
    {
        public void Export(IEnumerable<T> collection, TextWriter writer)
        {
            try
            {
                new Serializer().Serialize(writer, collection);
            }
            catch (Exception ex)
            {
                throw new ExportException(ex);
            }
        }

        public IEnumerable<T> Import(TextReader reader)
        {
            try
            {
                return new Deserializer().Deserialize<IEnumerable<T>>(reader);
            }
            catch (Exception ex)
            {
                throw new ImportException(ex);
            }
        }
    }
}
