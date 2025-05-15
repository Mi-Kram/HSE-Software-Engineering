using Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Api.Controllers
{
    [Route("api/analyse")]
    [ApiController]
    public class AnalyseController(IAnalyseService analyseService) : ControllerBase
    {
        private readonly IAnalyseService analyseService = analyseService ?? throw new ArgumentNullException(nameof(analyseService));

        // api/analyse?id=1&id=2
        [HttpGet]
        public async Task GetAsync([FromQuery(Name = "id")] int[] _)
        {
            await analyseService.GetAnalyseReportsAsync(HttpContext);
        }

        // GET: /api/analyse/wordcloud/5?regenerate=false
        [HttpGet("wordcloud/{workID}")]
        public async Task GetWordsCloudAsync(int workID, [FromQuery(Name = "regenerate")] bool _)
        {
            await analyseService.GetWordsCloudAsync(HttpContext, workID);
        }
    }
}
