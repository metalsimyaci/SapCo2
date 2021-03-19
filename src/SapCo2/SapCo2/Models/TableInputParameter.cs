using SapCo2.Abstraction.Attributes;

namespace SapCo2.Models
{
    internal sealed class TableInputParameter
    {
        #region Properties

        [RfcEntityProperty("QUERY_TABLE")]
        public string Query { get; set; }

        [RfcEntityProperty("DELIMITER")]
        public string Delimiter { get; set; }

        [RfcEntityProperty("NO_DATA")]
        public string NoData { get; set; }

        [RfcEntityProperty("ROWCOUNT")]
        public int RowCount { get; set; }

        [RfcEntityProperty("ROWSKIPS")]
        public int RowSkips { get; set; }

        [RfcEntityProperty("FIELDS")]
        public TableField[] Fields { get; set; }

        [RfcEntityProperty("OPTIONS")]
        public TableOption[] Options { get; set; }

        #endregion
    }
}
