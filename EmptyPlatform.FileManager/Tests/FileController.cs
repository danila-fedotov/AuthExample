using EmptyPlatform.FileManager.Api.File;
using EmptyPlatform.FileManager.Db;
using EmptyPlatform.FileManager.Services;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EmptyPlatform.FileManager.Tests
{
    public class FileController
    {
        [Fact]
        public async Task CreateAsync()
        {
            Assert.Equal(1, 2);
            var mockFileService = new Mock<IFileService>();
            var controller = new Api.File.FileController(mockFileService.Object);
            var fileStream = new MemoryStream(Encoding.ASCII.GetBytes("Hello World"));
            var formFile = new FormFile(fileStream, 0, fileStream.Length, "name", "fileName")
            {
                ContentType = "text/html"
            };

            // Act
            var result = await controller.CreateAsync(formFile);

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(viewResult.Value, "");
        }
    }
}
