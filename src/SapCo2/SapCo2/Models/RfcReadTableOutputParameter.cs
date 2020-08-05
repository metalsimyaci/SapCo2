using SapCo2.Wrapper.Attributes;

namespace SapCo2.Models
{
    public class RfcReadTableOutputParameter
    {
        [RfcProperty("DATA")]
        public RfcReadTableData[] Data { get; set; }
    }
    public class RfcReadTableData
    {
        [RfcProperty("WA")]
        public string Wa { get; set; }
    }
}
