using Main.Interfaces;
using Main.Models.ThingArguments;
using Main.Models.Things;
using Main.Services.ThingFactories;

namespace Main.Tests.Services.ThingFactories
{
    public class ComputerFactoryTests
    {
        [Fact]
        public void Create_ReturnAnimal()
        {
            ComputerFactory factory = new();

            IInventory inventory = factory.Create(0, new ComputerArguments());

            Assert.IsType<Computer>(inventory);
        }
    }
}
