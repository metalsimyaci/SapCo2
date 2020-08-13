using SapCo2.Wrapper.Attributes;

namespace SapCo2.Samples.NetCore.RfcExamples.Models
{
    public sealed class Topmat
    {
        [RfcEntityProperty("MATNR", Description = "Malzeme Tanımı")]
        public string Code { get; set; }

        [RfcEntityProperty("MAKTX", Description = "Tanım")]
        public string Definition { get; set; }
    }
}
