using System;
using SapCo2.Core.Abstract;
using SapCo2.Core.Models;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Exception;

namespace SapCo2.Core
{
    public class RfcLibrary:IRfcLibrary
    {
        private readonly IRfcInterop _interop;

        public RfcLibrary(IRfcInterop interop)
        {
            _interop = interop;
        }

        public void EnsureLibraryPresent()
        {
            GetVersion();
        }

        public LibraryVersionModel GetVersion()
        {
            try
            {
                RfcResultCodes resultCode = _interop
                    .GetVersion(out uint majorVersion, out uint minorVersion, out uint patchLevel);

                return new LibraryVersionModel
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
    }
}
