using System;
using System.Diagnostics.CodeAnalysis;
using SapCo2.Core.Abstract;
using SapCo2.Core.Models;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Exception;

namespace SapCo2.Core
{
    [ExcludeFromCodeCoverage]
    public class RfcNetWeaverLibrary : IRfcNetWeaverLibrary
    {
        private readonly IRfcInterop _interop;
        private bool _libraryChecked;
        private RfcNetWeaverLibraryVersion _libraryVersion;
        public RfcNetWeaverLibraryVersion LibraryVersion
        {
            get
            {
                EnsureLibraryPresent();
                return _libraryVersion;
            }
            private set => _libraryVersion = value;
        }

        public RfcNetWeaverLibrary(IRfcInterop interop)
        {
            _interop = interop;
        }
        

        #region IRfcLibrary Implementation

        public void EnsureLibraryPresent()
        {
            if (_libraryChecked) return;

            LibraryVersion = GetVersion();
            _libraryChecked = true;
        }

        public RfcNetWeaverLibraryVersion GetVersion()
        {
            try
            {
                _interop.GetVersion(out uint majorVersion, out uint minorVersion, out uint patchLevel);

                return new RfcNetWeaverLibraryVersion
                {
                    Major = majorVersion,
                    Minor = minorVersion,
                    Patch = patchLevel,
                };
            }
            catch (DllNotFoundException ex)
            {
                throw new RfcLibraryNotFoundException(ex);
            }
        }

        #endregion
    }
}
