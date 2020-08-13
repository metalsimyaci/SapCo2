using SapCo2.Wrapper.Attributes;

namespace SapCo2.Samples.NetCore.BapiExamples.Models
{
    public class VendorBapiInputParameter
    {
        [RfcEntityProperty("COMP_CODE")]
        public string CompanyCode { get; set; }
    }
}
