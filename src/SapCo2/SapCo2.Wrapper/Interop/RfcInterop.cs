using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Wrapper.Interop
{
    [ExcludeFromCodeCoverage]
    internal sealed partial class RfcInterop : IRfcInterop
    {
        private const string NetWeaverRfcLib = "sapnwrfc";

        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcGetVersion(out uint majorVersion, out uint minorVersion,
            out uint patchLevel);

        public RfcResultCodes GetVersion(out uint majorVersion, out uint minorVersion, out uint patchLevel)
            => RfcGetVersion(out majorVersion, out minorVersion, out patchLevel);
    }
}
