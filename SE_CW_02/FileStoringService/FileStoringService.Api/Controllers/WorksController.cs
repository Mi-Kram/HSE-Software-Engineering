using FileStoringService.Api.DTO;
using FileStoringService.Domain.Entities;
using FileStoringService.Domain.Interfaces;
using FileStoringService.Domain.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FileStoringService.Api.Controllers
{
    [Route("api/works")]
    [ApiController]
    public class WorksController(IWorkService workService) : ControllerBase
    {
        /// <summary>
        /// Сервис работ.
        /// </summary>
        private readonly IWorkService workService = workService;

        // GET: api/works
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return new JsonResult((await workService.GetAllWorksAsync()).Select(x => new {
                id = x.ID,
                title = x.Title,
                userID = x.UserID,
                uploaded = x.Uploaded
            }));
        }

        // GET api/works/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            MemoryStream ms = new();
            WorkInfo work = await workService.DownloadWorkAsync(id, ms);

            // Отправка потока данных работы.
            return File(ms, "text/plain", work.Title);
        }

        // POST api/works
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromForm] UploadWorkDTO workDTO)
        {
            if (workDTO == null) return BadRequest();
            if (workDTO.File == null) throw new ArgumentException("Работа не отправлена");
            if (workDTO.File.ContentType?.ToLower() != "text/plain") throw new ArgumentException("Ожидается текстовый файл");

            using Stream stream = workDTO.File.OpenReadStream();
            
            // Загрузка работы.
            UploadWorkData uploadData = new()
            {
                UserID = workDTO.UserID,
                Title = string.IsNullOrWhiteSpace(workDTO.File.FileName) ? "data.txt" : workDTO.File.FileName.Trim()
            };
            int workID = await workService.UploadWorkAsync(stream, uploadData);
            
            return new JsonResult(new
            {
                workID
            });
        }

        // DELETE api/works/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await workService.DeleteWorkAsync(id);
            return NoContent();
        }
    }
}
