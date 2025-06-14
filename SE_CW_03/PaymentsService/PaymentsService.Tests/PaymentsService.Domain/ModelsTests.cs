using PaymentsService.Domain.Models;

namespace PaymentsService.Tests.PaymentsService.Domain
{
    public class ModelsTests
    {
        [Fact]
        public void ApplicationVariables_Exists()
        {
            Assert.NotNull(ApplicationVariables.DB_CONNECTION);
            Assert.NotEmpty(ApplicationVariables.DB_CONNECTION);

            Assert.NotNull(ApplicationVariables.KAFKA);
            Assert.NotEmpty(ApplicationVariables.KAFKA);

            Assert.NotNull(ApplicationVariables.TOPIC_NEW_ORDER);
            Assert.NotEmpty(ApplicationVariables.TOPIC_NEW_ORDER);

            Assert.NotNull(ApplicationVariables.TOPIC_PAID_ORDER);
            Assert.NotEmpty(ApplicationVariables.TOPIC_PAID_ORDER);
        }
    }
}
