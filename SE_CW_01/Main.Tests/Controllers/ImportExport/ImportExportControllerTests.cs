using HseBankLibrary.Models;
using HseBankLibrary.Storage;
using ImportExportLibrary;
using Main.Controllers.ImportExport.Export;
using Main.Controllers.ImportExport.Import;
using Main.Helpers;
using HseBankLibrary.Models.Domain;
using Microsoft.Extensions.DependencyInjection;
using Main.Models.Timing;

namespace Main.Tests.Controllers.ImportExport
{
    [Collection("Console")]
    public class ImportExportControllerTests
    {
        [Fact]
        public void ExportGeneralTests()
        {
            ServiceCollection services = new();
            services.AddSingleton(Utils.GetConfiguration());
            services.AddSingleton(Helper.GetDefaultDatabase());
            services.AddSingleton(new TimingReport());
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            TextReader consoleIn = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new("4\n");

            Console.SetOut(sw);
            Console.SetIn(sr);

            Main.Controllers.ImportExport.Import.GeneralController cmd = new(provider);
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.WriteLine("Выберите формат");
            s.WriteLine("1. Json формат");
            s.WriteLine("2. Csv формат");
            s.WriteLine("3. Yaml формат");
            s.WriteLine("4. Назад");
            s.WriteLine("Выберите действие: ");
            s.WriteLine();

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());
        }

        [Fact]
        public void ImportGeneralTests()
        {
            ServiceCollection services = new();
            services.AddSingleton(Utils.GetConfiguration());
            services.AddSingleton(Helper.GetDefaultDatabase());
            services.AddSingleton(new TimingReport());
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            TextReader consoleIn = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new("4\n");

            Console.SetOut(sw);
            Console.SetIn(sr);

            Main.Controllers.ImportExport.Import.GeneralController cmd = new(provider);
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.WriteLine("Выберите формат");
            s.WriteLine("1. Json формат");
            s.WriteLine("2. Csv формат");
            s.WriteLine("3. Yaml формат");
            s.WriteLine("4. Назад");
            s.WriteLine("Выберите действие: ");
            s.WriteLine();

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());
        }

        [Fact]
        public void CsvFileTests()
        {
            Export(x => new ExportCsvController(x));
            Import(x => new ImportCsvController(x));

            Configuration cfg = Utils.GetConfiguration();
            File.Delete(cfg.BankAccountsFileName);
            File.Delete(cfg.CategoriesFileName);
            File.Delete(cfg.OperationsFileName);
        }

        [Fact]
        public void JsonFileTests()
        {
            Export(x => new ExportJsonController(x));
            Import(x => new ImportJsonController(x));

            Configuration cfg = Utils.GetConfiguration();
            File.Delete(cfg.BankAccountsFileName);
            File.Delete(cfg.CategoriesFileName);
            File.Delete(cfg.OperationsFileName);
        }

        [Fact]
        public void YamlFileTests()
        {
            Export(x => new ExportYamlController(x));
            Import(x => new ImportYamlController(x));

            Configuration cfg = Utils.GetConfiguration();
            File.Delete(cfg.BankAccountsFileName);
            File.Delete(cfg.CategoriesFileName);
            File.Delete(cfg.OperationsFileName);
        }

        private static void Export(Func<ServiceProvider, ExportFileCotroller> func)
        {
            Configuration cfg = Utils.GetConfiguration();
            Database db = Helper.GetDefaultDatabase();

            db.BankAccounts.Add(new BankAccount() { Name = "TestA", Balance = 100 });
            db.Categories.Add(new Category() { Name = "TestC" });
            db.Operations.Add(new Operation() { BankAccountID = 0, CategoryID = 0, Date = DateTime.Now, IsIncome = true, Amount = 100 });

            ServiceCollection services = new();
            services.AddSingleton(cfg);
            services.AddSingleton(db);
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            TextReader consoleIn = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new($".\n");

            Console.SetOut(sw);
            Console.SetIn(sr);

            ExportFileCotroller cmd = func(provider);
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.Write("Введите путь к папке для сохранения (пустая строка - отмена): ");
            s.WriteLine("Данные успешно записаны");

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());

            Assert.True(File.Exists(cfg.BankAccountsFileName));
            Assert.True(File.Exists(cfg.CategoriesFileName));
            Assert.True(File.Exists(cfg.OperationsFileName));
        }

        private static void Import(Func<ServiceProvider, ImportFileCotroller> func)
        {
            Configuration cfg = Utils.GetConfiguration();
            Database db = Helper.GetDefaultDatabase();

            ServiceCollection services = new();
            services.AddSingleton(cfg);
            services.AddSingleton(db);
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            TextReader consoleIn = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new($".\n");

            Console.SetOut(sw);
            Console.SetIn(sr);

            ImportFileCotroller cmd = func(provider);
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.Write("Введите путь к папке для чтения (пустая строка - отмена): ");
            s.WriteLine("Данные успешно прочитаны");

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());

            Assert.True(File.Exists(cfg.BankAccountsFileName));
            Assert.True(File.Exists(cfg.CategoriesFileName));
            Assert.True(File.Exists(cfg.OperationsFileName));
        }
    }
}
