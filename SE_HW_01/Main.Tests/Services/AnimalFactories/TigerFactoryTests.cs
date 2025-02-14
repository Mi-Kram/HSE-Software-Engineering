using Main.Models.AnimalArguments;
using Main.Models.Animals;
using Main.Models.Animals.Predators;
using Main.Services.AnimalFactories;

namespace Main.Tests.Services.AnimalFactories
{
    public class TigerFactoryTests
    {
        [Fact]
        public void Create_ReturnAnimal()
        {
            TigerFactory factory = new();

            Animal animal = factory.Create(0, new TigerArguments());

            Assert.IsType<Tiger>(animal);
        }
    }
}
