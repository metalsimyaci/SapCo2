using SapCo2.Attributes;
using SapCo2.Enumeration;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.MaterialManagement.Entity
{
    [RfcTable("MAKT", Description = "Material Definition Table")]
    public class MaterialDefinition
    {
        [RfcTableProperty("MATNR", Description = "Material Code", EntityPropertySapType = RfcEntityPropertySapTypes.CHAR, Length = 18)]
        public string Code { get; set; }

        [RfcTableProperty("MAKTX", Description = "Material Short Definition", EntityPropertySapType = RfcEntityPropertySapTypes.CHAR, Length = 40)]
        public string Definition { get; set; }
    }
}
