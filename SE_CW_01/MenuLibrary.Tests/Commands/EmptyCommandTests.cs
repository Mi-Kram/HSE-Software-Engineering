using MenuLibrary.Commands;
using Moq;

namespace MenuLibrary.Tests.Commands
{
    public class EmptyCommandTests
    {
        [Fact]
        public async Task Instance_RetunSingleObject()
        {
            EmptyCommand? cmd1 = null, cmd2 = null;
            Task task = Task.Run(() => cmd1 = EmptyCommand.Instance);
            cmd2 = EmptyCommand.Instance;

            await task.WaitAsync(CancellationToken.None);

            Assert.NotNull(cmd1);
            Assert.True(cmd1 == cmd2);
        }

        [Fact]
        public void Execute_RetunVoid()
        {
            EmptyCommand.Instance.Execute(null!);
        }
    }
}
