using SapCo2.Abstraction.Attributes;

namespace SapCo2.Models
{
    internal sealed class TableOption
    {
        #region Properties

        [RfcEntityProperty("TEXT")]
        public string Text { get; set; }

        #endregion
    }
}
