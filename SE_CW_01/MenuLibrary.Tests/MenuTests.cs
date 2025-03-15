using MenuLibrary.Commands;

namespace MenuLibrary.Tests
{
    [Collection("Console")]
    public class MenuTests
    {
        [Fact]
        public void Constructor_DefaultValues()
        {
            Menu menu = new();
            Assert.True(menu.ClearConsole);
            Assert.Equal(string.Empty, menu.Title);
        }

        [Fact]
        public void Add_CheckWork()
        {
            Menu menu = new();
            menu.Add(new MenuItem());
            menu.Add(new MenuItem("title"));
        }

        [Fact]
        public void Insert_CheckWork()
        {
            Menu menu = new();
            menu.Insert(0, new MenuItem());
            menu.Insert(0, new MenuItem("title"));
        }

        [Fact]
        public void Delete_CheckWork()
        {
            Menu menu = new();

            MenuItem item = new("title");
            menu.Add(item);
            menu.Delete(item);
        }

        [Fact]
        public void Run_EmptyMenu()
        {
            Menu menu = new() { ClearConsole = false };

            TextReader consoleReader = Console.In;
            using StringWriter sw = new();
            Console.SetOut(sw);

            menu.Run();
            Console.SetIn(consoleReader);

            Assert.Equal($"В меню нет пунктов!{Environment.NewLine}", sw.ToString());
        }

        [Fact]
        public void Stop()
        {
            Menu menu = new();
            menu.Stop();
        }


        [Fact]
        public void PrintMenu()
        {
            Menu menu = new() { ClearConsole = false };
            menu.Add(new MenuItem("First", new NoEnter()));
            menu.Add(new MenuItem("Second", new NoEnter()));
            menu.Add(new MenuItem("Exit", new ExitMenu(menu)));

            TextWriter consoleWriter = Console.Out;
            TextReader consoleReader = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new("1\n3\n");
            Console.SetOut(sw);
            Console.SetIn(sr);

            menu.Run();
            Console.SetOut(consoleWriter);
            Console.SetIn(consoleReader);

            Assert.Equal(-1, sr.Read());

            using StringWriter writer = new();
            writer.WriteLine("1. First");
            writer.WriteLine("2. Second");
            writer.WriteLine("3. Exit");
            writer.WriteLine("Выберите действие: ");
            writer.WriteLine();
            writer.WriteLine("1. First");
            writer.WriteLine("2. Second");
            writer.WriteLine("3. Exit");
            writer.WriteLine("Выберите действие: ");
            writer.WriteLine();
            Assert.Equal(writer.ToString(), sw.ToString());
        }

        [Fact]
        public void SelectItem()
        {
            Menu menu = new() { ClearConsole = false };
            menu.Add(new MenuItem("First", new NoEnter()));
            menu.Add(new MenuItem("Second", new NoEnter()));
            menu.Add(new MenuItem("Exit", new ExitMenu(menu)));

            TextWriter consoleWriter = Console.Out;
            TextReader consoleReader = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new("4\n3\n");
            Console.SetOut(sw);
            Console.SetIn(sr);

            menu.Run();    
            Console.SetOut(consoleWriter);
            Console.SetIn(consoleReader);

            Assert.Equal(-1, sr.Read());

            using StringWriter writer = new();
            writer.WriteLine("1. First");
            writer.WriteLine("2. Second");
            writer.WriteLine("3. Exit");
            writer.WriteLine("Выберите действие: ");
            writer.WriteLine("Неверный ввод!\n");
            writer.WriteLine("1. First");
            writer.WriteLine("2. Second");
            writer.WriteLine("3. Exit");
            writer.WriteLine("Выберите действие: ");
            writer.WriteLine();
            Assert.Equal(writer.ToString(), sw.ToString());
        }
}

    internal class ExitMenu(Menu menu) : ICommand<CommandArguments>
    {
        public void Execute(CommandArguments args)
        {
            args.AskForEnter = false;
            menu.Stop();
        }
    }

    internal class NoEnter: ICommand<CommandArguments>
    {
        public void Execute(CommandArguments args)
        {
            args.AskForEnter = false;
        }
    }
}
