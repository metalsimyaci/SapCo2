using SapCo2.Wrapper.Attributes;

namespace SapCo2.Samples.NetCore.RfcExamples.Models
{
    public sealed class Stb
    {
        [RfcEntityProperty("STUFE", Description = "Seviye")]
        public int Level { get; set; }

        [RfcEntityProperty("OJTXB", Description = "Nesne kısa metni (bileşen grubu)")]
        public string Name { get; set; }

        [RfcEntityProperty("OJTXP", Description = "Nesne kısa metni (kalem)")]
        public string SubItemName { get; set; }

        [RfcEntityProperty("MMEIN", Description = "Temel ölçü birimi")]
        public string Unit { get; set; }

        [RfcEntityProperty("MNGLG", Description = "Temel ölçü biriminde hesaplanan bileşen miktarı")]
        public decimal UnitAmount { get; set; }

        [RfcEntityProperty("MNGKO", Description = "Bileşen ölçü biriminde hesaplanan bileşen miktarı")]
        public decimal Amount { get; set; }

        [RfcEntityProperty("IDNRK", Description = "Ürün ağacı bileşenleri")]
        public string SubItemCode { get; set; }

        [RfcEntityProperty("MENGE", Description = "Bileşen miktarı")]
        public decimal ComponentAmount { get; set; }

        [RfcEntityProperty("STLTY", Description = "Ürün ağacı tipi")]
        public string TreeType { get; set; }

        [RfcEntityProperty("STLKN", Description = "Ürün ağacı kalemi düğüm numarası")]
        public decimal TreeNodeNumber { get; set; }

        [RfcEntityProperty("STPOZ", Description = "Dahili Sayaç")]
        public decimal InternalCounter { get; set; }

        [RfcEntityProperty("STLNR", Description = "Üst Seviye")]
        public int UpperTreeId { get; set; }

        [RfcEntityProperty("XTLNR", Description = "Alt Seviye")]
        public int SubTreeId { get; set; }

        [RfcEntityProperty("MATMK", Description = "Mal Grubu")]
        public string MaterialGroup { get; set; }
    }
}
