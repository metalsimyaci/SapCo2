using System.Diagnostics.CodeAnalysis;
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SapCo2.Core.Models
{
    [ExcludeFromCodeCoverage]
    public sealed class RfcNetWeaverLibraryVersion
    {
        /// <summary>
        /// Gets the major version value.
        /// </summary>
        public uint Major { get; internal set; }

        /// <summary>
        /// Gets the minor version value.
        /// </summary>
        public uint Minor { get; internal set; }

        /// <summary>
        /// Gets the patch version value.
        /// </summary>
        public uint Patch { get; internal set; }
    }
}
