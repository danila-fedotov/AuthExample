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
        public async Task<IActionResult> UploadAsync(IFormFile request)
        {
            if (request is null || request.Length == 0)
            {
                return NoContent();
            }

            using var fileStream = request.OpenReadStream();

            var fileId = await _fileService.CreateAsync(request.FileName,
                request.ContentType,
                request.Length,
                fileStream);

            return Ok(fileId);
        }

        [HttpGet]
        [Display(Description = "Downloading a file")]
        public IActionResult Download([Required] string fileId)
        {
            var file = _fileService.GetOrDefault(fileId);

            if (file is null)
            {
                return NotFound();
            }

            var fileStream = _fileService.OpenRead(file);

            return File(fileStream, file.ContentType, file.FileName);
        }

        [HttpDelete]
        [Display(Description = "Deleting a file")]
        public IActionResult Remove([Required] string fileId)
        {
            var file = _fileService.GetOrDefault(fileId);

            if (file is null)
            {
                return NotFound();
            }

            _fileService.Remove(file);

            return Ok();
        }
    }
}
