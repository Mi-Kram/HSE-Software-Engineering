using CsvHelper;
using ImportExportLibrary.Exceptions;
using System.Globalization;

namespace ImportExportLibrary
{
    /// <summary>
    /// Парсер формата CSV.
    /// </summary>
    public class CsvParser<T> : IDataParser<T>
    {
        public void Export(IEnumerable<T> collection, TextWriter writer)
        {
            try
            {
                using CsvWriter csv = new(writer, CultureInfo.InvariantCulture);
                csv.WriteRecords(collection);
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
                CsvReader csv = new(reader, CultureInfo.InvariantCulture);
                return [.. csv.GetRecords<T>()];
            }
            catch (Exception ex)
            {
                throw new ImportException(ex);
            }
        }
    }
}
