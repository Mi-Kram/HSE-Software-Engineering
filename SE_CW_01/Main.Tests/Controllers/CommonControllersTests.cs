using HseBankLibrary.Storage;
using Main.Controllers;
using MenuLibrary;
using Microsoft.Extensions.DependencyInjection;
using MenuLibrary.Commands;
using Main.Controllers.Common;

namespace Main.Tests.Controllers
{
    [Collection("Console")]
    public class CommonControllersTests
    {

        [Fact]
        public void SaveExitTests()
        {
            Database db = Helper.GetDefaultDatabase();

            TextWriter consoleOut = Console.Out;
            using StringWriter sw = new();
            Console.SetOut(sw);

            ServiceCollection sc = new();
            sc.AddSingleton(db);
            using ServiceProvider provider = sc.BuildServiceProvider();

            SaveExitController cmd = new(provider, new Menu());
            cmd.Execute(new CommandArguments());

            Console.SetOut(consoleOut);

            string expected = $"Прогрмма успешно завершена.{Environment.NewLine}";
            Assert.Equal(expected, sw.ToString());
        }

        [Fact]
        public void CommonExitTests()
        {
            ExitController cmd = new(new Menu());
            cmd.Execute(new CommandArguments());
        }
    }
}
