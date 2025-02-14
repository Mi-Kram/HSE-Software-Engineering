using Main.Interfaces;
using Main.Models.AnimalArguments;
using Main.Models.Animals;
using Main.Models.Animals.Herbos;

namespace Main.Services.AnimalFactories
{
    public class RabbitFactory : IAnimalFactory<RabbitArguments>
    {
        public Animal Create(int number, RabbitArguments args)
        {
            return new Rabbit(number, args.Food, args.KindnessLevel);
        }
    }
}
