using System.Runtime.InteropServices;

namespace SapCo2.Wrapper.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct RfcConnectionParameter
    {
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Name;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string Value;
    }
}
