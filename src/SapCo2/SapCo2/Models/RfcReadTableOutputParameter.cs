using SapCo2.Wrapper.Attributes;

namespace SapCo2.Models
{
    public sealed class RfcReadTableOutputParameter
    {
        [RfcProperty("DATA")]
        public RfcReadTableData[] Data { get; set; }
    }
    public sealed class RfcReadTableData
    {
        [RfcProperty("WA")]
        public string Wa { get; set; }
    }
}
