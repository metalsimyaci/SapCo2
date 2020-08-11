using System.Diagnostics.CodeAnalysis;
using SapCo2.Wrapper.Attributes;

namespace SapCo2.Parameters
{
    [ExcludeFromCodeCoverage]
    public sealed class RfcReadTableInputParameter
    {
        [RfcEntityProperty("QUERY_TABLE")] public string Query { get; set; }

        [RfcEntityProperty("DELIMITER")] public string Delimiter { get; set; }

        [RfcEntityProperty("NO_DATA")] public string NoData { get; set; }

        [RfcEntityProperty("ROWCOUNT")] public int RowCount { get; set; }

        [RfcEntityProperty("ROWSKIPS")] public int RowSkips { get; set; }

        [RfcEntityProperty("FIELDS")] public RfcReadTableField[] Fields { get; set; }
        [RfcEntityProperty("OPTIONS")] public RfcReadTableOption[] Options { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public sealed class RfcReadTableField
    {
        [RfcEntityProperty("FIELDNAME")]
        public string FieldName { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public sealed class RfcReadTableOption
    {
        [RfcEntityProperty("TEXT")]
        public string Text { get; set; }
    }
}
