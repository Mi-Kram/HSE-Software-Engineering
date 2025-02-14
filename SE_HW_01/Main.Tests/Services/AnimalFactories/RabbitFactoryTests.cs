using Main.Models.AnimalArguments;
using Main.Models.Animals;
using Main.Models.Animals.Herbos;
using Main.Services.AnimalFactories;

namespace Main.Tests.Services.AnimalFactories
{
    public class RabbitFactoryTests
    {
        [Fact]
        public void Create_ReturnAnimal()
        {
            RabbitFactory factory = new();

            Animal animal = factory.Create(0, new RabbitArguments());

            Assert.IsType<Rabbit>(animal);
        }
    }
}
