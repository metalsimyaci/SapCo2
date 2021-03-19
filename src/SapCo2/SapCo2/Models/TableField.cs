using SapCo2.Abstraction.Attributes;

namespace SapCo2.Models
{
    internal sealed class TableField
    {
        #region Properties

        [RfcEntityProperty("FIELDNAME")]
        public string FieldName { get; set; }

        #endregion
    }
}
