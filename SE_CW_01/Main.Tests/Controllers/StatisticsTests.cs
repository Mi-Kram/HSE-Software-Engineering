using Main.Models.Timing;
using Main.Controllers.Statistics;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Tests.Controllers
{
    [Collection("Console")]
    public class StatisticsTests
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
            s.WriteLine("1. Измерение времени работы отдельных пользовательских сценариев");
            s.WriteLine("2. Назад");
            s.WriteLine("Выберите действие: ");
            s.WriteLine();

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());
        }

        [Fact]
        public void TimingTests()
        {
            TimingReport reporter = new();
            TimeSpan t1 = TimeSpan.FromMinutes(1.5);
            TimeSpan t2 = TimeSpan.FromMilliseconds(121);
            TimeSpan t3 = TimeSpan.FromMilliseconds(23);
            reporter.Add("Test1", t1);
            reporter.Add("Test1", t2);
            reporter.Add("Test2", t3);

            ServiceCollection services = new();
            services.AddSingleton(reporter);
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            TextReader consoleIn = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new("\n");

            Console.SetOut(sw);
            Console.SetIn(sr);

            TimingController cmd = new(provider);
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.WriteLine(" Метки │ Время исполнения");
            s.WriteLine("───────┼──────────────────");
            s.WriteLine(" Test1 │ 01 мин 30 сек 0 мс");
            s.WriteLine("       │ 121 мс");
            s.WriteLine("───────┼──────────────────");
            s.WriteLine(" Test2 │ 23 мс");

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());
        }
    }
}
