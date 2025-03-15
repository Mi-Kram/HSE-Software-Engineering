using HseBankLibrary.Factories;
using Main.Models.Timing;
using HseBankLibrary.Storage;
using Main.Controllers.Crud.Operations;
using HseBankLibrary.Models.Domain;
using Main.Printers;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Tests.Controllers.Crud
{
    [Collection("Console")]
    public class OperationsControllerTests
    {
        [Fact]
        public void GeneralTests()
        {
            ServiceCollection services = new();
            services.AddSingleton(Helper.GetDefaultDatabase());
            services.AddSingleton(new TimingReport());
            services.AddSingleton<OperationFactory>();
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
            s.WriteLine("1. Просмотр операций");
            s.WriteLine("2. Создать операцию");
            s.WriteLine("3. Удалить операцию");
            s.WriteLine("4. Редактировать операцию");
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
            db.BankAccounts.Add(new BankAccount() { Name = "TestA" });
            db.Categories.Add(new Category() { Name = "TestC" });

            ServiceCollection services = new();
            services.AddSingleton(db);
            services.AddSingleton<OperationFactory>();
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            TextReader consoleIn = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new("1\n0\n0\n100\nCreateOp\n");

            Console.SetOut(sw);
            Console.SetIn(sr);

            CreateController cmd = new(provider);
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.Write("Выберите тип операции (1 - доход, 2 - расход, пустая строка - отмена): ");
            s.WriteLine("\nСписок счетов:");
            s.WriteLine("ID: 0, Название: TestA, Баланс: 0");
            s.Write("Выберите счет (ID, пустая строка - отмена): ");
            s.WriteLine("\nСписок категорий:");
            s.WriteLine("ID: 0, Название: TestC");
            s.Write("Выберите категорию (ID, пустая строка - отмена): ");
            s.Write("Выберите сумму операции (пустая строка - отмена): ");
            s.Write("Введите комментарий операции: ");
            s.WriteLine("Операция успешно добавлена");

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());

            var operations = db.Operations.GetAll();
            Assert.Single(operations);
            Assert.Equal(0UL, operations.First().BankAccountID);
            Assert.Equal(0UL, operations.First().CategoryID);
            Assert.Equal(100, operations.First().Amount);
            Assert.True(operations.First().IsIncome);
            Assert.Equal("CreateOp", operations.First().Description);

            BankAccount? account = db.BankAccounts.Get(0);
            Assert.NotNull(account);
            Assert.Equal(100, account.Balance);
        }

        [Fact]
        public void DeleteTests()
        {
            Database db = Helper.GetDefaultDatabase();
            db.BankAccounts.Add(new BankAccount() { Name = "TestA", Balance = 100 });
            db.Categories.Add(new Category() { Name = "TestC" });
            string id = db.Operations.Add(new Operation() { BankAccountID = 0, CategoryID = 0, Date = DateTime.Now, IsIncome = true, Amount = 100 });

            ServiceCollection services = new();
            services.AddSingleton(db);
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            TextReader consoleIn = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new($"{id}\n");

            Console.SetOut(sw);
            Console.SetIn(sr);

            DeleteController cmd = new(provider);
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.Write("Введите ID операции (пустая строка - отмена): ");
            s.WriteLine("Объект успешно удалён");

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());

            var operations = db.Operations.GetAll();
            Assert.Empty(operations);

            BankAccount? account = db.BankAccounts.Get(0);
            Assert.NotNull(account);
            Assert.Equal(0, account.Balance);
        }

        [Fact]
        public void UpdateTests()
        {
            Database db = Helper.GetDefaultDatabase();
            db.BankAccounts.Add(new BankAccount() { Name = "TestA", Balance = 100 });
            db.Categories.Add(new Category() { Name = "TestC" });
            string id = db.Operations.Add(new Operation() { BankAccountID = 0, CategoryID = 0, Date = DateTime.Now, IsIncome = true, Amount = 100 });

            ServiceCollection services = new();
            services.AddSingleton(db);
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            TextReader consoleIn = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new($"{id}\n1\n\n\n80\nUpdated");

            Console.SetOut(sw);
            Console.SetIn(sr);

            UpdateController cmd = new(provider);
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.Write("Введите ID операции (пустая строка - отмена): ");
            s.Write("Выберите тип операции (1 - доход, 2 - расход, пустая строка - не изменять): ");
            s.WriteLine("\nСписок счетов:");
            s.WriteLine("ID: 0, Название: TestA, Баланс: 100");
            s.Write("Выберите счет (ID, пустая строка - не изменять): ");
            s.WriteLine("\nСписок категорий:");
            s.WriteLine("ID: 0, Название: TestC");
            s.Write("Выберите категорию (ID, пустая строка - не изменять): ");
            s.Write("Выберите сумму операции (пустая строка - не изменять): ");
            s.Write("Введите комментарий операции: ");
            s.WriteLine("Операция успешно обновлена");

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());

            var operations = db.Operations.GetAll();
            Assert.Single(operations);
            Assert.Equal(0UL, operations.First().BankAccountID);
            Assert.Equal(0UL, operations.First().CategoryID);
            Assert.Equal(80, operations.First().Amount);
            Assert.True(operations.First().IsIncome);
            Assert.Equal("Updated", operations.First().Description);

            BankAccount? account = db.BankAccounts.Get(0);
            Assert.NotNull(account);
            Assert.Equal(80, account.Balance);
        }

        [Fact]
        public void PrintTests()
        {
            Database db = Helper.GetDefaultDatabase();
            DateTime dt = DateTime.Now;
            db.BankAccounts.Add(new BankAccount() { Name = "TestA", Balance = 100 });
            db.Categories.Add(new Category() { Name = "TestC" });
            string id1 = db.Operations.Add(new Operation() { BankAccountID = 0, CategoryID = 0, Date = dt, IsIncome = true, Amount = 7, Description = "111" });
            string id2 = db.Operations.Add(new Operation() { BankAccountID = 0, CategoryID = 0, Date = dt, IsIncome = false, Amount = 3, Description = "222" });

            ServiceCollection services = new();
            services.AddSingleton(db);
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            using StringWriter sw = new();
            Console.SetOut(sw);

            PrintController cmd = new(provider, new OperationPrinter());
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);

            using StringWriter s = new();
            s.WriteLine($"ID: {id1}");
            s.Write($"Тип: доход, ");
            s.Write($"ID счёта: 0, ");
            s.WriteLine($"ID категории: 0");

            s.Write($"Сумма: 7, ");
            s.WriteLine($"Дата: {dt}");

            s.WriteLine($"Комментарий: 111");

            s.WriteLine($"ID: {id2}");
            s.Write($"Тип: расход, ");
            s.Write($"ID счёта: 0, ");
            s.WriteLine($"ID категории: 0");

            s.Write($"Сумма: 3, ");
            s.WriteLine($"Дата: {dt}");

            s.WriteLine($"Комментарий: 222");

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());
        }
    }
}
