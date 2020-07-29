using System.Runtime.InteropServices;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Wrapper.Interop
{
    public sealed partial class RfcInterop
    {
        private const string NetWeaverRfcLib = "sapnwrfc";
       
        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcGetVersion(out uint majorVersion, out uint minorVersion, out uint patchLevel);

        public RfcResultCodes GetVersion(out uint majorVersion, out uint minorVersion, out uint patchLevel)
            => RfcGetVersion(out majorVersion, out minorVersion, out patchLevel);
    }
}
