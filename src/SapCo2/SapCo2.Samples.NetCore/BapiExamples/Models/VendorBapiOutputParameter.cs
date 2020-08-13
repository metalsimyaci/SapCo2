using SapCo2.Abstract;
using SapCo2.Parameters;
using SapCo2.Wrapper.Attributes;

namespace SapCo2.Samples.NetCore.BapiExamples.Models
{
    public class VendorBapiOutputParameter:IRfcBapiOutput
    {
        [RfcEntityProperty("RETURN")]
        public RfcBapiOutputParameter BapiReturn { get; set; }

        [RfcEntityProperty("VENDOR")]
        public VendorModel[] Vendors { get; set; }
    }
}
