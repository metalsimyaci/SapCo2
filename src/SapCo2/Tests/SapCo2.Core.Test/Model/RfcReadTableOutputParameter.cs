using SapCo2.Wrapper.Attributes;

namespace SapCo2.Core.Test.Model
{
    public class RfcReadTableOutputParameter
    {
        [RfcEntityProperty("DATA")]
        public RfcReadTableData[] Data { get; set; }
    }
    public class RfcReadTableData
    {
        [RfcEntityProperty("WA")]
        public string Wa { get; set; }
    }
}
