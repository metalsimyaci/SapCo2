using SapCo2.Wrapper.Attributes;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.MaterialManagement.Entity
{
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
