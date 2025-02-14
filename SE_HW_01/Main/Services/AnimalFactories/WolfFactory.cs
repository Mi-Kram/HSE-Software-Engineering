using Main.Interfaces;
using Main.Models.AnimalArguments;
using Main.Models.Animals;
using Main.Models.Animals.Predators;

namespace Main.Services.AnimalFactories
{
    public class WolfFactory : IAnimalFactory<WolfArguments>
    {
        public Animal Create(int number, WolfArguments args)
        {
            return new Wolf(number, args.Food);
        }
    }
}
