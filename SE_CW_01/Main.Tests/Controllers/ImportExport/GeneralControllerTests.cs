using Main.Controllers.ImportExport;
using Main.Models.Timing;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Tests.Controllers.ImportExport
{
    [Collection("Console")]
    public class GeneralControllerTests
    {
        [Fact]
        public void GeneralTests()
        {
            ServiceCollection services = new();
            services.AddSingleton(new TimingReport());
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            TextReader consoleIn = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new("3\n");

            Console.SetOut(sw);
            Console.SetIn(sr);

            GeneralController cmd = new(provider);
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.WriteLine("Выберите действие");
            s.WriteLine("1. Экспорт");
            s.WriteLine("2. Импорт");
            s.WriteLine("3. Назад");
            s.WriteLine("Выберите действие: ");
            s.WriteLine();

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());
        }
    }
}
