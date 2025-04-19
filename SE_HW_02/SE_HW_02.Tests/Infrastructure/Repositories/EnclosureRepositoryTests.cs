using SE_HW_02.Entities.Models;
using SE_HW_02.Infrastructure.Repositories;

namespace SE_HW_02.Tests.Infrastructure.Repositories
{
    public class EnclosureRepositoryTests
    {
        [Fact]
        public void Test()
        {
            EnclosureRepository repository = new();

            Assert.Throws<ArgumentNullException>(() => repository.Add(null!));
            int? id = repository.Add(new Enclosure());
            Assert.NotNull(id);

            Assert.Null(repository.Get(id.Value + 1));

            Enclosure? enclosure = repository.Get(id.Value);
            Assert.NotNull(enclosure);
            Assert.Equal(id.Value, enclosure.ID);

            Assert.Single(repository.GetAll());
            Assert.NotNull(repository.GetAll().FirstOrDefault());
            Assert.Equal(id.Value, repository.GetAll().First().ID);

            Assert.Throws<ArgumentNullException>(() => repository.Update(1, null!));
            Assert.False(repository.Update(id.Value + 1, new Enclosure()));
            Assert.True(repository.Update(id.Value, new Enclosure()));

            Assert.Throws<ArgumentNullException>(() => repository.Update(1, null!));
            Assert.False(repository.Remove(id.Value + 1));
            Assert.True(repository.Remove(id.Value));
            Assert.False(repository.Remove(id.Value));
        }
    }
}
