using SapCo2.Wrapper.Attributes;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.MaterialManagement.Entity
{
    [RfcEntityClass("MARA", Description = "Material General Table")]
    public class Material
    {
        [RfcEntityProperty("MATNR", Description = "Material Code", EntityPropertySapType = RfcEntityPropertySapTypes.CHAR, Length = 18)]
        public string Code { get; set; }

        [RfcEntityProperty("MATNR", Description = "Material Definition", EntityPropertySapType = RfcEntityPropertySapTypes.CHAR, Length = 18, SubTypePropertyName = "Code")]
        public virtual MaterialDefinition Definition { get; set; }

        [RfcEntityProperty("ZZEXTWG", Description = "Material Category Code", EntityPropertySapType = RfcEntityPropertySapTypes.CHAR, Length = 25, Unsafe = true)]
        public string MaterialCategoryCode { get; set; }

        [RfcEntityProperty("ZZEXTWG", Description = "Ürün Sınıfı", EntityPropertySapType = RfcEntityPropertySapTypes.CHAR, Length = 25, SubTypePropertyName = "Code", Unsafe = true)]
        public virtual MaterialCategoryDefinition MaterialCategory { get; set; }
    }
}
