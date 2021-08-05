using SapCo2.Abstraction;
using SapCo2.Abstraction.Attributes;
using SapCo2.Abstraction.Enumerations;

namespace SapCo2.Samples.Core.Models
{
    [RfcEntity("ZMM24030", Description = "Material Category Definition Table", Unsafe = true)]
    public class MaterialCategoryDefinition : ISapTable
    {
        #region Properties

        [RfcEntityProperty("ZZEXTWG", Description = "Material Category Code", SapDataType = RfcDataTypes.CHAR, Length = 25)]
        public string Code { get; set; }

        [RfcEntityProperty("TANIM", Description = "Material Category Definition", SapDataType = RfcDataTypes.CHAR, Length = 50)]
        public string Definition { get; set; }

        [RfcEntityProperty("ZZPARCASAYI", Description = "Material Category Unit Part Count", SapDataType = RfcDataTypes.INTEGER, Length = 10)]
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
