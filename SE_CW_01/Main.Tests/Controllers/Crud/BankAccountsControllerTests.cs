using HseBankLibrary.Factories;
using Main.Models.Timing;
using HseBankLibrary.Storage;
using Main.Controllers.Crud.BankAccounts;
using HseBankLibrary.Models.Domain;
using Main.Printers;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Tests.Controllers.Crud
{
    [Collection("Console")]
    public class BankAccountsControllerTests
    {
        [Fact]
        public void GeneralTests()
        {
            ServiceCollection services = new();
            services.AddSingleton(Helper.GetDefaultDatabase());
            services.AddSingleton(new TimingReport());
            services.AddSingleton<BankAccountFactory>();
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            TextReader consoleIn = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new("5\n");

            Console.SetOut(sw);
            Console.SetIn(sr);

            GeneralController cmd = new(provider);
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.WriteLine("Выберите действие");
            s.WriteLine("1. Просмотр счетов");
            s.WriteLine("2. Создать счет");
            s.WriteLine("3. Удалить счет");
            s.WriteLine("4. Редактировать счет");
            s.WriteLine("5. Назад");
            s.WriteLine("Выберите действие: ");
            s.WriteLine();

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());
        }

        [Fact]
        public void CreateTests()
        {
            Database db = Helper.GetDefaultDatabase();

            ServiceCollection services = new();
            services.AddSingleton(db);
            services.AddSingleton<BankAccountFactory>();
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            TextReader consoleIn = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new("NewAccount\n");

            Console.SetOut(sw);
            Console.SetIn(sr);

            CreateController cmd = new(provider);
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.Write("Введите название счета (пустая строка - отмена): ");
            s.WriteLine("Счет успешно добавлен");

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());

            var accounts = db.BankAccounts.GetAll();
            Assert.Single(accounts);
            Assert.Equal("NewAccount", accounts.First().Name);
        }

        [Fact]
        public void DeleteTests()
        {
            Database db = Helper.GetDefaultDatabase();
            db.BankAccounts.Add(new BankAccount() { Name = "DeleteMe" });

            ServiceCollection services = new();
            services.AddSingleton(db);
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            TextReader consoleIn = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new("0\n");

            Console.SetOut(sw);
            Console.SetIn(sr);

            DeleteController cmd = new(provider);
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.Write("Введите ID счета (пустая строка - отмена): ");
            s.WriteLine("Объект успешно удалён");

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());

            var accounts = db.BankAccounts.GetAll();
            Assert.Empty(accounts);
        }

        [Fact]
        public void UpdateTests()
        {
            Database db = Helper.GetDefaultDatabase();
            db.BankAccounts.Add(new BankAccount() { Name = "UpdateMe" });

            ServiceCollection services = new();
            services.AddSingleton(db);
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            TextReader consoleIn = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new("0\nUpdated\n");

            Console.SetOut(sw);
            Console.SetIn(sr);

            UpdateController cmd = new(provider);
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.Write("Введите ID счета (пустая строка - отмена): ");
            s.Write("Введите новое название счета (пустая строка - отмена): ");
            s.WriteLine("Счет успешно обновлён");

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());

            var accounts = db.BankAccounts.GetAll();
            Assert.Single(accounts);
            Assert.Equal("Updated", accounts.First().Name);
        }

        [Fact]
        public void PrintTests()
        {
            Database db = Helper.GetDefaultDatabase();
            db.BankAccounts.Add(new BankAccount() { Name = "Test1", Balance = 7.5M });
            db.BankAccounts.Add(new BankAccount() { Name = "Test2", Balance = 12M });
            db.BankAccounts.Add(new BankAccount() { Name = "Test3", Balance = 2.003M });

            ServiceCollection services = new();
            services.AddSingleton(db);
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            using StringWriter sw = new();
            Console.SetOut(sw);

            PrintController cmd = new(provider, new BankAccountPrinter());
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);

            using StringWriter s = new();
            s.WriteLine($"ID: 0, Название: Test1, Баланс: {7.5M}");
            s.WriteLine($"ID: 1, Название: Test2, Баланс: {12M}");
            s.WriteLine($"ID: 2, Название: Test3, Баланс: {2.003M}");

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());
        }
    }
}
