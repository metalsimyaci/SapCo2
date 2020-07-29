using System;
using System.Runtime.InteropServices;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Wrapper.Interop
{
    public sealed partial class RfcInterop
    {
        [DllImport(NetWeaverRfcLib, CharSet = CharSet.Unicode)]
        private static extern IntPtr RfcGetFunctionDesc(IntPtr rfcHandle, string funcName, out RfcErrorInfo errorInfo);

        public IntPtr GetFunctionDesc(IntPtr rfcHandle, string funcName, out RfcErrorInfo errorInfo)
            => RfcGetFunctionDesc(rfcHandle, funcName, out errorInfo);

        [DllImport(NetWeaverRfcLib, CharSet = CharSet.Unicode)]
        private static extern IntPtr RfcCreateFunction(IntPtr funcDescHandle, out RfcErrorInfo errorInfo);

        public IntPtr CreateFunction(IntPtr funcDescHandle, out RfcErrorInfo errorInfo)
            => RfcCreateFunction(funcDescHandle, out errorInfo);

        [DllImport(NetWeaverRfcLib, CharSet = CharSet.Unicode)]
        private static extern RfcResultCodes RfcDestroyFunction(IntPtr funcHandle, out RfcErrorInfo errorInfo);

        public RfcResultCodes DestroyFunction(IntPtr funcHandle, out RfcErrorInfo errorInfo)
            => RfcDestroyFunction(funcHandle, out errorInfo);

        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcInvoke(IntPtr rfcHandle, IntPtr funcHandle, out RfcErrorInfo errorInfo);

        public RfcResultCodes Invoke(IntPtr rfcHandle, IntPtr funcHandle, out RfcErrorInfo errorInfo)
            => RfcInvoke(rfcHandle, funcHandle, out errorInfo);
    }
}
