using Gateway.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gateway.Api.Controllers
{
    [Route("api/works")]
    [ApiController]
    public class WorksController(IStorageService storageService, IWorkDeletionService workDeletionService) : ControllerBase
    {
        private readonly IStorageService storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        private readonly IWorkDeletionService workDeletionService = workDeletionService ?? throw new ArgumentNullException(nameof(workDeletionService));
        
        // GET: api/works
        [HttpGet]
        public async Task GetAsync()
        {
            await storageService.GetAllWorksAsync(HttpContext);
        }

        // GET api/works/5
        [HttpGet("{workID}")]
        public async Task GetAsync(int workID)
        {
            await storageService.GetWorkAsync(HttpContext, workID);
        }

        // POST api/works
        [HttpPost]
        public async Task PostAsync()
        {
            await storageService.UploadWorkAsync(HttpContext);
        }

        // DELETE api/works/5
        [HttpDelete("{workID}")]
        public async Task DeleteAsync(int workID)
        {
            await workDeletionService.DeleteWorkAsync(HttpContext, workID);
        }
    }
}
