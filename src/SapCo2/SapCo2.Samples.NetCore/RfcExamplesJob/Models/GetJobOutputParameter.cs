using SapCo2.Wrapper.Attributes;

namespace SapCo2.Samples.NetCore.RfcExamplesJob.Models
{
    public sealed class GetJobOutputParameter
    {
        [RfcEntityProperty("T_V_OP")]
        public JobStatus[] JobStatuses { get; set; }
    }
}
