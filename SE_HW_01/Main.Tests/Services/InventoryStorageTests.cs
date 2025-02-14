using Main.Interfaces;
using Main.Models.AnimalArguments;
using Main.Models.Animals;
using Main.Models.ThingArguments;
using Main.Services;
using Main.Services.AnimalFactories;
using Main.Services.ThingFactories;

namespace Main.Tests.Services
{
    public class InventoryStorageTests
    {
        [Fact]
        public void AddWithArgs_ReturnInt()
        {
            TableFactory factory = new TableFactory();
            InventoryStorage inventoryStorage = new();

            for (int i = 1; i < 5; i++)
            {
                int number = inventoryStorage.Add(factory, new TableArguments());
                Assert.Equal(i, number);
            }
        }

        [Fact]
        public void Add_ReturnInt()
        {
            TableFactory tableFactory = new();
            TigerFactory tigerFactory = new();
            InventoryStorage inventoryStorage = new();

            for (int i = 1; i <= 5; i++)
            {
                int number = inventoryStorage.Add(tableFactory.Create(0, new TableArguments()));
                Assert.Equal(i, number);
            }

            for (int i = 6; i < 10; i++)
            {
                int number = inventoryStorage.Add(tigerFactory.Create(0, new TigerArguments()));
                Assert.Equal(i, number);
            }
        }

        [Fact]
        public void GetAll_ReturnIEnumerable()
        {
            TableFactory tableFactory = new();
            TigerFactory tigerFactory = new();
            InventoryStorage inventoryStorage = new();

            for (int i = 1; i <= 5; i++)
            {
                inventoryStorage.Add(tableFactory.Create(0, new TableArguments()));
                inventoryStorage.Add(tigerFactory.Create(0, new TigerArguments()));
            }

            IEnumerable<IInventory> inventories = inventoryStorage.GetAll();
            Assert.Equal(10, inventories.Count());
        }

        [Fact]
        public void GetAllAnimals_ReturnIEnumerable()
        {
            TableFactory tableFactory = new();
            TigerFactory tigerFactory = new();
            InventoryStorage inventoryStorage = new();

            for (int i = 1; i <= 5; i++)
            {
                inventoryStorage.Add(tableFactory.Create(0, new TableArguments()));
                inventoryStorage.Add(tigerFactory.Create(0, new TigerArguments()));
            }

            IEnumerable<Animal> animals = inventoryStorage.GetAllAnimals();
            Assert.Equal(5, animals.Count());
        }

        [Fact]
        public void Contains_ReturnBool()
        {
            TableFactory tableFactory = new();
            InventoryStorage inventoryStorage = new();

            Assert.False(inventoryStorage.Contains(1));
            inventoryStorage.Add(tableFactory, new TableArguments());
            Assert.True(inventoryStorage.Contains(1));
        }

        [Fact]
        public void Get_ReturnIInventory()
        {
            TableFactory tableFactory = new();
            InventoryStorage inventoryStorage = new();

            Assert.Null(inventoryStorage.Get(1));

            inventoryStorage.Add(tableFactory, new TableArguments());
            Assert.NotNull(inventoryStorage.Get(1));
        }

        [Fact]
        public void Remove_ReturnBool()
        {
            TableFactory tableFactory = new();
            InventoryStorage inventoryStorage = new();

            Assert.False(inventoryStorage.Remove(1));
            inventoryStorage.Add(tableFactory, new TableArguments());
            Assert.True(inventoryStorage.Remove(1));
            Assert.False(inventoryStorage.Remove(1));
        }

        [Fact]
        public void GetCount_ReturnInt()
        {
            TableFactory tableFactory = new();
            TigerFactory tigerFactory = new();
            InventoryStorage inventoryStorage = new();

            for (int i = 1; i <= 5; i++)
            {
                inventoryStorage.Add(tableFactory.Create(0, new TableArguments()));
                inventoryStorage.Add(tigerFactory.Create(0, new TigerArguments()));
            }

            Assert.Equal(10, inventoryStorage.GetCount());
        }
    }
}
