using System;
using SapCo2.Abstraction;
using SapCo2.Abstraction.Attributes;

namespace SapCo2.Samples.Core.Models
{
    public sealed class BomInputParameter : IRfcInput
    {
        [RfcEntityProperty("AUMGB")]
        public string Aumgb { get; set; }

        [RfcEntityProperty("CAPID")]
        public string Capid { get; set; }

        [RfcEntityProperty("DATUV")]
        public DateTime Datuv { get; set; }

        [RfcEntityProperty("EMENG")]
        public string Emeng { get; set; }

        [RfcEntityProperty("MKTLS")]
        public string Mktls { get; set; }

        [RfcEntityProperty("MEHRS")]
        public string Mehrs { get; set; }

        [RfcEntityProperty("STPST")]
        public string Stpst { get; set; }

        [RfcEntityProperty("SVWVO")]
        public string Svwvo { get; set; }

        [RfcEntityProperty("WERKS")]
        public string Werks { get; set; }

        [RfcEntityProperty("VRSVO")]
        public string Vrsvo { get; set; }

        [RfcEntityProperty("STLAN")]
        public string Stlan { get; set; }

        [RfcEntityProperty("STLAL")]
        public string Stlal { get; set; }

        [RfcEntityProperty("MTNRV")]
        public string Mtnrv { get; set; }
    }
}
