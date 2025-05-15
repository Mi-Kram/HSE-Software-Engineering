using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FileAnalysisService.Api.Controllers
{
    [Route("api/analyse")]
    [ApiController]
    public class AnalysisController(IAnalyseService analyseService) : ControllerBase
    {
        private readonly IAnalyseService analyseService = analyseService ?? throw new ArgumentNullException(nameof(analyseService));

        // GET: /api/analyse?id=1&id=2
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery(Name = "id")] int[] worksID)
        {
            if (worksID == null || worksID.Length == 0) return BadRequest();

            List<AnalyzeReport> reports = new(worksID.Length);

            // Для каждого уникального id получить отчёт.
            foreach (int workID in worksID.Distinct())
            {
                AnalyzeReport? report = await analyseService.GetReportAsync(workID);
                if (report == null) continue;
                reports.Add(report);
            }

            return new JsonResult(reports);
        }

        // GET: /api/analyse/wordcloud/5?regenerate=false
        [HttpGet("wordcloud/{workID}")]
        public async Task<IActionResult> GetWordsCloudAsync(int workID, [FromQuery] bool regenerate = false)
        {
            MemoryStream ms = new();
            string contentType = await analyseService.GetWordsCloudAsync(workID, ms, regenerate);

            string ext = contentType.Contains("png", StringComparison.InvariantCultureIgnoreCase)
                ? "jpg"
                : contentType.Contains("jpeg", StringComparison.InvariantCultureIgnoreCase)
                ? "jpg"
                : contentType.Contains("svg", StringComparison.InvariantCultureIgnoreCase) ? "svg" : "png";
            
            return File(ms, contentType, $"cloud_{workID}.{ext}");
        }
    }
}
