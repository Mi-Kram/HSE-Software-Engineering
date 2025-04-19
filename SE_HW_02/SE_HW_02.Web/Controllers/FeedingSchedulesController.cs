using Microsoft.AspNetCore.Mvc;
using SE_HW_02.Entities.Models;
using SE_HW_02.UseCases.Feeding;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SE_HW_02.Web.Controllers
{
    [Route("api/feeding")]
    [ApiController]
    public class FeedingSchedulesController : ControllerBase
    {
        private IFeedingOrganizationService feedingService;

        public FeedingSchedulesController(IFeedingOrganizationService feedingService)
        {
            this.feedingService = feedingService;
        }

        // GET: api/feeding
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(feedingService.GetAll());
        }

        // GET api/feeding/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            FeedingSchedule? schedule = feedingService.Get(id);
            if (schedule == null) return BadRequest();
            return new JsonResult(schedule);
        }

        // POST api/feeding
        [HttpPost]
        public IActionResult Post([FromBody]FeedingSchedule value)
        {
            if (value == null) return BadRequest();

            int? id = feedingService.Add(value);
            if (id == null) return BadRequest();

            return new JsonResult(new
            {
                id
            });
        }

        // PUT api/feeding/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]FeedingSchedule value)
        {
            if (value == null) return BadRequest();

            return feedingService.Update(id, value) ? NoContent() : BadRequest();
        }

        // DELETE api/feeding/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return feedingService.Remove(id) ? NoContent() : BadRequest();
        }


    }
}
