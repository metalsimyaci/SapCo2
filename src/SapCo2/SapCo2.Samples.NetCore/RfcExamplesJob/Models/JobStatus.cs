using SapCo2.Wrapper.Attributes;

namespace SapCo2.Samples.NetCore.RfcExamplesJob.Models
{
    public sealed class JobStatus
    {
        [RfcEntityProperty("JOBNAME", Description = "Görev Tanımı")]
        public string JobName { get; set; }

        [RfcEntityProperty("JOBCOUNT", Description = "Görev Numarası")]
        public string JobNo { get; set; }

        [RfcEntityProperty("TCODE", Description = "İşlem Kodu")]
        public string TransactionCode { get; set; }

        [RfcEntityProperty("TTEXT", Description = "İşlem Kodu Açıklaması")]
        public string TransactionCodeDefinition { get; set; }
    }
}
