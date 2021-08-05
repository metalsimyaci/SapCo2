using System.Diagnostics.CodeAnalysis;
using SapCo2.Abstraction.Attributes;

namespace SapCo2.Models
{
    [ExcludeFromCodeCoverage]
    internal sealed class TableData
    {
        #region Properties

        [RfcEntityProperty("WA")]
        public string Wa { get; set; }

        #endregion
    }
}
