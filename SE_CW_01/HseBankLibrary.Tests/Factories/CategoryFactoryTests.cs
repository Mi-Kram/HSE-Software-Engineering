using HseBankLibrary.Models.Domain.DTO;
using HseBankLibrary.Models.Domain;
using HseBankLibrary.Factories;

namespace HseBankLibrary.Tests.Factories
{
    public class CategoryFactoryTests
    {
        [Fact]
        public void Create_ReturnBankAccount()
        {
            CategoryFactory factory = new();

            CategoryDTO dto = new()
            {
                ID = 7,
                Name = "SuperCategory",
            };

            Category category = factory.Create(dto);

            Assert.NotNull(category);
            Assert.Equal(dto.ID, category.ID);
            Assert.Equal(dto.Name, category.Name);
        }

        [Fact]
        public void Create_ThrowException()
        {
            CategoryFactory factory = new();

            Assert.Throws<ArgumentNullException>(() => factory.Create(null!));
        }
    }
}
