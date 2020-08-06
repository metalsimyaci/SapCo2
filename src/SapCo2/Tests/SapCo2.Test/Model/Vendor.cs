using SapCo2.Wrapper.Attributes;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Test.Model
{
    [RfcClass("LFA1",Description = "Vendor Table",Unsafe = false)]
    public class Vendor
    {

        [RfcProperty("LIFNR",DataType = RfcDataTypes.Char)]
        public string VendorCode { get; set; }

        [RfcProperty("KUNNR", DataType = RfcDataTypes.Char)]
        public string CustomerCode { get; set; }

        [RfcProperty("NAME1", DataType = RfcDataTypes.Char)]
        public string Name1 { get; set; }

        [RfcProperty("NAME2", DataType = RfcDataTypes.Char)]
        public string Name2 { get; set; }
    }
}
