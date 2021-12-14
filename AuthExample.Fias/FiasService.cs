using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthExample.Fias
{
    internal class FiasService : IFiasService
    {
        private readonly ILogger<FiasService> _logger;

        protected virtual string _url => "http://fias.nalog.ru/WebServices/Public/GetAllDownloadFileInfo";

        public FiasService(ILogger<FiasService> logger)
        {
            _logger = logger;
        }

        public virtual async Task<(List<FiasPackage> packages, string errorMessage)> GetPackagesAsync()
        {
            Stopwatch sw = Stopwatch.StartNew();

            try
            {
                using HttpClient client = new(null);
                HttpResponseMessage httpResponce = await client.GetAsync(_url, HttpCompletionOption.ResponseContentRead);

                httpResponce.EnsureSuccessStatusCode();

                string content = await httpResponce.Content.ReadAsStringAsync();
                List<FiasPackage> packages = JsonConvert.DeserializeObject<List<FiasPackage>>(content);

                return (packages, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "User {User} requested fias packages from {_url}");

                return (null, "An error occurred when contacting FIAS");
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation("User {User} requested fias packages from {_url}");
            }
        }
    }
}
