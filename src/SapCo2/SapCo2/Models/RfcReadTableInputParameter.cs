using SapCo2.Wrapper.Attributes;

namespace SapCo2.Models
{
    public sealed class RfcReadTableInputParameter
    {
        [RfcProperty("QUERY_TABLE")] public string Query { get; set; }

        [RfcProperty("DELIMITER")] public string Delimiter { get; set; }

        [RfcProperty("NO_DATA")] public string NoData { get; set; }

        [RfcProperty("ROWCOUNT")] public int RowCount { get; set; }

        [RfcProperty("ROWSKIPS")] public int RowSkips { get; set; }

        [RfcProperty("FIELDS")] public RfcReadTableField[] Fields { get; set; }
        [RfcProperty("OPTIONS")] public RfcReadTableOption[] Options { get; set; }
    }

    public sealed class RfcReadTableField
    {
        [RfcProperty("FIELDNAME")]
        public string FieldName { get; set; }
    }

    public sealed class RfcReadTableOption
    {
        [RfcProperty("TEXT")]
        public string Text { get; set; }
    }
}
