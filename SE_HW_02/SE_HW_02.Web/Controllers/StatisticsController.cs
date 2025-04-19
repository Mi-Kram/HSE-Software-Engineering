using Microsoft.AspNetCore.Mvc;
using SE_HW_02.Entities.Models.Statistics;
using SE_HW_02.UseCases.Statistics;

namespace SE_HW_02.Web.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private IZooStatisticsService statisticsService;

        public StatisticsController(IZooStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        // GET: api/statistics/animals
        [HttpGet("animals")]
        public IActionResult GetAnimalsStatistics()
        {
            AnimalsStatistics? stat = statisticsService.GetAnimalsStatistics();
            return stat != null ? new JsonResult(stat) : new StatusCodeResult(StatusCodes.Status503ServiceUnavailable);
        }

        // GET: api/statistics/enclosures
        [HttpGet("enclosures")]
        public IActionResult GetEnclosuresStatistics()
        {
            EnclosureStatistics? stat = statisticsService.GetEnclosuresStatistics();
            return stat != null ? new JsonResult(stat) : new StatusCodeResult(StatusCodes.Status503ServiceUnavailable);
        }

        // GET: api/statistics/feeding
        [HttpGet("feeding")]
        public IActionResult GetFeedingStatistics()
        {
            FeedingScheduleStatistics? stat = statisticsService.GetFeedingScheduleStatistics();
            return stat != null ? new JsonResult(stat) : new StatusCodeResult(StatusCodes.Status503ServiceUnavailable);
        }

    }
}
