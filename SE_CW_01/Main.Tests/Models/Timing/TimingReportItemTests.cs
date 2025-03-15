using Main.Models.Timing;

namespace Main.Tests.Models.Timing
{
    public class TimingReportItemTests
    {
        [Fact]
        public void Tests()
        {
            TimeSpan t1 = TimeSpan.FromSeconds(5);
            TimeSpan t2 = TimeSpan.FromHours(2);
            TimeSpan t3 = TimeSpan.FromDays(77);

            TimingReportItem item = new("TestLabel", [t1, t2, t3]);
            Assert.Equal("TestLabel", item.Label);
            Assert.Equal(3, item.Times.Count);
            Assert.Equal(t1, item.Times[0]);
            Assert.Equal(t2, item.Times[1]);
            Assert.Equal(t3, item.Times[2]);
        }
    }
}
