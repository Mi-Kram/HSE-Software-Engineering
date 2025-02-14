using Main.Interfaces;
using Main.Models.AnimalArguments;
using Main.Models.Animals;
using Main.Models.Animals.Herbos;

namespace Main.Services.AnimalFactories
{
    public class MonkeyFactory : IAnimalFactory<MonkeyArguments>
    {
        public Animal Create(int number, MonkeyArguments args)
        {
            return new Monkey(number, args.Food, args.KindnessLevel);
        }
    }
}
