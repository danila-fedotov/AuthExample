using EmptyPlatform.FileManager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EmptyPlatform.FileManager.Api.File
{
    [ApiController]
    [Route("api/[controller]")]
    [Display(Description = "File management")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        [Display(Description = "A file upload")]
        public async Task<IActionResult> CreateAsync(IFormFile request)
        {
            using var fileStream = request.OpenReadStream();

            var fileId = await _fileService.CreateAsync(request.FileName,
                request.ContentType,
                request.Length,
                fileStream);

            return Ok(fileId);
        }

        [HttpGet]
        [Display(Description = "Downloading a file")]
        public IActionResult Get([Required] string fileId)
        {
            var file = _fileService.Get(fileId);

            using var fileStream = _fileService.Read(file);

            return File(fileStream, file.ContentType, file.FileName);
        }

        [HttpDelete]
        [Display(Description = "Deleting a file")]
        public IActionResult Remove([Required] string fileId)
        {
            _fileService.Remove(fileId);

            return Ok();
        }
    }
}
