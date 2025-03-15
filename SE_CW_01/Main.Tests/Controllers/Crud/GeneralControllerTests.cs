using Main.Controllers.Crud;
using Main.Models.Timing;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Tests.Controllers.Crud
{
    [Collection("Console")]
    public class GeneralControllerTests
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
            using StringReader sr = new("4\n");

            Console.SetOut(sw);
            Console.SetIn(sr);

            GeneralController cmd = new(provider);
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.WriteLine("Выберите таблицу");
            s.WriteLine("1. Счета");
            s.WriteLine("2. Категории");
            s.WriteLine("3. Операции");
            s.WriteLine("4. Назад");
            s.WriteLine("Выберите действие: ");
            s.WriteLine();

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());
        }
    }
}
