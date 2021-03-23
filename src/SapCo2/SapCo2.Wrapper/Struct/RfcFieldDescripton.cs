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

        [MarshalAs(UnmanagedType.SysInt)]
        public int NucLength;

        [MarshalAs(UnmanagedType.SysInt)]
        public int NucOffset;

        [MarshalAs(UnmanagedType.SysInt)]
        public int UcLength;

        [MarshalAs(UnmanagedType.SysInt)]
        public int UcOffset;

        [MarshalAs(UnmanagedType.SysInt)]
        public int Decimals;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public IntPtr TypeDescHandle;
    }
}
