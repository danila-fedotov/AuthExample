using EmptyPlatform.FileManager.Api.File;
using EmptyPlatform.FileManager.Db;
using EmptyPlatform.FileManager.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace FileManager.Tests
{
    public class FileController_Test
    {
        [Fact]
        public async Task CreateAsync_Test()
        {
            EmptyPlatform.FileManager.Db.File file = null;
            var mockDbRepository = new Mock<IDbRepository>();

            mockDbRepository.Setup(x => x.CreateFile(null)).Callback<EmptyPlatform.FileManager.Db.File>(x => file = x);

            var mockFileStorage = new Mock<IFileStorage>();

            mockFileStorage.Setup(x => x.WriteAsync(string.Empty, null))
                .Returns((string fileName, Stream fileStream) =>
                {
                    return Task.Factory.StartNew(() => fileStream);
                });

            var mockFileService = new Mock<IFileService>(mockDbRepository.Object, mockFileStorage.Object);
            var controller = new FileController(mockFileService.Object);
            var formFile = new TxtFormFile();
            var result = await controller.CreateAsync(formFile);
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(okResult?.Value, "");
        }
    }
}
