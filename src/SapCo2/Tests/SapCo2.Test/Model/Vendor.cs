using SapCo2.Wrapper.Attributes;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Test.Model
{
    [RfcEntityClass("LFA1",Description = "Vendor Table",Unsafe = false)]
    public class Vendor
    {

        [RfcEntityProperty("LIFNR",EntityPropertySapType = RfcEntityPropertySapTypes.CHAR)]
        public string VendorCode { get; set; }

        [RfcEntityProperty("KUNNR", EntityPropertySapType = RfcEntityPropertySapTypes.CHAR)]
        public string CustomerCode { get; set; }

        [RfcEntityProperty("NAME1", EntityPropertySapType = RfcEntityPropertySapTypes.CHAR)]
        public string Name1 { get; set; }

        [RfcEntityProperty("NAME2", EntityPropertySapType = RfcEntityPropertySapTypes.CHAR)]
        public string Name2 { get; set; }
    }
}
