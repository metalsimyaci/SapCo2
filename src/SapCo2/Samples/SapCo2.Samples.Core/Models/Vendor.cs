using SapCo2.Abstraction.Attributes;

namespace SapCo2.Samples.Core.Models
{
    public class Vendor
    {
        [RfcEntityProperty("VENDOR_NO")]
        public string VendorNo { get; set; }

        [RfcEntityProperty("NAME")]
        public string Name { get; set; }
    }
}
