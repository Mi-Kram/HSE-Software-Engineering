using Main.Models.Timing;

namespace Main.Tests.Models.Timing
{
    public class TimingReportTests
    {
        [Fact]
        public void RecordLimitTests()
        {
            TimingReport report = new();
            report.Add("1", TimeSpan.FromSeconds(1));

            report.RecordLimit = 20;
            Assert.Equal(20u, report.RecordLimit);

            report.RecordLimit = 0;
            Assert.Equal(0u, report.RecordLimit);

            report.Add("1", TimeSpan.FromSeconds(2));

            Assert.Equal(2, report.GetResult()[0].Times.Count);

            report.RecordLimit = 1;
            Assert.Single(report.GetResult()[0].Times);
            Assert.Equal(TimeSpan.FromSeconds(2), report.GetResult()[0].Times[0]);
        }

        [Fact]
        public void AddTests()
        {
            TimingReport report = new() { RecordLimit = 1 };
            report.Add("1", TimeSpan.FromSeconds(1));
            report.Add("1", TimeSpan.FromSeconds(2));

            Assert.Single(report.GetResult()[0].Times);
            Assert.Equal(TimeSpan.FromSeconds(2), report.GetResult()[0].Times[0]);
        }

        [Fact]
        public void Add_ThrowException()
        {
            TimingReport report = new();
            Assert.Throws<ArgumentNullException>(() => report.Add(null!, TimeSpan.FromSeconds(1)));
        }
    }
}
