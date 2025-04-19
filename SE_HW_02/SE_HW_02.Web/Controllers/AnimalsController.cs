using Microsoft.AspNetCore.Mvc;
using SE_HW_02.Entities.Models;
using SE_HW_02.UseCases.Animals;
using SE_HW_02.UseCases.Feeding;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SE_HW_02.Web.Controllers
{
    [Route("api/animals")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private IAnimalService animalService;
        private IFeedingMasterService feedingService;

        public AnimalsController(IAnimalService animalService, IFeedingMasterService feedingService)
        {
            this.animalService = animalService;
            this.feedingService = feedingService;
        }

        // GET api/animals
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(animalService.GetAll());
        }

        // GET api/animals/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Animal? animal = animalService.Get(id);
            if (animal == null) return BadRequest();
            return new JsonResult(animal);
        }

        // POST api/animals
        [HttpPost]
        public IActionResult Post([FromBody] Animal animal)
        {
            if (animal == null) return BadRequest();

            int? id = animalService.Add(animal);
            if (id == null) return BadRequest();

            return new JsonResult(new
            {
                id
            });
        }

        // POST api/animals/5/treat
        [HttpPost("{id}/transfer")]
        public IActionResult Post(int id, [FromQuery]int? enclosureID)
        {
            if (enclosureID == null) return BadRequest();

            Animal? animal = animalService.Get(id);
            if (animal == null) return BadRequest();

            if (animal.EnclosureID == enclosureID.Value) return NoContent();

            animal.EnclosureID = enclosureID.Value;
            return animalService.Update(id, animal) ? NoContent() : BadRequest();
        }

        // POST api/animals/5/treat
        [HttpPost("{id}/treat")]
        public IActionResult Post(int id)
        {
            Animal? animal = animalService.Get(id);
            if (animal == null) return BadRequest();

            if (animal.IsHealthy) return NoContent();

            animal.IsHealthy = true;
            return animalService.Update(id, animal) ? NoContent() : BadRequest();
        }

        // POST api/animals/5/feed
        [HttpPost("{id}/feed")]
        public IActionResult Post(int id, [FromQuery] string food)
        {
            if (string.IsNullOrWhiteSpace(food)) return BadRequest();
            return feedingService.Feed(id, food) ? NoContent() : BadRequest();
        }

        // PUT api/animals/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Animal animal)
        {
            if (animal == null) return BadRequest();
            return animalService.Update(id, animal) ? NoContent() : BadRequest();
        }

        // DELETE api/animals/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return animalService.Remove(id) ? NoContent() : BadRequest();
        }
    }
}
