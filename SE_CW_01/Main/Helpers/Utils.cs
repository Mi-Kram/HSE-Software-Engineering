using HseBankLibrary.Exceptions;
using HseBankLibrary.Factories;
using HseBankLibrary.Models;
using HseBankLibrary.Models.Domain;
using HseBankLibrary.Models.Domain.DTO;
using Main.Models.Timing;
using HseBankLibrary.Storage;
using HseBankLibrary.Storage.AutoIncrement;
using ImportExportLibrary;
using Main.Models.ParserGenerator;
using MenuLibrary;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Helpers
{
    public static class Utils
    {
        /// <summary>
        /// Формирование конфигурации.
        /// </summary>
        /// <param name="args">Аргументы комендной строки.</param>
        /// <returns>Объект конфигурации.</returns>
        public static Configuration GetConfiguration()
        {
            return new Configuration
            {
                StoragePath = Environment.CurrentDirectory, // Текущая папка.
            };
        }

        /// <summary>
        /// Инициализация сервисов.
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static ServiceProvider? Initialize(Configuration cfg)
        {
            ServiceCollection services = new();

            // Создание базы данных.
            Database? db = CreateDatabase(cfg.StoragePath, cfg, new JsonParserGenerator());
            if (db == null) return null;

            // Добавление сервисов.
            services.AddSingleton(db);
            services.AddSingleton(cfg);
            services.AddSingleton<BankAccountFactory>();
            services.AddSingleton<OperationFactory>();
            services.AddSingleton<CategoryFactory>();
            services.AddSingleton<TimingReport>();

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Создание базы данных.
        /// </summary>
        /// <param name="dir">Папка с файлами, хранящими данные о сущностях.</param>
        /// <param name="cfg">Конфиг.</param>
        /// <param name="parser">Парсер данных.</param>
        /// <returns></returns>
        public static Database? CreateDatabase(string dir, Configuration cfg, IParserGenerator parser)
        {
            // Проверка доступности данных.
            if (!Directory.Exists(dir))
            {
                Console.WriteLine($"Папка с данными не найдена \"{dir}\"");
                return null;
            }

            // Создание репозитория счетов.
            string path = Path.Combine(dir, cfg.BankAccountsFileName);
            CachedStorage<ulong, BankAccount, BankAccountDTO>? bankAccountStorage = CreateStorage(path, new UlongAutoIncrement(), new BankAccountFactory(), parser.Create<BankAccountDTO>());
            if (bankAccountStorage == null) return null;

            // Создание репозитория категорий.
            path = Path.Combine(dir, cfg.CategoriesFileName);
            CachedStorage<ulong, Category, CategoryDTO>? categoriesStorage = CreateStorage(path, new UlongAutoIncrement(), new CategoryFactory(), parser.Create<CategoryDTO>());
            if (categoriesStorage == null) return null;

            // Создание репозитория операций.
            path = Path.Combine(dir, cfg.OperationsFileName);
            CachedStorage<string, Operation, OperationDTO>? operationsStorage = CreateStorage(path, new StringAutoIncrement(), new OperationFactory(), parser.Create<OperationDTO>());
            if (operationsStorage == null) return null;

            try
            {
                // Создание базы данных.
                Database db = Database.CreateBuilder()
                    .SetBankAccountsStorage(bankAccountStorage)
                    .SetCategoriesStorage(categoriesStorage)
                    .SetOperationsStorage(operationsStorage)
                    .Build();

                return db;
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("База данных не полностью инициализирована.");
            }
            catch (DatabaseEntitiesMismatchException)
            {
                Console.WriteLine("Нарушена структура сущностей базы данных");
            }

            return null;
        }

        /// <summary>
        /// Создание репозитория.
        /// </summary>
        /// <param name="path">Путь к файлу с данными.</param>
        /// <param name="autoIncrement">Автоувеличитель идентификатора.</param>
        /// <param name="factory">Фабрика для создания сущностей.</param>
        /// <param name="parser">Парсер данных.</param>
        /// <returns></returns>
        private static CachedStorage<T_ID, T, T_DTO>? CreateStorage<T_ID, T, T_DTO>(string path, IAutoIncrement<T_ID> autoIncrement, 
            IDomainFactory<T, T_DTO> factory, IDataParser<T_DTO> parser)
            where T_ID : notnull where T : IEntity<T_ID, T_DTO>
        {
            // Файловый репозиторий.
            FileStorage<T_ID, T, T_DTO> storage = new(path, parser, autoIncrement, factory);
            if (!storage.Ping())
            {
                Console.WriteLine($"Не удалось открыть файл \"{path}\"");
                return null;
            }

            try
            {
                // Кэшируем репозиторий.
                return CachedStorage<T_ID, T, T_DTO>.Create(storage, autoIncrement);
            }
            catch (ReadDataException)
            {
                Console.WriteLine($"Не удалость прочитать файл \"{path}\"");
            }

            return null;
        }

        /// <summary>
        /// Обработчик Ctrl+C.
        /// </summary>
        /// <param name="provider"></param>
        public static void Abort(ServiceProvider provider)
        {
            // Сохранение данных и выход с кодом 0.
            try
            {
                provider.GetRequiredService<Database>().SaveData();
            }
            catch { }
            Environment.Exit(0);
        }

        /// <summary>
        /// Создание главного меню.
        /// </summary>
        /// <param name="provider">Контейнер сервисов.</param>
        /// <returns></returns>
        public static Menu CreateMainMenu(ServiceProvider provider)
        {
            TimingReport reporter = provider.GetRequiredService<TimingReport>();

            // Создание обработчиков меню.
            TimingDecorator crud = new(new Controllers.Crud.GeneralController(provider), "CRUD", reporter);
            TimingDecorator analitisc = new(new Controllers.Analitics.GeneralController(provider), "Analitics", reporter);
            TimingDecorator importExport = new(new Controllers.ImportExport.GeneralController(provider), "ImportExport", reporter);
            TimingDecorator manage = new(new Controllers.ManageData.GeneralController(provider), "ManageData", reporter);
            TimingDecorator stat = new(new Controllers.Statistics.GeneralController(provider), "Stat", reporter);

            // Создание меню
            Menu menu = new();
            menu.Add(new MenuItem("Редактирование базы (CRUD)", crud));
            menu.Add(new MenuItem("Аналитика", analitisc));
            menu.Add(new MenuItem("Импорт/Экспорт", importExport));
            menu.Add(new MenuItem("Управление данными", manage));
            menu.Add(new MenuItem("Статистика", stat));

            menu.Add(new MenuItem("Выход", new Controllers.SaveExitController(provider, menu)));

            return menu;
        }
    }
}
