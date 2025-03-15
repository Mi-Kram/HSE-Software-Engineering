using HseBankLibrary.Factories;
using HseBankLibrary.Storage;
using HseBankLibrary.Storage.AutoIncrement;
using HseBankLibrary.Tests.Models;
using ImportExportLibrary;
using Moq;

namespace HseBankLibrary.Tests.Storage
{
    public class FileStorageTests
    {
        [Fact]
        public void Constructor_ThrowException()
        {
            IDataParser<Person> parser = new Mock<IDataParser<Person>>().Object;
            IAutoIncrement<ulong> ainc = new Mock<IAutoIncrement<ulong>>().Object;
            IDomainFactory<Person, Person> factory = new Mock<IDomainFactory<Person, Person>>().Object;

            Assert.Throws<ArgumentNullException>(() => new FileStorage<ulong, Person, Person>(null!, parser, ainc, factory));
            Assert.Throws<ArgumentNullException>(() => new FileStorage<ulong, Person, Person>("", null!, ainc, factory));
            Assert.Throws<ArgumentNullException>(() => new FileStorage<ulong, Person, Person>("", parser, null!, factory));
            Assert.Throws<ArgumentNullException>(() => new FileStorage<ulong, Person, Person>("", parser, ainc, null!));
        }

        [Fact]
        public void WriteDataTests()
        {
            JsonParser<Person> parser = new();
            UlongAutoIncrement ainc = new();
            Mock<IDomainFactory<Person, Person>> mockFactory = new();
            mockFactory.Setup(x => x.Create(It.IsAny<Person>())).Returns<Person>(x => x);
            IDomainFactory<Person, Person> factory = mockFactory.Object;

            string fileName = Guid.NewGuid().ToString();

            FileStorage<ulong, Person, Person> storage = new(fileName, parser, ainc, factory);
            Assert.True(storage.Ping());

            Person p1 = new() { Name = "Test1", Age = 17 };
            Person p2 = new() { Name = "Test2", Age = 18 };
            Person p3 = new() { Name = "Test3", Age = 19 };

            storage.Add(p1);
            storage.Add(p2);
            storage.Add(p3);

            string json = File.ReadAllText(fileName);

            using StringWriter sw = new();
            parser.Export([p1, p2, p3], sw);

            File.Delete(fileName);
            Assert.Equal(sw.ToString(), json);
        }

        [Fact]
        public void ReadDataTests()
        {
            JsonParser<Person> parser = new();
            UlongAutoIncrement ainc = new();
            Mock<IDomainFactory<Person, Person>> mockFactory = new();
            mockFactory.Setup(x => x.Create(It.IsAny<Person>())).Returns<Person>(x => x);
            IDomainFactory<Person, Person> factory = mockFactory.Object;

            Person p1 = new() { Name = "Test1", Age = 17 };
            Person p2 = new() { Name = "Test2", Age = 18 };
            Person p3 = new() { Name = "Test3", Age = 19 };

            string fileName = Guid.NewGuid().ToString();
            using (StreamWriter sw = new(fileName))
            {
                parser.Export([p1, p2, p3], sw);
            }

            FileStorage<ulong, Person, Person> storage = new(fileName, parser, ainc, factory);
            Assert.True(storage.Ping());

            Assert.NotNull(storage.Get(p1.ID));
            Assert.NotNull(storage.Get(p2.ID));
            Assert.NotNull(storage.Get(p3.ID));

            File.Delete(fileName);
        }

        [Fact]
        public void Ping_RetunBool()
        {
            IDataParser<Person> parser = new Mock<IDataParser<Person>>().Object;
            IAutoIncrement<ulong> ainc = new Mock<IAutoIncrement<ulong>>().Object;
            IDomainFactory<Person, Person> factory = new Mock<IDomainFactory<Person, Person>>().Object;


            FileInfo[] files = new DirectoryInfo("./").GetFiles();
            Assert.NotEmpty(files);

            {
                FileStorage<ulong, Person, Person> storage = new(files[0].FullName, parser, ainc, factory);
                Assert.True(storage.Ping());
            }

            {
                string fileName = Guid.NewGuid().ToString();
                FileStorage<ulong, Person, Person> storage = new(fileName, parser, ainc, factory);
                Assert.True(storage.Ping());
                File.Delete(fileName);
            }
        }


    }
}
