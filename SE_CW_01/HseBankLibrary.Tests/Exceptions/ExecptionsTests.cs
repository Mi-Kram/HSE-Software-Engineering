using HseBankLibrary.Exceptions;

namespace HseBankLibrary.Tests.Exceptions
{
    public class ExecptionsTests
    {
        [Fact]
        public void DatabaseEntitiesMismatchExceptionTests()
        {
            DatabaseEntitiesMismatchException ex = new();
            Assert.IsType<Exception>(ex, exactMatch: false);
        }

        [Fact]
        public void OnSaveDatabaseExceptionTests()
        {
            OnSaveDatabaseException ex = new();
            Assert.NotNull(ex.NameOfStorages);

            ex.NameOfStorages.Add("Test1");
            Assert.Single(ex.NameOfStorages);
            Assert.Equal("Test1", ex.NameOfStorages[0]);
        }

        [Fact]
        public void ReadDataExceptionTests()
        {
            Exception inner = new("Test1");
            ReadDataException ex = new(inner);
            Assert.Equal(inner.Message, ex.InnerException?.Message);
        }

        [Fact]
        public void WriteDataExceptionTests()
        {
            Exception inner = new("Test1");
            WriteDataException ex = new(inner);
            Assert.Equal(inner.Message, ex.InnerException?.Message);
        }
    }
}
