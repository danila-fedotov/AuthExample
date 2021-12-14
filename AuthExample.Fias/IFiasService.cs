using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthExample.Fias
{
    public interface IFiasService
    {
        [Authorize(Policy = "FIAS_GET_PACKAGES")]

        Task<(List<FiasPackage> packages, string errorMessage)> GetPackagesAsync();
    }
}
