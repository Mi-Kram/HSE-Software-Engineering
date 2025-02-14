using Main.Models.AnimalArguments;
using Main.Models.Animals;
using Main.Models.Animals.Herbos;
using Main.Services.AnimalFactories;

namespace Main.Tests.Services.AnimalFactories
{
    public class MonkeyFactoryTests
    {
        [Fact]
        public void Create_ReturnAnimal()
        {
            MonkeyFactory factory = new();

            Animal animal = factory.Create(0, new MonkeyArguments());

            Assert.IsType<Monkey>(animal);
        }
    }
}
