using MenuLibrary.Commands;

namespace MenuLibrary.Tests.Commands
{
    public class CommandArgumentsTest
    {
        [Fact]
        public void AskForEnter_Default_RetunsTrue()
        {
            CommandArguments arg = new();
            Assert.True(arg.AskForEnter);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AskForEnter_ReturnBool(bool input)
        {
            CommandArguments arg = new(input);
            Assert.True(arg.AskForEnter == input);
        }
    }
}
