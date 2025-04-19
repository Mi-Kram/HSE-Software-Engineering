using SE_HW_02.Entities.Models;
using SE_HW_02.Web.DTO;

namespace SE_HW_02.Tests.Web.DTO
{
    public class DtoTests
    {
        [Fact]
        public void EnclosureDTO_Test()
        {
            Dimensions dim = new Dimensions()
            {
                Width = 5,
                Height = 7,
                Length = 2
            };

            EnclosureDTO dto = new()
            {
                Type = "A",
                AnimalsCapacity = 10,
                Size = dim
            };

            Assert.Equal("A", dto.Type);
            Assert.Equal(10, dto.AnimalsCapacity);
            Assert.Equal(dim, dto.Size);
        }
    }
}
