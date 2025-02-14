using Main.Interfaces;
using Main.Models.ThingArguments;
using Main.Models.Things;

namespace Main.Services.ThingFactories
{
    public class ComputerFactory : IInventoryFactory<ComputerArguments>
    {
        public IInventory Create(int number, ComputerArguments args)
        {
            return new Computer(number);
        }
    }
}
