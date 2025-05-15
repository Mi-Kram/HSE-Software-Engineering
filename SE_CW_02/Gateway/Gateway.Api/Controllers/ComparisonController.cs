using Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Api.Controllers
{
    [Route("api/compare")]
    [ApiController]
    public class ComparisonController(IAnalyseService analyseService) : ControllerBase
    {
        private readonly IAnalyseService analyseService = analyseService ?? throw new ArgumentNullException(nameof(analyseService));

        // Сравнение нескольких работ.
        //  id1  id2
        // { 1    2 }
        // { 1    3 }       сравнение 1  сравнение 2  сравнение 3
        // { 2    3 }       ┌─────┴─────┬─────┴─────┬─────┴─────┐
        // Get: /api/compare?id1=1&id2=2&id1=1&id2=3&id1=2&id2=3
        [HttpGet]
        public async Task GetAsync(
            [FromQuery(Name = "id1")] int[] _1,
            [FromQuery(Name = "id2")] int[] _2)
        {
            await analyseService.GetCoupleComparisonReportsAsync(HttpContext);
        }

        // Сравнение всех работ.
        // Get: /api/compare/all?id=1&id=2&id=3
        [HttpGet("all")]
        public async Task CompareAllAsync([FromQuery(Name = "id")] int[] _)
        {
            await analyseService.GetAllComparisonReportsAsync(HttpContext);
        }
    }
}
