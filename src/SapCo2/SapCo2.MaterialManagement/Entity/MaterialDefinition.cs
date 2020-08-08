using SapCo2.Wrapper.Attributes;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.MaterialManagement.Entity
{
    [RfcEntityClass("MAKT", Description = "Material Definition Table")]
    public class MaterialDefinition
    {
        [RfcEntityProperty("MATNR", Description = "Material Code", EntityPropertySapType = RfcEntityPropertySapTypes.CHAR, Length = 18)]
        public string Code { get; set; }

        [RfcEntityProperty("MAKTX", Description = "Material Short Definition", EntityPropertySapType = RfcEntityPropertySapTypes.CHAR, Length = 40)]
        public string Definition { get; set; }
    }
}
