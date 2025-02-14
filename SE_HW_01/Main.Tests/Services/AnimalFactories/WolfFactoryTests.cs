using Main.Models.AnimalArguments;
using Main.Models.Animals;
using Main.Models.Animals.Predators;

namespace Main.Services.AnimalFactories
{
    public class WolfFactoryTests
    {
        [Fact]
        public void Create_ReturnAnimal()
        {
            WolfFactory factory = new();

            Animal animal = factory.Create(0, new WolfArguments());

            Assert.IsType<Wolf>(animal);
        }
    }
}
