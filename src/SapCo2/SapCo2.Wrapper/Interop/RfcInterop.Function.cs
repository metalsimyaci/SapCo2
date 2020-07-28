using System;
using System.Runtime.InteropServices;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Wrapper.Interop
{
    public partial class RfcInterop
    {
        [DllImport(NETWEAVER_RFC_LIB, CharSet = CharSet.Unicode)]
        private static extern IntPtr RfcGetFunctionDesc(IntPtr rfcHandle, string funcName, out RfcErrorInfo errorInfo);

        public virtual IntPtr GetFunctionDesc(IntPtr rfcHandle, string funcName, out RfcErrorInfo errorInfo)
            => RfcGetFunctionDesc(rfcHandle, funcName, out errorInfo);

        [DllImport(NETWEAVER_RFC_LIB, CharSet = CharSet.Unicode)]
        private static extern IntPtr RfcCreateFunction(IntPtr funcDescHandle, out RfcErrorInfo errorInfo);

        public virtual IntPtr CreateFunction(IntPtr funcDescHandle, out RfcErrorInfo errorInfo)
            => RfcCreateFunction(funcDescHandle, out errorInfo);

        [DllImport(NETWEAVER_RFC_LIB, CharSet = CharSet.Unicode)]
        private static extern RfcResultCodes RfcDestroyFunction(IntPtr funcHandle, out RfcErrorInfo errorInfo);

        public virtual RfcResultCodes DestroyFunction(IntPtr funcHandle, out RfcErrorInfo errorInfo)
            => RfcDestroyFunction(funcHandle, out errorInfo);

        [DllImport(NETWEAVER_RFC_LIB)]
        private static extern RfcResultCodes RfcInvoke(IntPtr rfcHandle, IntPtr funcHandle, out RfcErrorInfo errorInfo);

        public virtual RfcResultCodes Invoke(IntPtr rfcHandle, IntPtr funcHandle, out RfcErrorInfo errorInfo)
            => RfcInvoke(rfcHandle, funcHandle, out errorInfo);
    }
}