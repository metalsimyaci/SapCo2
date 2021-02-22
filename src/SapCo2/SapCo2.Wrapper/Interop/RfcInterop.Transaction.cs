using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Wrapper.Interop
{
    internal sealed partial class RfcInterop
    {
        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcGetTransactionID(IntPtr rfcHandle, string tid, out RfcErrorInfo errorInfo);

        public RfcResultCodes GetTransactionID(IntPtr rfcHandle, out string tid, out RfcErrorInfo errorInfo) =>
            RfcGetTransactionID(rfcHandle, out tid, out errorInfo);

        [DllImport(NetWeaverRfcLib)]
        private static extern IntPtr RfcCreateTransaction(IntPtr rfcHandle, string tid, string queueName, out RfcErrorInfo errorInfo);

        public IntPtr CreateTransaction(IntPtr rfcHandle, string tid, string queueName, out RfcErrorInfo errorInfo) =>
            RfcCreateTransaction(rfcHandle, tid, out queueName, errorInfo);


        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcInvokeInTransaction(IntPtr rfcTransactionHandle,IntPtr funcHandle, out RfcErrorInfo errorInfo);

        public RfcResultCodes InvokeInTransaction(IntPtr rfcTransactionHandle,IntPtr funcHandle, out RfcErrorInfo errorInfo) =>
            RfcInvokeInTransaction(rfcTransactionHandle,funcHandle, out errorInfo);


        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcSubmitTransaction(IntPtr rfcTransactionHandle, out RfcErrorInfo errorInfo);

        public RfcResultCodes SubmitTransaction(IntPtr rfcTransactionHandle, out RfcErrorInfo errorInfo) =>
            RfcSubmitTransaction(rfcTransactionHandle, out errorInfo);

        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcConfirmTransaction(IntPtr rfcTransactionHandle, out RfcErrorInfo errorInfo);

        public RfcResultCodes ConfirmTransaction(IntPtr rfcTransactionHandle, out RfcErrorInfo errorInfo) =>
            RfcConfirmTransaction(rfcTransactionHandle, out errorInfo);
    }
}
