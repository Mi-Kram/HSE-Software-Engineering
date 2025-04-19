using SE_HW_02.Entities.Events;

namespace SE_HW_02.Entities.Tests
{
    public class EventsTests
    {
        [Theory]
        [InlineData(4, 5, 7)]
        public void AnimalMovedEventTests(int id, int from, int to)
        {
            AnimalMovedEvent e = new AnimalMovedEvent(id, from, to);
            Assert.Equal(id, e.AnimalID);
            Assert.Equal(from, e.FromEnclosureID);
            Assert.Equal(to, e.ToEnclosureID);
        }

        [Theory]
        [InlineData(7)]
        public void FeedingTimeEventTests(int id)
        {
            FeedingTimeEvent e = new FeedingTimeEvent(id);
            Assert.Equal(id, e.FeedingScheduleID);
        }
    }
}
