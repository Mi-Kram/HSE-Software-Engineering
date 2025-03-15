using Main.Models.Timing;
using MenuLibrary.Commands;

namespace Main.Tests.Models.Timing
{
    public class TimingDecoratorTests
    {
        [Fact]
        public void CheckTime()
        {
            SleepCommand cmd = new(150);
            TimingReport reporter = new();

            TimingDecorator decorator = new(cmd, "lab", reporter);
            decorator.Execute(null!);

            var lst = reporter.GetResult();
            Assert.Single(lst);
            Assert.Equal("lab", lst[0].Label);
            Assert.Single(lst[0].Times);
            Assert.InRange(lst[0].Times[0], TimeSpan.FromMilliseconds(120), TimeSpan.FromMilliseconds(180));
        }
    }

    internal class SleepCommand(uint milliseconds) : ICommand<CommandArguments>
    {
        private readonly uint milliseconds = milliseconds;

        public void Execute(CommandArguments args)
        {
            Thread.Sleep((int)milliseconds);
        }
    }
}
