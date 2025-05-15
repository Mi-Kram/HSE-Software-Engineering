using FileStoringService.Domain.Entities;
using FileStoringService.Domain.Exceptions;
using FileStoringService.Domain.Models;

namespace FileStoringService.Tests.Domain
{
    public class ModelsTests
    {
        [Theory]
        [InlineData(5, 6, "filename", "hash", -8584545744413561961)]
        public void WorkInfo_Tests(int workID, int userID, string title, string hash, long dateBinary)
        {
            WorkInfo work = new()
            {
                ID = workID,
                UserID = userID,
                Title = title,
                Hash = hash,
                Uploaded = DateTime.FromBinary(dateBinary)
            };

            Assert.Equal(workID, work.ID);
            Assert.Equal(userID, work.UserID);
            Assert.Equal(title, work.Title);
            Assert.Equal(hash, work.Hash);
            Assert.Equal(DateTime.FromBinary(dateBinary), work.Uploaded);

            Assert.Throws<ArgumentNullException>(() => work.Hash = null!);
        }

        [Theory]
        [InlineData("myEnv")]
        public void EnvVariableException_Tests(string envName)
        {
            EnvVariableException ex = new(envName);
            Assert.Equal(envName, ex.EnvName);
            Assert.Equal($"{envName}: переменная среды не найдена", ex.Message);
        }

        [Theory]
        [InlineData(5)]
        public void WorkAlreadyUploadedException_Tests(int workID)
        {
            WorkAlreadyUploadedException ex = new(workID);
            Assert.Equal(workID, ex.ID);
            Assert.Equal("Работа уже загружена", ex.Message);
        }

        [Fact]
        public void ApplicationVariables_Tests()
        {
            Assert.NotNull(ApplicationVariables.TOKEN);
            Assert.NotNull(ApplicationVariables.DB_CONNECTION);
            Assert.NotNull(ApplicationVariables.SRORAGE_ADDRESS);
            Assert.NotNull(ApplicationVariables.SRORAGE_LOGIN);
            Assert.NotNull(ApplicationVariables.SRORAGE_PASSWORD);
            Assert.NotNull(ApplicationVariables.SRORAGE_BUCKET);
            Assert.NotNull(ApplicationVariables.SRORAGE_SSL);
        }
    }
}
