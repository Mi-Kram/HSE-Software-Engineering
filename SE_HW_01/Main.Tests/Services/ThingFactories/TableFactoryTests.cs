using Main.Interfaces;
using Main.Models.ThingArguments;
using Main.Models.Things;
using Main.Services.ThingFactories;

namespace Main.Tests.Services.ThingFactories
{
    public class TableFactoryTests
    {
        [Fact]
        public void Create_ReturnAnimal()
        {
            TableFactory factory = new();

            IInventory inventory = factory.Create(0, new TableArguments());

            Assert.IsType<Table>(inventory);
        }
    }
}
