using SapCo2.Wrapper.Attributes;

namespace SapCo2.Models
{
    public sealed class RfcReadTableOutputParameter
    {
        [RfcEntityProperty("DATA")]
        public RfcReadTableData[] Data { get; set; }
    }
    public sealed class RfcReadTableData
    {
        [RfcEntityProperty("WA")]
        public string Wa { get; set; }
    }
}
