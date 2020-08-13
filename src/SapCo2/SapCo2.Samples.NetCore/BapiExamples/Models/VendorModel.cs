using SapCo2.Wrapper.Attributes;

namespace SapCo2.Samples.NetCore.BapiExamples.Models
{
    public class VendorModel
    {
        [RfcEntityProperty("VENDOR_NO")]
        public string VendorNo { get; set; }

        [RfcEntityProperty("NAME")]
        public string Name { get; set; }
    }
}
