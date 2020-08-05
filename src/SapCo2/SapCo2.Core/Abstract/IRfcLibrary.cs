using SapCo2.Core.Models;

namespace SapCo2.Core.Abstract
{
    public interface IRfcLibrary
    {
        void EnsureLibraryPresent();
        LibraryVersionModel GetVersion();
    }
}
