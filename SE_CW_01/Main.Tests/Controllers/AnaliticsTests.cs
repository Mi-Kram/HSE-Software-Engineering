using HseBankLibrary.Storage;
using Main.Controllers.Analitics;
using HseBankLibrary.Models.Domain;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;
using Main.Models.Timing;

namespace Main.Tests.Controllers
{
    [Collection("Console")]
    public class AnaliticsTests
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
            using StringReader sr = new("3\n");

            Console.SetOut(sw);
            Console.SetIn(sr);

            GeneralController cmd = new(provider);
            cmd.Execute(new CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.WriteLine("Выберите действие");
            s.WriteLine("1. Подсчет разницы доходов и расходов за выбранный период");
            s.WriteLine("2. Группировка доходов и расходов по категориям");
            s.WriteLine("3. Назад");
            s.WriteLine("Выберите действие: ");
            s.WriteLine();

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());
        }

        [Fact]
        public void PeriodTests()
        {
            Database db = Helper.GetDefaultDatabase();
            db.BankAccounts.Add(new BankAccount() { Name = "TestA", Balance = 5 });
            db.Categories.Add(new Category() { Name = "TestC" });
            db.Operations.Add(new Operation()
            {
                BankAccountID = 0,
                CategoryID = 0,
                Amount = 5,
                IsIncome = true,
                Date = DateTime.Now,
                Description = string.Empty
            });

            ServiceCollection services = new();
            services.AddSingleton(db);
            services.AddSingleton(new TimingReport());
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            TextReader consoleIn = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new($"{DateTime.Now:dd.MM.yyyy}\n{DateTime.Now:dd.MM.yyyy}\n\n");

            Console.SetOut(sw);
            Console.SetIn(sr);

            PeriodController cmd = new(provider);
            cmd.Execute(new CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.WriteLine($"Найдены операции за период {DateTime.Now:dd.MM.yyyy}-{DateTime.Now:dd.MM.yyyy}\n");
            s.Write("Введите дату, начиная с которой надо начать расчёт (dd.MM.yyyy): ");
            s.Write("Введите дату, до которой надо производить расчёт (dd.MM.yyyy): ");
            s.WriteLine();
            s.WriteLine(" Счёт  │ Доход │ Расход │ Разница");
            s.WriteLine("───────┼───────┼────────┼─────────");
            s.WriteLine(" TestA │ 5     │ 0      │ 5");

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());
        }

        [Fact]
        public void GroupByCategoryTests()
        {
            Database db = Helper.GetDefaultDatabase();
            db.BankAccounts.Add(new BankAccount() { Name = "TestA", Balance = 5 });
            db.Categories.Add(new Category() { Name = "TestC" });
            db.Operations.Add(new Operation()
            {
                BankAccountID = 0,
                CategoryID = 0,
                Amount = 5,
                IsIncome = true,
                Date = DateTime.Now,
                Description = string.Empty
            });

            ServiceCollection services = new();
            services.AddSingleton(db);
            services.AddSingleton(new TimingReport());
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            TextReader consoleIn = Console.In;

            using StringWriter sw = new();

            Console.SetOut(sw);

            GroupByCategoryController cmd = new(provider);
            cmd.Execute(new CommandArguments());

            Console.SetOut(consoleOut);

            using StringWriter s = new();
            s.WriteLine("Категория: TestC");
            s.WriteLine($" - {db.Operations.GetAll().First().Date:dd.MM.yyyyy HH:mm:ss}: +5");
            s.WriteLine();

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());
        }
    }
}
