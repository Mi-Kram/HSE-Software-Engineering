using Microsoft.AspNetCore.Mvc;
using SE_HW_02.Entities.Models;
using SE_HW_02.UseCases.Enclosures;
using SE_HW_02.Web.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SE_HW_02.Web.Controllers
{
    [Route("api/enclosures")]
    [ApiController]
    public class EnclosuresController : ControllerBase
    {
        private IEnclosureService enclosureService;

        public EnclosuresController(IEnclosureService enclosureService)
        {
            this.enclosureService = enclosureService;
        }

        // GET: api/enclosures
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(enclosureService.GetAll());
        }

        // GET api/enclosures/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Enclosure? enclosure = enclosureService.Get(id);
            if (enclosure == null) return BadRequest();
            return new JsonResult(enclosure);
        }

        // POST api/enclosures
        [HttpPost]
        public IActionResult Post([FromBody] EnclosureDTO value)
        {
            if (value == null) return BadRequest();

            Enclosure enclosure = new Enclosure
            {
                Type = value.Type,
                Size = value.Size,
                AnimalsAmount = 0,
                AnimalsCapacity = value.AnimalsCapacity
            };

            int? id = enclosureService.Add(enclosure);
            if (id == null) return BadRequest();

            return new JsonResult(new
            {
                id
            });
        }

        // POST api/enclosures/5/clean
        [HttpPost("{id}/clean")]
        public IActionResult Post(int id)
        {
            Enclosure? enclosure = enclosureService.Get(id);
            if (enclosure == null) return BadRequest();

            // Clean service, etc.

            return NoContent();
        }

        // PUT api/enclosures/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] EnclosureDTO value)
        {
            if (value == null) return BadRequest();

            Enclosure? enclosure = enclosureService.Get(id);
            if (enclosure == null) return BadRequest();

            enclosure.Type = value.Type;
            enclosure.Size = value.Size;
            enclosure.AnimalsCapacity = value.AnimalsCapacity;

            return enclosureService.Update(id, enclosure) ? NoContent() : BadRequest();
        }

        // DELETE api/enclosures/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return enclosureService.Remove(id) ? NoContent() : BadRequest();
        }
    }
}
