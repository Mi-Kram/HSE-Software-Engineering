using OrderStatusChangeNotifier.Domain.Exceptions;
using OrderStatusChangeNotifier.Domain.Models;

namespace OrderStatusChangeNotifier.Tests.OrderStatusChangeNotifier.Domain
{
    public class ModelsTests
    {
        [Fact]
        public void ApplicationVariables_Test()
        {
            Assert.NotNull(ApplicationVariables.KAFKA);
            Assert.NotEmpty(ApplicationVariables.KAFKA);

            Assert.NotNull(ApplicationVariables.TOPIC_NEW_ORDER);
            Assert.NotEmpty(ApplicationVariables.TOPIC_NEW_ORDER);

            Assert.NotNull(ApplicationVariables.TOPIC_PAID_ORDER);
            Assert.NotEmpty(ApplicationVariables.TOPIC_PAID_ORDER);
        }

        [Fact]
        public void EnvVariableException_Test()
        {
            EnvVariableException ex = new("Test");
            Assert.Equal("Test: переменная среды не найдена", ex.Message);
            Assert.Equal("Test", ex.EnvName);
        }
    }
}
