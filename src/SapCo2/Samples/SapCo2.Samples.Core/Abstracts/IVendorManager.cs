using System.Threading.Tasks;
using SapCo2.Samples.Core.Models;

namespace SapCo2.Samples.Core.Abstracts
{
    public interface IVendorManager: IPrintable<VendorBapiOutputParameter>
    {
        Task<VendorBapiOutputParameter> GetVendorsByCompanyCodeAsync(string companyCode);
    }
}
