using Gateway.Domain.Exceptions;
using Gateway.Domain.Models;

namespace Gateway.Tests.Domain
{
    public class ModelsTests
    {
        [Fact]
        public void ApplicationVariables_Tests()
        {
            Assert.NotNull(ApplicationVariables.STORAGE_TOKEN);
            Assert.NotNull(ApplicationVariables.STORAGE_SERVER);
            Assert.NotNull(ApplicationVariables.ANALYSE_TOKEN);
            Assert.NotNull(ApplicationVariables.ANALYSE_SERVER);
        }

        [Theory]
        [InlineData("myEnv")]
        public void EnvVariableException_Tests(string envName)
        {
            EnvVariableException ex = new(envName);
            Assert.Equal(envName, ex.EnvName);
            Assert.Equal($"{envName}: переменная среды не найдена", ex.Message);
        }

    }
}
