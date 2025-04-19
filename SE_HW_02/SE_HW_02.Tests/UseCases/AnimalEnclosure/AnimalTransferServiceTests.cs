using Microsoft.Extensions.DependencyInjection;
using SE_HW_02.Entities.Models;
using SE_HW_02.Infrastructure.Repositories;
using SE_HW_02.UseCases.AnimalEnclosure;
using SE_HW_02.UseCases.Animals;
using SE_HW_02.UseCases.Enclosures;

namespace SE_HW_02.Tests.UseCases.AnimalEnclosure
{
    public class AnimalTransferServiceTests
    {
        private static IServiceProvider GetServiceProvider()
        {
            ServiceCollection services = new();
            services.AddSingleton<IAnimalRepository, AnimalRepository>();
            services.AddSingleton<IEnclosureRepository, EnclosureRepository>();
            return services.BuildServiceProvider();
        }

        [Fact]
        public void Register_ReturnFalse()
        {
            IServiceProvider provider = GetServiceProvider();
            AnimalTransferService service = new(provider);

            IAnimalRepository animalRepository = provider.GetRequiredService<IAnimalRepository>();
            IEnclosureRepository enclosureRepository = provider.GetRequiredService<IEnclosureRepository>();

            Assert.False(service.Register(100, 100));

            int? animalID = animalRepository.Add(new Animal());
            Assert.NotNull(animalID);

            Assert.False(service.Register(animalID.Value, 1));

            int? enclosureID = enclosureRepository.Add(new Enclosure() { AnimalsAmount = 5, AnimalsCapacity = 5 });
            Assert.NotNull(enclosureID);

            Assert.False(service.Register(animalID.Value, enclosureID.Value));
        }

        [Fact]
        public void Register_ReturnTrue()
        {
            IServiceProvider provider = GetServiceProvider();
            AnimalTransferService service = new(provider);

            IAnimalRepository animalRepository = provider.GetRequiredService<IAnimalRepository>();
            IEnclosureRepository enclosureRepository = provider.GetRequiredService<IEnclosureRepository>();

            int? animalID = animalRepository.Add(new Animal());
            Assert.NotNull(animalID);

            int? enclosureID = enclosureRepository.Add(new Enclosure() { AnimalsCapacity = 5 });
            Assert.NotNull(enclosureID);

            Assert.True(service.Register(animalID.Value, enclosureID.Value));

            Animal? a = animalRepository.Get(animalID.Value);
            Assert.NotNull(a);

            Enclosure? b = enclosureRepository.Get(enclosureID.Value);
            Assert.NotNull(b);

            Assert.Equal(1, b.AnimalsAmount);
            Assert.Equal(enclosureID.Value, a.EnclosureID);
        }

        [Fact]
        public void Unregister_ReturnFalse()
        {
            IServiceProvider provider = GetServiceProvider();
            AnimalTransferService service = new(provider);

            IAnimalRepository animalRepository = provider.GetRequiredService<IAnimalRepository>();

            Assert.False(service.Unregister(100));

            int? animalID = animalRepository.Add(new Animal());
            Assert.NotNull(animalID);

            Assert.False(service.Unregister(animalID.Value));
        }

        [Fact]
        public void Unregister_ReturnTrue()
        {
            IServiceProvider provider = GetServiceProvider();
            AnimalTransferService service = new(provider);

            IAnimalRepository animalRepository = provider.GetRequiredService<IAnimalRepository>();
            IEnclosureRepository enclosureRepository = provider.GetRequiredService<IEnclosureRepository>();

            int? animalID = animalRepository.Add(new Animal());
            Assert.NotNull(animalID);

            int? enclosureID = enclosureRepository.Add(new Enclosure() { AnimalsCapacity = 5 });
            Assert.NotNull(enclosureID);

            Assert.True(service.Register(animalID.Value, enclosureID.Value));
            Assert.True(service.Unregister(animalID.Value));

            Enclosure? b = enclosureRepository.Get(enclosureID.Value);
            Assert.NotNull(b);

            Assert.Equal(0, b.AnimalsAmount);
        }

        [Fact]
        public void Transfer_ReturnFalse()
        {
            IServiceProvider provider = GetServiceProvider();
            AnimalTransferService service = new(provider);

            IAnimalRepository animalRepository = provider.GetRequiredService<IAnimalRepository>();
            IEnclosureRepository enclosureRepository = provider.GetRequiredService<IEnclosureRepository>();

            Assert.False(service.Transfer(100, 100));

            int? animalID = animalRepository.Add(new Animal());
            Assert.NotNull(animalID);

            Assert.False(service.Transfer(animalID.Value, 100));
        }

        [Fact]
        public void Transfer_ReturnTrue()
        {
            IServiceProvider provider = GetServiceProvider();
            AnimalTransferService service = new(provider);

            IAnimalRepository animalRepository = provider.GetRequiredService<IAnimalRepository>();
            IEnclosureRepository enclosureRepository = provider.GetRequiredService<IEnclosureRepository>();

            int? animalID = animalRepository.Add(new Animal());
            Assert.NotNull(animalID);

            int? enclosure1ID = enclosureRepository.Add(new Enclosure() { AnimalsCapacity = 5 });
            Assert.NotNull(enclosure1ID);
            Assert.True(service.Register(animalID.Value, enclosure1ID.Value));

            Assert.True(service.Transfer(animalID.Value, enclosure1ID.Value));

            int? enclosure2ID = enclosureRepository.Add(new Enclosure() { AnimalsCapacity = 5 });
            Assert.NotNull(enclosure2ID);

            Assert.True(service.Transfer(animalID.Value, enclosure2ID.Value));

            Enclosure? a = enclosureRepository.Get(enclosure1ID.Value);
            Assert.NotNull(a);
            Assert.Equal(0, a.AnimalsAmount);

            Enclosure? b = enclosureRepository.Get(enclosure2ID.Value);
            Assert.NotNull(b);
            Assert.Equal(1, b.AnimalsAmount);
        }

        [Fact]
        public void TransferEvent_Tests()
        {
            IServiceProvider provider = GetServiceProvider();
            AnimalTransferService service = new(provider);

            IAnimalRepository animalRepository = provider.GetRequiredService<IAnimalRepository>();
            IEnclosureRepository enclosureRepository = provider.GetRequiredService<IEnclosureRepository>();

            int? animalID = animalRepository.Add(new Animal());
            Assert.NotNull(animalID);

            int? enclosure1ID = enclosureRepository.Add(new Enclosure() { AnimalsCapacity = 5 });
            Assert.NotNull(enclosure1ID);
            Assert.True(service.Register(animalID.Value, enclosure1ID.Value));

            int? enclosure2ID = enclosureRepository.Add(new Enclosure() { AnimalsCapacity = 5 });
            Assert.NotNull(enclosure2ID);

            service.OnAnimalTransfered += (_, e) =>
            {
                Assert.Equal(animalID.Value, e.AnimalID);
                Assert.Equal(enclosure1ID.Value, e.FromEnclosureID);
                Assert.Equal(enclosure2ID.Value, e.ToEnclosureID);
            };

            Assert.True(service.Transfer(animalID.Value, enclosure2ID.Value));
        }
    }
}
