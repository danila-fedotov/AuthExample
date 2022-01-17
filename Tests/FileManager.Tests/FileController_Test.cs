using EmptyPlatform.FileManager;
using EmptyPlatform.FileManager.Api.File;
using EmptyPlatform.FileManager.Entities;
using EmptyPlatform.FileManager.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FileManager.Tests
{
    public class FileController_Test
    {
        [Theory]
        [InlineData("FileId1", 405)]
        [InlineData("FileId2", 200)]
        public void Download_Test(string fileId, int statusCode)
        {
            //var mockDbRepository = new Mock<IDbRepository>();

            //mockDbRepository.Setup(x => x.FindFile(It.IsAny<string>()))
            //    .Returns(fileId == "FileId2" ? new File() : null);

            //var mockFileStorage = new Mock<IFileStorage>();

            //mockFileStorage.Setup(x => x.OpenRead(It.IsAny<string>()))
            //    .Returns(fileId == "FileId2" ? new System.IO.MemoryStream() : null);

            //var mockFileService = new Mock<FileService>(mockDbRepository.Object, mockFileStorage.Object);
            //var controller = new FileController(mockFileService.Object);
            //var result = controller.Download(fileId);
            //var statusCodeResult = Assert.IsType<StatusCodeResult>(result);

            //Assert.Equal(statusCode, statusCodeResult.StatusCode);
        }
    }
}
