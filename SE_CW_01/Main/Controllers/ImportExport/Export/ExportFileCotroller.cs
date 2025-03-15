using HseBankLibrary.Models;
using HseBankLibrary.Storage;
using HseBankLibrary.Models.Domain;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.ImportExport.Export
{
    /// <summary>
    /// Экспорт данных в файл.
    /// </summary>
    public abstract class ExportFileCotroller(ServiceProvider provider) : ICommand<CommandArguments>
    {
        private readonly Database db = provider.GetRequiredService<Database>();
        private readonly Configuration cfg = provider.GetRequiredService<Configuration>();

        /// <summary>
        /// Чтение пути для сохранения.
        /// </summary>
        /// <returns>Путь к папке.</returns>
        protected virtual string ReadPath()
        {
            Console.Write("Введите путь к папке для сохранения (пустая строка - отмена): ");
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Проверка директории для сохранения.
        /// </summary>
        /// <param name="path">Путь до директории.</param>
        /// <returns>True, если директория существует, иначе - False.</returns>
        protected virtual bool CheckDirectory(string? path)
        {
            // Проверка существования.
            if (string.IsNullOrEmpty(path)) return false;
            if (Directory.Exists(path)) return true;

            // Попытка создания.
            try
            {
                DirectoryInfo dInfo = Directory.CreateDirectory(path);
                if (dInfo.Exists) return true;
            }
            catch { }

            Console.WriteLine("Не удалось создать папку");
            return false;
        }

        /// <summary>
        /// Преобразование данных в TextWriter.
        /// </summary>
        /// <param name="collection">Данные для записи.</param>
        /// <param name="textWriter">Место, куда записывать.</param>
        protected abstract void Parse<T>(IEnumerable<T> collection, TextWriter textWriter);

        public void Execute(CommandArguments args)
        {
            // Получение пути к файлу.
            string? path = ReadPath();

            // Проверка пути.
            if (!CheckDirectory(path))
            {
                args.AskForEnter = false;
                return;
            }

            bool success = true;

            // Экспорт данных.
            if (!Export(db.BankAccounts, Path.Combine(path, cfg.BankAccountsFileName)))
            {
                success = false;
                Console.WriteLine("Не удалось сохранить счета");
            }
            if (!Export(db.Operations, Path.Combine(path, cfg.OperationsFileName)))
            {
                success = false;
                Console.WriteLine("Не удалось сохранить операции");
            }
            if (!Export(db.Categories, Path.Combine(path, cfg.CategoriesFileName)))
            {
                success = false;
                Console.WriteLine("Не удалось сохранить категории");
            }

            // Проверка успешности экспорта.
            if (success)
            {
                Console.WriteLine("Данные успешно записаны");
            }
        }

        /// <summary>
        /// Экспорт репозитория.
        /// </summary>
        /// <param name="storage">Репозиторий.</param>
        /// <param name="path">Путь для сохранения.</param>
        /// <returns></returns>
        private bool Export<T_ID, T, T_DTO>(IStorage<T_ID, T, T_DTO> storage, string path)
            where T : IEntity<T_ID, T_DTO>
            where T_ID : notnull
        {
            try
            {
                IEnumerable<T_DTO> collection = storage.GetAll().Select(x => x.ToDTO());
                using StreamWriter sw = new(path);
                Parse(collection, sw);
                return true;
            }
            catch { }

            return false;
        }
    }
}
