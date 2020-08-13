using SapCo2.Attributes;
using SapCo2.Enumeration;

namespace SapCo2.Samples.NetCore.TableExamples.Models
{
    [RfcTable("MAKT", Description = "Material Definition Table")]
    public class MaterialDefinition
    {
        [RfcTableProperty("MATNR", Description = "Material Code", TablePropertySapType = RfcTablePropertySapTypes.CHAR, Length = 18)]
        public string Code { get; set; }

        [RfcTableProperty("MAKTX", Description = "Material Short Definition", TablePropertySapType = RfcTablePropertySapTypes.CHAR, Length = 40)]
        public string Definition { get; set; }
    }
}
