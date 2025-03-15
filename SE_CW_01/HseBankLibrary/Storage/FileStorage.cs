using HseBankLibrary.Exceptions;
using HseBankLibrary.Factories;
using HseBankLibrary.Models.Domain;
using HseBankLibrary.Storage.AutoIncrement;
using ImportExportLibrary;

namespace HseBankLibrary.Storage
{
    /// <summary>
    /// Файловый репозиторий.
    /// </summary>
    /// <typeparam name="T_ID">Тип идентификатора сущности.</typeparam>
    /// <typeparam name="T">Тип сущности.</typeparam>
    /// <typeparam name="T_DTO">Тип DTO сущности.</typeparam>
    public class FileStorage<T_ID, T, T_DTO>(
        string path, 
        IDataParser<T_DTO> parser, 
        IAutoIncrement<T_ID> autoIncrement, 
        IDomainFactory<T, T_DTO> factory
        ) : StorageBase<T_ID, T, T_DTO>(parser, autoIncrement, factory)
        where T : IEntity<T_ID, T_DTO> where T_ID : notnull
    {
        private readonly string path = path ?? throw new ArgumentNullException(nameof(path));

        protected override IEnumerable<T> ReadData(IDataParser<T_DTO> parser, IDomainFactory<T, T_DTO> factory)
        {
            try
            {
                using StreamReader sr = new(path);
                return [.. parser.Import(sr).Select(factory.Create)];
            }
            catch (Exception ex)
            {
                throw new ReadDataException(ex);
            }
        }

        protected override void WriteData(IEnumerable<T> collection, IDataParser<T_DTO> parser)
        {
            try
            {
                using StreamWriter sw = new(path);
                parser.Export(collection.Select(x => x.ToDTO()), sw);
            }
            catch (Exception ex)
            {
                throw new WriteDataException(ex);
            }
        }

        public override bool Ping()
        {
            // Проверка существования файла.
            if (File.Exists(path)) return true;

            // Если не существует, попытаться создать файл.
            try
            {
                File.Create(path).Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
