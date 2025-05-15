using FileAnalysisService.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FileAnalysisService.Api.Controllers
{
    [Route("api/works")]
    [ApiController]
    public class WorksController(IWorkService workService) : ControllerBase
    {
        private readonly IWorkService workService = workService ?? throw new ArgumentNullException(nameof(workService));

        // DELETE: /api/works/5
        [HttpDelete("{workID}")]
        public async Task<IActionResult> DeleteAsync(int workID)
        {
            await workService.DeleteAsync(workID);
            return NoContent();
        }
    }
}
