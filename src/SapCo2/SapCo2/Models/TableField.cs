using System.Diagnostics.CodeAnalysis;
using SapCo2.Abstraction.Attributes;

namespace SapCo2.Models
{
    [ExcludeFromCodeCoverage]
    internal sealed class TableField
    {
        #region Properties

        [RfcEntityProperty("FIELDNAME")]
        public string FieldName { get; set; }

        #endregion
    }
}
