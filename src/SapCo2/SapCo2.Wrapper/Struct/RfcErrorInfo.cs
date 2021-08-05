using System.Runtime.InteropServices;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Wrapper.Struct
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct RfcErrorInfo
    {
        [MarshalAs(UnmanagedType.I4)]
        public RfcResultCodes Code;

        [MarshalAs(UnmanagedType.I4)]
        public RfcErrorGroups Group;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string Key;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string Message;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20 + 1)]
        public string AbapMsgClass;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1 + 1)]
        public string AbapMsgType;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3 + 1)]
        public string AbapMsgNumber;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50 + 1)]
        public string AbapMsgV1;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50 + 1)]
        public string AbapMsgV2;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50 + 1)]
        public string AbapMsgV3;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50 + 1)]
        public string AbapMsgV4;
    }
}
