using MenuLibrary.Commands;

namespace MenuLibrary.Tests
{
    public class MenuItemTests
    {
        [Fact]
        public void Constructor1()
        {
            MenuItem item = new();
            Assert.Equal(string.Empty, item.Title);
            Assert.Equal(EmptyCommand.Instance, item.Command);
        }

        [Theory]
        [InlineData("SecretTitle")]
        public void Constructor2(string title)
        {
            MenuItem item = new(title);
            Assert.Equal(title, item.Title);
            Assert.Equal(EmptyCommand.Instance, item.Command);
        }

        [Fact]
        public void Constructor3()
        {
            string title = "ABC";
            ICommand<CommandArguments> cmd = new TestCommndArgument();

            MenuItem item = new(title, cmd);
            Assert.Equal(title, item.Title);
            Assert.IsType<TestCommndArgument>(item.Command);
        }

        [Fact]
        public void Execute()
        {
            TestCommndArgument cmd = new();

            MenuItem item = new() { Command = cmd };
            CommandArguments args = new();
            item.Execute(args);
            Assert.False(args.AskForEnter);
            Assert.Equal(100, cmd.MyProperty);
        }
    }

    internal class TestCommndArgument : ICommand<CommandArguments>
    {
        public int MyProperty { get; set; } = 5;
        public void Execute(CommandArguments args)
        {
            args.AskForEnter = false;
            MyProperty = 100;
        }
    }
}
