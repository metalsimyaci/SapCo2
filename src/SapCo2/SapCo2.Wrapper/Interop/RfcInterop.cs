using System.Runtime.InteropServices;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Wrapper.Interop
{
    public partial class RfcInterop
    {
        private const string NETWEAVER_RFC_LIB = "sapnwrfc";
       
        [DllImport(NETWEAVER_RFC_LIB)]
        private static extern RfcResultCodes RfcGetVersion(out uint majorVersion, out uint minorVersion, out uint patchLevel);

        public virtual RfcResultCodes GetVersion(out uint majorVersion, out uint minorVersion, out uint patchLevel)
            => RfcGetVersion(out majorVersion, out minorVersion, out patchLevel);
    }
}