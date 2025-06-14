using PaymentsService.Domain.Exceptions;

namespace PaymentsService.Tests.PaymentsService.Domain
{
    public class ExceptionsTests
    {
        [Fact]
        public void AccountNotExistsException_Test()
        {
            AccountNotExistsException ex = new AccountNotExistsException(5);
            Assert.Equal("Счет пользователя с id 5 не найден", ex.Message);
        }
        
        [Fact]
        public void EnvVariableException_Test()
        {
            EnvVariableException ex = new EnvVariableException("TestEnv");
            Assert.Equal("TestEnv", ex.EnvName);
            Assert.Equal("TestEnv: переменная среды не найдена", ex.Message);

            Assert.Throws<ArgumentNullException>(() => new EnvVariableException(null!));
        }
    }
}
