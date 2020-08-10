using SapCo2.Wrapper.Attributes;

namespace SapCo2.Test.Model
{
    public class Vendor
    {

        [RfcEntityProperty("LIFNR")]
        public string VendorCode { get; set; }

        [RfcEntityProperty("KUNNR")]
        public string CustomerCode { get; set; }

        [RfcEntityProperty("NAME1")]
        public string Name1 { get; set; }

        [RfcEntityProperty("NAME2")]
        public string Name2 { get; set; }
    }
}
