using System;
using System.Runtime.InteropServices;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Wrapper.Struct
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct RfcFieldDescription
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31 + 1)]
        public string Name;
        
        [MarshalAs(UnmanagedType.I4)]
        public RfcType Type;

        [MarshalAs(UnmanagedType.I4)]
        public int NucLength;

        [MarshalAs(UnmanagedType.I4)]
        public int NucOffset;

        [MarshalAs(UnmanagedType.I4)]
        public int UcLength;

        [MarshalAs(UnmanagedType.I4)]
        public int UcOffset;

        [MarshalAs(UnmanagedType.I4)]
        public int Decimals;

        [MarshalAs(UnmanagedType.SysUInt)]
        public IntPtr TypeDescHandle;
    }
}
