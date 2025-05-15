using FileAnalysisService.Domain.Interfaces;
using FileAnalysisService.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FileAnalysisService.Api.Controllers
{
    [Route("api/compare")]
    [ApiController]
    public class ComparisonController(IComparisonService comparisonService) : ControllerBase
    {
        private readonly IComparisonService comparisonService = comparisonService ?? throw new ArgumentNullException(nameof(comparisonService));

        // Сравнение нескольких работ.
        //  id1  id2
        // { 1    2 }
        // { 1    3 }       сравнение 1  сравнение 2  сравнение 3
        // { 2    3 }       ┌─────┴─────┬─────┴─────┬─────┴─────┐
        // Get: /api/compare?id1=1&id2=2&id1=1&id2=3&id1=2&id2=3
        [HttpGet]
        public async Task<IActionResult> CompareAsync(
            [FromQuery(Name = "id1")] int[] works1ID,
            [FromQuery(Name = "id2")] int[] works2ID)
        {
            // Проверка входных параметров.
            if (works1ID == null || works2ID == null) return BadRequest();
            if (works1ID.Length != works2ID.Length) throw new ArgumentException($"Массивы должны быть одинаковой длины");
            if (works1ID.Length == 0) return BadRequest();

            // Преобразование входных данных.
            IEnumerable<ComparisonKey> keys = works1ID
                .Select((x, i) => new ComparisonKey(x, works2ID[i]));

            return new JsonResult(await comparisonService.GetReportsAsync(keys));
        }

        // Сравнение всех работ.
        // Get: /api/compare/all?id=1&id=2&id=3
        [HttpGet("all")]
        public async Task<IActionResult> CompareAllAsync([FromQuery(Name = "id")] int[] worksID)
        {
            // Проверка входных параметров.
            if (worksID == null || worksID.Length == 0) return BadRequest();
            return worksID == null ? BadRequest() : new JsonResult(await comparisonService.GetReportsAsync(worksID));
        }
    }
}
