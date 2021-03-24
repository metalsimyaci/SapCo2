using System;
using System.Runtime.InteropServices;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Wrapper.Interop
{
    internal sealed partial class RfcInterop
    {
        [DllImport(NetWeaverRfcLib)]
        private static extern IntPtr RfcOpenConnection(RfcConnectionParameter[] connectionParams, uint paramCount,
            out RfcErrorInfo errorInfo);
        
        public IntPtr OpenConnection(RfcConnectionParameter[] connectionParams, uint paramCount,
            out RfcErrorInfo errorInfo)
            => RfcOpenConnection(connectionParams, paramCount, out errorInfo);

        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcCloseConnection(IntPtr rfcHandle, out RfcErrorInfo errorInfo);
        
        public RfcResultCodes CloseConnection(IntPtr rfcHandle, out RfcErrorInfo errorInfo)
            => RfcCloseConnection(rfcHandle, out errorInfo);

        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcIsConnectionHandleValid(IntPtr rfcHandle, out int isValid,
            out RfcErrorInfo errorInfo);
        
        public RfcResultCodes IsConnectionHandleValid(IntPtr rfcHandle, out int isValid,
            out RfcErrorInfo errorInfo)
            => RfcIsConnectionHandleValid(rfcHandle, out isValid, out errorInfo);

        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcPing(IntPtr rfcHandle, out RfcErrorInfo errorInfo);
        
        public RfcResultCodes Ping(IntPtr rfcHandle, out RfcErrorInfo errorInfo)
            => RfcPing(rfcHandle, out errorInfo);
    }
}
