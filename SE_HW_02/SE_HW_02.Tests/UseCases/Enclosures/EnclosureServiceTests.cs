using Microsoft.Extensions.DependencyInjection;
using SE_HW_02.Entities.Models;
using SE_HW_02.Infrastructure.Repositories;
using SE_HW_02.UseCases.Enclosures;

namespace SE_HW_02.Tests.UseCases.Enclosures
{
    public class EnclosureServiceTests
    {
        private static IServiceProvider GetServiceProvider()
        {
            ServiceCollection services = new();
            services.AddSingleton<IEnclosureRepository, EnclosureRepository>();
            return services.BuildServiceProvider();
        }

        [Fact]
        public void Add_ThrowException()
        {
            EnclosureService service = new(GetServiceProvider());
            Assert.Throws<ArgumentNullException>(() => service.Add(null!));
        }

        [Fact]
        public void Add_ReturnNull()
        {
            EnclosureService service = new(GetServiceProvider());

            Assert.Null(service.Add(new Enclosure() { AnimalsCapacity = 0 }));
            Assert.Null(service.Add(new Enclosure() { AnimalsCapacity = 1, AnimalsAmount = 2 }));
            Assert.Null(service.Add(new Enclosure() { AnimalsCapacity = 1, Size = new Dimensions() { Width = 0, Length = 1, Height = 1 } }));
            Assert.Null(service.Add(new Enclosure() { AnimalsCapacity = 1, Size = new Dimensions() { Width = 1, Length = 0, Height = 1 } }));
            Assert.Null(service.Add(new Enclosure() { AnimalsCapacity = 1, Size = new Dimensions() { Width = 1, Length = 1, Height = 0 } }));
        }

        [Fact]
        public void Add_ReturnID()
        {
            EnclosureService service = new(GetServiceProvider());

            Enclosure item = new Enclosure()
            {
                Type = "type",
                Size = new Dimensions()
                {
                    Width = 10,
                    Length = 20,
                    Height = 5
                },
                AnimalsAmount = 5,
                AnimalsCapacity = 10
            };

            Assert.NotNull(service.Add(item));
        }

        [Fact]
        public void Get_Test()
        {
            EnclosureService service = new(GetServiceProvider());

            Assert.Null(service.Get(1));

            Enclosure item = new Enclosure()
            {
                Type = "type",
                Size = new Dimensions()
                {
                    Width = 10,
                    Length = 20,
                    Height = 5
                },
                AnimalsAmount = 5,
                AnimalsCapacity = 10
            };

            int? id = service.Add(item);
            Assert.NotNull(id);
            Assert.NotNull(service.Get(id.Value));
        }

        [Fact]
        public void GetAll_Test()
        {
            EnclosureService service = new(GetServiceProvider());

            Assert.Empty(service.GetAll());

            Enclosure item = new Enclosure()
            {
                Type = "type",
                Size = new Dimensions()
                {
                    Width = 10,
                    Length = 20,
                    Height = 5
                },
                AnimalsAmount = 5,
                AnimalsCapacity = 10
            };

            service.Add(item);
            service.Add(item);
            service.Add(item);
            service.Add(item);
            
            Assert.Equal(4, service.GetAll().Count());
        }

        [Fact]
        public void Remove_Test()
        {
            EnclosureService service = new(GetServiceProvider());

            Assert.False(service.Remove(1));

            Enclosure item = new Enclosure()
            {
                Type = "type",
                Size = new Dimensions()
                {
                    Width = 10,
                    Length = 20,
                    Height = 5
                },
                AnimalsAmount = 0,
                AnimalsCapacity = 10
            };

            int? id = service.Add(item);
            Assert.NotNull(id);
            Assert.True(service.Remove(id.Value));

            item.AnimalsAmount = 1;
            id = service.Add(item);
            Assert.NotNull(id);
            Assert.False(service.Remove(id.Value));
            Assert.Single(service.GetAll());
        }


        [Fact]
        public void Update_ThrowException()
        {
            EnclosureService service = new(GetServiceProvider());
            Assert.Throws<ArgumentNullException>(() => service.Update(1, null!));
        }

        [Fact]
        public void Update_ReturnFalse()
        {
            EnclosureService service = new(GetServiceProvider());

            Assert.False(service.Update(1, new Enclosure()));

            Enclosure item = new Enclosure()
            {
                Type = "type",
                Size = new Dimensions()
                {
                    Width = 10,
                    Length = 20,
                    Height = 5
                },
                AnimalsAmount = 0,
                AnimalsCapacity = 10
            };

            int? id = service.Add(item);
            Assert.NotNull(id);

            Assert.False(service.Update(id.Value, new Enclosure() { AnimalsCapacity = 0 }));
            Assert.False(service.Update(id.Value, new Enclosure() { AnimalsCapacity = 1, AnimalsAmount = 2 }));
            Assert.False(service.Update(id.Value, new Enclosure() { AnimalsCapacity = 1, Size = new Dimensions() { Width = 0, Length = 1, Height = 1 } }));
            Assert.False(service.Update(id.Value, new Enclosure() { AnimalsCapacity = 1, Size = new Dimensions() { Width = 1, Length = 0, Height = 1 } }));
            Assert.False(service.Update(id.Value, new Enclosure() { AnimalsCapacity = 1, Size = new Dimensions() { Width = 1, Length = 1, Height = 0 } }));
            Assert.False(service.Update(id.Value + 1, item));
        }

        [Fact]
        public void Update_ReturnTrue()
        {
            EnclosureService service = new(GetServiceProvider());

            Enclosure item = new Enclosure()
            {
                Type = "type",
                Size = new Dimensions()
                {
                    Width = 10,
                    Length = 20,
                    Height = 5
                },
                AnimalsAmount = 0,
                AnimalsCapacity = 10
            };

            int? id = service.Add(item);
            Assert.NotNull(id);
            Assert.True(service.Update(id.Value, item));
        }
    }
}
