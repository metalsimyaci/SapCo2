using SapCo2.Attributes;
using SapCo2.Enumeration;

namespace SapCo2.MaterialManagement.Entity
{
    [RfcTable("MARA", Description = "Material General Table")]
    public class Material
    {
        [RfcTableProperty("MATNR", Description = "Material Code", EntityPropertySapType = RfcEntityPropertySapTypes.CHAR, Length = 18)]
        public string Code { get; set; }

        [RfcTableProperty("MATNR", Description = "Material Definition", EntityPropertySapType = RfcEntityPropertySapTypes.CHAR, Length = 18, SubTypePropertyName = "Code")]
        public virtual MaterialDefinition Definition { get; set; }

        [RfcTableProperty("ZZEXTWG", Description = "Material Category Code", EntityPropertySapType = RfcEntityPropertySapTypes.CHAR, Length = 25, Unsafe = true)]
        public string MaterialCategoryCode { get; set; }

        [RfcTableProperty("ZZEXTWG", Description = "Ürün Sınıfı", EntityPropertySapType = RfcEntityPropertySapTypes.CHAR, Length = 25, SubTypePropertyName = "Code", Unsafe = true)]
        public virtual MaterialCategoryDefinition MaterialCategory { get; set; }
    }
}
