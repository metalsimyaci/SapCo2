using SapCo2.Wrapper.Attributes;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.MaterialManagement.Entity
{
    [RfcEntityClass("ZMM24030", Description = "Material Category Definition Table", Unsafe = true)]
    public class MaterialCategoryDefinition
    {

        #region Properties

        [RfcEntityProperty("ZZEXTWG", Description = "Material Category Code", EntityPropertySapType = RfcEntityPropertySapTypes.CHAR, Length = 25)]
        public string Code { get; set; }

        [RfcEntityProperty("TANIM", Description = "Material Category Definition", EntityPropertySapType = RfcEntityPropertySapTypes.CHAR, Length = 50)]
        public string Definition { get; set; }

        [RfcEntityProperty("ZZPARCASAYI", Description = "Material Category Unit Part Count", EntityPropertySapType = RfcEntityPropertySapTypes.INTEGER, Length = 10)]
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
