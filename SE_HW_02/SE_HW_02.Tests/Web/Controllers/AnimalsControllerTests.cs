using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SE_HW_02.Entities.Models;
using SE_HW_02.Infrastructure.Repositories;
using SE_HW_02.UseCases.AnimalEnclosure;
using SE_HW_02.UseCases.Animals;
using SE_HW_02.UseCases.Enclosures;
using SE_HW_02.UseCases.Feeding;
using SE_HW_02.Web.Controllers;

namespace SE_HW_02.Tests.Web.Controllers
{
    [Collection("Console")]
    public class AnimalsControllerTests
    {
        private static IServiceProvider GetServiceProvider()
        {
            ServiceCollection services = new();
            services.AddSingleton<IAnimalRepository, AnimalRepository>();
            services.AddSingleton<IEnclosureRepository, EnclosureRepository>();
            services.AddSingleton<IFeedingScheduleRepository, FeedingScheduleRepository>();

            services.AddSingleton<IAnimalTransferService, AnimalTransferService>();
            services.AddSingleton<IFeedingOrganizationService>(FeedingOrganizationService.Initialize);

            services.AddSingleton<IAnimalService, AnimalService>();
            services.AddSingleton<IFeedingMasterService, FeedingMasterService>();

            return services.BuildServiceProvider();
        }

        [Fact]
        public void Test()
        {
            IServiceProvider provider = GetServiceProvider();
            IEnclosureRepository enclosureRepository = provider.GetRequiredService<IEnclosureRepository>();
            IAnimalService animalService = provider.GetRequiredService<IAnimalService>();
            IFeedingMasterService feedingService = provider.GetRequiredService<IFeedingMasterService>();

            AnimalsController controller = new(animalService, feedingService);

            // ########################################### POST
            int? enclosureID = enclosureRepository.Add(new Enclosure() { AnimalsCapacity = 10 });
            Assert.NotNull(enclosureID);
            int? enclosure2ID = enclosureRepository.Add(new Enclosure() { AnimalsCapacity = 10 });
            Assert.NotNull(enclosure2ID);

            IActionResult result = controller.Post(new Animal() { EnclosureID = enclosureID.Value });
            Assert.NotNull(result);

            Assert.NotNull(controller.Post(null!));
            Assert.NotNull(controller.Post(new Animal()));

            // ########################################### POST: /treat
            Assert.NotNull(controller.Post(0));
            Assert.NotNull(controller.Post(1));


            // ########################################### GET
            Assert.NotNull(controller.Get());
            Assert.NotNull(controller.Get(0));
            Assert.NotNull(controller.Get(1));

            // ########################################### PUT
            Assert.NotNull(controller.Put(0, null!));
            Assert.NotNull(controller.Put(0, new Animal() { EnclosureID = enclosureID.Value }));
            Assert.NotNull(controller.Put(1, new Animal() { EnclosureID = enclosureID.Value }));

            // ########################################### POST: /transfer
            Assert.NotNull(controller.Post(0, enclosure2ID.Value));
            Assert.NotNull(controller.Post(1, -1));
            Assert.NotNull(controller.Post(1, enclosure2ID.Value));

            // ########################################### DELETE
            Assert.NotNull(controller.Delete(0));
            Assert.NotNull(controller.Delete(1));
        }

        [Fact]
        public void Feed_Test()
        {
            IServiceProvider provider = GetServiceProvider();
            IAnimalRepository animalRepository = provider.GetRequiredService<IAnimalRepository>();
            IAnimalService animalService = provider.GetRequiredService<IAnimalService>();
            IFeedingMasterService feedingService = provider.GetRequiredService<IFeedingMasterService>();

            int? animalID = animalRepository.Add(new Animal() { Name = "animal" });
            Assert.NotNull(animalID);

            AnimalsController controller = new(animalService, feedingService);

            TextWriter consoleOut = Console.Out;
            StringWriter sw = new();

            try
            {
                Console.SetOut(sw);
                Assert.NotNull(controller.Post(animalID.Value, "food"));
            }
            finally
            {
                Console.SetOut(consoleOut);
            }

            StringWriter expected = new();
            expected.WriteLine($"Кормление животного animal (id={animalID}). Пища: food");
            Assert.Equal(expected.ToString().Trim(), sw.ToString().Trim());
        }
    }
}
