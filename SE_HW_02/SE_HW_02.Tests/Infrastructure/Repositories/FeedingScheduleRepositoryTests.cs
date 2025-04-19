using SE_HW_02.Entities.Models;
using SE_HW_02.Infrastructure.Repositories;

namespace SE_HW_02.Tests.Infrastructure.Repositories
{
    public class FeedingScheduleRepositoryTests
    {
        [Fact]
        public void Test()
        {
            FeedingScheduleRepository repository = new();

            Assert.Throws<ArgumentNullException>(() => repository.Add(null!));
            int? id = repository.Add(new FeedingSchedule());
            Assert.NotNull(id);

            Assert.Null(repository.Get(id.Value + 1));

            FeedingSchedule? schedule = repository.Get(id.Value);
            Assert.NotNull(schedule);
            Assert.Equal(id.Value, schedule.ID);

            Assert.Single(repository.GetAll());
            Assert.NotNull(repository.GetAll().FirstOrDefault());
            Assert.Equal(id.Value, repository.GetAll().First().ID);

            Assert.Throws<ArgumentNullException>(() => repository.Update(1, null!));
            Assert.False(repository.Update(id.Value + 1, new FeedingSchedule()));
            Assert.True(repository.Update(id.Value, new FeedingSchedule()));

            Assert.Throws<ArgumentNullException>(() => repository.Update(1, null!));
            Assert.False(repository.Remove(id.Value + 1));
            Assert.True(repository.Remove(id.Value));
            Assert.False(repository.Remove(id.Value));
        }

    }
}
