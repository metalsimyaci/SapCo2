using SapCo2.Abstraction;
using SapCo2.Abstraction.Attributes;

namespace SapCo2.Samples.Core.Models
{
    public class GetJobInputParameter : IRfcInput
    {
        [RfcEntityProperty("I_DATA_S")]
        public string StartDate { get; set; }

        [RfcEntityProperty("I_DATA_E")]
        public string EndDate { get; set; }

        [RfcEntityProperty("I_STATUS")]
        public string Status { get; set; }

        [RfcEntityProperty("I_PROGNAME")]
        public string ProgramName { get; set; }

        [RfcEntityProperty("I_AUTHCKMAN")]
        public string ClientCode { get; set; }
    }
}
