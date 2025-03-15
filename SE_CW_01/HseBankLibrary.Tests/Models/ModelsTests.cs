using HseBankLibrary.Models.Domain;
using HseBankLibrary.Models.Domain.DTO;

namespace HseBankLibrary.Tests.Models
{
    public class ModelsTests
    {
        [Fact]
        public void OpearionToDTO_Tests()
        {
            Operation operation = new()
            {
                ID = "SecretKey",
                BankAccountID = 4,
                CategoryID = 7,
                Amount = 8.3M,
                Date = DateTime.Now,
                IsIncome = true,
                Description = "Test object"
            };

            OperationDTO operationDTO = operation.ToDTO();

            Assert.Equal(operation.ID, operationDTO.ID);
            Assert.Equal(operation.BankAccountID, operationDTO.BankAccountID);
            Assert.Equal(operation.CategoryID, operationDTO.CategoryID);
            Assert.Equal(operation.Amount, operationDTO.Amount);
            Assert.Equal(operation.Date, operationDTO.Date);
            Assert.Equal(operation.IsIncome, operationDTO.IsIncome);
            Assert.Equal(operation.Description, operationDTO.Description);
        }

        [Fact]
        public void CategoryToDTO_Tests()
        {
            Category category = new()
            {
                ID = 7,
                Name = "SuperCategory",
            };

            CategoryDTO categoryDTO = category.ToDTO();

            Assert.Equal(category.ID, categoryDTO.ID);
            Assert.Equal(category.Name, categoryDTO.Name);
        }

        [Fact]
        public void BankAccountToDTO_Tests()
        {
            BankAccount account = new()
            {
                ID = 7,
                Name = "SuperAccount",
                Balance = 7.5M
            };

            BankAccountDTO accountDTO = account.ToDTO();

            Assert.Equal(account.ID, accountDTO.ID);
            Assert.Equal(account.Name, accountDTO.Name);
            Assert.Equal(account.Balance, accountDTO.Balance);
        }

        [Fact]
        public void OpearationValidDataTests()
        {
            Operation operation = new()
            {
                ID = "SecretKey",
                BankAccountID = 4,
                CategoryID = 7,
                Amount = 8.3M,
                Date = DateTime.Now,
                IsIncome = true,
                Description = "Test object"
            };

            Assert.Throws<ArgumentNullException>(() => operation.ID = null!);
            Assert.Throws<InvalidDataException>(() => operation.Amount = 0);
            Assert.Throws<InvalidDataException>(() => operation.Amount = -2);
            Assert.Throws<ArgumentNullException>(() => operation.Description = null!);
        }

        [Fact]
        public void CategoryValidDataTests()
        {
            Category category = new()
            {
                ID = 7,
                Name = "SuperCategory",
            };

            Assert.Throws<ArgumentNullException>(() => category.Name = null!);
        }

        [Fact]
        public void BankAccountValidDataTests()
        {
            BankAccount account = new()
            {
                ID = 7,
                Name = "SuperAccount",
                Balance = 7.5M
            };

            Assert.Throws<ArgumentNullException>(() => account.Name = null!);
        }
    }
}
