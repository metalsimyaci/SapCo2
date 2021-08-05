using System;
using SapCo2.Core.Abstract;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Extension;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Core
{
    internal sealed class RfcTransaction : IRfcTransaction
    {
        #region Variables

        private readonly IRfcInterop _interop;
        private readonly IntPtr _rfcConnectionHandle;
        private readonly IntPtr _transactionHandle;
        private bool _disposed;

        #endregion

        #region Methods

        #region Constructors

        public RfcTransaction(IRfcInterop interop, IntPtr rfcConnectionHandle, IntPtr transactionHandle)
        {
            _interop = interop;
            _rfcConnectionHandle = rfcConnectionHandle;
            _transactionHandle = transactionHandle;
        }

        #endregion

        #region IRfcTransaction Implementation

        public IRfcTransactionFunction CreateFunction(string name)
        {
            IntPtr functionDescriptionHandle = GetFunctionDescription(name);

            return CreateFromDescriptionHandle(_interop, functionDescriptionHandle);
        }
        public void SubmitTransaction()
        {
            RfcResultCodes resultCode = _interop.SubmitTransaction(_transactionHandle, out RfcErrorInfo errorInfo);
            resultCode.ThrowOnError(errorInfo);
        }
        public void ConfirmTransaction()
        {
            RfcResultCodes resultCode = _interop.ConfirmTransaction(_transactionHandle, out RfcErrorInfo errorInfo);

            resultCode.ThrowOnError(errorInfo);
        }

        #endregion

        #region Private Function
        
        private void DestroyTransaction()
        {
            if (_transactionHandle == IntPtr.Zero)
                return;

            RfcResultCodes resultCode = _interop.DestroyTransaction(_transactionHandle, out RfcErrorInfo errorInfo);
            resultCode.ThrowOnError(errorInfo);
        }
        private IntPtr GetFunctionDescription(string methodName)
        {
            IntPtr functionDescriptionHandle = _interop.GetFunctionDesc(_rfcConnectionHandle, methodName, out RfcErrorInfo errorInfo);
            errorInfo.ThrowOnError();

            return functionDescriptionHandle;
        }
        private IRfcTransactionFunction CreateFromDescriptionHandle(IRfcInterop interop, IntPtr functionDescriptionHandle)
        {
            IntPtr functionHandle = interop.CreateFunction(functionDescriptionHandle, out RfcErrorInfo errorInfo);

            errorInfo.ThrowOnError();

            return new RfcTransactionFunction(interop,_transactionHandle, functionHandle);
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                DestroyTransaction();

            _disposed = true;
        }

        #endregion

        #endregion
    }
}
