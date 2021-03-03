namespace SapCo2.Core.Models
{
    public sealed class LibraryVersionModel
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
