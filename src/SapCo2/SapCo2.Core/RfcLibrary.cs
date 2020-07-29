using System;
using SapCo2.Core.Models;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Exception;

namespace SapCo2.Core
{
    public static class RfcLibrary
    {

        /// <summary>
        /// Ensures the SAP RFC binaries are present. Throws an <see cref="SapLibraryNotFoundException"/> exception when the SAP RFC binaries could not be found.
        /// </summary>
        public static void EnsureLibraryPresent(IRfcInterop interop)
        {
            GetVersion(interop);
        }

        /// <summary>
        /// Gets the SAP RFC library version.
        /// </summary>
        /// <returns>The SAP RFC library version.</returns>
        public static RfcLibraryVersion GetVersion(IRfcInterop interop)
        {
            try
            {
                RfcResultCodes resultCode = interop
                    .GetVersion(out uint majorVersion, out uint minorVersion, out uint patchLevel);

                return new RfcLibraryVersion
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
