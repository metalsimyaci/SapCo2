using SapCo2.Wrapper.Attributes;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.MaterialManagement.Entity
{
    [RfcClass("MARA", Description = "Material General Table")]
    public class Material
    {
        [RfcProperty("MATNR", Description = "Material Code", DataType = RfcDataTypes.Char, Length = 18)]
        public string Code { get; set; }

        [RfcProperty("MATNR", Description = "Material Definition", DataType = RfcDataTypes.Char, Length = 18, SubTypePropertyName = "Code")]
        public virtual MaterialDefinition Definition { get; set; }

        [RfcProperty("ZZEXTWG", Description = "Material Category Code", DataType = RfcDataTypes.Char, Length = 25, Unsafe = true)]
        public string MaterialCategoryCode { get; set; }

        [RfcProperty("ZZEXTWG", Description = "Ürün Sınıfı", DataType = RfcDataTypes.Char, Length = 25, SubTypePropertyName = "Code", Unsafe = true)]
        public virtual MaterialCategoryDefinition MaterialCategory { get; set; }
    }
}
