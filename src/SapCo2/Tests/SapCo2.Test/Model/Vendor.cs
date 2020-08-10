using SapCo2.Attributes;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Test.Model
{
    [RfcTable("LFA1",Description = "Vendor Table",Unsafe = false)]
    public class Vendor
    {

        [RfcTableProperty("LIFNR",EntityPropertySapType = RfcEntityPropertySapTypes.CHAR)]
        public string VendorCode { get; set; }

        [RfcTableProperty("KUNNR", EntityPropertySapType = RfcEntityPropertySapTypes.CHAR)]
        public string CustomerCode { get; set; }

        [RfcTableProperty("NAME1", EntityPropertySapType = RfcEntityPropertySapTypes.CHAR)]
        public string Name1 { get; set; }

        [RfcTableProperty("NAME2", EntityPropertySapType = RfcEntityPropertySapTypes.CHAR)]
        public string Name2 { get; set; }
    }
}
