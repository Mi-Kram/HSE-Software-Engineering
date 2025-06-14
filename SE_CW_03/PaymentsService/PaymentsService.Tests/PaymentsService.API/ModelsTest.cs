using PaymentsService.API.DTO;

namespace PaymentsService.Tests.PaymentsService.API
{
    public class ModelsTest
    {
        [Fact]
        public void BalanceTopUpDTO_Test()
        {
            BalanceTopUpDTO dto = new()
            {
                UserID = 5,
                Operation = 20
            };

            Assert.Equal(5, dto.UserID);
            Assert.Equal(20, dto.Operation);
        }

        [Fact]
        public void CreateAccountDTO_Test()
        {
            CreateAccountDTO dto = new()
            {
                Caption = "Test",
                InitialBalance = 20
            };

            Assert.Equal("Test", dto.Caption);
            Assert.Equal(20, dto.InitialBalance);
        }
    }
}
