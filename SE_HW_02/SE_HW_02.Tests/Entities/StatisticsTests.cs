using SE_HW_02.Entities.Models.Statistics;

namespace SE_HW_02.Entities.Tests
{
    public class StatisticsTests
    {
        [Theory]
        [InlineData(5, 8, 3, 4)]
        public void AnimalsStatisticsTests(int totalAmount, int capacity, int maleAmount, int healthyAmount)
        {
            AnimalsStatistics stat = new AnimalsStatistics()
            {
                TotalAmount = totalAmount,
                TotalCapacity = capacity,
                MaleAmount = maleAmount,
                HealthyAmount = healthyAmount
            };

            Assert.Equal(totalAmount, stat.TotalAmount);
            Assert.Equal(capacity, stat.TotalCapacity);
            Assert.Equal(maleAmount, stat.MaleAmount);
            Assert.Equal(healthyAmount, stat.HealthyAmount);
            Assert.NotNull(stat.TypeAmount);
        }

        [Theory]
        [InlineData(10, 3)]
        public void EnclosureStatisticsTests(int totalAmount, int emptyAmount)
        {
            EnclosureStatistics stat = new EnclosureStatistics()
            {
                TotalAmount = totalAmount,
                EmptyAmount = emptyAmount
            };

            Assert.Equal(totalAmount, stat.TotalAmount);
            Assert.Equal(emptyAmount, stat.EmptyAmount);
            Assert.NotNull(stat.TypeAmount);
        }

        [Theory]
        [InlineData(10, 6)]
        public void FeedingScheduleStatisticsTests(int amount, int completed)
        {
            FeedingScheduleStatistics stat = new FeedingScheduleStatistics()
            {
                Amount = amount,
                CompletedAmount = completed
            };

            Assert.Equal(amount, stat.Amount);
            Assert.Equal(completed, stat.CompletedAmount);
            Assert.NotNull(stat.TimeAmount);
        }
    }
}
