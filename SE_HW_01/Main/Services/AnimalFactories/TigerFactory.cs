using Main.Interfaces;
using Main.Models.AnimalArguments;
using Main.Models.Animals;
using Main.Models.Animals.Predators;

namespace Main.Services.AnimalFactories
{
    public class TigerFactory : IAnimalFactory<TigerArguments>
    {
        public Animal Create(int number, TigerArguments args)
        {
            return new Tiger(number, args.Food);
        }
    }
}
