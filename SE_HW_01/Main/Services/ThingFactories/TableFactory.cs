using Main.Interfaces;
using Main.Models.ThingArguments;
using Main.Models.Things;

namespace Main.Services.ThingFactories
{
    public class TableFactory : IInventoryFactory<TableArguments>
    {
        public IInventory Create(int number, TableArguments args)
        {
            return new Table(number);
        }
    }
}
