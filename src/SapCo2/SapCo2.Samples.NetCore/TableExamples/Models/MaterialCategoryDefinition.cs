using SapCo2.Attributes;
using SapCo2.Enumeration;

namespace SapCo2.Samples.NetCore.TableExamples.Models
{
    [RfcTable("ZMM24030", Description = "Material Category Definition Table", Unsafe = true)]
    public class MaterialCategoryDefinition
    {

        #region Properties

        [RfcTableProperty("ZZEXTWG", Description = "Material Category Code", TablePropertySapType = RfcTablePropertySapTypes.CHAR, Length = 25)]
        public string Code { get; set; }

        [RfcTableProperty("TANIM", Description = "Material Category Definition", TablePropertySapType = RfcTablePropertySapTypes.CHAR, Length = 50)]
        public string Definition { get; set; }

        [RfcTableProperty("ZZPARCASAYI", Description = "Material Category Unit Part Count", TablePropertySapType = RfcTablePropertySapTypes.INTEGER, Length = 10)]
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
