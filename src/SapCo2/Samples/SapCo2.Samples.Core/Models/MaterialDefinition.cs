using SapCo2.Abstraction;
using SapCo2.Abstraction.Attributes;
using SapCo2.Abstraction.Enumerations;

namespace SapCo2.Samples.Core.Models
{
    [RfcEntity("MAKT", Description = "Material Definition Table")]
    public class MaterialDefinition : ISapTable
    {
        [RfcEntityProperty("MATNR", Description = "Material Code", SapDataType = RfcDataTypes.CHAR, Length = 18)]
        public string Code { get; set; }

        [RfcEntityProperty("MAKTX", Description = "Material Short Definition", SapDataType = RfcDataTypes.CHAR, Length = 40)]
        public string Definition { get; set; }
    }
}
