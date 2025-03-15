using HseBankLibrary.Storage;
using Main.Helpers;
using HseBankLibrary.Models;
using Main.Models.ParserGenerator;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.ImportExport.Import
{
    /// <summary>
    /// Импорт данных из файла.
    /// </summary>
    public abstract class ImportFileCotroller(ServiceProvider provider) : ICommand<CommandArguments>
    {
        private readonly Database db = provider.GetRequiredService<Database>();
        private readonly Configuration cfg = provider.GetRequiredService<Configuration>();

        /// <summary>
        /// Чтение пути для импорта данных.
        /// </summary>
        /// <returns>Путь к папке.</returns>
        protected virtual string ReadPath()
        {
            Console.Write("Введите путь к папке для чтения (пустая строка - отмена): ");
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Проверка директории для чтения данных.
        /// </summary>
        /// <param name="path">Путь до директории.</param>
        /// <returns>True, если директория и файлы существуют, иначе - False.</returns>
        protected virtual bool CheckDirectory(string? path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            if (!Directory.Exists(path)) return false;
            if (!File.Exists(Path.Combine(path, cfg.BankAccountsFileName))) return false;
            if (!File.Exists(Path.Combine(path, cfg.OperationsFileName))) return false;
            if (!File.Exists(Path.Combine(path, cfg.CategoriesFileName))) return false;

            return true;
        }

        /// <summary>
        /// Получение парсера данных.
        /// </summary>
        /// <returns></returns>
        protected abstract IParserGenerator GetParserGenerator();

        public void Execute(CommandArguments args)
        {
            // Получение пути к файлу.
            string? path = ReadPath();

            // Проверка пути.
            if (!CheckDirectory(path))
            {
                Console.WriteLine("Невозможно прочитать данные");
                return;
            }

            // Чтение данных.
            Database? newDB = Utils.CreateDatabase(path, cfg, GetParserGenerator());
            if (newDB == null) return;

            // Замена данных.
            db.BankAccounts.Rewrite(newDB.BankAccounts);
            db.Operations.Rewrite(newDB.Operations);
            db.Categories.Rewrite(newDB.Categories);

            Console.WriteLine("Данные успешно прочитаны");
        }
    }
}
