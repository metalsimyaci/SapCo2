using System.Diagnostics.CodeAnalysis;
using SapCo2.Abstraction.Attributes;

namespace SapCo2.Models
{
    [ExcludeFromCodeCoverage]
    internal sealed class TableOutputParameter
    {
        #region Properties

        [RfcEntityProperty("DATA")]
        public TableData[] Data { get; set; }

        #endregion
    }
}
