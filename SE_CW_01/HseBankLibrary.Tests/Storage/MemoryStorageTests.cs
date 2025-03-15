using HseBankLibrary.Models.Domain;
using HseBankLibrary.Storage;
using HseBankLibrary.Storage.AutoIncrement;
using HseBankLibrary.Tests.Models;
using Moq;

namespace HseBankLibrary.Tests.Storage
{
    public class MemoryStorageTests
    {
        [Fact]
        public void ConstrucorTests()
        {
            UlongAutoIncrement autoIncrement = new();
            MemoryStorage<ulong, Person, Person> storage = new(autoIncrement);
            Assert.True(storage.Ping());
        }

        [Fact]
        public void Construcor_ThowException()
        {
            Assert.Throws<ArgumentNullException>(() => new MemoryStorage<ulong, Person, Person>(null!));
        }

        [Fact]
        public void Add_ReturnsID()
        {
            UlongAutoIncrement autoIncrement = new();
            MemoryStorage<ulong, Person, Person> storage = new(autoIncrement);

            Person p1 = new();
            Person p2 = new();
            Person p3 = new();

            Assert.Empty(storage.GetAll());

            storage.Add(p1);
            storage.Add(p2);
            storage.Add(p3);

            Assert.Equal(3, storage.GetAll().Count());

            Assert.NotNull(storage.Get(0));
            Assert.Equal(0UL, storage.Get(0)?.ID ?? ulong.MaxValue);

            Assert.NotNull(storage.Get(1));
            Assert.Equal(1UL, storage.Get(1)?.ID ?? ulong.MaxValue);

            Assert.NotNull(storage.Get(2));
            Assert.Equal(2UL, storage.Get(2)?.ID ?? ulong.MaxValue);
        }

        [Fact]
        public void Add_ThowException()
        {
            UlongAutoIncrement autoIncrement = new();
            MemoryStorage<ulong, Person, Person> storage = new(autoIncrement);

            Assert.Throws<ArgumentNullException>(() => storage.Add(null!));
        }

        [Fact]
        public void Get_RetunObject()
        {
            UlongAutoIncrement autoIncrement = new();
            MemoryStorage<ulong, Person, Person> storage = new(autoIncrement);

            Person p = new() { ID = 18, Name = "Test", Age = 77 };

            storage.Add(p);
            Assert.Null(storage.Get(18));

            Person? pp = storage.Get(0);
            Assert.NotNull(pp);

            Assert.Equal(p.Name, pp.Name);
            Assert.Equal(p.Age, pp.Age);
        }

        [Fact]
        public void Update_ReturnsBool()
        {
            UlongAutoIncrement autoIncrement = new();
            MemoryStorage<ulong, Person, Person> storage = new(autoIncrement);

            Person p1 = new() { Name = "Test1", Age = 17 };
            Person p2 = new() { Name = "Test2", Age = 18 };
            Person p3 = new() { Name = "Test3", Age = 19 };

            storage.Add(p1);
            storage.Add(p2);
            ulong id = storage.Add(p3);

            Person newPerson = new() { Name = "New", Age = 26 };

            Assert.True(storage.Update(id, newPerson));
            Assert.False(storage.Update(77, newPerson));
            Assert.Equal(p3.ID, newPerson.ID);

            Person? pp = storage.Get(id);
            Assert.NotNull(pp);

            Assert.Equal(newPerson.Name, pp.Name);
            Assert.Equal(newPerson.Age, pp.Age);
        }

        [Fact]
        public void Update_ThowException()
        {
            UlongAutoIncrement autoIncrement = new();
            MemoryStorage<ulong, Person, Person> storage = new(autoIncrement);

            Assert.Throws<ArgumentNullException>(() => storage.Update(1, null!));
        }

        [Fact]
        public void Delete_ReturnBool()
        {
            UlongAutoIncrement autoIncrement = new();
            MemoryStorage<ulong, Person, Person> storage = new(autoIncrement);

            Person p = new() { ID = 18, Name = "Test", Age = 77 };

            ulong id = storage.Add(p);
            Assert.False(storage.Delete(18));
            Assert.True(storage.Delete(id));
        }

        [Fact]
        public void Ping_ReturnTrue()
        {
            UlongAutoIncrement autoIncrement = new();
            MemoryStorage<ulong, Person, Person> storage = new(autoIncrement);

            Assert.True(storage.Ping());
        }

        [Fact]
        public void RewriteTests()
        {
            UlongAutoIncrement autoIncrement = new();
            MemoryStorage<ulong, Person, Person> storage1 = new(autoIncrement);
            MemoryStorage<ulong, Person, Person> storage2 = new(autoIncrement);

            Person p1 = new() { Name = "Test1", Age = 17 };
            Person p2 = new() { Name = "Test2", Age = 18 };
            Person p3 = new() { Name = "Test3", Age = 19 };

            storage1.Add(p1);
            storage1.Add(p2);
            storage1.Add(p3);

            storage2.Rewrite(storage1);
            Assert.Equal(3, storage2.GetAll().Count());

            Assert.NotNull(storage2.Get(p3.ID));
            Assert.NotNull(storage2.Get(p2.ID));
            Assert.NotNull(storage2.Get(p1.ID));
        }

        [Fact]
        public void Rewrite_ThrowException0()
        {
            UlongAutoIncrement autoIncrement = new();
            MemoryStorage<ulong, Person, Person> storage = new(autoIncrement);

            Assert.Throws<ArgumentNullException>(() => storage.Rewrite(null!));
        }

        [Fact]
        public void Rewrite_ThrowException1()
        {
            UlongAutoIncrement autoIncrement = new();
            MemoryStorage<ulong, Person, Person> storage = new(autoIncrement);

            List<Person> lst = null!;

            Mock<IStorage<ulong, Person, Person>> mock = new();
            mock.Setup(x => x.GetAll()).Returns(lst);
            Assert.Throws<InvalidDataException>(() => storage.Rewrite(mock.Object));
        }

        [Fact]
        public void Rewrite_ThrowException2()
        {
            UlongAutoIncrement autoIncrement = new();
            MemoryStorage<ulong, Person, Person> storage = new(autoIncrement);

            List<Person> lst =
            [
                new() { ID = 0, Name = "Test1", Age = 17 },
                null!,
                new() { ID = 2, Name = "Test3", Age = 19 }
            ];

            Mock<IStorage<ulong, Person, Person>> mock = new();
            mock.Setup(x => x.GetAll()).Returns(lst);
            Assert.Throws<InvalidDataException>(() => storage.Rewrite(mock.Object));
        }

        [Fact]
        public void Rewrite_ThrowException3()
        {
            UlongAutoIncrement autoIncrement = new();
            MemoryStorage<ulong, Person, Person> storage = new(autoIncrement);

            List<Person> lst =
            [
                new() { ID = 0, Name = "Test1", Age = 17 },
                new() { ID = 1, Name = "Test2", Age = 18 },
                new() { ID = 0, Name = "Test3", Age = 19 }
            ];

            Mock<IStorage<ulong, Person, Person>> mock = new();
            mock.Setup(x => x.GetAll()).Returns(lst);
            Assert.Throws<InvalidDataException>(() => storage.Rewrite(mock.Object));
        }

        [Fact]
        public void StringAutoIncrement_GetNext()
        {
            StringAutoIncrement s = new();

            Assert.NotEqual(s.GetNext(), s.GetNext());
        }
    }
}
