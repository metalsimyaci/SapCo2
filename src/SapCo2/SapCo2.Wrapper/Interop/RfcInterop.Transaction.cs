using System;
using System.Runtime.InteropServices;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Wrapper.Interop
{
    internal sealed partial class RfcInterop
    {
        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcGetTransactionID(IntPtr rfcHandle,out string tid, out RfcErrorInfo errorInfo);
        
        public RfcResultCodes GetTransactionId(IntPtr rfcHandle, out string tid, out RfcErrorInfo errorInfo) =>
            RfcGetTransactionID(rfcHandle, out tid, out errorInfo);

        [DllImport(NetWeaverRfcLib)]
        private static extern IntPtr RfcCreateTransaction(IntPtr rfcHandle, string tid, string queueName, out RfcErrorInfo errorInfo);
        
        public IntPtr CreateTransaction(IntPtr rfcHandle, string tid, string queueName, out RfcErrorInfo errorInfo) =>
            RfcCreateTransaction(rfcHandle, tid,  queueName, out errorInfo);

        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcInvokeInTransaction(IntPtr tHandle, IntPtr funcHandle, out RfcErrorInfo errorInfo);
        
        public RfcResultCodes InvokeInTransaction(IntPtr tHandle, IntPtr funcHandle, out RfcErrorInfo errorInfo) =>
            RfcInvokeInTransaction(tHandle, funcHandle, out errorInfo);

        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcSubmitTransaction(IntPtr tHandle, out RfcErrorInfo errorInfo);
        
        public RfcResultCodes SubmitTransaction(IntPtr tHandle, out RfcErrorInfo errorInfo) =>
            RfcSubmitTransaction(tHandle, out errorInfo);

        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcConfirmTransaction(IntPtr tHandle, out RfcErrorInfo errorInfo);
        
        public RfcResultCodes ConfirmTransaction(IntPtr tHandle, out RfcErrorInfo errorInfo) =>
            RfcConfirmTransaction(tHandle, out errorInfo);

        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcDestroyTransaction(IntPtr tHandle, out RfcErrorInfo errorInfo);
        
        public RfcResultCodes DestroyTransaction(IntPtr tHandle, out RfcErrorInfo errorInfo) =>
            RfcDestroyTransaction(tHandle, out errorInfo);
    }
}
