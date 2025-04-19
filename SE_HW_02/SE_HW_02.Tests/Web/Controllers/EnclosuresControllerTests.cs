using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SE_HW_02.Entities.Models;
using SE_HW_02.Infrastructure.Repositories;
using SE_HW_02.UseCases.AnimalEnclosure;
using SE_HW_02.UseCases.Animals;
using SE_HW_02.UseCases.Enclosures;
using SE_HW_02.UseCases.Feeding;
using SE_HW_02.Web.Controllers;
using SE_HW_02.Web.DTO;

namespace SE_HW_02.Tests.Web.Controllers
{
    public class EnclosuresControllerTests
    {
        private static IServiceProvider GetServiceProvider()
        {
            ServiceCollection services = new();
            services.AddSingleton<IEnclosureRepository, EnclosureRepository>();
            services.AddSingleton<IEnclosureService, EnclosureService>();
            return services.BuildServiceProvider();
        }

        [Fact]
        public void Test()
        {
            IServiceProvider provider = GetServiceProvider();
            IEnclosureService enclosureService = provider.GetRequiredService<IEnclosureService>();
            EnclosuresController controller = new(enclosureService);

            // ########################################### POST
            IActionResult result = controller.Post(new EnclosureDTO() { AnimalsCapacity = 10 });
            Assert.NotNull(result);

            Assert.NotNull(controller.Post(null!));
            Assert.NotNull(controller.Post(new EnclosureDTO()));

            // ########################################### POST /clean
            Assert.NotNull(controller.Post(0));
            Assert.NotNull(controller.Post(1));

            // ########################################### GET
            Assert.NotNull(controller.Get());
            Assert.NotNull(controller.Get(0));
            Assert.NotNull(controller.Get(1));

            // ########################################### PUT
            Assert.NotNull(controller.Put(0, null!));
            Assert.NotNull(controller.Put(0, new EnclosureDTO() { AnimalsCapacity = 10 }));
            Assert.NotNull(controller.Put(1, new EnclosureDTO() { AnimalsCapacity = 10 }));

            // ########################################### DELETE
            Assert.NotNull(controller.Delete(0));
            Assert.NotNull(controller.Delete(1));
        }
    }
}
