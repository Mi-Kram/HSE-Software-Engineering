using HseBankLibrary.Factories;
using HseBankLibrary.Models.Domain;
using HseBankLibrary.Models.Domain.DTO;

namespace HseBankLibrary.Tests.Factories
{
    public class BankAccountFactoryTests
    {
        [Fact]
        public void Create_ReturnBankAccount()
        {
            BankAccountFactory factory = new();

            BankAccountDTO dto = new()
            {
                ID = 7,
                Name = "SuperAccount",
                Balance = 7.5M
            };

            BankAccount account = factory.Create(dto);

            Assert.NotNull(account);
            Assert.Equal(dto.ID, account.ID);
            Assert.Equal(dto.Name, account.Name);
            Assert.Equal(dto.Balance, account.Balance);
        }

        [Fact]
        public void Create_ThrowException()
        {
            BankAccountFactory factory = new();

            Assert.Throws<ArgumentNullException>(() => factory.Create(null!));
        }
    }
}
