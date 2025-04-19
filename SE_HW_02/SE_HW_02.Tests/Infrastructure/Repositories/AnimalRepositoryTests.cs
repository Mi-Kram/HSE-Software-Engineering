using SE_HW_02.Entities.Models;
using SE_HW_02.Infrastructure.Repositories;

namespace SE_HW_02.Tests.Infrastructure.Repositories
{
    public class AnimalRepositoryTests
    {
        [Fact]
        public void Test()
        {
            AnimalRepository repository = new();

            Assert.Throws<ArgumentNullException>(() => repository.Add(null!));
            int? id = repository.Add(new Animal());
            Assert.NotNull(id);

            Assert.Null(repository.Get(id.Value + 1));

            Animal? animal = repository.Get(id.Value);
            Assert.NotNull(animal);
            Assert.Equal(id.Value, animal.ID);

            Assert.Single(repository.GetAll());
            Assert.NotNull(repository.GetAll().FirstOrDefault());
            Assert.Equal(id.Value, repository.GetAll().First().ID);

            Assert.Throws<ArgumentNullException>(() => repository.Update(1, null!));
            Assert.False(repository.Update(id.Value + 1, new Animal()));
            Assert.True(repository.Update(id.Value, new Animal()));

            Assert.Throws<ArgumentNullException>(() => repository.Update(1, null!));
            Assert.False(repository.Remove(id.Value + 1));
            Assert.True(repository.Remove(id.Value));
            Assert.False(repository.Remove(id.Value));
        }
    }
}
