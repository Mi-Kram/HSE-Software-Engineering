using HseBankLibrary.Storage;
using Main.Controllers.ManageData;
using HseBankLibrary.Models.Domain;
using Microsoft.Extensions.DependencyInjection;
using Main.Models.Timing;

namespace Main.Tests.Controllers
{
    [Collection("Console")]
    public class ManageDataController
    {
        [Fact]
        public void GeneralTests()
        {
            ServiceCollection services = new();
            services.AddSingleton(Helper.GetDefaultDatabase());
            services.AddSingleton(new TimingReport());
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            TextReader consoleIn = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new("2\n");

            Console.SetOut(sw);
            Console.SetIn(sr);

            GeneralController cmd = new(provider);
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.WriteLine("Выберите действие");
            s.WriteLine("1. Пересчета баланса");
            s.WriteLine("2. Назад");
            s.WriteLine("Выберите действие: ");
            s.WriteLine();

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());
        }

        [Fact]
        public void RecalculateTests()
        {
            Database db = Helper.GetDefaultDatabase();
            db.BankAccounts.Add(new BankAccount() { Name = "TestA", Balance = 70 });
            db.Categories.Add(new Category() { Name = "TestC" });
            DateTime dt = DateTime.Now;
            db.Operations.Add(new Operation() { BankAccountID = 0, CategoryID = 0, Date = dt, IsIncome = true, Amount = 100 });
            db.Operations.Add(new Operation() { BankAccountID = 0, CategoryID = 0, Date = dt, IsIncome = false, Amount = 80 });
            db.Operations.Add(new Operation() { BankAccountID = 0, CategoryID = 0, Date = dt, IsIncome = true, Amount = 30 });

            ServiceCollection services = new();
            services.AddSingleton(db);
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            TextReader consoleIn = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new("0\n1\n");

            Console.SetOut(sw);
            Console.SetIn(sr);

            RecalculateController cmd = new(provider);
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.WriteLine($"ID: 0, Название: TestA, Баланс: 70");
            s.Write("Выберите счет (id, пустая строка - отмена): ");
            s.WriteLine("\nВыбранный счет:");
            s.WriteLine($"ID: 0, Название: TestA, Баланс: 70");
            s.WriteLine();
            s.WriteLine("История операций:");
            s.WriteLine($"{dt:dd.MM.yyyy HH:mm:ss}: +100  -> Баланс: 100");
            s.WriteLine($"{dt:dd.MM.yyyy HH:mm:ss}: -80  -> Баланс: 20");
            s.WriteLine($"{dt:dd.MM.yyyy HH:mm:ss}: +30  -> Баланс: 50");
            s.WriteLine("Итого: 50");
            s.WriteLine("Баланс на счете не совпадает с операциями.");
            s.Write("Обновить баланс? (1 - да, 2 - нет): ");
            s.WriteLine("\nБаланс обновлён");

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());

            var accounts = db.BankAccounts.GetAll();
            Assert.Single(accounts);
            Assert.Equal(50, accounts.First().Balance);
        }
    }
}
