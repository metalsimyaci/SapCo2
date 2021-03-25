using SapCo2.Abstraction;
using SapCo2.Abstraction.Attributes;

namespace SapCo2.Samples.Core.Models
{
    public class GetJobOutputParameter : IRfcOutput
    {
        [RfcEntityProperty("T_V_OP")]
        public JobStatus[] JobStatuses { get; set; }
    }
}
