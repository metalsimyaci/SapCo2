using SapCo2.Abstraction;
using SapCo2.Abstraction.Attributes;
using SapCo2.Abstraction.Model;

namespace SapCo2.Samples.Core.Models
{
    public class VendorBapiOutputParameter : IBapiOutput
    {
        [RfcEntityProperty("RETURN")]
        public BapiReturnParameter BapiReturn { get; set; }

        [RfcEntityProperty("VENDOR")]
        public Vendor[] Vendors { get; set; }
    }
}
