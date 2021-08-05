using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SapCo2.Abstract;
using SapCo2.Samples.Core.Abstracts;
using SapCo2.Samples.Core.Models;

namespace SapCo2.Samples.Core.Managers
{
    public class VendorManager:IVendorManager
    {
        private readonly IServiceProvider _serviceProvider;

        public VendorManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<VendorBapiOutputParameter> GetVendorsByCompanyCodeAsync(string companyCode)
        {
            var inputParameter = new VendorBapiInputParameter
            {
                CompanyCode = companyCode
            };

            using IRfcClient sapClient = _serviceProvider.GetRequiredService<IRfcClient>();
            return await sapClient.ExecuteBapiAsync<VendorBapiInputParameter, VendorBapiOutputParameter>("BBP_VENDOR_GETLIST", inputParameter);
        }

        public void Print(VendorBapiOutputParameter model)
        {
            if (model == null)
            {
                Console.WriteLine("\n Vendor Not Found!");
                return;
            }

            Console.WriteLine($"\n ======= Vendor List ================");
            Console.WriteLine($"= TotalRecordCount={model.Vendors.Count()}. Printed top 10 record =");
            Console.WriteLine("".PadLeft(20, '='));

            foreach (Vendor vendorItem in model.Vendors.Take(10))
                Console.WriteLine($"{vendorItem.VendorNo} - {vendorItem.Name}");
        }
    }
}
