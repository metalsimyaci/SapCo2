using System.Diagnostics.CodeAnalysis;
using SapCo2.Wrapper.Attributes;

namespace SapCo2.Models
{
    [ExcludeFromCodeCoverage]
    public sealed class RfcReadTableOutputParameter
    {
        [RfcEntityProperty("DATA")]
        public RfcReadTableData[] Data { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public sealed class RfcReadTableData
    {
        [RfcEntityProperty("WA")]
        public string Wa { get; set; }
    }
}
