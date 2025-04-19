using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SE_HW_02.Entities.Models;
using SE_HW_02.Infrastructure.Repositories;
using SE_HW_02.UseCases.Animals;
using SE_HW_02.UseCases.Feeding;
using SE_HW_02.Web.Controllers;

namespace SE_HW_02.Tests.Web.Controllers
{
    public class FeedingSchedulesControllerTests
    {
        private static IServiceProvider GetServiceProvider()
        {
            ServiceCollection services = new();
            services.AddSingleton<IAnimalRepository, AnimalRepository>();
            services.AddSingleton<IFeedingScheduleRepository, FeedingScheduleRepository>();
            services.AddSingleton<IFeedingOrganizationService>(FeedingOrganizationService.Initialize);
            return services.BuildServiceProvider();
        }

        [Fact]
        public void Test()
        {
            IServiceProvider provider = GetServiceProvider();
            IAnimalRepository animalRepository = provider.GetRequiredService<IAnimalRepository>();
            IFeedingOrganizationService service = provider.GetRequiredService<IFeedingOrganizationService>();
            FeedingSchedulesController controller = new(service);

            // ########################################### POST
            int? animalID = animalRepository.Add(new Animal());
            Assert.NotNull(animalID);

            IActionResult result = controller.Post(new FeedingSchedule() { AnimalID = animalID.Value });
            Assert.NotNull(result);

            Assert.NotNull(controller.Post(null!));
            Assert.NotNull(controller.Post(new FeedingSchedule()));

            // ########################################### GET
            Assert.NotNull(controller.Get());
            Assert.NotNull(controller.Get(0));
            Assert.NotNull(controller.Get(1));

            // ########################################### PUT
            Assert.NotNull(controller.Put(0, null!));
            Assert.NotNull(controller.Put(0, new FeedingSchedule() { AnimalID = animalID.Value }));
            Assert.NotNull(controller.Put(1, new FeedingSchedule() { AnimalID = animalID.Value }));

            // ########################################### DELETE
            Assert.NotNull(controller.Delete(0));
            Assert.NotNull(controller.Delete(1));
        }
    }
}
