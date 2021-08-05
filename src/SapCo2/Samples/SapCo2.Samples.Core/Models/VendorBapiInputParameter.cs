using SapCo2.Abstraction;
using SapCo2.Abstraction.Attributes;

namespace SapCo2.Samples.Core.Models
{
    public class VendorBapiInputParameter : IBapiInput
    {
        [RfcEntityProperty("COMP_CODE")]
        public string CompanyCode { get; set; }
    }
}
