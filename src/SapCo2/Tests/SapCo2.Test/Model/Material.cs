using SapCo2.Wrapper.Attributes;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Test.Model
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

    [RfcClass("MAKT", Description = "Material Definition Table")]
    public class MaterialDefinition
    {
        [RfcProperty("MATNR", Description = "Material Code", DataType = RfcDataTypes.Char, Length = 18)]
        public string Code { get; set; }

        [RfcProperty("MAKTX", Description = "Material Short Definition", DataType = RfcDataTypes.Char, Length = 40)]
        public string Definition { get; set; }
    }

    [RfcClass("ZMM24030", Description = "Material Category Definition Table", Unsafe = true)]
    public class MaterialCategoryDefinition
    {

        #region Properties

        [RfcProperty("ZZEXTWG", Description = "Material Category Code", DataType = RfcDataTypes.Char, Length = 25)]
        public string Code { get; set; }

        [RfcProperty("TANIM", Description = "Material Category Definition", DataType = RfcDataTypes.Char, Length = 50)]
        public string Definition { get; set; }

        [RfcProperty("ZZPARCASAYI", Description = "Material Category Unit Part Count", DataType = RfcDataTypes.Integer, Length = 10)]
        public int PartCount { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return Definition;
        }

        #endregion

    }
}
