using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileManager.Tests
{
    internal class TxtFormFile : IFormFile
    {
        private string _content => "Hello World";

        public string ContentType => "text/html";

        public string ContentDisposition => throw new NotImplementedException();

        public IHeaderDictionary Headers => throw new NotImplementedException();

        public long Length => _content.Length;

        public string Name => throw new NotImplementedException();

        public string FileName => "test.txt";

        public void CopyTo(Stream target)
        {
            throw new NotImplementedException();
        }

        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Stream OpenReadStream() => new MemoryStream(Encoding.ASCII.GetBytes(_content));
    }
}
