using HseBankLibrary.Models.Domain.DTO;
using HseBankLibrary.Models.Domain;
using HseBankLibrary.Factories;

namespace HseBankLibrary.Tests.Factories
{
    public class OperationFactoryTests
    {
        [Fact]
        public void Create_ReturnBankAccount()
        {
            OperationFactory factory = new();

            OperationDTO dto = new()
            {
                ID = "SecretKey",
                BankAccountID = 4,
                CategoryID = 7,
                Amount = 8.3M,
                Date = DateTime.Now,
                IsIncome = true,
                Description = "Test object"
            };

            Operation operation = factory.Create(dto);

            Assert.NotNull(operation);
            Assert.Equal(dto.ID, operation.ID);
            Assert.Equal(dto.BankAccountID, operation.BankAccountID);
            Assert.Equal(dto.CategoryID, operation.CategoryID);
            Assert.Equal(dto.Amount, operation.Amount);
            Assert.Equal(dto.Date, operation.Date);
            Assert.Equal(dto.IsIncome, operation.IsIncome);
            Assert.Equal(dto.Description, operation.Description);
        }

        [Fact]
        public void Create_ThrowException()
        {
            OperationFactory factory = new();

            Assert.Throws<ArgumentNullException>(() => factory.Create(null!));
        }
    }
}
