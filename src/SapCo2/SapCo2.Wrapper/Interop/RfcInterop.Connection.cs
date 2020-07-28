using System;
using System.Runtime.InteropServices;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Wrapper.Interop
{
    public partial class RfcInterop
    {
        [DllImport(NETWEAVER_RFC_LIB)]
        private static extern IntPtr RfcOpenConnection(RfcConnectionParameter[] connectionParams, uint paramCount,
            out RfcErrorInfo errorInfo);

        public virtual IntPtr OpenConnection(RfcConnectionParameter[] connectionParams, uint paramCount,
            out RfcErrorInfo errorInfo)
            => RfcOpenConnection(connectionParams, paramCount, out errorInfo);

        [DllImport(NETWEAVER_RFC_LIB)]
        private static extern RfcResultCodes RfcCloseConnection(IntPtr rfcHandle, out RfcErrorInfo errorInfo);

        public virtual RfcResultCodes CloseConnection(IntPtr rfcHandle, out RfcErrorInfo errorInfo)
            => RfcCloseConnection(rfcHandle, out errorInfo);

        [DllImport(NETWEAVER_RFC_LIB)]
        private static extern RfcResultCodes RfcIsConnectionHandleValid(IntPtr rfcHandle, out int isValid,
            out RfcErrorInfo errorInfo);

        public virtual RfcResultCodes IsConnectionHandleValid(IntPtr rfcHandle, out int isValid,
            out RfcErrorInfo errorInfo)
            => RfcIsConnectionHandleValid(rfcHandle, out isValid, out errorInfo);

        [DllImport(NETWEAVER_RFC_LIB)]
        private static extern RfcResultCodes RfcPing(IntPtr rfcHandle, out RfcErrorInfo errorInfo);

        public virtual RfcResultCodes Ping(IntPtr rfcHandle, out RfcErrorInfo errorInfo)
            => RfcPing(rfcHandle, out errorInfo);
    }
}