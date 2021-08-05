using SapCo2.Abstraction;
using SapCo2.Abstraction.Attributes;
using SapCo2.Abstraction.Enumerations;

namespace SapCo2.Samples.Core.Models
{
    public class MaterialSaveDataBapiInputParameter:IBapiInput
    {
        [RfcEntityProperty("HEADDATA", "Header segment with control information")]
        public HeadData HeadData { get; set; }

        [RfcEntityProperty("CLIENTDATA", "Upper unit level material data")]
        public ClientData ClientData { get; set; }

        [RfcEntityProperty("MATERIALDESCRIPTION", "Material Descriptions")]
        public MaterialDescription MaterialDescription { get; set; }
    }
    public class HeadData
    {
        [RfcEntityProperty("MATERIAL","Material Code [Matnr]",RfcDataTypes.CHAR,18)]
        public string MaterialCode { get; set; }

        [RfcEntityProperty("IND_SECTOR","Sector [MBRSH]",RfcDataTypes.CHAR,1)]
        public char Sector { get; set; }

        [RfcEntityProperty("MATL_TYPE", "Material Type Code [MBRSH]", RfcDataTypes.CHAR, 4)]
        public string MaterialType { get; set; }

        [RfcEntityProperty("BASIC_VIEW", "Basic data view [SICHT_K]", RfcDataTypes.CHAR, 1)]
        public char IsBasicView { get; set; }
    }

    public class ClientData
    {
        [RfcEntityProperty("MATL_GROUP", "Material Group Code [MATKL]", RfcDataTypes.CHAR, 9)]
        public string MaterialGroup { get; set; }

        [RfcEntityProperty("PROD_HIER", "Product Hierarchy Code [PRODH_D]", RfcDataTypes.CHAR, 18)]
        public string ProductionHierarchy { get; set; }

        [RfcEntityProperty("BASE_UOM", "Main Measurement Unit [MEINS]", RfcDataTypes.UNIT, 3)]
        public string BaseMeasurementUnit { get; set; }

        [RfcEntityProperty("BASE_UOM_ISO", "Main Measurement Unit For ISO [MEINS_ISO]", RfcDataTypes.CHAR, 3)]
        public string BaseMeasurementUnitIso { get; set; }

        [RfcEntityProperty("DIVISION", "Material Division [SPART]", RfcDataTypes.CHAR, 2)]
        public string Divison { get; set; }

        [RfcEntityProperty("STOR_CONDS", "Storage Condition [RAUBE]", RfcDataTypes.CHAR, 2)]
        public string StorageCondition { get; set; }
    }

    public class MaterialDescription
    {
        [RfcEntityProperty("MATL_DESC", "Material Description [MAKTX]", RfcDataTypes.CHAR, 40)]
        public string Name { get; set; }

        [RfcEntityProperty("LANGU_ISO", "Language Code [SPRAS]", RfcDataTypes.CHAR, 2)]
        public string LanguageCode { get; set; }
    }

}
