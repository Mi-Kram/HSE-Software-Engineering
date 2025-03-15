using ImportExportLibrary.Exceptions;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ImportExportLibrary
{
    /// <summary>
    /// Парсер формата JSON.
    /// </summary>
    public class JsonParser<T> : IDataParser<T>
    {
        private static readonly JsonSerializerOptions DEFAULT_OPTIONS = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public void Export(IEnumerable<T> collection, TextWriter writer)
        {
            try
            {
                string json = JsonSerializer.Serialize(collection, DEFAULT_OPTIONS);
                writer.Write(json);
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
                string json = reader.ReadToEnd();
                if (string.IsNullOrWhiteSpace(json)) return [];

                IEnumerable<T>? result = JsonSerializer.Deserialize<IEnumerable<T>>(json);
                return result ?? throw new Exception();
            }
            catch (Exception ex)
            {
                throw new ImportException(ex);
            }
        }
    }
}
