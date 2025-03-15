using HseBankLibrary.Storage;
using HseBankLibrary.Storage.AutoIncrement;
using HseBankLibrary.Tests.Models;
using Moq;

namespace HseBankLibrary.Tests.Storage
{
    public class CachedStorageTests
    {
        [Fact]
        public void Construcot_ThrowException()
        {
            var storage = new Mock<IStorage<ulong, Person, Person>>().Object;
            var ainc = new Mock<IAutoIncrement<ulong>>().Object;

            Assert.Throws<ArgumentNullException>(() => CachedStorage<ulong, Person, Person>.Create(null!, ainc));
            Assert.Throws<ArgumentNullException>(() => CachedStorage<ulong, Person, Person>.Create(storage, null!));
        }

        [Fact]
        public void PingTests()
        {
            Mock<IStorage<ulong, Person, Person>> mockStorage = new();

            mockStorage.Setup(x => x.Ping()).Returns(true);
            var trueStorage = mockStorage.Object;

            var ainc = new Mock<IAutoIncrement<ulong>>().Object;

            var storage = CachedStorage<ulong, Person, Person>.Create(trueStorage, ainc);
            Assert.True(storage.Ping());

            mockStorage.Setup(x => x.Ping()).Returns(false);
            var falseStorage = mockStorage.Object;

            storage = CachedStorage<ulong, Person, Person>.Create(falseStorage, ainc);
            Assert.False(storage.Ping());
        }

        [Fact]
        public void SaveDataTests()
        {
            UlongAutoIncrement ainc = new();
            MemoryStorage<ulong, Person, Person> memStorage = new(ainc);

            Person p1 = new() { Name = "Test1", Age = 17 };
            Person p2 = new() { Name = "Test2", Age = 18 };
            Person p3 = new() { Name = "Test3", Age = 19 };

            memStorage.Add(p1);
            memStorage.Add(p2);
            memStorage.Add(p3);

            var cacheStorage = CachedStorage<ulong, Person, Person>.Create(memStorage, ainc);

            Assert.Equal(3, cacheStorage.GetAll().Count());

            cacheStorage.Add(new Person());
            Assert.Equal(3, memStorage.GetAll().Count());

            cacheStorage.SaveData();
            Assert.Equal(4, memStorage.GetAll().Count());
        }
    }
}
