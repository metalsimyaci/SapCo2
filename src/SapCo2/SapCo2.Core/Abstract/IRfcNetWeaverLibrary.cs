using SapCo2.Core.Models;

namespace SapCo2.Core.Abstract
{
    public interface IRfcNetWeaverLibrary
    {
        public RfcNetWeaverLibraryVersion LibraryVersion { get; }

        void EnsureLibraryPresent();
        RfcNetWeaverLibraryVersion GetVersion();
    }
}
