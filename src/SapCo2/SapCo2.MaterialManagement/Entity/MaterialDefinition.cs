using SapCo2.Wrapper.Attributes;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.MaterialManagement.Entity
{
    [RfcClass("MAKT", Description = "Material Definition Table")]
    public class MaterialDefinition
    {
        [RfcProperty("MATNR", Description = "Material Code", DataType = RfcDataTypes.Char, Length = 18)]
        public string Code { get; set; }

        [RfcProperty("MAKTX", Description = "Material Short Definition", DataType = RfcDataTypes.Char, Length = 40)]
        public string Definition { get; set; }
    }
}
