using System;
using System.Runtime.InteropServices;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Wrapper.Struct
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct RfcParameterDescription
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31 + 1)]
        public string Name;

        [MarshalAs(UnmanagedType.I4)]
        public RfcType Type;

        [MarshalAs(UnmanagedType.SysInt)]
        public RfcDirection Direction;

        [MarshalAs(UnmanagedType.SysInt)]
        public int NucLength;

        [MarshalAs(UnmanagedType.SysInt)]
        public int UcLength;

        [MarshalAs(UnmanagedType.SysInt)]
        public int Decimals;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public IntPtr TypeDescriptionHandle;

        [MarshalAs(UnmanagedType.LPStr, SizeConst = 80)]
        public string ParameterText;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31 + 1)]
        public string DefaultValue;

        [MarshalAs(UnmanagedType.I1)]
        public bool Optional;
    }
}
