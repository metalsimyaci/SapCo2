using SapCo2.Abstraction.Attributes;

namespace SapCo2.Models
{
    internal sealed class TableData
    {
        #region Properties

        [RfcEntityProperty("WA")]
        public string Wa { get; set; }

        #endregion
    }
}
